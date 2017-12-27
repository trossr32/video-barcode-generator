using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using FilmBarcodes.Common;
using FilmBarcodes.Common.Enums;
using FilmBarcodes.Common.Models;
using FilmBarcodes.Common.Models.BarcodeManager;

namespace BarcodeManager.ViewModels
{
    public class TaskProgressViewModel : ObservableObject
    {
        public int Id { get; set; }

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
            set
            {
                _state = value;

                QueuedVisible = _state == State.Queued ? Visibility.Visible : Visibility.Collapsed;
                RunningVisible = _state == State.Running ? Visibility.Visible : Visibility.Collapsed;
                SuccessVisible = _state == State.Success ? Visibility.Visible : Visibility.Collapsed;
                FailureVisible = _state == State.Failure ? Visibility.Visible : Visibility.Collapsed;
                
                RaisePropertyChangedEvent("State");
            }
        }

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
            get { return _isSelected; }
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

        public TaskProgressViewModel()
        {
            
        }

        public TaskProgressViewModel(VideoFile videoFile, int id)
        {
            VideoFile = videoFile;
            Id = id;
            State = State.Queued;
        }

        public async Task<State> RunTask()
        {
            State = State.Running;

            StartTimer();

            var progress = new Progress<ProgressWrapper>(p =>
            {
                Progress = p;
                RaisePropertyChangedEvent("Progress");
            });

            await Task.Run(() =>
            {
                // check if the video output directory exists, otherwise create
                if (!Directory.Exists(VideoFile.FullOutputDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(VideoFile.FullOutputDirectory);
                    }
                    catch (Exception e)
                    {
                        var l = "Unable to create output directory";
                        throw;
                    }
                }
            });

            VideoFile = await Task.Run(() => VideoProcessor.BuildColourListAsync(VideoFile, progress));

            await Task.Run(() => ImageProcessor.RenderImageAsync(VideoFile, progress));

            await Task.Run(() => { VideoFile.WriteAsync(progress); });

            StopTimer();

            State = State.Success;

            Progress.Status = "Complete!";

            RaisePropertyChangedEvent("Progress");
            RaisePropertyChangedEvent("State");

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