namespace Extranet_EF
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("FORNITORE")]
    public partial class FORNITORE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FORNITORE()
        {
            EDI_TESTATA = new HashSet<EDI_TESTATA>();
            Users = new HashSet<Users>();
        }

        [Key]
        [StringLength(10)]
        public string CLFCOD { get; set; }

        public virtual ICollection<EDI_TESTATA> EDI_TESTATA { get; set; }

        public virtual ICollection<Users> Users { get; set; }
    }
}
