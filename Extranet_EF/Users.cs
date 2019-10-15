namespace Extranet_EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Users
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Users()
        {
            Roles = new HashSet<Roles>();
            //Shares = new HashSet<Shares>();
        }

        [Key]
        public int UserId { get; set; }

        public string Username { get; set; }

        [StringLength(10)]
        public string CodiceFornitore { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsActive { get; set; }

        public string DOI { get; set; }

        public Guid ActivationCode { get; set; }

        public virtual FORNITORE FORNITORE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Roles> Roles { get; set; }
        //public virtual ICollection<Shares> Shares { get; set; }
    }
}
