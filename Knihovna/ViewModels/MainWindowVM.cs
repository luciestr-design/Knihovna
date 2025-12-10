using Knihovna.Helpers; // Nutné pro RelayCommand
using PropertyChanged;  // Nutné pro Fody
using System.Windows;
using System.Windows.Input;

namespace Knihovna.ViewModels
{

    [AddINotifyPropertyChangedInterface]
    public class MainWindowVM : BaseVM
    {
       
        public ICommand OpenKnihyCommand { get; }
        public ICommand OpenUzivateleCommand { get; }
        public ICommand OpenVypujckyCommand { get; }

        public MainWindowVM()
        {
           
            OpenKnihyCommand = new RelayCommand(ExecuteOpenKnihy);
            OpenUzivateleCommand = new RelayCommand(ExecuteOpenUzivatele);
            OpenVypujckyCommand = new RelayCommand(ExecuteOpenVypujcky);
        }

      
        private void ExecuteOpenKnihy(object obj)
        {
            MessageBox.Show("Otevírám správu knih...");
        }

        private void ExecuteOpenUzivatele(object obj)
        {
            MessageBox.Show("Otevírám správu uživatelů...");
        }

        private void ExecuteOpenVypujcky(object obj)
        {
            MessageBox.Show("Otevírám správu výpůjček...");
        }
    }
}