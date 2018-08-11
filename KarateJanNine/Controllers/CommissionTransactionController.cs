using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KarateJanNine.Controllers
{
    public class CommissionTransactionController : Controller
    {
        private RechargeEntities db = new RechargeEntities();

        //
        // GET: /CommissionTransaction/

        public ActionResult Index()
        {
            var commissiontransactions = db.CommissionTransactions.Include(c => c.Customer);
            return View(commissiontransactions.ToList());
        }

        //
        // GET: /CommissionTransaction/Details/5

        public ActionResult Details(int id = 0)
        {
            CommissionTransaction commissiontransaction = db.CommissionTransactions.Find(id);
            if (commissiontransaction == null)
            {
                return HttpNotFound();
            }
            return View(commissiontransaction);
        }

        //
        // GET: /CommissionTransaction/Create

        public ActionResult Create()
        {
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CustomerName");
            return View();
        }

        //
        // POST: /CommissionTransaction/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CommissionTransaction commissiontransaction)
        {
            if (ModelState.IsValid)
            {
                db.CommissionTransactions.Add(commissiontransaction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CustomerName", commissiontransaction.CustomerID);
            return View(commissiontransaction);
        }

        //
        // GET: /CommissionTransaction/Edit/5

        public ActionResult Edit(int id = 0)
        {
            CommissionTransaction commissiontransaction = db.CommissionTransactions.Find(id);
            if (commissiontransaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CustomerName", commissiontransaction.CustomerID);
            return View(commissiontransaction);
        }

        //
        // POST: /CommissionTransaction/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CommissionTransaction commissiontransaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(commissiontransaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CustomerName", commissiontransaction.CustomerID);
            return View(commissiontransaction);
        }

        //
        // GET: /CommissionTransaction/Delete/5

        public ActionResult Delete(int id = 0)
        {
            CommissionTransaction commissiontransaction = db.CommissionTransactions.Find(id);
            if (commissiontransaction == null)
            {
                return HttpNotFound();
            }
            return View(commissiontransaction);
        }

        //
        // POST: /CommissionTransaction/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CommissionTransaction commissiontransaction = db.CommissionTransactions.Find(id);
            db.CommissionTransactions.Remove(commissiontransaction);
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