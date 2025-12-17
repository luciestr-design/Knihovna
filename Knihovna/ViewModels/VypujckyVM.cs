using DataEntity.Data;
using Knihovna.Globals;
using Knihovna.Helpers;
using Microsoft.EntityFrameworkCore;
using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Knihovna.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class VypujckyVM : BaseVM
    {
        // Hlavní seznam výpůjček
        public ObservableCollection<Vypujcka> VypujckyCol { get; set; } = null!;
        public ICollectionView VypujckyView { get; set; } = null!;

        // --- ZDE BYLA CHYBA: Opraveno Kniha -> Knihy ---
        public ObservableCollection<Knihy> SeznamKnih { get; set; } = null!;
        public ObservableCollection<Uzivatel> SeznamUzivatelu { get; set; } = null!;

        // Vybraná výpůjčka
        private Vypujcka? _selectedVypujcka;
        public Vypujcka? SelectedVypujcka
        {
            get => _selectedVypujcka;
            set
            {
                _selectedVypujcka = value;
                OnPropertyChanged();
            }
        }

        // Filtr
        private string _filtrPrijmeni = string.Empty;
        public string FiltrPrijmeni
        {
            get => _filtrPrijmeni;
            set
            {
                _filtrPrijmeni = value;
                VypujckyView.Refresh();
            }
        }

        public ICommand PridatCommand { get; }
        public ICommand SmazatCommand { get; }
        public ICommand UlozitCommand { get; }
        public ICommand VratitKnihuCommand { get; }

        public VypujckyVM()
        {
            if (AppGlobals.Context != null)
            {
                // 1. Načteme výpůjčky
                AppGlobals.Context.Vypujcky.Load();
                VypujckyCol = AppGlobals.Context.Vypujcky.Local.ToObservableCollection();

                // 2. Načteme seznamy pro ComboBoxy
                AppGlobals.Context.Knihy.Load();
                // --- Opraveno Kniha -> Knihy ---
                SeznamKnih = AppGlobals.Context.Knihy.Local.ToObservableCollection();

                AppGlobals.Context.Uzivatele.Load();
                SeznamUzivatelu = AppGlobals.Context.Uzivatele.Local.ToObservableCollection();
            }
            else
            {
                VypujckyCol = new ObservableCollection<Vypujcka>();
                SeznamKnih = new ObservableCollection<Knihy>(); // Opraveno
                SeznamUzivatelu = new ObservableCollection<Uzivatel>();
            }

            // 3. Filtr
            VypujckyView = CollectionViewSource.GetDefaultView(VypujckyCol);
            VypujckyView.Filter = FilterLogika;

            // 4. Příkazy
            PridatCommand = new RelayCommand(ExecutePridat);
            SmazatCommand = new RelayCommand(ExecuteSmazat, CanExecuteSelect);
            UlozitCommand = new RelayCommand(ExecuteUlozit);
            VratitKnihuCommand = new RelayCommand(ExecuteVratitKnihu, CanExecuteSelect);

            if (VypujckyCol.Any()) SelectedVypujcka = VypujckyCol.First();
        }

        private bool FilterLogika(object item)
        {
            if (string.IsNullOrEmpty(FiltrPrijmeni)) return true;
            if (item is Vypujcka v && v.Uzivatel != null)
            {
                return v.Uzivatel.Prijmeni.ToLower().Contains(FiltrPrijmeni.ToLower());
            }
            return false;
        }

        private void ExecutePridat(object obj)
        {
            var nova = new Vypujcka
            {
                DatumPujceni = DateTime.Now,
                DatumSplatnosti = DateTime.Now.AddDays(14),
                Stav = "Vypůjčeno"
            };

            if (SeznamKnih.Any()) nova.Kniha = SeznamKnih.First();
            if (SeznamUzivatelu.Any()) nova.Uzivatel = SeznamUzivatelu.First();

            AppGlobals.Context.Vypujcky.Add(nova);
            SelectedVypujcka = nova;
        }

        private void ExecuteSmazat(object obj)
        {
            if (SelectedVypujcka != null)
            {
                if (MessageBox.Show("Opravdu smazat záznam?", "Smazat", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    AppGlobals.Context.Vypujcky.Remove(SelectedVypujcka);
                }
            }
        }

        private void ExecuteVratitKnihu(object obj)
        {
            if (SelectedVypujcka != null)
            {
                SelectedVypujcka.Stav = "Vráceno";
                SelectedVypujcka.DatumVraceni = DateTime.Now;
                SelectedVypujcka = SelectedVypujcka; // Refresh UI
                OnPropertyChanged(nameof(SelectedVypujcka));
            }
        }

        private bool CanExecuteSelect(object obj) => SelectedVypujcka != null;

        private void ExecuteUlozit(object obj)
        {
            AppGlobals.UlozitData();
        }
    }
}