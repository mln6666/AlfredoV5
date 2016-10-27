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
    public class SaleLinesController : Controller
    {
        private AlfredoContext db = new AlfredoContext();

        // GET: SaleLines
        public ActionResult Index()
        {
            var saleLines = db.SaleLines.Include(s => s.Product).Include(s => s.Sale);
            return View(saleLines.ToList());
        }

        // GET: SaleLines/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SaleLine saleLine = db.SaleLines.Find(id);
            if (saleLine == null)
            {
                return HttpNotFound();
            }
            return View(saleLine);
        }

        // GET: SaleLines/Create
        public ActionResult Create()
        {
            ViewBag.IdProduct = new SelectList(db.Products, "IdProduct", "ProductDescription");
            ViewBag.IdSale = new SelectList(db.Sales, "IdSale", "SaleAddress");
            return View();
        }

        // POST: SaleLines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdSaleLine,IdProduct,LinePrice,LineDiscount,LineQuantity,LineTotal,IdSale")] SaleLine saleLine)
        {
            if (ModelState.IsValid)
            {
                db.SaleLines.Add(saleLine);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdProduct = new SelectList(db.Products, "IdProduct", "ProductDescription", saleLine.IdProduct);
            ViewBag.IdSale = new SelectList(db.Sales, "IdSale", "SaleAddress", saleLine.IdSale);
            return View(saleLine);
        }

        // GET: SaleLines/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SaleLine saleLine = db.SaleLines.Find(id);
            if (saleLine == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdProduct = new SelectList(db.Products, "IdProduct", "ProductDescription", saleLine.IdProduct);
            ViewBag.IdSale = new SelectList(db.Sales, "IdSale", "SaleAddress", saleLine.IdSale);
            return View(saleLine);
        }

        // POST: SaleLines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdSaleLine,IdProduct,LinePrice,LineDiscount,LineQuantity,LineTotal,IdSale")] SaleLine saleLine)
        {
            if (ModelState.IsValid)
            {
                db.Entry(saleLine).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdProduct = new SelectList(db.Products, "IdProduct", "ProductDescription", saleLine.IdProduct);
            ViewBag.IdSale = new SelectList(db.Sales, "IdSale", "SaleAddress", saleLine.IdSale);
            return View(saleLine);
        }

        // GET: SaleLines/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SaleLine saleLine = db.SaleLines.Find(id);
            if (saleLine == null)
            {
                return HttpNotFound();
            }
            return View(saleLine);
        }

        // POST: SaleLines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SaleLine saleLine = db.SaleLines.Find(id);
            db.SaleLines.Remove(saleLine);
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
