using System.Windows.Controls;
using VideoBarcodeGenerator.Core.Models.Settings;
using VideoBarcodeGenerator.Wpf.ViewModels;

namespace VideoBarcodeGenerator.Wpf.Views
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