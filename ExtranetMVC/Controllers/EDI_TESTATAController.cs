using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Extranet_EF;
using ExtranetMVC.CustomAuthentication;
using System.Linq;

namespace ExtranetMVC.Controllers
{
    [CustomAuthorize(Roles= "Fornitore_EDI,Admin")]
    public class EDI_TESTATAController : Controller
    {
        private ExtranetDB db = new ExtranetDB();

        // GET: EDI_TESTATA
        public async Task<ActionResult> Index()
        {
            var user = db.Users.Where(u => u.Username == User.Identity.Name).FirstOrDefault();
            if (user != null)
            {
                var eDI_TESTATA = db.EDI_TESTATA.Where(t => t.CLFCOD == user.CodiceFornitore).Include(e => e.FORNITORE);
                return View(await eDI_TESTATA.ToListAsync());
            }
            return View();
        }

        // GET: EDI_TESTATA/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EDI_TESTATA eDI_TESTATA = await db.EDI_TESTATA.FindAsync(id);
            if (eDI_TESTATA == null)
            {
                return HttpNotFound();
            }
            return View(eDI_TESTATA);
        }

        // GET: EDI_TESTATA/Create
        public ActionResult Create()
        {
            ViewBag.CLFCOD = new SelectList(db.FORNITORE, "CLFCOD", "CLFCOD");
            return View();
        }

        // POST: EDI_TESTATA/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,NUMORDINE,CLFCOD,DATAPIANO,CLFDES,CLFIND,MAGAZZINO,CONTATTOLOG,CONTATTOFOR,DATAVIS")] EDI_TESTATA eDI_TESTATA)
        {
            if (ModelState.IsValid)
            {
                db.EDI_TESTATA.Add(eDI_TESTATA);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CLFCOD = new SelectList(db.FORNITORE, "CLFCOD", "CLFCOD", eDI_TESTATA.CLFCOD);
            return View(eDI_TESTATA);
        }

        // GET: EDI_TESTATA/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EDI_TESTATA eDI_TESTATA = await db.EDI_TESTATA.FindAsync(id);
            if (eDI_TESTATA == null)
            {
                return HttpNotFound();
            }
            ViewBag.CLFCOD = new SelectList(db.FORNITORE, "CLFCOD", "CLFCOD", eDI_TESTATA.CLFCOD);
            return View(eDI_TESTATA);
        }

        // POST: EDI_TESTATA/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,NUMORDINE,CLFCOD,DATAPIANO,CLFDES,CLFIND,MAGAZZINO,CONTATTOLOG,CONTATTOFOR,DATAVIS")] EDI_TESTATA eDI_TESTATA)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eDI_TESTATA).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CLFCOD = new SelectList(db.FORNITORE, "CLFCOD", "CLFCOD", eDI_TESTATA.CLFCOD);
            return View(eDI_TESTATA);
        }

        // GET: EDI_TESTATA/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EDI_TESTATA eDI_TESTATA = await db.EDI_TESTATA.FindAsync(id);
            if (eDI_TESTATA == null)
            {
                return HttpNotFound();
            }
            return View(eDI_TESTATA);
        }

        // POST: EDI_TESTATA/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            EDI_TESTATA eDI_TESTATA = await db.EDI_TESTATA.FindAsync(id);
            db.EDI_TESTATA.Remove(eDI_TESTATA);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
