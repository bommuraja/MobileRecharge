using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KarateJanNine.Controllers
{
    public class WalletTransactionController : Controller
    {
        private RechargeEntities db = new RechargeEntities();

        //
        // GET: /WalletTransaction/

        public ActionResult Index()
        {
            var wallettransactions = db.WalletTransactions.Include(w => w.Customer);
            return View(wallettransactions.ToList());
        }

        //
        // GET: /WalletTransaction/Details/5

        public ActionResult Details(int id = 0)
        {
            WalletTransaction wallettransaction = db.WalletTransactions.Find(id);
            if (wallettransaction == null)
            {
                return HttpNotFound();
            }
            return View(wallettransaction);
        }

        //
        // GET: /WalletTransaction/Create

        public ActionResult Create()
        {
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CustomerName");
            return View();
        }

        //
        // POST: /WalletTransaction/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WalletTransaction wallettransaction)
        {
            if (ModelState.IsValid)
            {
                using (var dbUse = new RechargeEntities())
                {
                    var lastWalletBalance = dbUse.WalletTransactions.Where(m => m.CustomerID == wallettransaction.CustomerID).OrderByDescending(m => m.WalletTransactionID).FirstOrDefault().WalletBalance;

                    wallettransaction.IsCredit = true;
                    wallettransaction.CreatedDate = DateTime.Now.ToString();
                    wallettransaction.WalletTransactionReferenceDescription = "CashTransaction";
                    wallettransaction.WalletTransactionReferenceID = "0";
                    wallettransaction.WalletTransactionDescription = "Move cash to wallet";
                    wallettransaction.WalletBalance = (Convert.ToDecimal(wallettransaction.WalletTransactionAmount) + Convert.ToDecimal(lastWalletBalance)).ToString();
                    dbUse.WalletTransactions.Add(wallettransaction);

                    var lastCashBalance = dbUse.CashTransactions.Where(m => m.CustomerID == wallettransaction.CustomerID).OrderByDescending(m => m.CashTransactionID).FirstOrDefault().CashBalance;

                    CashTransaction cashtransaction = new CashTransaction();
                    cashtransaction.IsCredit = false;
                    cashtransaction.CustomerID = wallettransaction.CustomerID;
                    cashtransaction.CreatedDate = wallettransaction.CreatedDate;
                    cashtransaction.CreatedBy = wallettransaction.CreatedBy;
                    cashtransaction.CashTransactionReferenceID = wallettransaction.WalletTransactionID.ToString();
                    cashtransaction.CashTransactionReferenceDescription = "WalletTransaction";
                    cashtransaction.CashTransactionDescription = "Debit amount from cash transaction";
                    cashtransaction.CashTransactionAmount = wallettransaction.WalletTransactionAmount;
                    cashtransaction.CashBalance = (Convert.ToDecimal(lastCashBalance) - Convert.ToDecimal(wallettransaction.WalletTransactionAmount)).ToString();
                    dbUse.CashTransactions.Add(cashtransaction);

                    var updatedTransaction = dbUse.WalletTransactions.Find(wallettransaction.WalletTransactionID);
                    updatedTransaction.WalletTransactionReferenceID = cashtransaction.CashTransactionID.ToString();
                    dbUse.SaveChanges();

                }

                return RedirectToAction("Index");
            }

            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CustomerName", wallettransaction.CustomerID);
            return View(wallettransaction);
        }

        //
        // GET: /WalletTransaction/Edit/5

        public ActionResult Edit(int id = 0)
        {
            WalletTransaction wallettransaction = db.WalletTransactions.Find(id);
            if (wallettransaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CustomerName", wallettransaction.CustomerID);
            return View(wallettransaction);
        }

        //
        // POST: /WalletTransaction/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(WalletTransaction wallettransaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(wallettransaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CustomerName", wallettransaction.CustomerID);
            return View(wallettransaction);
        }

        //
        // GET: /WalletTransaction/Delete/5

        public ActionResult Delete(int id = 0)
        {
            WalletTransaction wallettransaction = db.WalletTransactions.Find(id);
            if (wallettransaction == null)
            {
                return HttpNotFound();
            }
            return View(wallettransaction);
        }

        //
        // POST: /WalletTransaction/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WalletTransaction wallettransaction = db.WalletTransactions.Find(id);
            db.WalletTransactions.Remove(wallettransaction);
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