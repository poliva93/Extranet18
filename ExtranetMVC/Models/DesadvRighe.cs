using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExtranetMVC.Models
{
    public class DesadvRighe
    {
        public string CLIENTE { get; set; }

        
        public string IDNUMDES { get; set; }

       
        public string IDEMB { get; set; }

        public string NUMLIN { get; set; }

        public string REFCOMPRADOR { get; set; }

        public int? NIVELCONFIG { get; set; }

        public string REFPROVEEDOR { get; set; }

        public decimal? PESONETO { get; set; }
        
        public string UMPESONETO { get; set; }

        public int CANTENT { get; set; }

        public string UMCANTENT { get; set; }

        public string PAISORIG { get; set; }

        public string TIPOREGIMEN { get; set; }

        public string TEXTOLIBRE { get; set; }

        public decimal? VALORADUANA { get; set; }

        public string DIVISAVARADU { get; set; }

        public string NUMPEDIDO { get; set; }

        public string NUMPROGRAMA { get; set; }

        public string NUMLINPEDIDO { get; set; }

        public string INFOADICIONAL { get; set; }

        public string NUMSERIE { get; set; }

        public string NUMENGINE { get; set; }

        public string NUMVEHICLE { get; set; }
        
        public string RFF_AAP { get; set; }

        public string RFF_AAE { get; set; }

        public string DTM_AAE { get; set; }

        public string RFF_IV { get; set; }

        public string RFF_IV2 { get; set; }

        public string FINALDESCARGA { get; set; }

        public int NUMLINALB { get; set; }

        public string APPLICACION { get; set; }

        public string INVENTARIO { get; set; }
        [JsonIgnore]
        public virtual DesadvImballi Imballi { get; set; }
    }
}