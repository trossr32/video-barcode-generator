using System.Windows.Controls;
using FilmBarcodes.Common.Models.Settings;

namespace BarcodeManager.Views
{
    /// <summary>
    /// Interaction logic for Tasks.xaml
    /// </summary>
    public partial class Tasks : UserControl
    {
        public Tasks(SettingsWrapper settings)
        {
            InitializeComponent();

            DataContext = new ViewModels.TasksViewModel(settings);
        }
    }
}