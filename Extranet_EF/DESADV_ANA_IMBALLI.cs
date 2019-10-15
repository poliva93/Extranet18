namespace Extranet_EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("vAnagraficaImballi")]
    public partial class DESADV_ANA_IMBALLI
    {
        [Key, Column(Order = 0)]
        [StringLength(15)]
        public string ARTCOD { get; set; }

        [Key, Column(Order = 3)]
        [StringLength(50)]
        public string CLIENTE { get; set; }

        public int? BOXNUM { get; set; }

        [Key, Column(Order = 1)]
        [StringLength(50)]
        public string BOXCOD { get; set; }

        public int? PARTXBOX { get; set; }
        [Key, Column(Order = 2)]
        [StringLength(50)]
        public string BOXCODDO { get; set; }

        [StringLength(50)]
        public string LUGDESCARGA { get; set; }

        [StringLength(50)]
        public string TIPOLOGIA { get; set; }

        [StringLength(1)]
        public string imbStandard { get; set; }

        [StringLength(1)]
        public string PalletCompleto { get; set; }

        public Int16 Sort { get; set; }
        
        [StringLength(3)]
        public string Origine { get; set; }



    }
}
