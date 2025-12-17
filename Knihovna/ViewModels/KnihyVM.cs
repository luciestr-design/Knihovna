using DataEntity.Data;
using Knihovna.Globals;
using Knihovna.Helpers;
using Microsoft.EntityFrameworkCore;
using PropertyChanged; // Stále necháváme, ale pro klíčovou vlastnost použijeme ruční notifikaci
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Knihovna.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class KnihyVM : BaseVM
    {
        public ObservableCollection<Knihy> KnihyCol { get; set; } = new ObservableCollection<Knihy>();
        public ICollectionView? KnihyView { get; set; }
        public ObservableCollection<int> SeznamRoku { get; set; } = new ObservableCollection<int>();

        // --- ZMĚNA: Ručně implementovaná vlastnost pro jistotu ---
        private Knihy? _vybranaKniha;
        public Knihy? VybranaKniha
        {
            get => _vybranaKniha;
            set
            {
                if (_vybranaKniha != value)
                {
                    _vybranaKniha = value;
                    // Vynucení aktualizace UI
                    OnPropertyChanged(nameof(VybranaKniha));
                    OnPropertyChanged(nameof(JeKnihaVybrana));
                }
            }
        }

        public bool JeKnihaVybrana => VybranaKniha != null;

        // --- FILTRY ---
        private string _hledanyText = "";
        public string HledanyText
        {
            get { return _hledanyText; }
            set
            {
                if (_hledanyText != value)
                {
                    _hledanyText = value;
                    ObnovitFiltr();
                }
            }
        }

        private int? _vybranyRok = null;
        public int? VybranyRok
        {
            get { return _vybranyRok; }
            set
            {
                if (_vybranyRok != value)
                {
                    _vybranyRok = value;
                    ObnovitFiltr();
                }
            }
        }

        public ICommand PridatCommand { get; set; }
        public ICommand SmazatCommand { get; set; }
        public ICommand UlozitCommand { get; set; }
        public ICommand ZrusitFiltrRokuCommand { get; set; }

        public KnihyVM()
        {
            PridatCommand = new RelayCommand(o => PridatKnihu());
            SmazatCommand = new RelayCommand(o => SmazatKnihu());
            UlozitCommand = new RelayCommand(o => UlozitZmeny());
            ZrusitFiltrRokuCommand = new RelayCommand(o => { VybranyRok = null; });

            NacistData();
        }

        public void NacistData()
        {
            if (AppGlobals.Context == null) return;

            // Načtení dat (Load() zajistí stažení do lokální cache, .Local.ToObservableCollection je rychlejší pro binding)
            // Toto je stejný styl jako máte v UzivateleVM
            AppGlobals.Context.Knihy.Load();
            KnihyCol = AppGlobals.Context.Knihy.Local.ToObservableCollection();

            var roky = KnihyCol.Select(k => k.RokVydani).Distinct().OrderByDescending(r => r).ToList();
            SeznamRoku = new ObservableCollection<int>(roky);

            KnihyView = CollectionViewSource.GetDefaultView(KnihyCol);
            KnihyView.Filter = FilterKnihy;
        }

        private bool FilterKnihy(object item)
        {
            if (item is Knihy kniha)
            {
                bool textShoda = string.IsNullOrEmpty(HledanyText) ||
                                 kniha.Nazev.ToLower().Contains(HledanyText.ToLower()) ||
                                 kniha.Autor.ToLower().Contains(HledanyText.ToLower());
                bool rokShoda = (VybranyRok == null) || (kniha.RokVydani == VybranyRok);
                return textShoda && rokShoda;
            }
            return false;
        }

        private void ObnovitFiltr()
        {
            KnihyView?.Refresh();
        }

        private void PridatKnihu()
        {
            var nova = new Knihy { Nazev = "Nová kniha", Autor = "Neznámý", RokVydani = 2025 };
            // Díky .Local.ToObservableCollection se přidáním do Contextu automaticky přidá i do KnihyCol
            AppGlobals.Context.Knihy.Add(nova);
            VybranaKniha = nova;
        }

        private void SmazatKnihu()
        {
            if (VybranaKniha == null) return;

            if (VybranaKniha.Vypujcky != null && VybranaKniha.Vypujcky.Any())
            {
                MessageBox.Show("Nelze smazat knihu s historií výpůjček.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show($"Opravdu smazat knihu '{VybranaKniha.Nazev}'?", "Smazat", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                AppGlobals.Context.Knihy.Remove(VybranaKniha);
                AppGlobals.UlozitData();
                // Pokud by se to nesmazalo z UI samo (občas se stane u Local bindingu), tak:
                // KnihyCol.Remove(VybranaKniha); 
            }
        }

        private void UlozitZmeny()
        {
            AppGlobals.UlozitData();
        }
    }
}