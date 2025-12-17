using Knihovna.Helpers; // Pro RelayCommand
using Knihovna.Views;   // DŮLEŽITÉ: Abychom viděli okna KnihyView, UzivateleView a VypujckyView
using PropertyChanged;
using System.Windows;   // Pro MessageBox
using System.Windows.Input;

namespace Knihovna.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class MainWindowVM : BaseVM
    {
        // --- Příkazy pro tlačítka v menu ---
        public ICommand OpenKnihyCommand { get; }
        public ICommand OpenUzivateleCommand { get; }
        public ICommand OpenVypujckyCommand { get; }

        // --- Konstruktor ---
        public MainWindowVM()
        {
            OpenKnihyCommand = new RelayCommand(ExecuteOpenKnihy);
            OpenUzivateleCommand = new RelayCommand(ExecuteOpenUzivatele);
            OpenVypujckyCommand = new RelayCommand(ExecuteOpenVypujcky);
        }

        // --- Metody (Logika) ---

        private void ExecuteOpenKnihy(object obj)
        {
            // Otevře okno správy knih
            var view = new KnihyView();
            view.DataContext = new KnihyVM();
            view.Show();
        }

        private void ExecuteOpenUzivatele(object obj)
        {
            // Otevře okno správy uživatelů
            var view = new UzivateleView();
            view.DataContext = new UzivateleVM();
            view.Show();
        }

        private void ExecuteOpenVypujcky(object obj)
        {
            // Otevře okno správy výpůjček
            var view = new VypujckyView();
            view.DataContext = new VypujckyVM();
            view.Show();
        }
    }
}