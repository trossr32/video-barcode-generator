using System;
using System.IO;
using System.Windows;
using NLog;
using VideoBarcodeGenerator.Core;
using VideoBarcodeGenerator.Core.Enums;
using VideoBarcodeGenerator.Core.Models.Settings;

namespace VideoBarcodeGenerator.Wpf.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly BarcodeCreatorParent _barcodeCreatorParent;
        private readonly VideoLibrary _videoLibrary;

        private static Logger _logger;

        public MainWindow()
        {
            InitializeComponent();

            _logger = LogManager.GetCurrentClassLogger();

            SettingsWrapper settings = Settings.GetSettings();

            LogManager.Configuration.Variables["logfile"] = Path.Combine(settings.CoreSettings.OutputDirectory, ".logs", $"{DateTime.Now:yyyy.MM.dd}.log");

            _barcodeCreatorParent = new BarcodeCreatorParent(settings);

            _videoLibrary = new VideoLibrary(settings);

            ContentArea.Content = _barcodeCreatorParent;
        }

        private void MenuCreate_Click(object sender, RoutedEventArgs e)
        {
            SetTab(TabType.Create);
        }

        private void MenuVideoLibrary_Click(object sender, RoutedEventArgs e)
        {
            SetTab(TabType.VideoLibrary);
        }

        private void SetTab(TabType tab)
        {
            switch (tab)
            {
                case TabType.Create:
                    ContentArea.Content = _barcodeCreatorParent;
                    break;

                case TabType.VideoLibrary:
                    ContentArea.Content = _videoLibrary;
                    break;
            }
        }
    }
}