using AutoMapper;
using Extranet_EF;
using ExtranetMVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;



namespace ExtranetMVC.Api
{


    public class DesadvTestataController : ApiController
    {
        private DesadvDB db = new DesadvDB();

        [HttpGet]
        [Route("api/desadv/{cliente}")]
        public IHttpActionResult testate(string cliente, string ID)
        {
            var testate = db.DESADV_TESTATA.Where(t => t.CLIENTE == cliente && (ID == null || t.NUMDES == ID) && t.STATO != "CHIUSO").OrderByDescending(o => o.NUMDES).ToList();
            return Ok(Mapper.Map<List<DesadvTestata>>(testate));

        }
        [HttpGet]
        [Route("api/desadv/ImballoStandard/{bolla}")]
        public IHttpActionResult ImballoStandard(string bolla)
        {
            string sESECOD = bolla.Substring(0, 4);
            string sBAMSEZ = bolla.Substring(4, 2);
            string sBAMNRR = bolla.Substring(6);
            bool bMultipli = false;
            ConnectionStringSettings cnnString = ConfigurationManager.ConnectionStrings["AlnusConnection"];
            string strConn = cnnString.ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(strConn))
            {
                sqlConnection.Open();
                string StrSql = "select top(1) ARTCOD, ORCCLI from BAMDET00 where ESECOD=" + sESECOD + " and BAMSEZ='" + sBAMSEZ + "' and BAMNRR=" + sBAMNRR;
                SqlCommand sqlCmd = new SqlCommand(StrSql, sqlConnection);
                SqlDataReader reader = sqlCmd.ExecuteReader();
                reader.Read();
                string sArticolo = reader["ARTCOD"].ToString().TrimEnd();
                string sCliente = reader["ORCCLI"].ToString().TrimEnd();
                var imballi = db.DESADV_ANA_IMBALLI.Where(i => i.ARTCOD == sArticolo && i.imbStandard != "S" && i.CLIENTE == sCliente).ToList();
                if (imballi.Count != 0)
                {
                    bMultipli = true;
                }


            }
            return Ok(bMultipli.ToString());

        }
        //
        [HttpGet]
        [Route("api/desadv/ListaClienti/")]
        public IHttpActionResult ListaClienti()
        {
            //var clienti = db.DESADV_ANAGRAFICHE.Select(a => a.CLIENTE).ToList(); //db.DESADV_TESTATA.Where(t => t.CLIENTE == cliente && (ID == null || t.NUMDES == ID)).ToList();
            //modifica del 20190409: per messa in produzione metto nel testo codice
            var clienti = db.DESADV_ANAGRAFICHE.Select(a => new { id = a.CLIENTE, nome = a.RAGIONESOCIALE }).ToList();
            return Ok(clienti);

        }
        //[HttpGet]
        //[Route("api/desadv/ListaSpedizioni/{sCliente?}")]
        //public IHttpActionResult ListaSpedizioni(string sCliente="")
        //{
        //    //lista spedizioni x visualizzazione deufol

        //    var spedizioni2 = db.DESADV_RIGHE.Include(d => d.DESADV_IMBALLI.DESADV_TESTATA)
        //        .Select(x => new {
        //            Cliente = x.DESADV_IMBALLI.DESADV_TESTATA.CLIENTE,
        //            Numero = x.DESADV_IMBALLI.DESADV_TESTATA.NUMDES,
        //            DataSpedizione = x.DESADV_IMBALLI.DESADV_TESTATA.FECDES,
        //            Bolla = x.DESADV_IMBALLI.DESADV_TESTATA.CODEQUIP,
        //            Articolo = x.REFPROVEEDOR,
        //            Quantita = x.CANTENT,
        //            Stato = x.DESADV_IMBALLI.DESADV_TESTATA.STATO
        //        })
        //        .GroupBy(x=> new { x.Cliente, x.Numero, x.DataSpedizione, x.Bolla, x.Articolo, x.Stato}, (key, group) => new
        //        {
        //            Cliente=key.Cliente,
        //            Numero=key.Numero,
        //            DataSpedizione=key.DataSpedizione,
        //            Bolla=key.Bolla,
        //            Articolo=key.Articolo,
        //            Quantita = group.Sum(k => k.Quantita),
        //            Stato =key.Stato

