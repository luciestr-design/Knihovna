using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataEntity.Data
{
    [AddINotifyPropertyChangedInterface]
    [Table("Uzivatele")]
    public class Uzivatel : Base.BaseModel
    {
        [Key]
        public int UzivatelId { get; set; }

        [Required(ErrorMessage = "Jméno je povinné pole")]
        [StringLength(100, ErrorMessage = "Maximální délka je 100")]
        public string Jmeno { get; set; } = string.Empty;

        [Required(ErrorMessage = "Příjmení je povinné pole")]
        [StringLength(100, ErrorMessage = "Maximální délka je 100")]
        public string Prijmeni { get; set; } = string.Empty;

        // --- ZMĚNA: Přidáno Required (povinné) ---
        [Required(ErrorMessage = "Adresa je povinný údaj")]
        [StringLength(300, ErrorMessage = "Maximální délka adresy je 300")]
        public string Adresa { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Maximální délka e-mailu je 200")]
        [EmailAddress(ErrorMessage = "Neplatný formát e-mailu")]
        public string? Email { get; set; }

        // --- ZMĚNA: Přidáno Required (povinné) ---
        [Required(ErrorMessage = "Telefon je povinný údaj")]
        [StringLength(50, ErrorMessage = "Maximální délka telefonu je 50")]
        [Phone]
        public string Telefon { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Dluh { get; set; } = 0m;

        public virtual ObservableCollection<Vypujcka> Vypujcky { get; set; } = new ObservableCollection<Vypujcka>();
    }
}