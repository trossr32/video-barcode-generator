using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using BarcodeManager.ViewModels.Dialogs;
using BarcodeManager.Views.Dialogs;
using CafePress.Api;
using FilmBarcodes.Common;
using FilmBarcodes.Common.Models.BarcodeManager;
using FilmBarcodes.Common.Models.CafePress;
using FilmBarcodes.Common.Models.Settings;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;

namespace BarcodeManager.ViewModels
{
    public class CafePressDesignsViewModel : ObservableObject
    {
        private StoreMethods _storeMethods;
        private DesignMethods _designMethods;
        private List<VideoFile> _localVideoFiles;

        private SettingsWrapper _settings;
        private Stores _stores;
        private Store _store;
        private string _storeId;
        private Designs _designs;
        //private Visibility _visibleIfVideoFileValid = Visibility.Collapsed;

        public SettingsWrapper Settings 
        {
            get => _settings;
            set
            {
                _settings = value;

                RaisePropertyChangedEvent("Settings");
            }
        }

        public Stores Stores
        {
            get => _stores;
            set
            {
                _stores = value;

                //VisibleIfVideoFileValid = _videoFile.IsValid ? Visibility.Visible : Visibility.Collapsed;

                RaisePropertyChangedEvent("Stores");
            }
        }

        public Store Store
        {
            get => _store;
            set
            {
                _store = value;

                RaisePropertyChangedEvent("Store");
            }
        }

        public string StoreId
        {
            get => _storeId;
            set
            {
                _storeId = value;

                // select store
                Store = Stores.StoreList.First(s => s.Id == _storeId);

                Designs = _designMethods.List();

                RaisePropertyChangedEvent("StoreId");
            }
        }

        public Designs Designs
        {
            get => _designs;
            set
            {
                _designs = value;

                RaisePropertyChangedEvent("Designs");
            }
        }

        public List<VideoFile> LocalVideoFiles
        {
            get => _localVideoFiles;
            set
            {
                _localVideoFiles = value;

                RaisePropertyChangedEvent("LocalVideoFiles");
            }
        }

        //public Visibility VisibleIfVideoFileValid
        //{
        //    get => _visibleIfVideoFileValid;
        //    set
        //    {
        //        _visibleIfVideoFileValid = value;
        //        RaisePropertyChangedEvent("VisibleIfVideoFileValid");
        //    }
        //}

        public ICommand CreateStoreCommand => new DelegateCommand(CreateStore);

        public CafePressDesignsViewModel(SettingsWrapper settings)
        {
            Settings = settings;

            _storeMethods = new StoreMethods(Settings);
            _designMethods = new DesignMethods(Settings);

            Stores = _storeMethods.ListStores();

            //RaisePropertyChangedEvent("Stores");

            Designs = _designMethods.List();

            GetLocalVideoFiles();
        }

        private void GetLocalVideoFiles()
        {
            LocalVideoFiles = new List<VideoFile>();

            foreach (var directory in Directory.GetDirectories(Settings.BarcodeManager.OutputDirectory))
            {
                var collectionFile = Path.Combine(directory, "videocollection.json");

                if (!File.Exists(collectionFile))
                    continue;

                LocalVideoFiles.AddRange(JsonConvert.DeserializeObject<VideoCollection>(File.ReadAllText(collectionFile)).VideoFiles);
            }
        }

        private async void CreateStore()
        {
            var view = new CreateStoreDialog
            {
                DataContext = new CreateStoreDialogViewModel()
            };
            
            var result = await DialogHost.Show(view, "RootDialog", CreateStoreOpenedEventHandler, CreateStoreClosingEventHandler);
        }

        private void CreateStoreOpenedEventHandler(object sender, DialogOpenedEventArgs eventargs)
        {
            //Console.WriteLine("You could intercept the open and affect the dialog using eventArgs.Session.");
        }

        private void CreateStoreClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == false)
                return;

            Stores = _storeMethods.ListStores();

            //OK, lets cancel the close...
            //eventArgs.Cancel();

            //...now, lets update the "session" with some new content!
            //eventArgs.Session.UpdateContent(new SampleProgressDialog());
            //note, you can also grab the session when the dialog opens via the DialogOpenedEventHandler

            //lets run a fake operation for 3 seconds then close this baby.
            //Task.Delay(TimeSpan.FromSeconds(3))
            //    .ContinueWith((t, _) => eventArgs.Session.Close(false), null,
            //        TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}