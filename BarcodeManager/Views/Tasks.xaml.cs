using System.Windows.Controls;

namespace BarcodeManager
{
    /// <summary>
    /// Interaction logic for Tasks.xaml
    /// </summary>
    public partial class Tasks : UserControl
    {
        public Tasks()
        {
            InitializeComponent();

            DataContext = new ViewModels.TasksViewModel();
        }
    }
}