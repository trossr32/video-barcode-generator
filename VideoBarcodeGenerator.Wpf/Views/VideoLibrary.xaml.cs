using System.Windows.Controls;
using VideoBarcodeGenerator.Core.Models.Settings;
using VideoBarcodeGenerator.Wpf.ViewModels;

namespace VideoBarcodeGenerator.Wpf.Views
{
    /// <summary>
    /// Interaction logic for VideoLibrary.xaml
    /// </summary>
    public partial class VideoLibrary : UserControl
    {
        private VideoLibraryViewModel _dataContext;

        public VideoLibrary(SettingsWrapper settings)
        {
            InitializeComponent();

            _dataContext = new VideoLibraryViewModel(settings);

            DataContext = _dataContext;
        }

        public void Update()
        {
            _dataContext.Update();
        }
    }
}