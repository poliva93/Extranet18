namespace Extranet_EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DESADV_ANAGRAFICHE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DESADV_ANAGRAFICHE()
        {
            DESADV_TESTATA = new HashSet<DESADV_TESTATA>();
        }

        public int ID { get; set; }

        [Key]
        [StringLength(50)]
        public string CLIENTE { get; set; }

        [StringLength(100)]
        public string RAGIONESOCIALE { get; set; }

        [StringLength(3)]
        public string TIPO { get; set; }

        [StringLength(35)]
        public string VENDEDOR { get; set; }

        [StringLength(35)]
        public string CONSIGNATARIO { get; set; }

        [StringLength(35)]
        public string NOMBRE_CN { get; set; }

        [StringLength(25)]
        public string LUGDESCARGA { get; set; }

        [StringLength(35)]
        public string SHIPFROM { get; set; }

        [StringLength(25)]
        public string FINALDESCARGA { get; set; }

        [StringLength(10)]
        public string EDI { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DESADV_TESTATA> DESADV_TESTATA { get; set; }
    }
}
