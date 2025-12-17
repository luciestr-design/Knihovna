using Knihovna.ViewModels; // Abychom viděli ViewModel
using System.Windows;

namespace Knihovna.Views // <--- DŮLEŽITÉ: Musí to být ve jmenném prostoru Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Propojíme okno s ViewModelem, aby fungovala tlačítka
            DataContext = new MainWindowVM();
        }
    }
}