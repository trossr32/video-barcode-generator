using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using FilmBarcodes.Common;
using FilmBarcodes.Common.Models.BarcodeManager;
using FilmBarcodes.Common.Models.Settings;
using Newtonsoft.Json;
using NLog;

namespace BarcodeManager.ViewModels
{
    public class VideoLibraryViewModel : ObservableObject
    {
        private readonly Logger _logger;
        private readonly SettingsWrapper _settings;
        private ObservableCollection<VideoLibraryItem> _items;
        private List<VideoCollection> _processed;
        private List<string> _source;
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

            _items = new ObservableCollection<VideoLibraryItem>();

            _processed = new List<VideoCollection>();

            _source = new List<string>();

            //Init();

            //BuildGrid();
        }

        private void Init()
        {
            foreach (var directory in Directory.GetDirectories(_settings.BarcodeManager.OutputDirectory))
            {
                var file = Path.Combine(directory, "videocollection.json");

                if (File.Exists(file))
                {
                    _processed.Add(JsonConvert.DeserializeObject<VideoCollection>(File.ReadAllText(file)));
                }
            }

            _source = Directory.GetFiles(_settings.BarcodeManager.DefaultSource).ToList();
        }

        private void BuildGrid()
        {
            foreach (var videoCollection in _processed)
            {
                var source = _source.FirstOrDefault(f => f.Split('\\').Last() == videoCollection.Config.FileName);

                if (!string.IsNullOrEmpty(source))
                {
                    Items.Add(new VideoLibraryItem (videoCollection, source));
                }
            }
        }
    }
}