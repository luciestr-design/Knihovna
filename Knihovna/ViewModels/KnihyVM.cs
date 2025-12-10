using DataEntity.Data;
using Knihovna.Globals;
using Knihovna.Helpers;
using Microsoft.EntityFrameworkCore;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Knihovna.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class KnihyVM : BaseVM
    {
        // "null!" = Slibujeme, že to naplníme v konstruktoru
        public ObservableCollection<Knihy> KnihyCol { get; set; } = null!;

        public ICollectionView KnihyView { get; set; } = null!;

        // ZDE JE ZMĚNA: Přidali jsme otazník "?", protože na začátku není vybrána žádná kniha
        public Knihy? SelectedKniha { get; set; }

        private string _filtrRok = string.Empty; // Inicializujeme prázdným řetězcem
        public string FiltrRok
        {
            get => _filtrRok;
            set
            {
                _filtrRok = value;
                KnihyView.Refresh();
            }
        }

        // "null!" = Slibujeme naplnění v konstruktoru
        public ICommand PridatCommand { get; } = null!;
        public ICommand SmazatCommand { get; } = null!;
        public ICommand UlozitCommand { get; } = null!;

        public KnihyVM()
        {
            // 1. Načtení dat
            AppGlobals.Context.Knihy.Load();
            KnihyCol = AppGlobals.Context.Knihy.Local.ToObservableCollection();

            // 2. Filtrování
            KnihyView = CollectionViewSource.GetDefaultView(KnihyCol);
            KnihyView.Filter = FilterLogika;

            // 3. Tlačítka
            PridatCommand = new RelayCommand(ExecutePridat);
            SmazatCommand = new RelayCommand(ExecuteSmazat, CanExecuteSmazat);
            UlozitCommand = new RelayCommand(ExecuteUlozit);
        }

        private bool FilterLogika(object item)
        {
            if (string.IsNullOrEmpty(FiltrRok)) return true;

            if (item is Knihy kniha)
            {
                return kniha.RokVydani.ToString().Contains(FiltrRok);
            }
            return false;
        }

        private void ExecutePridat(object obj)
        {
            var novaKniha = new Knihy
            {
                Nazev = "Nová kniha",
                Autor = "Neznámý autor", // Doplněno, aby nebyl null
                RokVydani = 2025,
                CelkemKusu = 1,
                KusuKDispozici = 1,
                ISBN = "000-00-000"     // Doplněno
            };
            AppGlobals.Context.Knihy.Add(novaKniha);
            SelectedKniha = novaKniha;
        }

        private void ExecuteSmazat(object obj)
        {
            // Kontrola pro jistotu, i když CanExecute to hlídá
            if (SelectedKniha == null) return;

            if (MessageBox.Show($"Opravdu smazat knihu '{SelectedKniha.Nazev}'?", "Smazat",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                AppGlobals.Context.Knihy.Remove(SelectedKniha);
            }
        }

        private bool CanExecuteSmazat(object obj)
        {
            return SelectedKniha != null;
        }

        private void ExecuteUlozit(object obj)
        {
            AppGlobals.UlozitData();
        }
    }
}