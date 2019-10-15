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
            var testate = db.DESADV_TESTATA.Where(t => t.CLIENTE == cliente && (ID == null || t.NUMDES == ID) && t.STATO !="CHIUSO").OrderByDescending(o => o.NUMDES).ToList();
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
                string sCliente= reader["ORCCLI"].ToString().TrimEnd();
                var imballi = db.DESADV_ANA_IMBALLI.Where(i => i.ARTCOD == sArticolo && i.imbStandard != "S" && i.CLIENTE==sCliente).ToList();
                if (imballi.Count !=0)
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
            var clienti = db.DESADV_ANAGRAFICHE.Select(a => new { id = a.CLIENTE, nome= a.RAGIONESOCIALE }).ToList();
            return Ok(clienti);

        }
        [HttpGet]
        [Route("api/desadv/ListaSpedizioni/{sCliente?}")]
        public IHttpActionResult ListaSpedizioni(string sCliente="")
        {
            //lista spedizioni x visualizzazione deufol

            var spedizioni2 = db.DESADV_RIGHE.Include(d => d.DESADV_IMBALLI.DESADV_TESTATA)
                .Select(x => new {
                    Cliente = x.DESADV_IMBALLI.DESADV_TESTATA.CLIENTE,
                    Numero = x.DESADV_IMBALLI.DESADV_TESTATA.NUMDES,
                    DataSpedizione = x.DESADV_IMBALLI.DESADV_TESTATA.FECDES,
                    Bolla = x.DESADV_IMBALLI.DESADV_TESTATA.CODEQUIP,
                    Articolo = x.REFPROVEEDOR,
                    Quantita = x.CANTENT,
                    Stato = x.DESADV_IMBALLI.DESADV_TESTATA.STATO
                })
                .GroupBy(x=> new { x.Cliente, x.Numero, x.DataSpedizione, x.Bolla, x.Articolo, x.Stato}, (key, group) => new
                {
                    Cliente=key.Cliente,
                    Numero=key.Numero,
                    DataSpedizione=key.DataSpedizione,
                    Bolla=key.Bolla,
                    Articolo=key.Articolo,
                    Quantita = group.Sum(k => k.Quantita),
                    Stato =key.Stato
                    
                })
                //.OrderByDescending(e => new { e.Cliente,e.Numero})
                .OrderByDescending(e => new { e.Cliente, e.Numero.Length, e.Numero })
                .ToList();
            if (sCliente!="")
            {
                spedizioni2=spedizioni2.FindAll(e => e.Cliente==sCliente);
            }


           // spedizioni2.ConvertAll(x => Int32.Parse(x.Numero));

           //db.DESADV_ANAGRAFICHE.Select(a => new { id = a.CLIENTE, nome = a.RAGIONESOCIALE }).ToList();
            return Ok(spedizioni2);

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
                        if (reader["LoroBolla"].ToString()== "DO392381")
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
