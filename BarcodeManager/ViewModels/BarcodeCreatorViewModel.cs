using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BarcodeManager.Models;
using FilmBarcodes.Common;
using FilmBarcodes.Common.Enums;
using FilmBarcodes.Common.Models.BarcodeManager;
using FilmBarcodes.Common.Models.CafePress;
using GongSolutions.Wpf.DragDrop;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace BarcodeManager.ViewModels
{
    public class BarcodeCreatorViewModel : ObservableObject, IDropTarget
    {
        private readonly List<string> _acceptedVideoFiles = new List<string> { "avi", "divx", "m4v", "mkv", "m2ts", "mp4", "mpg", "wmv" };
        private readonly BarcodeCreatorModel _model = new BarcodeCreatorModel();
        private TasksViewModel _tasksViewModel;
        //private MainWindow _mainWindow;

        private SettingsWrapper _settings;
        private VideoFile _videoFile;
        private Visibility _visibleIfVideoFileValid = Visibility.Collapsed;

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

                VisibleIfVideoFileValid = _videoFile.IsValid ? Visibility.Visible : Visibility.Collapsed;

                RaisePropertyChangedEvent("VideoFile");
            }
        }

        public Visibility VisibleIfVideoFileValid
        {
            get => _visibleIfVideoFileValid;
            set
            {
                _visibleIfVideoFileValid = value;
                RaisePropertyChangedEvent("VisibleIfVideoFileValid");
            }
        }

        public ICommand ImportFileCommand => new DelegateCommand(ImportFile);
        public ICommand PlayVideoCommand => new DelegateCommand(PlayVideo);
        public ICommand SetWidthToDurationCommand => new DelegateCommand(SetWidthToDuration);
        public ICommand SetWidthToCanvasRatioCommand => new DelegateCommand(SetWidthToCanvasRatio);
        public ICommand CreateBarcodeCommand => new DelegateCommand(CreateBarcode);
        public ICommand ChooseSettingsOutputDirectoryCommand => new DelegateCommand(ChooseSettingsOutputDirectory);

        public BarcodeCreatorViewModel(TasksViewModel tasksViewModel)
        {
            Settings = FilmBarcodes.Common.Settings.GetSettings();

            //_mainWindow = mainWindow;

            _tasksViewModel = tasksViewModel;
        }

        private void ImportFile()
        {
            string videoFilePattern = _acceptedVideoFiles.Aggregate("", (current, file) => current + $"*.{file};").TrimEnd(';');

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = $"Video files ({videoFilePattern})|{videoFilePattern}|All files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

            if (openFileDialog.ShowDialog() == true)
            {
                //foreach (string filename in openFileDialog.FileNames)
                //FileTextBox.Text = Path.GetFileName(filename);

                VideoFile = _model.ProcessNewFile(openFileDialog.FileNames.First());
            }
        }

        private void SetWidthToDuration()
        {
            VideoFile.OutputWidth = VideoFile.Duration;

            RaisePropertyChangedEvent("VideoFile");
        }

        private void SetWidthToCanvasRatio()
        {
            VideoFile.OutputWidth = VideoFile.OutputWidthAsCanvasRatio;

            RaisePropertyChangedEvent("VideoFile");
        }

        private void PlayVideo()
        {
            System.Diagnostics.Process.Start(VideoFile.FullPath);
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

            VideoFile = _model.ProcessNewFile(dragFileList.First());
        }

        private void CreateBarcode()
        {
            // check if the settings directory has changed (been manually typed)
            if (!Directory.Exists(Settings.BarcodeManager.OutputDirectory))
            {
                var tempSettings = FilmBarcodes.Common.Settings.GetSettings();

                if (tempSettings.BarcodeManager.OutputDirectory != Settings.BarcodeManager.OutputDirectory)
                    FilmBarcodes.Common.Settings.SetSettings(Settings);
            }

            VideoFile.FullOutputDirectory = Path.Combine(Settings.BarcodeManager.OutputDirectory, VideoFile.OutputDirectory);

            _tasksViewModel.AddTask(VideoFile);

            //_mainWindow.SetTab(TabType.Tasks);
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