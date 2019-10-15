namespace Extranet_EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DESADV_ETICHETTE
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
        [StringLength(12)]
        public string IDETQ { get; set; }

        [StringLength(12)]
        public string IDETQPACK { get; set; }


        //[StringLength(35)]
        //public string NUMETIQUETA { get; set; }
        public int NUMETIQUETA { get; set; }

        [StringLength(35)]
        public string NUMPACCOMP { get; set; }

        [StringLength(35)]
        public string NUMLOTE { get; set; }

        [StringLength(35)]
        public string HANNUM { get; set; }

        [StringLength(35)]
        public string AGENCY { get; set; }

        [StringLength(35)]
        public string HANTYPE { get; set; }
        [StringLength(3)]
        public string CPS { get; set; }
        //[StringLength(35)]
        //public string NUMETIQUETA2 { get; set; }
        public int? NUMETIQUETA2 { get; set; }
        //public int _INumetiqueta2
        //{
        //get{
        //        int numero;
        //        Int32.TryParse(NUMETIQUETA2, out numero);
        //        return numero;
        //    } }


        public virtual DESADV_IMBALLI DESADV_IMBALLI { get; set; }
    }
}
