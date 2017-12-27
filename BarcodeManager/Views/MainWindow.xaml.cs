using System.Windows;
using BarcodeManager.ViewModels;
using FilmBarcodes.Common.Enums;

namespace BarcodeManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BarcodeCreator _barcodeCreator;
        private Home _home;
        private Tasks _tasks;

        public MainWindow()
        {
            InitializeComponent();

            _home = new Home();
            _tasks = new Tasks();

            _barcodeCreator = new BarcodeCreator(this, (TasksViewModel) _tasks.DataContext);

            ContentArea.Content = _barcodeCreator;
        }
        
        private void MenuHome_Click(object sender, RoutedEventArgs e)
        {
            SetTab(TabType.Home);
        }

        private void MenuCreate_Click(object sender, RoutedEventArgs e)
        {
            SetTab(TabType.Create);
        }

        private void MenuTasks_Click(object sender, RoutedEventArgs e)
        {
            SetTab(TabType.Tasks);
        }

        public void SetTab(TabType tab)
        {
            switch (tab)
            {
                case TabType.Home:
                    ContentArea.Content = _home;
                    break;
                case TabType.Create:
                    ContentArea.Content = _barcodeCreator;
                    break;
                case TabType.Tasks:
                    ContentArea.Content = _tasks;
                    break;
            }
        }
    }
}