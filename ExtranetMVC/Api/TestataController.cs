using AutoMapper;
using Extranet_EF;
using ExtranetMVC.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web;
using System.Web.Http;
using System.Web.Security;

namespace ExtranetMVC.Api
{
    public class TestataController : ApiController
    {
        private ExtranetDB db = new ExtranetDB();

        [HttpGet]
        [Route("api/testata/{username}/{ordine}")]
        public IHttpActionResult testate(string username, string ordine)
        {
            var user = db.Users.Where(u => u.Username == username).FirstOrDefault();
            var authCookie = HttpContext.Current.Request.Cookies["edi_auth"];

            if (authCookie == null) return Content(HttpStatusCode.NoContent, "Utente non trovato"); ;
            var cookieValue = authCookie.Value;

            if (String.IsNullOrWhiteSpace(cookieValue)) return Content(HttpStatusCode.NoContent, "Utente non trovato"); 
            var ticket = FormsAuthentication.Decrypt(cookieValue);

            var test= JsonConvert.DeserializeObject(ticket.UserData);
            JObject jObject = JObject.Parse(JsonConvert.DeserializeObject(ticket.UserData).ToString());
            string interno = (string)jObject.SelectToken("DOI");
            

            if (interno!="y")
            {
                if (user == null)
                {

                    return Content(HttpStatusCode.NoContent, "Utente non trovato");
                }
            }
            if (interno=="y")
            {
                
                var testate = db.EDI_TESTATA.Where(t => t.NUMORDINE == ordine)
                                          .OrderByDescending(t => t.ID)
                                          .ToList();
                try
                {

                    var righe = db.EDI_RIGHE.Where(t => t.ID_TESTATA == 4 && t.NUMORDINE == ordine).ToList();
                }
                catch (Exception e)
                    {
                    var a=e.InnerException;
                    var b = e;

                }
                //testate = testate.o OrderBy(e => e.EDI_RIGHE.OrderBy(f => new { f.ID_TESTATA, f.NUMORDINE, f.ARTCOD, f.ARTVER, f.ARTDES, f.ARTUM, f.Ordinamento, f.DATA_CONSEGNA })).ToList();
                
                var viewTestate = Mapper.Map<List<EdiTestata>>(testate);
                
                return Ok(viewTestate);
            }
            else
            {
                var testate = db.EDI_TESTATA.Where(t => t.CLFCOD == user.CodiceFornitore && (ordine == null || t.NUMORDINE == ordine))
                                           .OrderByDescending(t => t.ID)
                                           .ToList();
                //testate.OrderBy(t => t.EDI_RIGHE.OrderBy(e => new { e.NUMORDINE, e.ID_TESTATA, e.ARTCOD, e.ARTVER, e.Ordinamento }));
                var viewTestate = Mapper.Map<List<EdiTestata>>(testate);
                
                //viewTestate.OrderBy(t => { t.ID}, t.Righe.OrderBy(e => new { e.NUMORDINE, e.ID_TESTATA, e.ARTCOD, e.ARTVER, e.Ordinamento })});
                return Ok(viewTestate);
            }
            // var viewTestate = Mapper.Map<List<EdiTestata>>(testate);
            // return Ok(viewTestate);
        }



        [HttpGet]
        [Route("api/ordine/{username}")]
        public IHttpActionResult ordini(string username)
        {
            var user = db.Users.Where(u => u.Username == username).FirstOrDefault();
            if (user == null)
            {
                return Content(HttpStatusCode.NoContent, "Utente non trovato");
            }
            var ordini = db.EDI_TESTATA.Where(t => t.CLFCOD == user.CodiceFornitore)
                                        .Select(t => t.NUMORDINE)
                                        .Distinct()
                                        .OrderByDescending(o => o);


            return Ok(ordini);
        }
        [HttpGet]
        [Route("api/ordineChecked/{username}/{ordine}/{testata}")]
        public IHttpActionResult ordineChecked(string username, string ordine, int testata)
        {
            var user = db.Users.Where(u => u.Username == username).FirstOrDefault();
            if (user == null)
            {
                return Content(HttpStatusCode.NoContent, "Utente non trovato");
            }

            EDI_TESTATA testataVista = db.EDI_TESTATA.Where(a => a.NUMORDINE == ordine && a.ID == testata).Single();
            if (testataVista.DATAVIS != null)
            {
                return Content(HttpStatusCode.NoContent, "");
            }
            if (testataVista.DATAVIS == null && testataVista.CLFCOD == User.Identity.Name)
            {
                DateTime myDateTime = DateTime.Now;
                string sqlFormattedDate = myDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                testataVista.DATAVIS = myDateTime;
                db.SaveChanges();
                //MailMessage mail = new MailMessage("edp@dellorto.it", "paolo.oliva@dellorto.it");
                //mail.To.Add("paolo.berizzi@dellorto.it, marco.gerosa@dellorto.it");
                MailMessage mail = new MailMessage("dos@dellorto.it", "edp@dellorto.it");
                string[] indirizzi = testataVista.CONTATTOLOG.ToString().Split(';');
                foreach (var indirizzo in indirizzi)
                {
                    mail.To.Add(indirizzo);
                }

                SmtpClient client = new SmtpClient();
                client.Port = 25;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Host = "mail.dellorto.it";
                mail.Subject = "Piano visualizzato: " + testataVista.CLFDES + " " + ordine + " piano del " + testataVista.DATAPIANO.ToString("yyyy-MM-dd HH:mm:ss.fff");
                mail.Body = "Fornitore: " + username +" " + testataVista.CLFDES + Environment.NewLine + "Ordine: " + ordine + Environment.NewLine + "Data Piano: " + testataVista.DATAPIANO.ToString("yyyy-MM-dd HH:mm:ss.fff") + Environment.NewLine + "Visualizzato alle ore: " + sqlFormattedDate;
                client.Send(mail);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, "Utente non valido");
            }
            return Ok("Update eseguito con orario: " + DateTime.Now.ToString() + " per i seguenti parametri: " + username + ' ' + ordine + ' ' + testata);
        }
    }
}
