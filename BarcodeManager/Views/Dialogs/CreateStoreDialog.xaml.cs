using System.Windows.Controls;
using BarcodeManager.ViewModels.Dialogs;

namespace BarcodeManager.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for CreateStoreDialog.xaml
    /// </summary>
    public partial class CreateStoreDialog : UserControl
    {
        public CreateStoreDialog()
        {
            InitializeComponent();

            DataContext = new CreateStoreDialogViewModel();
        }
    }
}
