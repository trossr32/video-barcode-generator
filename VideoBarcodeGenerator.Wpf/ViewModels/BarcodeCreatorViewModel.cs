using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GongSolutions.Wpf.DragDrop;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;
using NLog;
using VideoBarcodeGenerator.Core;
using VideoBarcodeGenerator.Core.Helpers;
using VideoBarcodeGenerator.Core.Models.BarcodeGenerator;
using VideoBarcodeGenerator.Core.Models.Settings;

namespace VideoBarcodeGenerator.Wpf.ViewModels
{
    public class BarcodeCreatorViewModel : ObservableObject, IDropTarget
    {
        private readonly List<string> _acceptedVideoFiles = new List<string> { "avi", "divx", "m4v", "mkv", "m2ts", "mp4", "mpg", "wmv" };
        private readonly TasksViewModel _tasksViewModel;
        private static Logger _logger;

        private SettingsWrapper _settings;
        private BarcodeConfig _barcodeConfig;
        private VideoCollection _videoCollection;
        private bool _useExistingFrameImagesVisible;
        private bool _previousBarcodesVisible;
        private bool _videoSettingsVisible;

        public SettingsWrapper Settings 
        {
            get => _settings;
            set
            {
                _settings = value;

                RaisePropertyChangedEvent("Settings");
            }
        }

        public BarcodeConfig BarcodeConfig
        {
            get => _barcodeConfig;
            set
            {
                _barcodeConfig = value;
                
                RaisePropertyChangedEvent("BarcodeConfig");
            }
        }

