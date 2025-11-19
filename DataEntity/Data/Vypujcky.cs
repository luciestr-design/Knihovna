using PropertyChanged;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataEntity.Data
{
    [AddINotifyPropertyChangedInterface()]
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
        public DateTime DatumSplatnosti { get; set; } = DateTime.Now.AddDays(14);

        [Column(TypeName = "date")]
        public DateTime? DatumVraceni { get; set; }

        [StringLength(100)]
        public string? Stav { get; set; } = "vypůjčeno";
    }
}