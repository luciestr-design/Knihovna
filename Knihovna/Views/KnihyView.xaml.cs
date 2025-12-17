using Knihovna.ViewModels;
using DataEntity.Data; // Aby znal typ "Knihy"
using System.Windows;
using System.Windows.Controls;

namespace Knihovna.Views
{
    public partial class KnihyView : Window
    {
        public KnihyView()
        {
            InitializeComponent();
            DataContext = new KnihyVM();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is KnihyVM vm)
            {
                var vybranaKniha = ((DataGrid)sender).SelectedItem as Knihy;
                vm.VybranaKniha = vybranaKniha;
            }
        }
    }
}
