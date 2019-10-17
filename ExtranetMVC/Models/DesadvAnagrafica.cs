using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExtranetMVC.Models
{
    public class DesadvAnagrafica
    {
        public int ID { get; set; }

        public string CLIENTE { get; set; }

        public string RAGIONESOCIALE { get; set; }

        public string TIPO { get; set; }

        public string VENDEDOR { get; set; }

        public string CONSIGNATARIO { get; set; }

        public string NOMBRE_CN { get; set; }

        public string LUGDESCARGA { get; set; }

        public string SHIPFROM { get; set; }

        public string FINALDESCARGA { get; set; }

        public string EDI { get; set; }


        public virtual ICollection<DesadvTestata> Testata { get; set; }
    }
}