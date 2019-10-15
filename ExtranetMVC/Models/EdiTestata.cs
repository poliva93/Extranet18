using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExtranetMVC.Models
{
    public class EdiTestata
    {
        public EdiTestata()
        {
            _items = new List<EdiRighe>();
        }
        public int ID { get; set; }

        public string NUMORDINE { get; set; }

        public string CLFCOD { get; set; }
        
        public DateTime DATAPIANO { get; set; }

        public string DataData
        {
            get
            {
                return DATAPIANO.ToString("dd/MM/yyyy");
            }
        }
        public string CLFDES { get; set; }

        public string CLFIND { get; set; }

        public string MAGAZZINO { get; set; }

        public string CONTATTOLOG { get; set; }

        public string CONTATTOFOR { get; set; }

        //public DateTime? DATAVIS { get; set; }
        public DateTime? DATAVIS { get; set; }

        private ICollection<EdiRighe> _items;
        

        public virtual ICollection<EdiRighe> Righe {
            get
            {
                //ICollection<EdiRighe> temp
                this._items = this._items.OrderBy(e => e.NUMORDINE).ThenBy(e => e.ID_TESTATA).ThenBy(e => e.ARTCOD).ThenBy(e => e.ARTVER).ThenBy(e => e.Ordinamento).ThenBy(e => e.DATA_CONSEGNA).ToList(); // e.Ordinamento, e.DATA_CONSEGNA }).ToList();
                //return this._items.OrderBy(e => new { e.NUMORDINE, e.ID_TESTATA, e.ARTCOD, e.ARTVER, e.Ordinamento, e.DATA_CONSEGNA }).ToList();
                return _items;
                //this.Righe.OrderBy(EdiRighe, "NUMORDINE, ID_TESTATA, ARTCOD, ARTVER, Ordinamento, DATA_CONSEGNA");
            }
            set
            {
                this._items = value.ToList();
            } }
        



        //public virtual ICollection<EdiRighe> Righe {
        //    get {
        //        //var temp = Righe.ToList().OrderBy(e => new { e.NUMORDINE, e.ID_TESTATA, e.ARTCOD, e.ARTVER, e.Ordinamento, e.DATA_CONSEGNA });
        //        //Righe.OrderBy(e => new { e.NUMORDINE, e.ID_TESTATA, e.ARTCOD, e.ARTVER, e.Ordinamento, e.DATA_CONSEGNA });

        //        return RigheNO.OrderBy(e => new { e.NUMORDINE, e.ID_TESTATA, e.ARTCOD, e.ARTVER, e.Ordinamento, e.DATA_CONSEGNA }).ToList();
                    
        //         }
        //}

        public virtual Fornitore FORNITORE { get; set; }
    }
}