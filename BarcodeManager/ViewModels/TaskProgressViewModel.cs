﻿using System;
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

        public VideoCollection VideoCollection { get; set; }

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

        public TaskProgressViewModel(VideoFile videoFile, VideoCollection videoCollection, int id)
        {
            VideoFile = videoFile;
            VideoCollection = videoCollection;
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
                if (!Directory.Exists(VideoCollection.Config.FullOutputDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(VideoCollection.Config.FullOutputDirectory);
                    }
                    catch (Exception e)
                    {
                        var l = "Unable to create output directory";
                        throw;
                    }
                }
            });

            if (!VideoCollection.Data.Colours.Any())
                VideoCollection = await Task.Run(() => VideoProcessor.BuildColourListAsync(VideoCollection, progress));

            await Task.Run(() => ImageProcessor.RenderImageAsync(VideoCollection, VideoFile, progress));

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