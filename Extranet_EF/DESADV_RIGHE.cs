namespace Extranet_EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DESADV_RIGHE
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string CLIENTE { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(35)]
        public string IDNUMDES { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(12)]
        public string IDEMB { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(6)]
        public string NUMLIN { get; set; }

        [Required]
        [StringLength(35)]
        public string REFCOMPRADOR { get; set; }

        public int? NIVELCONFIG { get; set; }

        [Required]
        [StringLength(35)]
        public string REFPROVEEDOR { get; set; }

        public decimal? PESONETO { get; set; }

        [StringLength(3)]
        public string UMPESONETO { get; set; }

        public int CANTENT { get; set; }

        [Required]
        [StringLength(3)]
        public string UMCANTENT { get; set; }

        [Required]
        [StringLength(3)]
        public string PAISORIG { get; set; }

        [StringLength(3)]
        public string TIPOREGIMEN { get; set; }

        [StringLength(70)]
        public string TEXTOLIBRE { get; set; }

        public decimal? VALORADUANA { get; set; }

        [StringLength(3)]
        public string DIVISAVARADU { get; set; }

        [StringLength(35)]
        public string NUMPEDIDO { get; set; }

        [StringLength(35)]
        public string NUMPROGRAMA { get; set; }

        [StringLength(6)]
        public string NUMLINPEDIDO { get; set; }

        [StringLength(35)]
        public string INFOADICIONAL { get; set; }

        [StringLength(35)]
        public string NUMSERIE { get; set; }

        [StringLength(35)]
        public string NUMENGINE { get; set; }

        [StringLength(35)]
        public string NUMVEHICLE { get; set; }

        [StringLength(35)]
        public string RFF_AAP { get; set; }

        [StringLength(35)]
        public string RFF_AAE { get; set; }

        [StringLength(35)]
        public string DTM_AAE { get; set; }

        [StringLength(35)]
        public string RFF_IV { get; set; }

        [StringLength(35)]
        public string RFF_IV2 { get; set; }

        [StringLength(25)]
        public string FINALDESCARGA { get; set; }

        public int NUMLINALB { get; set; }

        //[Required]
        [StringLength(1)]
        public string APPLICACION { get; set; }

        [StringLength(1)]
        public string INVENTARIO { get; set; }
        [StringLength(3)]
        public string CPS { get; set; }

        public virtual DESADV_IMBALLI DESADV_IMBALLI { get; set; }
    }
}
