using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using BarcodeManager.Models;
using FilmBarcodes.Common;
using FilmBarcodes.Common.Models.BarcodeManager;
using FilmBarcodes.Common.Models.Settings;
using GongSolutions.Wpf.DragDrop;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;
using NLog;

namespace BarcodeManager.ViewModels
{
    public class BarcodeCreatorViewModel : ObservableObject, IDropTarget
    {
        private readonly List<string> _acceptedVideoFiles = new List<string> { "avi", "divx", "m4v", "mkv", "m2ts", "mp4", "mpg", "wmv" };
        private readonly BarcodeCreatorModel _model = new BarcodeCreatorModel();
        private TasksViewModel _tasksViewModel;
        private static Logger _logger;

        private SettingsWrapper _settings;
        private VideoFile _videoFile;
        private VideoCollection _videoCollection;
        private bool _useExistingFrameImagesVisible;
        private bool _previousBarcodesVisible;

        public SettingsWrapper Settings 
        {
            get => _settings;
            set
            {
                _settings = value;

                RaisePropertyChangedEvent("Settings");
            }
        }

        public VideoFile VideoFile
        {
            get => _videoFile;
            set
            {
                _videoFile = value;
                
                RaisePropertyChangedEvent("VideoFile");
            }
        }

        public VideoCollection VideoCollection
        {
            get => _videoCollection;
            set
            {
                _videoCollection = value;

                //VisibleIfVideoFileValid = _videoFile.IsValid ? Visibility.Visible : Visibility.Collapsed;

                RaisePropertyChangedEvent("VideoCollection");
            }
        }

        public bool UseExistingFrameImagesVisible
        {
            get => _useExistingFrameImagesVisible;
            set
            {
                _useExistingFrameImagesVisible = value;

                RaisePropertyChangedEvent("UseExistingFrameImagesVisible");
            }
        }

        public bool PreviousBarcodesVisible
        {
            get => _previousBarcodesVisible;
            set
            {
                _previousBarcodesVisible = value;

                RaisePropertyChangedEvent("PreviousBarcodesVisible");
            }
        }

        public ICommand ImportFileCommand => new DelegateCommand(ImportFile);
        public ICommand PlayVideoCommand => new DelegateCommand(PlayVideo);
        public ICommand SetWidthToDurationCommand => new DelegateCommand(SetWidthToDuration);
        public ICommand SetWidthToCanvasRatioCommand => new DelegateCommand(SetWidthToCanvasRatio);
        public ICommand CreateBarcodeCommand => new DelegateCommand(CreateBarcode);
        public ICommand ChooseSettingsOutputDirectoryCommand => new DelegateCommand(ChooseSettingsOutputDirectory);

        public BarcodeCreatorViewModel(TasksViewModel tasksViewModel, SettingsWrapper settings)
        {
            Settings = settings;

            _logger = LogManager.GetCurrentClassLogger();

            _tasksViewModel = tasksViewModel;
        }

        private void ImportFile()
        {
            string videoFilePattern = _acceptedVideoFiles.Aggregate("", (current, file) => $"{current}*.{file};").TrimEnd(';');

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = $"Video files ({videoFilePattern})|{videoFilePattern}|All files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

            if (openFileDialog.ShowDialog() == true)
                BuildVideoCollectionAndVideoFile(openFileDialog.FileNames.First());
        }

        private void BuildVideoCollectionAndVideoFile(string file)
        {
            var videoCollectionFile = Path.Combine(Settings.BarcodeManager.OutputDirectory, Path.GetFileNameWithoutExtension(file), "videocollection.json");

            if (File.Exists(videoCollectionFile))
            {
                try
                {
                    VideoCollection = JsonConvert.DeserializeObject<VideoCollection>(File.ReadAllText(videoCollectionFile));
                }
                catch (Exception)
                {
                    // on failure we want to continue as the video collection file format may have changed
                }
            }

            if (VideoCollection == null)
                VideoCollection = new VideoCollection(file, Settings.BarcodeManager);

            VideoFile = new VideoFile(VideoCollection.Config.Duration, file);

            PreviousBarcodesVisible = VideoCollection.VideoFiles.Any();

            if (Directory.Exists(VideoCollection.Config.ImageDirectory))
            {
                var fileCount = Directory.GetFiles(VideoCollection.Config.ImageDirectory).Length;

                // frame image count tends to be 1 less than duration seconds (rounding or for index error?)
                UseExistingFrameImagesVisible = fileCount >= VideoCollection.Config.Duration - 1 &&
                                                fileCount <= VideoCollection.Config.Duration;

                VideoFile.UseExistingFrameImages = true;

                RaisePropertyChangedEvent("VideoFile");
            }
            else
                UseExistingFrameImagesVisible = false;
        }

        private void SetWidthToDuration()
        {
            VideoFile.OutputWidth = VideoCollection.Config.Duration;

            RaisePropertyChangedEvent("VideoFile");
        }

        private void SetWidthToCanvasRatio()
        {
            VideoFile.OutputWidth = VideoFile.OutputWidthAsCanvasRatio;

            RaisePropertyChangedEvent("VideoFile");
        }

        private void PlayVideo()
        {
            System.Diagnostics.Process.Start(VideoCollection.Config.FullPath);
        }

        public void DragOver(IDropInfo dropInfo)
        {
            var dragFileList = ((DataObject)dropInfo.Data).GetFileDropList().Cast<string>();

            dropInfo.Effects = dragFileList.Any(item =>
            {
                var extension = Path.GetExtension(item);
                return extension != null && _acceptedVideoFiles.Contains(extension.TrimStart('.'));
            })
                ? DragDropEffects.Copy
                : DragDropEffects.None;
        }

        public void Drop(IDropInfo dropInfo)
        {
            var dragFileList = ((DataObject)dropInfo.Data).GetFileDropList().Cast<string>();

            BuildVideoCollectionAndVideoFile(dragFileList.First());

            //VideoFile = _model.ProcessNewFile(dragFileList.First());
        }

        private void CreateBarcode()
        {
            // check if the settings directory has changed (been manually typed)
            var tempSettings = FilmBarcodes.Common.Settings.GetSettings();

            if (tempSettings.BarcodeManager.OutputDirectory != Settings.BarcodeManager.OutputDirectory)
                FilmBarcodes.Common.Settings.SetSettings(Settings);
            
            VideoCollection.Config.FullOutputDirectory = Path.Combine(Settings.BarcodeManager.OutputDirectory, VideoCollection.Config.OutputDirectory);

            VideoFile.FullOutputFile = Path.Combine(VideoCollection.Config.FullOutputDirectory, VideoFile.OutputFilename);

            _tasksViewModel.AddTask(VideoFile, VideoCollection);

            VideoCollection = null;
            VideoFile = null;
            UseExistingFrameImagesVisible = false;
        }
        
        private void ChooseSettingsOutputDirectory()
        {
            var commonOpenFileDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                InitialDirectory = Settings.BarcodeManager.OutputDirectory
            };

            if (commonOpenFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                if (Settings.BarcodeManager.OutputDirectory != commonOpenFileDialog.FileName)
                {
                    Settings.BarcodeManager.OutputDirectory = commonOpenFileDialog.FileName;

                    FilmBarcodes.Common.Settings.SetSettings(Settings);

                    RaisePropertyChangedEvent("Settings");
                }
            }
        }
    }
}