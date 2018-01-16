using System.Windows.Controls;
using BarcodeManager.ViewModels;
using FilmBarcodes.Common.Models.Settings;

namespace BarcodeManager.Views
{
    /// <summary>
    /// Interaction logic for VideoLibrary.xaml
    /// </summary>
    public partial class VideoLibrary : UserControl
    {
        public VideoLibrary(SettingsWrapper settings)
        {
            InitializeComponent();

            DataContext = new VideoLibraryViewModel(settings);
        }
    }
}