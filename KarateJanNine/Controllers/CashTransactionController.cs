using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KarateJanNine.Controllers
{
    public class CashTransactionController : Controller
    {
        private RechargeEntities db = new RechargeEntities();

        //
        // GET: /CashTransaction/

        public ActionResult Index()
        {
            var cashtransactions = db.CashTransactions.Include(c => c.Customer);
            return View(cashtransactions.ToList());
        }

        //
        // GET: /CashTransaction/Details/5

        public ActionResult Details(int id = 0)
        {
            CashTransaction cashtransaction = db.CashTransactions.Find(id);
            if (cashtransaction == null)
            {
                return HttpNotFound();
            }
            return View(cashtransaction);
        }

        //
        // GET: /CashTransaction/Create

        public ActionResult Create()
        {
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CustomerName");

            return View();
        }

        //
        // POST: /CashTransaction/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CashTransaction cashtransaction)
        {
            if (ModelState.IsValid)
            {
                var lastCashBalance = db.CashTransactions.Where(m => m.CustomerID == cashtransaction.CustomerID).OrderByDescending(m=>m.CashTransactionID).FirstOrDefault().CashBalance;
                cashtransaction.CashBalance = (Convert.ToDecimal(cashtransaction.CashTransactionAmount) + Convert.ToDecimal(lastCashBalance)).ToString();
                db.CashTransactions.Add(cashtransaction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CustomerName", cashtransaction.CustomerID);
            return View(cashtransaction);
        }

        //
        // GET: /CashTransaction/Edit/5

        public ActionResult Edit(int id = 0)
        {
            CashTransaction cashtransaction = db.CashTransactions.Find(id);
            if (cashtransaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CustomerName", cashtransaction.CustomerID);
            return View(cashtransaction);
        }

        //
        // POST: /CashTransaction/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CashTransaction cashtransaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cashtransaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CustomerName", cashtransaction.CustomerID);
            return View(cashtransaction);
        }

        //
        // GET: /CashTransaction/Delete/5

        public ActionResult Delete(int id = 0)
        {
            CashTransaction cashtransaction = db.CashTransactions.Find(id);
            if (cashtransaction == null)
            {
                return HttpNotFound();
            }
            return View(cashtransaction);
        }

        //
        // POST: /CashTransaction/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CashTransaction cashtransaction = db.CashTransactions.Find(id);
            db.CashTransactions.Remove(cashtransaction);
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