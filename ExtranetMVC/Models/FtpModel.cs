using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExtranetMVC.Models
{
    public class FtpModel
    {
        public string FileName { get; set; }
        public string Folder { get; set; }
        public string Type { get; set; }
        public string Icon { get; set; }
        public string Size { get; set; }
        public bool Selected { get; set; }
        public string LastEdit { get; set; }
        public string ReadWrite { get; set; }
    }
}

