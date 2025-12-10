using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataEntity.Data
{
    [AddINotifyPropertyChangedInterface]
    [Table("Knihy")]
    public class Knihy : Base.BaseModel
    {
        [Key]
        public int KnihaId { get; set; }

        [Required(ErrorMessage = "Název knihy je povinné pole")]
        [StringLength(250, ErrorMessage = "Maximální délka je 250")]
        public string Nazev { get; set; } = string.Empty;

        [Required(ErrorMessage = "Autor je povinné pole")]
        [StringLength(150, ErrorMessage = "Maximální délka je 150")]
        public string Autor { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Maximální délka žánru je 100")]
        public string? Zanr { get; set; }

        [Range(1000, 9999, ErrorMessage = "Rok vydání musí být čtyřciferné číslo")]
        public int RokVydani { get; set; } = DateTime.Now.Year;

        [StringLength(20, ErrorMessage = "Maximální délka ISBN je 20")]
        public string? ISBN { get; set; }

        [Required(ErrorMessage = "Počet kusů celkem je povinné pole")]
        [Range(0, int.MaxValue, ErrorMessage = "Počet kusů musí být kladné nebo 0")]
        public int CelkemKusu { get; set; } = 1;

        [Required(ErrorMessage = "Počet kusů k dispozici je povinné pole")]
        [Range(0, int.MaxValue, ErrorMessage = "Počet kusů k dispozici musí být kladné nebo 0")]
        public int KusuKDispozici { get; set; } = 1;

        // --- NOVÉ: Vazba, abychom viděli historii výpůjček této knihy ---
        public virtual ObservableCollection<Vypujcka> Vypujcky { get; set; } = new ObservableCollection<Vypujcka>();
    }
}


