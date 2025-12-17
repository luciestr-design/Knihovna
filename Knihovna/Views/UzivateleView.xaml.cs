using Knihovna.Globals; // Abychom viděli AppGlobals
using System.ComponentModel; // Pro CancelEventArgs
using System.Windows;

namespace Knihovna.Views
{
    public partial class UzivateleView : Window
    {
        public UzivateleView()
        {
            InitializeComponent();
        }

        // Metoda se spustí automaticky, když klikneš na křížek pro zavření okna
        protected override void OnClosing(CancelEventArgs e)
        {
            // Kontrola neuložených změn v databázi
            if (AppGlobals.HasUnsavedChanges())
            {
                var vysledek = MessageBox.Show(
                    "Máte neuložené změny u uživatelů! Chcete je uložit?",
                    "Upozornění",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Warning);

                if (vysledek == MessageBoxResult.Yes)
                {
                    // Ano -> Uložit
                    AppGlobals.UlozitData();
                }
                else if (vysledek == MessageBoxResult.No)
                {
                    // Ne -> Zahodit změny (Rollback)
                    AppGlobals.Vratit();
                }
                else
                {
                    // Zrušit (Cancel) -> Zastavit zavírání okna
                    e.Cancel = true;
                }
            }

            base.OnClosing(e);
        }
    }
}
