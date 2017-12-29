using System.Windows.Controls;
using BarcodeManager.ViewModels;

namespace BarcodeManager
{
    /// <summary>
    /// Interaction logic for BarcodeCreator.xaml
    /// </summary>
    public partial class BarcodeCreator : UserControl
    {
        public BarcodeCreator(TasksViewModel tasksViewModel)
        {
            InitializeComponent();

            DataContext = new BarcodeCreatorViewModel(tasksViewModel);
        }
    }
}