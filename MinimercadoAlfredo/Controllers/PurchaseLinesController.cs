using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MinimercadoAlfredo.Context;
using MinimercadoAlfredo.Models;

namespace MinimercadoAlfredo.Controllers
{
    public class PurchaseLinesController : Controller
    {
        private AlfredoContext db = new AlfredoContext();

        // GET: PurchaseLines
        public ActionResult Index()
        {
            var purchaseLines = db.PurchaseLines.Include(p => p.Product).Include(p => p.Purchase);
            return View(purchaseLines.ToList());
        }

        // GET: PurchaseLines/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseLine purchaseLine = db.PurchaseLines.Find(id);
            if (purchaseLine == null)
            {
                return HttpNotFound();
            }
            return View(purchaseLine);
        }

        // GET: PurchaseLines/Create
        public ActionResult Create()
        {
            ViewBag.IdProduct = new SelectList(db.Products, "IdProduct", "ProductDescription");
            ViewBag.IdPurchase = new SelectList(db.Purchases, "IdPurchase", "Comments");
            return View();
        }

        // POST: PurchaseLines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdPurchaseLine,IdProduct,LinePrice,LineQuantity,LineTotal,IdPurchase")] PurchaseLine purchaseLine)
        {
            if (ModelState.IsValid)
            {
                db.PurchaseLines.Add(purchaseLine);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdProduct = new SelectList(db.Products, "IdProduct", "ProductDescription", purchaseLine.IdProduct);
            ViewBag.IdPurchase = new SelectList(db.Purchases, "IdPurchase", "Comments", purchaseLine.IdPurchase);
            return View(purchaseLine);
        }

        // GET: PurchaseLines/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseLine purchaseLine = db.PurchaseLines.Find(id);
            if (purchaseLine == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdProduct = new SelectList(db.Products, "IdProduct", "ProductDescription", purchaseLine.IdProduct);
            ViewBag.IdPurchase = new SelectList(db.Purchases, "IdPurchase", "Comments", purchaseLine.IdPurchase);
            return View(purchaseLine);
        }

        // POST: PurchaseLines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdPurchaseLine,IdProduct,LinePrice,LineQuantity,LineTotal,IdPurchase")] PurchaseLine purchaseLine)
        {
            if (ModelState.IsValid)
            {
                db.Entry(purchaseLine).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdProduct = new SelectList(db.Products, "IdProduct", "ProductDescription", purchaseLine.IdProduct);
            ViewBag.IdPurchase = new SelectList(db.Purchases, "IdPurchase", "Comments", purchaseLine.IdPurchase);
            return View(purchaseLine);
        }

        // GET: PurchaseLines/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseLine purchaseLine = db.PurchaseLines.Find(id);
            if (purchaseLine == null)
            {
                return HttpNotFound();
            }
            return View(purchaseLine);
        }

        // POST: PurchaseLines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PurchaseLine purchaseLine = db.PurchaseLines.Find(id);
            db.PurchaseLines.Remove(purchaseLine);
            db.SaveChanges();
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
