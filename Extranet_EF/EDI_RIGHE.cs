namespace Extranet_EF
{
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("RigheOrdinabili")]
    public partial class EDI_RIGHE
    {
        public int ID { get; set; }

        public int ID_TESTATA { get; set; }

        [Required]
        [StringLength(12)]
        public string NUMORDINE { get; set; }

        [Required]
        [StringLength(15)]
        public string ARTCOD { get; set; }

        [StringLength(5)]
        public string ARTVER { get; set; }

        [StringLength(100)]
        public string ARTDES { get; set; }

        public decimal ARTQTA { get; set; }

        public string TIPOLOGIA { get; set; }

        [Required]
        [StringLength(5)]
        public string ARTUM { get; set; }


        [RegularExpression(@"^\d+\.\d{0}$")]
        public decimal? DATA_CONSEGNA { get; set; }

        [StringLength(30)]
        public string LAST_DDT { get; set; }

        [StringLength(30)]
        public string LAST_DDT_F { get; set; }

        [Column(TypeName = "date")]
        
        public DateTime? LAST_DATA { get; set; }

        public decimal? LAST_QTA { get; set; }

        public decimal? PROG_ARTICOLO { get; set; }

        public int Ordinamento { get; set; }

        //public long rank { get; set; }

        public Int32 rank { get; set; }

        public virtual EDI_TESTATA EDI_TESTATA { get; set; }
    }
}
