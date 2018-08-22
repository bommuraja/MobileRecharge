using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KarateJanNine.Controllers
{
    public class RechargeController : Controller
    {
        private RechargeEntities db = new RechargeEntities();

        //
        // GET: /Recharge/

        public ActionResult Index()
        {
            var recharges = db.Recharges.Include(r => r.Customer);
            return View(recharges.ToList());
        }

        //
        // GET: /Recharge/Details/5

        public ActionResult Details(int id = 0)
        {
            Recharge recharge = db.Recharges.Find(id);
            if (recharge == null)
            {
                return HttpNotFound();
            }
            return View(recharge);
        }

        //
        // GET: /Recharge/Create

        public ActionResult Create()
        {
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CustomerName");
            return View();
        }

        //
        // POST: /Recharge/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Recharge recharge)
        {
            if (ModelState.IsValid)
            {
                db.Recharges.Add(recharge);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CustomerName", recharge.CustomerID);
            return View(recharge);
        }

        //
        // GET: /Recharge/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Recharge recharge = db.Recharges.Find(id);
            if (recharge == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CustomerName", recharge.CustomerID);
            return View(recharge);
        }

        //
        // POST: /Recharge/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Recharge recharge)
        {
            if (ModelState.IsValid)
            {
                db.Entry(recharge).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CustomerName", recharge.CustomerID);
            return View(recharge);
        }

        //
        // GET: /Recharge/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Recharge recharge = db.Recharges.Find(id);
            if (recharge == null)
            {
                return HttpNotFound();
            }
            return View(recharge);
        }

        //
        // POST: /Recharge/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Recharge recharge = db.Recharges.Find(id);
            db.Recharges.Remove(recharge);
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