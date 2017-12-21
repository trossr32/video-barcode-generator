using System.Windows;

namespace BarcodeManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool IsSample4DialogOpen { get; set; } = false;

        private BarcodeCreator _barcodeCreator;
        private Home _home;

        public MainWindow()
        {
            InitializeComponent();

            _barcodeCreator = new BarcodeCreator();
            _home = new Home();

            ContentArea.Content = _barcodeCreator;
        }
        
        private void MenuHome_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = _home;
        }

        private void MenuCreate_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = _barcodeCreator;
        }
    }
}