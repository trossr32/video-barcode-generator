using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using NLog;
using VideoBarcodeGenerator.Core;
using VideoBarcodeGenerator.Core.Models.BarcodeGenerator;
using VideoBarcodeGenerator.Core.Models.Settings;

namespace VideoBarcodeGenerator.Wpf.ViewModels
{
    public class VideoLibraryViewModel : ObservableObject
    {
        private readonly Logger _logger;
        private readonly SettingsWrapper _settings;
        private List<VideoCollection> _processed;
        private ObservableCollection<VideoLibraryItem> _items;
        private bool _taskListVisible;

        public ObservableCollection<VideoLibraryItem> Items
        {
            get => _items;
            set
            {
                _items = value;
                RaisePropertyChangedEvent("Items");
            }
        }

        public bool TaskListVisible
        {
            get => _taskListVisible;
            set
            {
                _taskListVisible = value;
                RaisePropertyChangedEvent("TaskListVisible");
            }
        }

        public VideoLibraryViewModel(SettingsWrapper settings)
        {
            _logger = LogManager.GetCurrentClassLogger();

            _settings = settings;
        }

        public async void Update()
        {
            TaskListVisible = false;

            _items = new ObservableCollection<VideoLibraryItem>();

            _processed = new List<VideoCollection>();

            Init()
                .ContinueWith(t => Application.Current.Dispatcher.Invoke(BuildGrid));
        }

        private async Task Init()
        {
            await Task.Run(() =>
            {
                if (!Directory.Exists(_settings.CoreSettings.OutputDirectory))
                    return;

                _processed.AddRange(Directory.GetDirectories(_settings.CoreSettings.OutputDirectory)
                    .Where(d => File.Exists(Path.Combine(d, "videocollection.json")))
                    .Select(d => JsonConvert.DeserializeObject<VideoCollection>(File.ReadAllText(Path.Combine(d, "videocollection.json")))));
            });
        }

        private void BuildGrid()
        {
            _processed
                .Where(p => p.Config?.IsValid ?? false)
                .ToList()
                .ForEach(videoCollection => Items.Add(new VideoLibraryItem(videoCollection)));

            RaisePropertyChangedEvent("Items");

            TaskListVisible = true;
        }
    }
}