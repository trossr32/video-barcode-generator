using System.Windows.Controls;
using BarcodeManager.ViewModels;
using FilmBarcodes.Common.Models.Settings;

namespace BarcodeManager.Views
{
    /// <summary>
    /// Interaction logic for CafePressDesigns.xaml
    /// </summary>
    public partial class CafePressDesigns : UserControl
    {
        public CafePressDesigns(SettingsWrapper settings)
        {
            InitializeComponent();

            DataContext = new CafePressDesignsViewModel(settings);
        }
    }
}