﻿using System;
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
                db.WalletTransactions.Add(wallettransaction);
                db.SaveChanges();
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