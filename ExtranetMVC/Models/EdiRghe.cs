using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ExtranetMVC.Models
{
    public class EdiRighe
    {

        public string Group
        {
            get
            {
                return ID_TESTATA + NUMORDINE + ARTCOD + ARTVER + ARTDES + ARTUM;
            }
        }
        public int ID_TESTATA { get; set; }

        public string NUMORDINE { get; set; }

        public string ARTCOD { get; set; }

        public string ARTVER { get; set; }

        public string ARTDES { get; set; }

        public decimal ARTQTA { get; set; }

        public string TIPOLOGIA { get; set; }

        public string ARTUM { get; set; }

        public decimal? DATA_CONSEGNA { get; set; }
        //public decimal? DATA_CONSEGNA_SHORT
        //{
        //    get
        //    {
        //        if (DATA_CONSEGNA == null)
        //        {
        //            return 0;
        //        }
        //        return DATA_CONSEGNA;

        //    }
        //}
        public string DATA_CONSEGNA_SHORT
        {
            get
            {
                if (DATA_CONSEGNA == null)
                {
                    return "";
                }
                Int32 temp = Convert.ToInt32(DATA_CONSEGNA.Value);
                string data = temp.ToString();
                string monthName = "";
                //gestisco il mese
                if (data.Length == 8 && data.Substring(data.Length - 2, 2) == "00")
                {
                    temp = Convert.ToInt32(data.Substring(4, 2));
                    monthName = data.Substring(0, 4) + "  ";
                    monthName += CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(temp);
                    return monthName;

                }
                //gestisco la settimana
                if (data.Length == 8 && data.Substring(4, 2) == "00")
                {
                    monthName = data.Substring(0, 4);
                    monthName += " week " + data.Substring(data.Length - 2, 2);
                    return monthName;
                }

                //gestisco il giorno


                monthName = data.Substring(data.Length - 2, 2) + "/";
                monthName += data.Substring(4, 2) + "/";
                monthName += data.Substring(0, 4);
                return monthName;

            }
        }

        public string LAST_DDT { get; set; }

        public string LAST_DDT_F { get; set; }

        public DateTime? LAST_DATA { get; set; }


        public string LAST_DATA_SHORT
        {
            get
            {
                if (LAST_DATA == null)
                {
                    return "";
                }

                return LAST_DATA.Value.ToShortDateString();
            }
        }


        public decimal? LAST_QTA { get; set; }

        public decimal? PROG_ARTICOLO { get; set; }

        public int Ordinamento { get; set; }

        public Int64 rank { get; set; }
        [JsonIgnore]
        public virtual EdiTestata Testata { get; set; }
    }
}