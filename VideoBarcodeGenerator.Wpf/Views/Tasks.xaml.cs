using System.Windows.Controls;
using VideoBarcodeGenerator.Core.Models.Settings;

namespace VideoBarcodeGenerator.Wpf.Views
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