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
using MinimercadoAlfredo.ViewModels;

namespace MinimercadoAlfredo.Controllers
{
    public class PurchasesController : Controller
    {
        private AlfredoContext db = new AlfredoContext();

        // GET: Purchases
        public ActionResult Index(bool? message)
        {
            var purchases = db.Purchases.Include(p => p.Provider);

            if (message != null)
            {
                ViewBag.message = "La Compra ha sido eliminada correctamente.";
            }

            return View(purchases.ToList().OrderByDescending(p => p.PurchaseDate));
        }

        // GET: Purchases/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Purchase purchase = db.Purchases.Find(id);
            if (purchase == null)
            {
                return HttpNotFound();
            }
            return View(purchase);
        }

        public ActionResult modalProvider(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Provider provider = db.Providers.Find(id);
            if (provider == null)
            {
                return HttpNotFound();
            }
            return View(provider);
        }

        // GET: Purchases/Create
        public ActionResult Create()
        {
            ViewBag.Providers = db.Providers.ToList().OrderBy(p => p.ProviderName);
            ViewBag.Products = db.Products.ToList().OrderBy(p => p.ProductDescription);
            return View("CreatePurchase");
        }

        // POST: Purchases/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdPurchase,PurchaseDate,Comments,PurchaseTotal,IdProvider")] Purchase purchase)
        {
            if (ModelState.IsValid)
            {
                db.Purchases.Add(purchase);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdProvider = new SelectList(db.Providers, "IdProvider", "ProviderName", purchase.IdProvider);
            return View(purchase);
        }
        public JsonResult Getproductdata(string pro)
        {
            var cero = 0;
            if (pro == "0")
            {

                return Json(cero, JsonRequestBehavior.AllowGet);
            }
            AlfredoContext db = new AlfredoContext();
            //IEnumerable<int> query = (from c in db.Products
            //                          where c.ProductDescription == pro
            //                          select c.IdProduct);


            //int id = query.ElementAt(0);
            //Product productdata = db.Products.Find(id);

            var proid = Int32.Parse(pro);


            Product productdata = db.Products.ToList().Find(u => u.IdProduct == proid);


            var midato = productdata.Cost.ToString();

            return Json(midato, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreatePurchase(int? x)
        {
            if (x != null)
            {
                if (x == 1)
                {
                    ViewBag.msg = 1;
                }
            }
            ViewBag.Providers = db.Providers.ToList();
            ViewBag.Products = db.Products.ToList();
            ViewBag.idCategory = new SelectList(db.Categories, "IdCategory", "CategoryName");

            var npurchase = 0;
            if (db.Purchases!=null & db.Purchases.Count()!=0)
            {
                npurchase = db.Purchases.ToList().LastOrDefault().IdPurchase;
            }
            

            ViewBag.npurchase = npurchase + 1;


            return View();
        }

        [HttpPost]
        public JsonResult CreatePurchase(PurchaseVM O)
        {
            //ProviderName contiene el id del proveedor
            bool status = false;
            Purchase purchase = new Purchase();
            var proid = Int32.Parse(O.ProviderName);


            if (ModelState.IsValid)
            {

                purchase.PurchaseDate = O.PurchaseDate;
             
                purchase.Comments = O.Comments;
                purchase.PurchaseTotal = O.PurchaseTotal;
                purchase.IdProvider = proid;
                db.Purchases.Add(purchase);
                db.SaveChanges();

                foreach (var i in O.PurchaseLines)
                {
                    PurchaseLine purchaseline = new PurchaseLine();
                    purchaseline.IdProduct = i.IdProduct;
                    purchaseline.LinePrice = i.LinePrice;
                    purchaseline.LineQuantity = i.LineQuantity;
                    purchaseline.LineTotal = i.LineTotal;
                    purchaseline.IdPurchase = purchase.IdPurchase;
                    db.PurchaseLines.Add(purchaseline);
                    db.SaveChanges();

                    Product prod = new Product();
                    prod = db.Products.Find(i.IdProduct);
                    prod.Stock = prod.Stock + i.LineQuantity;
                    prod.UploadDate = purchase.PurchaseDate;
                    prod.ParcialStock = prod.ParcialStock + i.LineQuantity;
                    prod.Cost = i.LinePrice;
                    db.Entry(prod).State = EntityState.Modified;
                    db.SaveChanges();

                }
                status = true;

            }
            else
            {
                status = false;
            }
            return new JsonResult { Data = new { status = status } };
        }


        // GET: Purchases/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Purchase purchase = db.Purchases.Find(id);
            if (purchase == null)
            {
                return HttpNotFound();
            }
            ViewBag.Providers = db.Providers.ToList().OrderBy(p => p.ProviderName);
            ViewBag.Products = db.Products.ToList();
            return View(purchase);
        }

        // POST: Purchases/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdPurchase,PurchaseDate,Comments,PurchaseTotal,IdProvider")] Purchase purchase)
        {
            if (ModelState.IsValid)
            {
                db.Entry(purchase).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdProvider = new SelectList(db.Providers, "IdProvider", "ProviderName", purchase.IdProvider);
            return View(purchase);
        }

        // GET: Purchases/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Purchase purchase = db.Purchases.Find(id);
            if (purchase == null)
            {
                return HttpNotFound();
            }
            
            return View(purchase);
        }

        // POST: Purchases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Purchase purchase = db.Purchases.Find(id);
            db.Purchases.Remove(purchase);
            db.SaveChanges();
            return RedirectToAction("Index", "Purchases", new { message = true });
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
