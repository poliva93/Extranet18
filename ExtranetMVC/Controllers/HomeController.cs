using Extranet_EF;
using ExtranetMVC.CustomAuthentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace ExtranetMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [CustomAuthorize(Roles = "Interni")]
        public ActionResult Report()
        {
            
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = "Interni")]
        public ActionResult Report(string sEmail, string sOggetto, string sDescrizione)
        {
            var iD = "";
            MailMessage mail = new MailMessage("edp@dellorto.it","odv@dellorto.it");
            //mail.To.Add("edp@dellorto.it");
            //mail.To.Add("paolo.oliva@dellorto.it");
            using (ExtranetDB dbContext = new ExtranetDB())
            {
                TicketWB nt = new TicketWB();
                int dToday = Int32.Parse(DateTime.Now.ToString("yyyyMMdd"));
                nt.Data = dToday;
                dbContext.TicketWB.Add(nt);
                dbContext.SaveChanges();
                iD = dbContext.TicketWB.Max(a=>a.ID_Ticket).ToString();
            }
            SmtpClient client = new SmtpClient();
            client.Port = 25;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = "mail.dellorto.it";
            mail.Subject = "Nuova segnalazione (ID: " + iD + "): " + sOggetto ;
            mail.Body = sDescrizione + Environment.NewLine + "(" + sEmail + ")";
            
            client.Send(mail);
            
            return View();
        }
    }
}