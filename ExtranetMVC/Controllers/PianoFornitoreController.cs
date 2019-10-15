using ExtranetMVC.CustomAuthentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExtranetMVC.Api;
using Extranet_EF;

namespace ExtranetMVC.Controllers
{
    
    [CustomAuthorize(Roles = "Fornitore_EDI,Admin,Gruppo_EDP, Gruppo_Logistica")]
    public class PianoFornitoreController : Controller
    {
        private ExtranetDB db = new ExtranetDB();
        // GET: PianoFornitore
        public ActionResult Index()
        {
            if (User.IsInRole("Gruppo_EDP")|| User.IsInRole("Gruppo_Logistica"))
                {
                SelectListItem selListItem = new SelectListItem() { Text = "Seleziona un Fornitore" , Value="0"};
                SelectList clienti = new SelectList(db.EDI_TESTATA.Select(t =>  new {  t.CLFCOD, Descrizione = string.Concat(t.CLFCOD , " - " , t.CLFDES)  }).Distinct().ToList(),"CLFCOD","Descrizione");
                List<SelectListItem> newList = clienti.ToList();
                newList.Insert(0, selListItem);
                //ViewBag.lClienti = db.EDI_TESTATA.Select(t => t.CLFCOD).Distinct().ToList();
                var selectedItemValue = String.Empty;
                ViewBag.lClienti =  new SelectList(newList.Select(t=>new { t.Text, t.Value }).ToList(),"Value","Text"); 
            }
            return View();
        }
        
    }
}