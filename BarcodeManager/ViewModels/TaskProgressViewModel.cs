using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using FilmBarcodes.Common;
using FilmBarcodes.Common.Enums;
using FilmBarcodes.Common.Models;
using FilmBarcodes.Common.Models.BarcodeManager;
using NLog;

namespace BarcodeManager.ViewModels
{
    public class TaskProgressViewModel : ObservableObject
    {
        public int Id { get; }

        private readonly TasksViewModel _parent;
        private readonly Logger _logger;
        private readonly CancellationTokenSource _cancellationTokenSource;

        private State _state;
        private VideoFile _videoFile;
        private TimeSpan _elapsed;
        private DispatcherTimer _timer;
        private Stopwatch _stopWatch;
        private ProgressWrapper _progress;
        private TaskProgressVisibility _taskProgressVisibility = new TaskProgressVisibility();

        public State State
        {
            get => _state;
            private set
            {
                _state = value;

                TaskProgressVisibility.QueuedVisible = _state == State.Queued;
                TaskProgressVisibility.RunningVisible = _state == State.Running;
                TaskProgressVisibility.SuccessVisible = _state == State.Success;
                TaskProgressVisibility.FailureVisible = _state == State.Failure;
                TaskProgressVisibility.CancelledVisible = _state == State.Cancelled;

                RaisePropertyChangedEvent("State");
            }
        }

        private VideoCollection VideoCollection { get; set; }

        public VideoFile VideoFile
        {
            get => _videoFile;
            set
            {
                //if (_videoFile == value) return;
                _videoFile = value;
                
                RaisePropertyChangedEvent("VideoFile");
            }
        }

        public TimeSpan Elapsed
        {
            get => _elapsed;
            set
            {
                //if (_isSelected == value) return;
                _elapsed = value;
                
                RaisePropertyChangedEvent("Elapsed");
            }
        }

        public ProgressWrapper Progress
        {
            get => _progress;
            set
            {
                //if (_isSelected == value) return;
                _progress = value;
                
                RaisePropertyChangedEvent("Progress");
            }
        }

        public TaskProgressVisibility TaskProgressVisibility
        {
            get => _taskProgressVisibility;
            set
            {
                _taskProgressVisibility = value;

                RaisePropertyChangedEvent("TaskProgressVisibility");
            }
        }

        public ICommand CancelTaskCommand => new DelegateCommand(CancelTask);
        public ICommand DeleteTaskCommand => new DelegateCommand(DeleteTask);

