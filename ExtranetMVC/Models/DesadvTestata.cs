using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExtranetMVC.Models
{
    public class DesadvTestata
    {
        public string CLIENTE { get; set; }

        
        public string NUMDES { get; set; }

        
        public string TIPO { get; set; }

        
        public string CODTIPO { get; set; }

        
        public string FECDES { get; set; }

  
        public string FECENT { get; set; }
        
        public string FECSAL { get; set; }

        public decimal TOTPESBRU { get; set; }
        
        public string UMTOTPESBRU { get; set; }

        public decimal TOTPESNE { get; set; }
        
        public string UMTOTPESNE { get; set; }
        
        public string NUMENVCARG { get; set; }
        
        public string VENDEDOR { get; set; }
        
        public string CALVENDEDOR { get; set; }
        
        public string DIRECCION_SE { get; set; }
        
        public string POBLACION_SE { get; set; }
        
        public string NOMBRE_SE { get; set; }
        
        public string CODINTVEND { get; set; }
        
        public string CONSIGNATARIO { get; set; }
        
        public string CALCONSIGNA { get; set; }
       
        public string DIRECCION_CN { get; set; }
        
        public string POBLACION_CN { get; set; }
        
        public string NOMBRE_CN { get; set; }
        
        public string LUGDESCARGA { get; set; }
        
        public string EXPEDCARGA { get; set; }
        
        public string CALITERLOCUTOR { get; set; }
        
        public string CALEXPEDIDOR { get; set; }
        
        public string NOMBRE_FW { get; set; }
        
        public string CODINTEXPE { get; set; }
        
        public string ENTREGACODIG { get; set; }
        
        public string ENTREGATRANS { get; set; }

        public string CALPAGO { get; set; }
        
        public string TIPTRANS { get; set; }
        
        public string CODDESCTRAS { get; set; }
        
        public string IDTRANSPORT { get; set; }
        
        public string CODTIPEQUIP { get; set; }
        
        public string CODEQUIP { get; set; }
        
        public string RFF_AAJ { get; set; }
        
        public string RFF_AAS { get; set; }
        
        public string RFF_CRN { get; set; }
        
        public string SHIPFROM { get; set; }

        public string NUMTRANOLD { get; set; }

        public string NUMTRANNEW { get; set; }

        public string LUGENTREGA { get; set; }

        public string CONTRATO { get; set; }

        public string STATO { get; set; }
        [JsonIgnore]
        public virtual DesadvAnagrafica Anagrafica { get; set; }

        public virtual ICollection<DesadvImballi> Imballi { get; set; }
    }
}