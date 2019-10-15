using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExtranetMVC.Models
{
    public class Fornitore
    {
        public string CLFCOD { get; set; }

        [JsonIgnore]
        public virtual ICollection<EdiTestata> EDI_TESTATA { get; set; }

    }
}