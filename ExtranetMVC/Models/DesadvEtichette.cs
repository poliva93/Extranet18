using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExtranetMVC.Models
{
    public class DesadvEtichette
    {
        public string CLIENTE { get; set; }

        public string IDNUMDES { get; set; }

        public string IDEMB { get; set; }

        public string IDETQ { get; set; }

        public string IDETQPACK { get; set; }

        public int NUMETIQUETA { get; set; }

        public string NUMPACCOMP { get; set; }

        public string NUMLOTE { get; set; }

        public string HANNUM { get; set; }

        public string AGENCY { get; set; }

        public string HANTYPE { get; set; }

        public int NUMETIQUETA2 { get; set; }
        [JsonIgnore]
        public virtual DesadvImballi Imballi { get; set; }
    }
}