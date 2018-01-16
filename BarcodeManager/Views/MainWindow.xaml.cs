using System;
using System.IO;
using System.Windows;
using FilmBarcodes.Common;
using FilmBarcodes.Common.Enums;
using FilmBarcodes.Common.Models.Settings;
using NLog;

namespace BarcodeManager.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BarcodeCreatorParent _barcodeCreatorParent;
        private Home _home;
        private CafePressDesigns _cafePressDesigns;
        private VideoLibrary _videoLibrary;

        private SettingsWrapper _settings;
        private static Logger _logger;

        public MainWindow()
        {
            InitializeComponent();

            _logger = LogManager.GetCurrentClassLogger();

            _settings = Settings.GetSettings();

            LogManager.Configuration.Variables["logfile"] = Path.Combine(_settings.BarcodeManager.OutputDirectory, ".logs", $"{DateTime.Now:yyyy.MM.dd}.log");

            _home = new Home();

            _barcodeCreatorParent = new BarcodeCreatorParent(_settings);

            _cafePressDesigns = new CafePressDesigns(_settings);

            _videoLibrary = new VideoLibrary(_settings);

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

        private void MenuVideoLibrary_Click(object sender, RoutedEventArgs e)
        {
            SetTab(TabType.VideoLibrary);
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
                case TabType.VideoLibrary:
                    ContentArea.Content = _videoLibrary;
                    break;
            }
        }
    }
}