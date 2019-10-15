using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExtranetMVC.Models
{
    public class DesadvImballi
    {
        public string CLIENTE { get; set; }

        public string IDNUMDES { get; set; }

        public string IDEMB { get; set; }

        public string CPS { get; set; }

        public int CANTEMB { get; set; }

        public string MEDIOEMB { get; set; }

        public string CALMEDIOEMB { get; set; }

        public int? LONGITUD { get; set; }

        public string UMLONGITUD { get; set; }

        public int? ANCHURA { get; set; }

        public string UMANCHURA { get; set; }

        public int? ALTURA { get; set; }

        public string UMALTURA { get; set; }

        public int? CANTPAQUETE { get; set; }

        public string UNIDMEDCPAC { get; set; }

        public string INSTMARCAJE { get; set; }

        public string IDENTETIQUETA { get; set; }

        public string CALIDENTETIQUETA { get; set; }

        public string ETQMAESTRA { get; set; }

        public string FECHAFABRIC { get; set; }

        public string FECHACADUC { get; set; }

        public string NUMETIQUETA { get; set; }

        public string NUMPACCOMP { get; set; }

        public string NUMLOTE { get; set; }

        public string NUMPACPRO { get; set; }

        public string NUMETIQUETA2 { get; set; }
        public virtual ICollection<DesadvEtichette> Etichette { get; set; }
        [JsonIgnore]
        public virtual DesadvTestata Testata { get; set; }

        public virtual ICollection<DesadvRighe> Righe { get; set; }
    }
}