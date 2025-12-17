using DataEntity.Data;
using Knihovna.Globals;
using Knihovna.Helpers;
using Microsoft.EntityFrameworkCore;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Knihovna.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class UzivateleVM : BaseVM
    {
        public ObservableCollection<Uzivatel> UzivateleCol { get; set; } = null!;
        public ICollectionView UzivateleView { get; set; } = null!;

        // Ruční notifikace pro detail, stejně jako u Knih
        private Uzivatel? _selectedUzivatel;
        public Uzivatel? SelectedUzivatel
        {
            get => _selectedUzivatel;
            set
            {
                _selectedUzivatel = value;
                OnPropertyChanged();
            }
        }

        // Filtr pro vyhledávání podle příjmení
        private string _filtrPrijmeni = string.Empty;
        public string FiltrPrijmeni
        {
            get => _filtrPrijmeni;
            set
            {
                _filtrPrijmeni = value;
                UzivateleView.Refresh();
            }
        }

        public ICommand PridatCommand { get; }
        public ICommand SmazatCommand { get; }
        public ICommand UlozitCommand { get; }

        public UzivateleVM()
        {
            // 1. Načtení dat
            if (AppGlobals.Context != null)
            {
                AppGlobals.Context.Uzivatele.Load();
                UzivateleCol = AppGlobals.Context.Uzivatele.Local.ToObservableCollection();
            }
            else
            {
                UzivateleCol = new ObservableCollection<Uzivatel>();
            }

            // 2. Filtr
            UzivateleView = CollectionViewSource.GetDefaultView(UzivateleCol);
            UzivateleView.Filter = FilterLogika;

            // 3. Příkazy
            PridatCommand = new RelayCommand(ExecutePridat);
            SmazatCommand = new RelayCommand(ExecuteSmazat, CanExecuteSmazat);
            UlozitCommand = new RelayCommand(ExecuteUlozit);

            // 4. Auto-výběr
            if (UzivateleCol.Any())
            {
                SelectedUzivatel = UzivateleCol.First();
            }
        }

        private bool FilterLogika(object item)
        {
            if (string.IsNullOrEmpty(FiltrPrijmeni)) return true;
            if (item is Uzivatel u)
            {
                return u.Prijmeni.ToLower().Contains(FiltrPrijmeni.ToLower());
            }
            return false;
        }

        private void ExecutePridat(object obj)
        {
            var novyUzivatel = new Uzivatel
            {
                Jmeno = "Nový",
                Prijmeni = "Uživatel",
                Email = "@",
                Dluh = 0
            };
            AppGlobals.Context.Uzivatele.Add(novyUzivatel);
            SelectedUzivatel = novyUzivatel;
        }

        private void ExecuteSmazat(object obj)
        {
            if (SelectedUzivatel == null) return;

            // Kontrola, zda nemá uživatel výpůjčky (aby nevznikl bordel v DB)
            if (SelectedUzivatel.Vypujcky != null && SelectedUzivatel.Vypujcky.Any())
            {
                MessageBox.Show("Nelze smazat uživatele, který má evidované výpůjčky!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (MessageBox.Show($"Smazat uživatele {SelectedUzivatel.Prijmeni}?", "Dotaz", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                AppGlobals.Context.Uzivatele.Remove(SelectedUzivatel);
            }
        }

        private bool CanExecuteSmazat(object obj) => SelectedUzivatel != null;

        private void ExecuteUlozit(object obj)
        {
            AppGlobals.UlozitData();
        }
    }
}