        public TaskProgressViewModel(TasksViewModel parent, VideoFile videoFile, VideoCollection videoCollection, int id)
        {
            _parent = parent;
            _logger = LogManager.GetCurrentClassLogger();

            VideoFile = videoFile;
            VideoCollection = videoCollection;
            Id = id;
            State = State.Queued;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public async Task<State> RunTask()
        {
            _logger.Info($"Running task id:{Id}, {VideoFile.FilenameWithoutExtension}");

            State = State.Running;

            StartTimer();

            // set up the progress control
            var progress = new Progress<ProgressWrapper>(p =>
            {
                Progress = p;
                RaisePropertyChangedEvent("Progress");
            });

            // check if the video output directory exists, otherwise create
            var response = await Task.Run(() =>
            {
                if (Directory.Exists(VideoCollection.Config.FullOutputDirectory))
                    return new TaskResponse{Success = true};

                try
                {
                    Directory.CreateDirectory(VideoCollection.Config.FullOutputDirectory);
                }
                catch (Exception e)
                {
                    return new TaskResponse { Success = false, Exception = e, Message = $"Unable to create output directory {VideoCollection.Config.FullOutputDirectory}" };
                }

                return new TaskResponse { Success = true };
            });

            if (!response.Success)
                return ProcessUnsuccessfulTaskResponse(response);

            // if required, build the colour list
            if (!VideoCollection.Data.Colours.Any())
            {
                var buildColourListResponse = await Task.Run(() =>
                {
                    VideoCollection videoCollection = null;
                    
                    try
                    {
                        videoCollection = VideoProcessor.BuildColourListAsync(VideoCollection, VideoFile, progress, _cancellationTokenSource.Token);
                    }
                    catch (OperationCanceledException)
                    {
                        return (new TaskResponse { Success = false, Cancelled = true, Message = $"Cancelled task id:{Id}, {VideoFile.FilenameWithoutExtension}" }, videoCollection);
                    }
                    catch (Exception e)
                    {
                        return (new TaskResponse { Success = false, Exception = e, Message = $"Unable to create output directory {VideoCollection.Config.FullOutputDirectory}" }, videoCollection);
                    }

                    return (new TaskResponse {Success = true}, videoCollection);
                });

                if (!buildColourListResponse.Item1.Success)
                    return ProcessUnsuccessfulTaskResponse(buildColourListResponse.Item1);

                VideoCollection = buildColourListResponse.Item2;

                // archive the frame images and then delete the directory
                response = await Task.Run(() =>
                {
                    try
                    {
                        ZipProcessor.ZipDirectoryAsync(VideoCollection.Config.ImageDirectory, progress, _cancellationTokenSource.Token);

                        Directory.Delete(VideoCollection.Config.ImageDirectory, true);
                    }
                    catch (OperationCanceledException)
                    {
                        return new TaskResponse { Success = false, Cancelled = true, Message = $"Cancelled task id:{Id}, {VideoFile.FilenameWithoutExtension}" };
                    }
                    catch (Exception e)
                    {
                        return new TaskResponse { Success = false, Exception = e, Message = "Failed to zip frame images" };
                    }

                    return new TaskResponse { Success = true };
                });

                if (!response.Success)
                    return ProcessUnsuccessfulTaskResponse(response);
            }

            // render the final image
            response = await Task.Run(() =>
            {
                try
                {
                    ImageProcessor.RenderImageAsync(VideoCollection, VideoFile, progress, _cancellationTokenSource.Token);
                }
                catch (OperationCanceledException)
                {
                    return new TaskResponse { Success = false, Cancelled = true, Message = $"Cancelled task id:{Id}, {VideoFile.FilenameWithoutExtension}" };
                }
                catch (Exception e)
                {
                    return new TaskResponse { Success = false, Exception = e, Message = $"Failed whilst rendering final image {VideoFile.OutputFilename}" };
                }

                return new TaskResponse { Success = true };
            });

            if (!response.Success)
                return ProcessUnsuccessfulTaskResponse(response);

            // update the video collection and write as json
            await Task.Run(() =>
            {
                VideoCollection.VideoFiles.Add(VideoFile);

                VideoCollection.WriteAsync(progress);
            });

            StopTimer();

            State = State.Success;

            Progress.Status = "Complete!";

            RaiseTaskResponsePropertyChangedEvents();

            return State;
        }

        private void CancelTask()
        {
            _cancellationTokenSource?.Cancel();
        }

        private void DeleteTask()
        {
            _parent.DeleteTask(this);
        }

        private State ProcessUnsuccessfulTaskResponse(TaskResponse response)
        {
            StopTimer();

            if (response.Cancelled)
            {
                State = State.Cancelled;
                _logger.Warn(response.Message);
            }
            else
            {
                State = State.Failure;
                _logger.Error(response.Exception, response.Message);
            }

            Progress.Status = response.Message;

            RaiseTaskResponsePropertyChangedEvents();

            return State;
        }

        private void RaiseTaskResponsePropertyChangedEvents()
        {
            RaisePropertyChangedEvent("Progress");
            RaisePropertyChangedEvent("State");
            RaisePropertyChangedEvent("TaskProgressVisibility");
        }

        private void StartTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Tick += DispatcherTimerTick;
            _timer.Interval = new TimeSpan(0, 0, 0, 1);
            _stopWatch = new Stopwatch();
            _stopWatch.Start();
            _timer.Start();
        }

        private void StopTimer()
        {
            _stopWatch.Stop();
            _timer.Stop();
        }

        private void DispatcherTimerTick(object sender, EventArgs e)
        {
            Elapsed = _stopWatch.Elapsed;
        }
    }
}