namespace Extranet_EF
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    [Table("UserSharesPath")]
    public partial class UserShares
    {
  

        [Key, Column(Order = 0), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string username { get; set; }

        [Key, Column(Order = 1), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int shareID { get; set; }
        
        public string abilitazione { get; set; }

        public string SharePath { get; set; }
        //public virtual ICollection<Shares> Shares { get; set; }
        public virtual Shares Shares { get; set; }
        public virtual FTPabilitazioni FTPabilitazioni { get; set; }
    }
}