        //        })
        //        //.OrderByDescending(e => new { e.Cliente,e.Numero})
        //        .OrderByDescending(e => new { e.Cliente, e.Numero.Length, e.Numero })
        //        .ToList();
        //    if (sCliente!="")
        //    {
        //        spedizioni2=spedizioni2.FindAll(e => e.Cliente==sCliente);
        //    }


        //   // spedizioni2.ConvertAll(x => Int32.Parse(x.Numero));

        //   //db.DESADV_ANAGRAFICHE.Select(a => new { id = a.CLIENTE, nome = a.RAGIONESOCIALE }).ToList();
        //    return Ok(spedizioni2);

        //}

        [HttpGet]
        [Route("api/desadv/ListaSpedizioni/{sCliente?}")]
        public IHttpActionResult ListaSpedizioni(string sCliente = "")
        {
            //lista spedizioni x visualizzazione deufol

            var spedizioni2 = db.DESADV_RIGHE.Include(d => d.DESADV_IMBALLI.DESADV_TESTATA)
                .Select(x => new
                {
                    Cliente = x.DESADV_IMBALLI.DESADV_TESTATA.CLIENTE,
                    ID_ASN = x.DESADV_IMBALLI.DESADV_TESTATA.ID_ASN,
                    DataSpedizione = x.DESADV_IMBALLI.DESADV_TESTATA.FECDES,
                    Bolla = x.DESADV_IMBALLI.DESADV_TESTATA.CODEQUIP,
                    Articolo = x.REFPROVEEDOR,
                    Quantita = x.CANTENT,
                    Stato = x.DESADV_IMBALLI.DESADV_TESTATA.STATO
                })
                .GroupBy(x => new { x.Cliente, x.ID_ASN, x.DataSpedizione, x.Bolla, x.Articolo, x.Stato }, (key, group) => new
                {
                    Cliente = key.Cliente,
                    ID_ASN = key.ID_ASN,
                    DataSpedizione = key.DataSpedizione,
                    Bolla = key.Bolla,
                    Articolo = key.Articolo,
                    Quantita = group.Sum(k => k.Quantita),
                    Stato = key.Stato

                })
                //.OrderByDescending(e => new { e.Cliente,e.Numero})
                .OrderByDescending(e => new { e.Cliente, e.ID_ASN })
                .ToList();
            if (sCliente != "")
            {
                spedizioni2 = spedizioni2.FindAll(e => e.Cliente == sCliente);
            }


            // spedizioni2.ConvertAll(x => Int32.Parse(x.Numero));

            //db.DESADV_ANAGRAFICHE.Select(a => new { id = a.CLIENTE, nome = a.RAGIONESOCIALE }).ToList();
            return Ok(spedizioni2);

        }

