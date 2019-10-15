namespace Extranet_EF
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Shares
    {
        //public Shares()
        //{
        //    Roles = new HashSet<Roles>();
        //}

        [Key]
        public int ShareID { get; set; }

        public string SharePath { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Roles> Roles { get; set; }
        public virtual ICollection<UserShares> UserShares { get; set; }
    }
}
