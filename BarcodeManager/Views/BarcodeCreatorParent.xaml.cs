using System.Windows.Controls;
using BarcodeManager.ViewModels;

namespace BarcodeManager
{
    /// <summary>
    /// Interaction logic for Tasks.xaml
    /// </summary>
    public partial class BarcodeCreatorParent : UserControl
    {
        private BarcodeCreator _barcodeCreator;
        private Tasks _tasks;

        public BarcodeCreatorParent()
        {
            InitializeComponent();

            _tasks = new Tasks();

            _barcodeCreator = new BarcodeCreator((TasksViewModel)_tasks.DataContext);

            BarcodeCreatorContent.Content = _barcodeCreator;
            TasksContent.Content = _tasks;
        }
    }
}