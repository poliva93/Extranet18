namespace Extranet_EF
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Roles
    {
        public Roles()
        {
            Users = new HashSet<Users>();
            Shares = new HashSet<Shares>();
        }

        [Key]
        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public virtual ICollection<Users> Users { get; set; }
        public virtual ICollection<Shares> Shares { get; set; }
    }
}
