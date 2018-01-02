using System;
using System.Linq;
using System.Windows.Input;
using CafePress.Api;
using FilmBarcodes.Common.Models.CafePress;
using FilmBarcodes.Common.Models.Settings;
using Microsoft.Win32;

namespace BarcodeManager.ViewModels
{
    public class CafePressDesignsViewModel : ObservableObject
    {
        private StoreMethods _storeMethods;
        private DesignMethods _designMethods;

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

        public CafePressDesignsViewModel()
        {
            Settings = FilmBarcodes.Common.Settings.GetSettings();

            _storeMethods = new StoreMethods(Settings);
            _designMethods = new DesignMethods(Settings);

            Stores = _storeMethods.ListStores();
        }

        private void CreateStore()
        {
            
        }
    }
}