        [HttpGet]
        [Route("api/desadv/Etichette/{Cliente}/{Numero}")]
        public IHttpActionResult Etichette(string Cliente, int Numero)
        {
            try
            {
                string EDI = "";
                int codCliente = 0;
                string pNetto, pLordo, dock;
                var anagrafica = db.DESADV_ANAGRAFICHE.Where(d => d.CLIENTE == Cliente).FirstOrDefault();
                if (anagrafica != null)
                {
                    EDI = anagrafica.EDI;
                    codCliente = anagrafica.ID;
                }
                var testata = db.DESADV_TESTATA.Where(d => d.CLIENTE == Cliente && d.ID_ASN == Numero).FirstOrDefault();
                int iASN;
                if (testata != null)
                {
                    iASN = testata.ID_ASN;
                    dock = testata.LUGDESCARGA;
                    pNetto = testata.TOTPESNE.ToString();
                    pLordo = testata.TOTPESBRU.ToString();
                }
                else
                {
                    var rispostaErrore = new { status = "500", data = "ERRORE. Contattare edp@dellorto.it" };
                    return Json(rispostaErrore);
                }
                var etichette2 = db.DESADV_ETICHETTE.Where(d => d.CLIENTE == Cliente && d.IDNUMDES == testata.NUMDES);
                /*
                 * select CLIENTE, IDNUMDES, IDEMB, HANTYPE,MIN(NUMETIQUETA) NUMETIQUETA, MAX(NUMETIQUETA2) NUMETIQUETA2
    from DESADV_ETICHETTE
    where CLIENTE='310310'
    and IDNUMDES=81
                 * */
                var etichette = db.DESADV_ETICHETTE
                    .Where(x => x.IDNUMDES == testata.NUMDES && x.CLIENTE == Cliente)
                   .Select(x => new
                   {
                       ASN = x.CLIENTE,
                       IDEMB = x.IDEMB,
                       HANTYPE = x.HANTYPE,
                       NUMETIQUETA = x.NUMETIQUETA,
                       NUMETIQUETA2 = x.NUMETIQUETA2
                   })
                   .GroupBy(x => new { x.ASN, x.IDEMB, x.HANTYPE }, (key, group) => new
                   {
                       ASN = key.ASN,
                       IDEMB = key.IDEMB,
                       HANTYPE = key.HANTYPE,
                       NUMETIQUETA = group.Min(k => k.NUMETIQUETA),
                       NUMETIQUETA2 = group.Max(k => k.NUMETIQUETA2)
                   })
                   //.OrderByDescending(e => new { e.Cliente,e.Numero})
                   .OrderByDescending(e => new { e.IDEMB })
                   .ToList();


                if (EDI == "EDIFACT")
                {
                    var etichetteM = db.DESADV_IMBALLI
                                    .Where(x => x.CLIENTE == Cliente && x.IDNUMDES == testata.NUMDES)
                                    .Select(x => new
                                    {
                                        ASN = x.CLIENTE,
                                        IDEMB = x.IDEMB,
                                        HANTYPE = x.IDENTETIQUETA,
                                        NUMETIQUETA = x.ETQMAESTRA,
                                        NUMETIQUETA2 = x.ETQMAESTRA
                                    })
                                    .OrderByDescending(e => e.IDEMB)
                                    .ToList();
                    foreach (var m in etichetteM) //il simpatico C# mi rompe perchè non riesce a unire i due oggetti in quanto numetiqueta 1 e 2 sono int in etichette e string in etichetteM
                    {

                        var tmp = new { ASN = Cliente, IDEMB = m.IDEMB, HANTYPE = m.HANTYPE, NUMETIQUETA = Int32.Parse(m.NUMETIQUETA), NUMETIQUETA2 = Utility.ToNullableInt(m.NUMETIQUETA2) };
                        etichette.Add(tmp);
                    }

                } //codCliente.ToString() + Numero.ToString()
                  //dovrò ordinarlo?
                var Modello = new { ASN = "", IDEMB = "", HANTYPE = "", NUMETIQUETA = 0, NUMETIQUETA2 = Utility.ToNullableInt(""), pNetto = "", pLordo = "", Dock = "" };
                var labels = (new[] { Modello }).ToList();
                foreach (var m in etichette)
                {
                    var tmp = new { ASN = codCliente.ToString() + Numero.ToString(), IDEMB = m.IDEMB, HANTYPE = m.HANTYPE, NUMETIQUETA = m.NUMETIQUETA, NUMETIQUETA2 = m.NUMETIQUETA2, pNetto = pNetto, pLordo = pLordo, Dock = dock };
                    if (m.NUMETIQUETA != 0 && m.NUMETIQUETA2 != 0)
                    {
                        labels.Add(tmp);
                    }
                }
                labels.Remove(Modello);
                labels = labels.OrderBy(x => x.IDEMB).ToList();
                if (labels == null)
                {
                    var rispostaErrore = new { status = "500", data = "ERRORE. Contattare edp@dellorto.it" };
                    return Json(rispostaErrore);
                }
                object risultato = new { test = 1 };
                return Ok(labels);

            }
        catch (Exception ex)
            {
                var rispostaErrore = new { status = "500", data = "ERRORE. Contattare edp@dellorto.it" };
                return Json(rispostaErrore);
            }
}

        
       



