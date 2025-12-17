using Knihovna.Globals; // Pro přístup k AppGlobals.HasUnsavedChanges
using System.ComponentModel; // Pro CancelEventArgs
using System.Windows;

namespace Knihovna.Views
{
    public partial class VypujckyView : Window
    {
        public VypujckyView()
        {
            InitializeComponent();
        }

        // Bezpečné zavírání okna
        protected override void OnClosing(CancelEventArgs e)
        {
            if (AppGlobals.HasUnsavedChanges())
            {
                var result = MessageBox.Show(
                    "Máte neuložené změny ve výpůjčkách! Chcete je uložit?",
                    "Upozornění",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    AppGlobals.UlozitData();
                }
                else if (result == MessageBoxResult.No)
                {
                    AppGlobals.Vratit();
                }
                else
                {
                    e.Cancel = true; // Zrušit zavírání
                }
            }
            base.OnClosing(e);
        }
    }
}