        public VideoCollection VideoCollection
        {
            get => _videoCollection;
            set
            {
                _videoCollection = value;

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

        public bool VideoSettingsVisible
        {
            get => _videoSettingsVisible;
            set
            {
                _videoSettingsVisible = value;

                RaisePropertyChangedEvent("VideoSettingsVisible");
            }
        }

        public ICommand ImportFileCommand => new DelegateCommand(ImportFile);
        public ICommand PlayVideoCommand => new DelegateCommand(PlayVideo);
        public ICommand SetWidthToDurationCommand => new DelegateCommand(SetWidthToDuration);
        public ICommand SetWidthToCanvasRatio5Command => new DelegateCommand(() => SetWidthToCanvasRatio(5));
        public ICommand SetWidthToCanvasRatio6Command => new DelegateCommand(() => SetWidthToCanvasRatio(6));
        public ICommand SetWidthToCanvasRatio7Command => new DelegateCommand(() => SetWidthToCanvasRatio(7));
        public ICommand SetWidthToCanvasRatio8Command => new DelegateCommand(() => SetWidthToCanvasRatio(8));
        public ICommand SetWidthToCanvasRatio9Command => new DelegateCommand(() => SetWidthToCanvasRatio(9));
        public ICommand SetWidthToCanvasRatio10Command => new DelegateCommand(() => SetWidthToCanvasRatio(10));
        public ICommand CreateBarcodeCommand => new DelegateCommand(CreateBarcode);
        public ICommand ChooseSettingsOutputDirectoryCommand => new DelegateCommand(ChooseSettingsOutputDirectory);
        public ICommand OpenImageDirectoryCommand => new DelegateCommand(OpenImageDirectory);

        public BarcodeCreatorViewModel(TasksViewModel tasksViewModel, SettingsWrapper settings)
        {
            Settings = settings;

            _logger = LogManager.GetCurrentClassLogger();

            _tasksViewModel = tasksViewModel;

            Cache.ClearMagickImageCache(Settings, _logger);

            //List<string> list = new List<string>();

            //foreach (var directory in Directory.GetDirectories(_Settings.CoreSettings.OutputDirectory))
            //{
            //    string file = Path.Combine(directory, "videocollection.json");

            //    if (!File.Exists(file))
            //        continue;

            //    VideoCollection videoCollection = JsonConvert.DeserializeObject<VideoCollection>(File.ReadAllText(file));

            //    if (!File.Exists(videoCollection.BarcodeConfigs.First().Barcode_1px.FullOutputFile))
            //        list.Add(videoCollection.Config.FullPath);
            //}

            //ProcessMultipleFiles(list.ToArray());
        }

        /// <summary>
        /// Show a file select dialog when the import file(s) button is pressed
        /// </summary>
        private void ImportFile()
        {
            string videoFilePattern = _acceptedVideoFiles.Aggregate("", (current, file) => $"{current}*.{file};").TrimEnd(';');

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = $"Video files ({videoFilePattern})|{videoFilePattern}|All files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

            if (openFileDialog.ShowDialog() ?? false)
            {
                if (openFileDialog.FileNames.Length == 1)
                    BuildVideoCollectionAndVideoFile(openFileDialog.FileNames.First());
                //else
                //    ProcessMultipleFiles(openFileDialog.FileNames);
            }
        }

        /// <summary>
        /// Not currently used
        /// </summary>
        /// <param name="files"></param>
        private void ProcessMultipleFiles(string[] files)
        {
            // display 'defaults will be used for all files'
            // display file list?

            foreach (var file in files)
            {
                var videoCollection = CreateVideoCollection(file);

                videoCollection.Config.FullOutputDirectory = Path.Combine(Settings.CoreSettings.OutputDirectory, videoCollection.Config.OutputDirectory);

                var videoFile = new BarcodeConfig(videoCollection.Config.Duration, videoCollection.Config.FilenameWithoutExtension);

                videoFile.SetOutputImagesFullDirectory(videoCollection);

                if (Directory.Exists(videoCollection.Config.ImageDirectory))
                {
                    var fileCount = Directory.GetFiles(videoCollection.Config.ImageDirectory).Length;

                    videoFile.UseExistingFrameImages = fileCount >= videoCollection.Config.Duration - 1 && fileCount <= videoCollection.Config.Duration;
                }

                _tasksViewModel.AddTask(videoFile, videoCollection);
            }
        }

        /// <summary>
        /// Create a video collection class from the supplied file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private void BuildVideoCollectionAndVideoFile(string file)
        {
            VideoCollection = CreateVideoCollection(file);

            VideoSettingsVisible = VideoCollection?.Config?.IsValid ?? false;
            RaisePropertyChangedEvent("VideoSettingsVisible");

            BarcodeConfig = new BarcodeConfig(VideoCollection.Config.Duration, VideoCollection.Config.FilenameWithoutExtension);

            PreviousBarcodesVisible = VideoCollection.BarcodeConfigs.Any();

            if (Directory.Exists(VideoCollection.Config.ImageDirectory))
            {
                var fileCount = Directory.GetFiles(VideoCollection.Config.ImageDirectory).Length;

                // frame image count tends to be 1 less than duration seconds (rounding or for index error?)
                UseExistingFrameImagesVisible = fileCount >= VideoCollection.Config.Duration - 1 && fileCount <= VideoCollection.Config.Duration;

                BarcodeConfig.UseExistingFrameImages = true;
            }
            else
                UseExistingFrameImagesVisible = false;

            RaisePropertyChangedEvent("BarcodeConfig");
        }

        /// <summary>
        /// Create a video collection class from the supplied file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private VideoCollection CreateVideoCollection(string file)
        {
            VideoCollection videoCollection = null;
            var videoCollectionFile = Path.Combine(Settings.CoreSettings.OutputDirectory, Path.GetFileNameWithoutExtension(file), "videocollection.json");

            if (!File.Exists(videoCollectionFile))
                return new VideoCollection(file, Settings.CoreSettings);

            try
            {
                videoCollection = JsonConvert.DeserializeObject<VideoCollection>(File.ReadAllText(videoCollectionFile));
            }
            catch (Exception)
            {
                // on failure we want to continue as the video collection file format may have changed
            }

            return videoCollection ?? new VideoCollection(file, Settings.CoreSettings);
        }

        /// <summary>
        /// Set the width of the image to 1 pixel for every second of the videos duration
        /// </summary>
        private void SetWidthToDuration()
        {
            BarcodeConfig.OutputWidth = VideoCollection.Config.Duration;

            RaisePropertyChangedEvent("BarcodeConfig");
        }

        /// <summary>
        /// Set the width of the image as a ratio of the height
        /// </summary>
        /// <param name="ratio"></param>
        private void SetWidthToCanvasRatio(int ratio)
        {
            BarcodeConfig.OutputRatio = ratio;
            BarcodeConfig.OutputWidth = BarcodeConfig.OutputWidthAsCanvasRatio;

            RaisePropertyChangedEvent("BarcodeConfig");
        }

        /// <summary>
        /// Once a file has been imported show a button to allow Windows to play the file in the
        /// system default video player
        /// </summary>
        private void PlayVideo()
        {
            Process.Start(VideoCollection.Config.FullPath);
        }

        /// <summary>
        /// Event fired when a file or collection of files is dragged over the app
        /// </summary>
        /// <param name="dropInfo"></param>
        public void DragOver(IDropInfo dropInfo)
        {
            List<string> dragFileList = ((DataObject)dropInfo.Data).GetFileDropList().Cast<string>().ToList();

            if (dragFileList.Count() > 1)
            {
                dropInfo.Effects = DragDropEffects.None;

                return;
            }

            dropInfo.Effects = dragFileList.Any(item =>
            {
                var extension = Path.GetExtension(item);
                return extension != null && _acceptedVideoFiles.Contains(extension.TrimStart('.'));
            })
                ? DragDropEffects.Copy
                : DragDropEffects.None;
        }

        /// <summary>
        /// Event fired when a file or collection of files is dropped over the app
        /// </summary>
        /// <param name="dropInfo"></param>
        public void Drop(IDropInfo dropInfo)
        {
            string[] dragFileList = ((DataObject)dropInfo.Data).GetFileDropList().Cast<string>().ToArray();

            if (dragFileList.Length == 1)
                BuildVideoCollectionAndVideoFile(dragFileList.First());
            //else
            //    ProcessMultipleFiles(dragFileList);

            // reset the dropInfo to remove previously dropped files
            dropInfo = null;
        }

        /// <summary>
        /// Adds the current video as a creation task
        /// </summary>
        private void CreateBarcode()
        {
            // Validation!

            // check if the settings directory has changed (been manually typed)
            var tempSettings = Core.Settings.GetSettings();

            if (tempSettings.CoreSettings.OutputDirectory != Settings.CoreSettings.OutputDirectory)
                Core.Settings.SetSettings(Settings);

            VideoCollection.Config.FullOutputDirectory = Path.Combine(Settings.CoreSettings.OutputDirectory, VideoCollection.Config.OutputDirectory);

            BarcodeConfig.SetOutputImagesFullDirectory(VideoCollection);

            _tasksViewModel.AddTask(BarcodeConfig, VideoCollection);

            VideoCollection = null;
            BarcodeConfig = null;
            UseExistingFrameImagesVisible = false;
            VideoSettingsVisible = false;

            RaisePropertyChangedEvent("VideoCollection");
            RaisePropertyChangedEvent("BarcodeConfig");
            RaisePropertyChangedEvent("VideoSettingsVisible");
            RaisePropertyChangedEvent("UseExistingFrameImagesVisible"); 
        }
        
        /// <summary>
        /// Open a folder browser dialog to select a new video output directory
        /// </summary>
        private void ChooseSettingsOutputDirectory()
        {
            var commonOpenFileDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                InitialDirectory = Settings.CoreSettings.OutputDirectory
            };

            if (commonOpenFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                if (Settings.CoreSettings.OutputDirectory != commonOpenFileDialog.FileName)
                {
                    Settings.CoreSettings.OutputDirectory = commonOpenFileDialog.FileName;

                    Core.Settings.SetSettings(Settings);

                    RaisePropertyChangedEvent("Settings");
                }
            }
        }

        /// <summary>
        /// If previously generated barcode images are found then this can be called to open
        /// Windows Explorer at the containing directory
        /// </summary>
        private void OpenImageDirectory()
        {
            Process.Start(_videoCollection.Config.FullOutputDirectory);
        }
    }
}