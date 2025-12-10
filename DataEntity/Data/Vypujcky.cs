using PropertyChanged;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataEntity.Data
{
    [AddINotifyPropertyChangedInterface]
    [Table("Vypujcky")]
    [Index(nameof(UzivatelId))]
    [Index(nameof(KnihaId))]
    public class Vypujcka : Base.BaseModel
    {
        [Key]
        public int VypujckaId { get; set; }

        public int UzivatelId { get; set; }
        [ForeignKey(nameof(UzivatelId))]
        public virtual Uzivatel Uzivatel { get; set; } = null!;

        public int KnihaId { get; set; }
        [ForeignKey(nameof(KnihaId))]
        public virtual Knihy Kniha { get; set; } = null!;

        [Column(TypeName = "date")]
        public DateTime DatumPujceni { get; set; } = DateTime.Now;

        [Column(TypeName = "date")]
        public DateTime DatumSplatnosti { get; set; } = DateTime.Now.AddDays(14); // Příklad: splatnost 14 dní

        [Column(TypeName = "date")]
        public DateTime? DatumVraceni { get; set; }

        // --- Zde zůstává string, výchozí hodnota je "Vypůjčeno" ---
        [Required(ErrorMessage = "Stav je povinné pole")]
        [StringLength(50)]
        public string Stav { get; set; } = "Vypůjčeno";
    }
}