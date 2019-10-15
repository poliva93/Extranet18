using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExtranetMVC.Models
{
    public class DesadvAnagraficaImballi
    {
        public string ARTCOD { get; set; }

        public string CLIENTE { get; set; }

        public int? BOXNUM { get; set; }

        public string BOXCOD { get; set; }

        public int? PARTXBOX { get; set; }

        public string PALLETCOD { get; set; }

        public string BOXCODDO { get; set; }

        public string PALLETCODDO { get; set; }

        public string LUGDESCARGA { get; set; }

        public int Sort { get; set; }

        public string Origine { get; set; }
    }
}