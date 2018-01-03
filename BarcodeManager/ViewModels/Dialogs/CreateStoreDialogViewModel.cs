using System.Windows.Input;
using CafePress.Api;
using FilmBarcodes.Common.Models.CafePress;
using FilmBarcodes.Common.Models.Settings;

namespace BarcodeManager.ViewModels.Dialogs
{
    public class CreateStoreDialogViewModel : ObservableObject
    {
        private StoreMethods _storeMethods;

        private SettingsWrapper _settings;
        private Store _store;

        public SettingsWrapper Settings
        {
            get => _settings;
            set
            {
                _settings = value;

                RaisePropertyChangedEvent("Settings");
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

        public ICommand CreateStoreCommand => new DelegateCommand(CreateStore);

        public CreateStoreDialogViewModel()
        {
            Settings = FilmBarcodes.Common.Settings.GetSettings();

            _storeMethods = new StoreMethods(Settings);
        }

        private void CreateStore()
        {
            _storeMethods.SaveStore(Store);
        }
    }
}
