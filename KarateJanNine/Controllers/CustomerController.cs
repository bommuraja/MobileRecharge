using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KarateJanNine.Controllers
{
    public class CustomerController : Controller
    {
        private RechargeEntities db = new RechargeEntities();

        //
        // GET: /Customer/

        public ActionResult Index()
        {
            return View(db.Customers.ToList());
        }

        //
        // GET: /Customer/Details/5

        public ActionResult Details(int id = 0)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        //
        // GET: /Customer/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Customer/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                // 1. Customer table entry
                customer.CreatedDate = DateTime.Now.ToString();
                customer.LastModifiedDate = DateTime.Now.ToString();
                db.Customers.Add(customer);
                db.SaveChanges();
                var CustomerID = customer.CustomerID;
                // 2. Commission table entry
                var ProvidersList = db.Providers.ToList();
                for(int i=0;i< ProvidersList.Count;i++)
                {
                    Commission objCommission = new Commission();
                    objCommission.CommissionPercentage = "1";
                    objCommission.CreatedBy = customer.CreatedBy;
                    objCommission.CreatedDate = customer.CreatedDate;
                    objCommission.CustomerID = CustomerID;
                    objCommission.LastModifiedBy = customer.LastModifiedBy;
                    objCommission.LastModifiedDate = customer.LastModifiedDate;
                    objCommission.ProviderID = ProvidersList[i].ProviderID;
                    db.Commissions.Add(objCommission);
                    db.SaveChanges();
                }
                // 3. CommissionTranaction table entry
                CommissionTransaction objCommissionTransaction = new CommissionTransaction();
                objCommissionTransaction.CreatedBy = customer.CreatedBy;
                objCommissionTransaction.CreatedDate = customer.CreatedDate;
                objCommissionTransaction.CustomerID = CustomerID;
                objCommissionTransaction.CommissionBalance = "0";
                objCommissionTransaction.CommissionTransactionAmount = "0";
                objCommissionTransaction.CommissionTransactionDate = customer.CreatedDate;
                objCommissionTransaction.CommissionTransactionDescription = "Primary Entry";
                objCommissionTransaction.CommissionTransactionReferenceDescription = "From customer creation entry";
                objCommissionTransaction.CommissionTransactionReferenceID = CustomerID.ToString();

                objCommissionTransaction.CreatedBy = customer.CreatedBy;
                objCommissionTransaction.CreatedDate = customer.CreatedDate;
                objCommissionTransaction.IsCredit = true;
                db.CommissionTransactions.Add(objCommissionTransaction);
                db.SaveChanges();
                // 4. WalletTransaction table entry
                WalletTransaction objWalletTransaction = new WalletTransaction();
                objWalletTransaction.CreatedBy = customer.CreatedBy;
                objWalletTransaction.CreatedDate = customer.CreatedDate;
                objWalletTransaction.CustomerID = CustomerID;
                objWalletTransaction.IsCredit = true;
                objWalletTransaction.WalletBalance = "0";
                objWalletTransaction.WalletTransactionAmount = "0";
                objWalletTransaction.WalletTransactionDate = customer.CreatedDate;
                objWalletTransaction.WalletTransactionDescription = "Primary Entry";
                objWalletTransaction.WalletTransactionReferenceDescription = "From customer creation entry";
                objWalletTransaction.WalletTransactionReferenceID = CustomerID.ToString();
                db.WalletTransactions.Add(objWalletTransaction);
                db.SaveChanges();
                // 5. CashTransaction table entry
                CashTransaction objCashTransaction = new CashTransaction();
                objCashTransaction.CashBalance = "0";
                objCashTransaction.CashTransactionAmount = "0";
                objCashTransaction.CashTransactionDate= customer.CreatedDate;
                objCashTransaction.CashTransactionDescription = "Primary Entry";
                objCashTransaction.CashTransactionReferenceDescription = "For customer creation entry";
                objCashTransaction.CashTransactionReferenceID = CustomerID.ToString();
                objCashTransaction.CreatedBy= customer.CreatedBy;
                objCashTransaction.CreatedDate= customer.CreatedDate;
                objCashTransaction.CustomerID = CustomerID;
                objCashTransaction.IsCredit = true;
                db.CashTransactions.Add(objCashTransaction);
                db.SaveChanges();



                return RedirectToAction("Index");
            }

            return View(customer);
        }

        //
        // GET: /Customer/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        //
        // POST: /Customer/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        //
        // GET: /Customer/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        //
        // POST: /Customer/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // 5. Remove fromo cash transaction table
            var cashTransactions = db.CashTransactions.Where(c => c.CustomerID == id).ToList();
            foreach (var item in cashTransactions)
            {
                db.CashTransactions.Remove(item);
                db.SaveChanges();
            }
            // 4. Remove from wallet transaction table
            var walletTransactions = db.WalletTransactions.Where(c => c.CustomerID == id).ToList();
            foreach (var item in walletTransactions)
            {
                db.WalletTransactions.Remove(item);
                db.SaveChanges();
            }

            // 3. Remove from commission transaction table
            var commissionTransactions = db.CommissionTransactions.Where(c => c.CustomerID == id).ToList();
            foreach (var item in commissionTransactions)
            {
                db.CommissionTransactions.Remove(item);
                db.SaveChanges();
            }
            // 2. Remove from commission table
            var commissions = db.Commissions.Where(c => c.CustomerID == id).ToList();
            foreach (var item in commissions)
            {
                db.Commissions.Remove(item);
                db.SaveChanges();
            }
            // 1. Remove from customer table
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);           
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