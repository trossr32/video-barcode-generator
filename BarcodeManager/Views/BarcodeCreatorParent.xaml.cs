using System.Windows.Controls;
using BarcodeManager.ViewModels;
using FilmBarcodes.Common.Models.Settings;

namespace BarcodeManager.Views
{
    /// <summary>
    /// Interaction logic for BarcodeCreatorParent.xaml
    /// </summary>
    public partial class BarcodeCreatorParent : UserControl
    {
        private Views.BarcodeCreator _barcodeCreator;
        private Tasks _tasks;

        public BarcodeCreatorParent(SettingsWrapper settings)
        {
            InitializeComponent();

            _tasks = new Tasks(settings);

            _barcodeCreator = new BarcodeCreator((TasksViewModel)_tasks.DataContext, settings);

            BarcodeCreatorContent.Content = _barcodeCreator;
            TasksContent.Content = _tasks;
        }
    }
}