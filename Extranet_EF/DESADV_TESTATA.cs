namespace Extranet_EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DESADV_TESTATA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DESADV_TESTATA()
        {
            DESADV_IMBALLI = new HashSet<DESADV_IMBALLI>();
        }

        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string CLIENTE { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(35)]
        public string NUMDES { get; set; }

        [StringLength(3)]
        public string TIPO { get; set; }

        [StringLength(3)]
        public string CODTIPO { get; set; }

        [Required]
        [StringLength(12)]
        public string FECDES { get; set; }

        [StringLength(12)]
        public string FECENT { get; set; }

        [Required]
        [StringLength(12)]
        public string FECSAL { get; set; }

        public decimal TOTPESBRU { get; set; }

        [StringLength(3)]
        public string UMTOTPESBRU { get; set; }

        public decimal TOTPESNE { get; set; }

        [StringLength(3)]
        public string UMTOTPESNE { get; set; }

        [StringLength(35)]
        public string NUMENVCARG { get; set; }

        [Required]
        [StringLength(35)]
        public string VENDEDOR { get; set; }

        [StringLength(3)]
        public string CALVENDEDOR { get; set; }

        [StringLength(35)]
        public string DIRECCION_SE { get; set; }

        [StringLength(35)]
        public string POBLACION_SE { get; set; }

        [StringLength(35)]
        public string NOMBRE_SE { get; set; }

        [StringLength(35)]
        public string CODINTVEND { get; set; }

        [Required]
        [StringLength(35)]
        public string CONSIGNATARIO { get; set; }

        [StringLength(3)]
        public string CALCONSIGNA { get; set; }

        [StringLength(35)]
        public string DIRECCION_CN { get; set; }

        [StringLength(35)]
        public string POBLACION_CN { get; set; }

        [StringLength(35)]
        public string NOMBRE_CN { get; set; }

        [Required]
        [StringLength(25)]
        public string LUGDESCARGA { get; set; }

        [StringLength(35)]
        public string EXPEDCARGA { get; set; }

        [StringLength(3)]
        public string CALITERLOCUTOR { get; set; }

        [StringLength(3)]
        public string CALEXPEDIDOR { get; set; }

        [Required]
        [StringLength(35)]
        public string NOMBRE_FW { get; set; }

        [StringLength(35)]
        public string CODINTEXPE { get; set; }

        [Required]
        [StringLength(3)]
        public string ENTREGACODIG { get; set; }

        [Required]
        [StringLength(3)]
        public string ENTREGATRANS { get; set; }

        [StringLength(3)]
        public string CALPAGO { get; set; }

        [Required]
        [StringLength(3)]
        public string TIPTRANS { get; set; }

        [StringLength(8)]
        public string CODDESCTRAS { get; set; }

        [Required]
        [StringLength(35)]
        public string IDTRANSPORT { get; set; }

        [StringLength(3)]
        public string CODTIPEQUIP { get; set; }

        [Required]
        [StringLength(17)]
        public string CODEQUIP { get; set; }

        [StringLength(35)]
        public string RFF_AAJ { get; set; }

        [StringLength(35)]
        public string RFF_AAS { get; set; }

        [StringLength(35)]
        public string RFF_CRN { get; set; }

        [StringLength(35)]
        public string SHIPFROM { get; set; }

        [Required]
        [StringLength(5)]
        public string NUMTRANOLD { get; set; }

        [Required]
        [StringLength(5)]
        public string NUMTRANNEW { get; set; }

        [Required]
        [StringLength(3)]
        public string LUGENTREGA { get; set; }

        [StringLength(12)]
        public string CONTRATO { get; set; }

        [StringLength(35)]
        public string STATO { get; set; }

        public virtual DESADV_ANAGRAFICHE DESADV_ANAGRAFICHE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DESADV_IMBALLI> DESADV_IMBALLI { get; set; }
    }
}
