using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KarateJanNine.Controllers
{
    public class CommissionController : Controller
    {
        private RechargeEntities db = new RechargeEntities();

        //
        // GET: /Commission/

        public ActionResult Index()
        {
            var commissions = db.Commissions.Include(c => c.Provider);
            return View(commissions.ToList());
        }

        //
        // GET: /Commission/Details/5

        public ActionResult Details(int id = 0)
        {
            Commission commission = db.Commissions.Find(id);
            if (commission == null)
            {
                return HttpNotFound();
            }
            return View(commission);
        }

        //
        // GET: /Commission/Create

        public ActionResult Create()
        {
            ViewBag.ProviderID = new SelectList(db.Providers, "ProviderID", "ProviderName");
            return View();
        }

        //
        // POST: /Commission/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Commission commission)
        {
            if (ModelState.IsValid)
            {
                db.Commissions.Add(commission);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProviderID = new SelectList(db.Providers, "ProviderID", "ProviderName", commission.ProviderID);
            return View(commission);
        }

        //
        // GET: /Commission/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Commission commission = db.Commissions.Find(id);
            if (commission == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProviderID = new SelectList(db.Providers, "ProviderID", "ProviderName", commission.ProviderID);
            return View(commission);
        }

        //
        // POST: /Commission/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Commission commission)
        {
            if (ModelState.IsValid)
            {
                db.Entry(commission).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProviderID = new SelectList(db.Providers, "ProviderID", "ProviderName", commission.ProviderID);
            return View(commission);
        }

        //
        // GET: /Commission/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Commission commission = db.Commissions.Find(id);
            if (commission == null)
            {
                return HttpNotFound();
            }
            return View(commission);
        }

        //
        // POST: /Commission/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Commission commission = db.Commissions.Find(id);
            db.Commissions.Remove(commission);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}