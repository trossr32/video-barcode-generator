using System.Windows;
using FilmBarcodes.Common.Enums;

namespace BarcodeManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BarcodeCreatorParent _barcodeCreatorParent;
        private Home _home;
        private CafePressDesigns _cafePressDesigns;

        public MainWindow()
        {
            InitializeComponent();

            _home = new Home();

            _barcodeCreatorParent = new BarcodeCreatorParent();

            _cafePressDesigns = new CafePressDesigns();

            ContentArea.Content = _barcodeCreatorParent;
        }
        
        private void MenuHome_Click(object sender, RoutedEventArgs e)
        {
            SetTab(TabType.Home);
        }

        private void MenuCreate_Click(object sender, RoutedEventArgs e)
        {
            SetTab(TabType.Create);
        }

        private void Designs_Click(object sender, RoutedEventArgs e)
        {
            SetTab(TabType.Designs);
        }

        private void SetTab(TabType tab)
        {
            switch (tab)
            {
                case TabType.Home:
                    ContentArea.Content = _home;
                    break;
                case TabType.Create:
                    ContentArea.Content = _barcodeCreatorParent;
                    break;
                case TabType.Designs:
                    ContentArea.Content = _cafePressDesigns;
                    break;
            }
        }
    }
}