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
        public VideoLibrary(SettingsWrapper settings)
        {
            InitializeComponent();

            DataContext = new VideoLibraryViewModel(settings);
        }
    }
}