using System.Windows.Controls;
using BarcodeManager.ViewModels;
using FilmBarcodes.Common.Models.Settings;

namespace BarcodeManager.Views
{
    /// <summary>
    /// Interaction logic for BarcodeCreator.xaml
    /// </summary>
    public partial class BarcodeCreator : UserControl
    {
        public BarcodeCreator(TasksViewModel tasksViewModel, SettingsWrapper settings)
        {
            InitializeComponent();

            DataContext = new BarcodeCreatorViewModel(tasksViewModel, settings);
        }
    }
}