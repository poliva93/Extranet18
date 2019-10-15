namespace Extranet_EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DESADV_IMBALLI
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DESADV_IMBALLI()
        {
            DESADV_ETICHETTE = new HashSet<DESADV_ETICHETTE>();
            DESADV_RIGHE = new HashSet<DESADV_RIGHE>();
        }

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
        [StringLength(3)]
        public string CPS { get; set; }

        public int CANTEMB { get; set; }

        [StringLength(17)]
        public string MEDIOEMB { get; set; }

        [StringLength(3)]
        public string CALMEDIOEMB { get; set; }

        public int? LONGITUD { get; set; }

        [StringLength(3)]
        public string UMLONGITUD { get; set; }

        public int? ANCHURA { get; set; }

        [StringLength(3)]
        public string UMANCHURA { get; set; }

        public int? ALTURA { get; set; }

        [StringLength(3)]
        public string UMALTURA { get; set; }

        public int? CANTPAQUETE { get; set; }

        [StringLength(3)]
        public string UNIDMEDCPAC { get; set; }

        [StringLength(3)]
        public string INSTMARCAJE { get; set; }

        [Required]
        [StringLength(3)]
        public string IDENTETIQUETA { get; set; }

        [StringLength(3)]
        public string CALIDENTETIQUETA { get; set; }

        [StringLength(35)]
        public string ETQMAESTRA { get; set; }

        [StringLength(12)]
        public string FECHAFABRIC { get; set; }

        [StringLength(12)]
        public string FECHACADUC { get; set; }

        [StringLength(35)]
        public string NUMETIQUETA { get; set; }

        [Required]
        [StringLength(35)]
        public string NUMPACCOMP { get; set; }

        [StringLength(35)]
        public string NUMLOTE { get; set; }

        [Required]
        [StringLength(22)]
        public string NUMPACPRO { get; set; }

        [StringLength(13)]
        public string CAPACITA { get; set; }

        [StringLength(35)]
        public string DESCEMB { get; set; }

        [StringLength(3)]
        public string TIPOENV { get; set; }

        [StringLength(3)]
        public string CONDIMB { get; set; }
        [StringLength(35)]
        public string NUMETIQUETA2 { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DESADV_ETICHETTE> DESADV_ETICHETTE { get; set; }

        public virtual DESADV_TESTATA DESADV_TESTATA { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DESADV_RIGHE> DESADV_RIGHE { get; set; }
    }
}
