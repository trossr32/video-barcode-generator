using System.Windows.Controls;

namespace BarcodeManager
{
    /// <summary>
    /// Interaction logic for BarcodeCreator.xaml
    /// </summary>
    public partial class BarcodeCreator : UserControl
    {
        public BarcodeCreator()
        {
            InitializeComponent();

            DataContext = new ViewModels.BarcodeCreatorViewModel();
        }
    }
}