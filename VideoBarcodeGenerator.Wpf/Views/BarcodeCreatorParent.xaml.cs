using System.Windows.Controls;
using VideoBarcodeGenerator.Core.Models.Settings;
using VideoBarcodeGenerator.Wpf.ViewModels;

namespace VideoBarcodeGenerator.Wpf.Views
{
    /// <summary>
    /// Interaction logic for BarcodeCreatorParent.xaml
    /// </summary>
    public partial class BarcodeCreatorParent : UserControl
    {
        public BarcodeCreatorParent(SettingsWrapper settings)
        {
            InitializeComponent();

            var tasks = new Tasks(settings);

            var barcodeCreator = new BarcodeCreator((TasksViewModel)tasks.DataContext, settings);

            BarcodeCreatorContent.Content = barcodeCreator;
            TasksContent.Content = tasks;
        }
    }
}