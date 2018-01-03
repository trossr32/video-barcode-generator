using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
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

        private readonly Logger _logger;

        private State _state;
        private VideoFile _videoFile;
        private bool _isSelected;
        private TimeSpan _elapsed;
        private DispatcherTimer _timer;
        private Stopwatch _stopWatch;
        private ProgressWrapper _progress;
        private Visibility _queuedVisible = Visibility.Collapsed;
        private Visibility _runningVisible = Visibility.Collapsed;
        private Visibility _successVisible = Visibility.Collapsed;
        private Visibility _failureVisible = Visibility.Collapsed;

        public State State
        {
            get => _state;
            private set
            {
                _state = value;

                QueuedVisible = _state == State.Queued ? Visibility.Visible : Visibility.Collapsed;
                RunningVisible = _state == State.Running ? Visibility.Visible : Visibility.Collapsed;
                SuccessVisible = _state == State.Success ? Visibility.Visible : Visibility.Collapsed;
                FailureVisible = _state == State.Failure ? Visibility.Visible : Visibility.Collapsed;
                
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
        
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                //if (_isSelected == value) return;
                _isSelected = value;
                
                RaisePropertyChangedEvent("IsSelected");
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

        public Visibility QueuedVisible
        {
            get => _queuedVisible;
            set
            {
                _queuedVisible = value;
                
                RaisePropertyChangedEvent("QueuedVisible");
            }
        }

        public Visibility RunningVisible
        {
            get => _runningVisible;
            set
            {
                _runningVisible = value;
                
                RaisePropertyChangedEvent("RunningVisible");
            }
        }

        public Visibility SuccessVisible
        {
            get => _successVisible;
            set
            {
                _successVisible = value;
                
                RaisePropertyChangedEvent("SuccessVisible");
            }
        }

        public Visibility FailureVisible
        {
            get => _failureVisible;
            set
            {
                _failureVisible = value;
                
                RaisePropertyChangedEvent("FailureVisible");
            }
        }

        public TaskProgressViewModel(VideoFile videoFile, VideoCollection videoCollection, int id)
        {
            _logger = LogManager.GetCurrentClassLogger();

            VideoFile = videoFile;
            VideoCollection = videoCollection;
            Id = id;
            State = State.Queued;
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
                return ProcessStandardTaskResponse(response);

            // if required, build the colour list
            if (!VideoCollection.Data.Colours.Any())
            {
                var buildColourListResponse = await Task.Run(() =>
                {
                    VideoCollection videoCollection = null;
                    
                    try
                    {
                        videoCollection = VideoProcessor.BuildColourListAsync(VideoCollection, progress);
                    }
                    catch (Exception e)
                    {
                        return (new TaskResponse { Success = false, Exception = e, Message = $"Unable to create output directory {VideoCollection.Config.FullOutputDirectory}" }, videoCollection);
                    }

                    return (new TaskResponse {Success = true}, videoCollection);
                });

                if (!buildColourListResponse.Item1.Success)
                {
                    StopTimer();

                    State = State.Failure;

                    Progress.Status = response.Message;

                    _logger.Error(response.Exception, response.Message);

                    return State;
                }

                VideoCollection = buildColourListResponse.Item2;

                // archive the frame images and then delete the directory
                response = await Task.Run(() =>
                {
                    try
                    {
                        ZipProcessor.ZipDirectoryAsync(VideoCollection.Config.ImageDirectory, progress);

                        Directory.Delete(VideoCollection.Config.ImageDirectory);
                    }
                    catch (Exception e)
                    {
                        return new TaskResponse { Success = false, Exception = e, Message = "Failed to zip frame images" };
                    }

                    return new TaskResponse { Success = true };
                });

                if (!response.Success)
                    return ProcessStandardTaskResponse(response);
            }

            // render the final image
            response = await Task.Run(() =>
            {
                try
                {
                    ImageProcessor.RenderImageAsync(VideoCollection, VideoFile, progress);
                }
                catch (Exception e)
                {
                    return new TaskResponse { Success = false, Exception = e, Message = $"Failed whilst rendering final image {VideoFile.OutputFilename}" };
                }

                return new TaskResponse { Success = true };
            });

            if (!response.Success)
                return ProcessStandardTaskResponse(response);

            // update the video collection and write as json
            await Task.Run(() =>
            {
                VideoCollection.VideoFiles.Add(VideoFile);

                VideoCollection.WriteAsync(progress);
            });

            StopTimer();

            State = State.Success;

            Progress.Status = "Complete!";

            RaisePropertyChangedEvent("Progress");
            RaisePropertyChangedEvent("State");

            return State;
        }

        private State ProcessStandardTaskResponse(TaskResponse response)
        {
            StopTimer();

            State = State.Failure;

            Progress.Status = response.Message;

            _logger.Error(response.Exception, response.Message);

            return State;
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