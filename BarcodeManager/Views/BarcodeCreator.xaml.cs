using System.Windows.Controls;
using BarcodeManager.ViewModels;

namespace BarcodeManager
{
    /// <summary>
    /// Interaction logic for BarcodeCreator.xaml
    /// </summary>
    public partial class BarcodeCreator : UserControl
    {
        public BarcodeCreator(MainWindow mainWindow, TasksViewModel tasksViewModel)
        {
            InitializeComponent();

            DataContext = new BarcodeCreatorViewModel(mainWindow, tasksViewModel);
        }
    }
}