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
using FilmBarcodes.Common.Models.Settings;
using FilmBarcodes.Common.Processors;
using NLog;

namespace BarcodeManager.ViewModels
{
    public class TaskProgressViewModel : ObservableObject
    {
        public int Id { get; }

        private readonly TasksViewModel _parent;
        private readonly Logger _logger;
        private readonly SettingsWrapper _settings;
        private readonly CancellationTokenSource _cancellationTokenSource;

        private State _state;
        private VideoCollection _videoCollection;
        private BarcodeConfig _barcodeConfig;
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

                TaskProgressVisibility.ProgressVisible = _state == State.Running;

                RaisePropertyChangedEvent("State");
            }
        }

        public VideoCollection VideoCollection
        {
            get => _videoCollection;
            set
            {
                //if (_barcodeConfig == value) return;
                _videoCollection = value;

                RaisePropertyChangedEvent("VideoCollection");
            }
        }

        public BarcodeConfig BarcodeConfig
        {
            get => _barcodeConfig;
            set
            {
                //if (_barcodeConfig == value) return;
                _barcodeConfig = value;
                
                RaisePropertyChangedEvent("BarcodeConfig");
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

        public TaskProgressViewModel(TasksViewModel parent, BarcodeConfig barcodeConfig, VideoCollection videoCollection, SettingsWrapper settings, int id)
        {
            _parent = parent;
            _logger = LogManager.GetCurrentClassLogger();
            _settings = settings;

            BarcodeConfig = barcodeConfig;
            VideoCollection = videoCollection;
            Id = id;
            State = State.Queued;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public async Task<State> RunTask()
        {
            _logger.Info($"Running task id:{Id}, {VideoCollection.Config.FilenameWithoutExtension}");

            State = State.Running;

            RaiseTaskResponsePropertyChangedEvents();

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
            if (VideoCollection.Data.Colours.Count != VideoCollection.Config.Duration)
            {
                var buildColourListResponse = await Task.Run(() =>
                {
                    VideoCollection videoCollection = null;

                    try
                    {
                        videoCollection = VideoProcessor.BuildColourListAsync(VideoCollection, BarcodeConfig, _settings, progress, _cancellationTokenSource.Token);
                    }
                    catch (OperationCanceledException)
                    {
                        return (new TaskResponse { Success = false, Cancelled = true, Message = $"Cancelled task id:{Id}, {VideoCollection.Config.FilenameWithoutExtension}" }, videoCollection);
                    }
                    catch (Exception e)
                    {
                        return (new TaskResponse { Success = false, Exception = e, Message = e.Message }, videoCollection);
                    }

                    return (new TaskResponse {Success = true}, videoCollection);
                });

                if (!buildColourListResponse.Item1.Success)
                    return ProcessUnsuccessfulTaskResponse(buildColourListResponse.Item1);

                VideoCollection = buildColourListResponse.videoCollection;
            }

            // if required, archive the frame images and then delete the directory
            if (!File.Exists(VideoCollection.Config.ZipFile) && BarcodeConfig.CreateZipAndDeleteFrameImages)
            {
                response = await Task.Run(() =>
                {
                    try
                    {
                        ZipProcessor.ZipDirectoryAsync(VideoCollection.Config, progress, _cancellationTokenSource.Token);

                        Directory.Delete(VideoCollection.Config.ImageDirectory, true);
                    }
                    catch (OperationCanceledException)
                    {
                        return new TaskResponse { Success = false, Cancelled = true, Message = $"Cancelled task id:{Id}, {VideoCollection.Config.FilenameWithoutExtension}" };
                    }
                    catch (Exception e)
                    {
                        return new TaskResponse { Success = false, Exception = e, Message = "Failed to zip frame images" };
                    }

                    return new TaskResponse {Success = true};
                });

                if (!response.Success)
                    return ProcessUnsuccessfulTaskResponse(response);
            }

            // render the final image
            response = await Task.Run(() =>
            {
                try
                {
                    ImageProcessor.RenderImageAsync(VideoCollection, BarcodeConfig, progress, _cancellationTokenSource.Token);
                }
                catch (OperationCanceledException)
                {
                    return new TaskResponse { Success = false, Cancelled = true, Message = $"Cancelled task id:{Id}, {VideoCollection.Config.FilenameWithoutExtension}" };
                }
                catch (Exception e)
                {
                    return new TaskResponse { Success = false, Exception = e, Message = $"Failed whilst rendering final image {BarcodeConfig.Barcode_Standard.FullOutputFile}" };
                }

                return new TaskResponse { Success = true };
            });

            if (!response.Success)
                return ProcessUnsuccessfulTaskResponse(response);

            // if required, render the compressed one pixel image
            if (BarcodeConfig.CreateOnePixelBarcode)
            { 
                response = await Task.Run(() =>
                {
                    try
                    {
                        ImageProcessor.BuildAndRenderImageCompressedToOnePixelWideImageAsync(VideoCollection, BarcodeConfig, _settings, progress, _cancellationTokenSource.Token);
                    }
                    catch (OperationCanceledException)
                    {
                        return new TaskResponse { Success = false, Cancelled = true, Message = $"Cancelled task id:{Id}, {VideoCollection.Config.FilenameWithoutExtension}" };
                    }
                    catch (Exception e)
                    {
                        return new TaskResponse { Success = false, Exception = e, Message = $"Failed whilst rendering final image {BarcodeConfig.Barcode_1px.FullOutputFile}" };
                    }

                    return new TaskResponse { Success = true };
                });

                if (!response.Success)
                    return ProcessUnsuccessfulTaskResponse(response);
            }

            // update the video collection and write as json
            await Task.Run(() =>
            {
                if (VideoCollection.BarcodeConfigs.Any(v => v.OutputWidth == BarcodeConfig.OutputWidth && v.OutputHeight == BarcodeConfig.OutputHeight))
                    VideoCollection.BarcodeConfigs.Remove(VideoCollection.BarcodeConfigs.First(v => v.OutputWidth == BarcodeConfig.OutputWidth && v.OutputHeight == BarcodeConfig.OutputHeight));

                VideoCollection.BarcodeConfigs.Add(BarcodeConfig);

                VideoCollection.WriteAsync(progress);
            });

            StopTimer();

            State = State.Success;

            Progress.Status = $"Completed in {Elapsed:hh\\:mm\\:ss}";

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