        [HttpGet]
        [Route("api/desadv/ClienteTrasportatori/{Cliente}")]
        public IHttpActionResult ClienteTrasportatori(string Cliente)
        {
            var trasportatori = db.DESADV_TRASPORTATORE.Where(t => t.CLIENTE == Cliente).Select(t => t.Descrizione).ToList();
            return Ok(trasportatori);

        }



        [HttpGet]
        [Route("api/desadv/ClienteBolle/{Cliente}")]
        public IHttpActionResult ClienteBolle(string Cliente) //, string Partenza)
        {
            if (Cliente == null)
            {
                return Content(HttpStatusCode.NoContent, "Cliente non trovato");
            }

            ConnectionStringSettings cnnString = ConfigurationManager.ConnectionStrings["AlnusConnection"];
            string strConn = cnnString.ConnectionString;
            var bolle = new List<DESADV_BOLLE>();
            using (SqlConnection sqlConnection = new SqlConnection(strConn))
            {
                sqlConnection.Open();
                string StrSql = "SELECT  b.ESECOD + b.BAMSEZ + cast(b.BAMNRR as varchar) AS NostraBolla,   isnull(rtrim(s.BDIDEM) + ' / ' + rtrim(b.BOLESE) + rtrim(b.BOLSEZ) + rtrim(cast(b.BOLNRR as varchar)) + ' del ' + cast(s.BDIDTDREV as varchar),   rtrim(b.BOLESE) + rtrim(b.BOLSEZ) + rtrim(cast(b.BOLNRR as varchar))  + ' del ' + cast(BAMDDOREV as varchar) ) AS Bolla ,isnull(s.BDIDTDREV,BAMDDOREV) AS DataBolla, s.BDIDEM as LoroBolla "
                + " FROM BAMTES00 AS b LEFT JOIN BAMDIS00 AS s ON b.ESECOD = s.ESECOD AND b.BAMSEZ = s.FATSEZ AND b.BAMNRR = s.FATNRP "
                + " WHERE (b.ORCCLI = '" + Cliente.ToString() + "') and BAMDDOREV>20190801 ORDER BY b.BAMDDOREV DESC ";// and not exists (SELECT RFF_AAS FROM DESADV_TESTATA WHERE RFF_AAS =  b.ESECOD + b.BAMSEZ + cast(b.BAMNRR as varchar)) ORDER BY b.BAMDDOREV DESC"; // and not exists (SELECT RFF_AAS FROM DESADV_TESTATA WHERE RFF_AAS = s.BDIDEM ) ";

                SqlCommand sqlCmd = new SqlCommand(StrSql, sqlConnection);
                SqlDataReader reader = sqlCmd.ExecuteReader();
                //reader.Read();
                //record = reader["NostraBolla"].ToString();
                Boolean test = true;
                do
                {
                    if (reader.Read() == true)
                    {
                        //controllo da riattivare
                        if (reader["LoroBolla"].ToString() == "DO392381")
                        {
                            test = false;
                        }
                        string sTemp = reader["NostraBolla"].ToString();
                        var testata = db.DESADV_TESTATA.Where(a => a.CODEQUIP == sTemp).Select(a => a.CODEQUIP).FirstOrDefault();

                        if (testata == null)
                        {
                            if (reader["LoroBolla"].ToString() != "")
                            {
                                sTemp = reader["LoroBolla"].ToString();
                                testata = db.DESADV_TESTATA.Where(a => a.CODEQUIP == sTemp).Select(a => a.CODEQUIP).FirstOrDefault();
                            }
                        }
                        if (testata == null)
                        {
                            bolle.Add(new DESADV_BOLLE() { id = reader["NostraBolla"].ToString(), bolla = reader["Bolla"].ToString(), data = reader["DataBolla"].ToString() });
                        }
                    }
                    else
                    {
                        test = false;
                    }

                } while (test == true);
                sqlConnection.Close();
            }
            //ViewBag.Records = record;
            //var viewTestate = Mapper.Map<List<EdiTestata>>(bolle);
            return Ok(bolle);
        }
    }
}
