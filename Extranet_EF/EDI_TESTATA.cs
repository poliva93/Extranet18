namespace Extranet_EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class EDI_TESTATA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EDI_TESTATA()
        {
            EDI_RIGHE = new HashSet<EDI_RIGHE>();
        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(12)]
        public string NUMORDINE { get; set; }

        [Required]
        [StringLength(10)]
        public string CLFCOD { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DATAPIANO { get; set; }

        [Required]
        [StringLength(50)]
        public string CLFDES { get; set; }

        [Required]
        [StringLength(200)]
        public string CLFIND { get; set; }

        [StringLength(10)]
        public string MAGAZZINO { get; set; }

        //[StringLength(MAX)]
        public string CONTATTOLOG { get; set; }

        
        //[StringLength(100)]
        public string CONTATTOFOR { get; set; }

        public DateTime? DATAVIS { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EDI_RIGHE> EDI_RIGHE { get; set; }

        public virtual FORNITORE FORNITORE { get; set; }
    }
}
