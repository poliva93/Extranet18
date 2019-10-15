namespace Extranet_EF
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Web;
    [Table("FTPabilitazioni")]
    public partial class FTPabilitazioni
    {
        
        private ICollection<UserShares> _items;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FTPabilitazioni()
        {
            _items = new List<UserShares>();
            UserShares = new HashSet<UserShares>();
        }
        [Key, Column(Order = 0), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ftpUser { get; set; }

        //[Key, Column(Order = 1)]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ftpMail { get; set; }

        public virtual ICollection<UserShares> UserShares
        {
            get
            {
                //ICollection<EdiRighe> temp
                this._items = this._items.Where(e=>e.username==ftpUser).OrderBy(e=>e.shareID).ToList(); // e.Ordinamento, e.DATA_CONSEGNA }).ToList();
                //return this._items.OrderBy(e => new { e.NUMORDINE, e.ID_TESTATA, e.ARTCOD, e.ARTVER, e.Ordinamento, e.DATA_CONSEGNA }).ToList();
                return _items;
                //this.Righe.OrderBy(EdiRighe, "NUMORDINE, ID_TESTATA, ARTCOD, ARTVER, Ordinamento, DATA_CONSEGNA");
            }
            set
            {
                this._items = value.ToList();
            }
        }

    }
}
