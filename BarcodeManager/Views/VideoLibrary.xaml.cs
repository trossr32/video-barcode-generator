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
        private Views.BarcodeCreator _barcodeCreator;
        private Tasks _tasks;

        public VideoLibrary(SettingsWrapper settings)
        {
            InitializeComponent();

            _tasks = new Tasks(settings);

            _barcodeCreator = new BarcodeCreator((TasksViewModel)_tasks.DataContext, settings);

            BarcodeCreatorContent.Content = _barcodeCreator;
            TasksContent.Content = _tasks;
        }
    }
}