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
        public ActionResult Index(int? message)
        {
            var purchases = db.Purchases.Include(p => p.Provider);

            if (message != null)
            {
                TempData["message"] = message;

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

        public JsonResult Getproviderdata(string prov)
        {
            var provid = Int32.Parse(prov);
            AlfredoContext db = new AlfredoContext();
            var providerdata = db.Providers.ToList().Find(u => u.IdProvider == provid).ProviderAddress;

            if (providerdata == null)
            {
                providerdata = String.Empty;
            }

            return Json(providerdata, JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public JsonResult EditPurchases(Purchase purchase)
        {
            Purchase p = db.Purchases.Find(purchase.IdPurchase);
            List<int> list = new List<int>();

            foreach (var item in p.PurchaseLines.ToList())
            {
                list.Add(item.IdPurchaseLine);
            }

            foreach (var elim in list)
            {
                PurchaseLine eline = db.PurchaseLines.Find(elim);

                Product product = new Product(); 
                product = db.Products.Find(eline.IdProduct);
                product.ParcialStock = product.ParcialStock - eline.LineQuantity;
                product.Stock = product.Stock - eline.LineQuantity;

                db.Entry(product).State = EntityState.Modified;

                db.PurchaseLines.Remove(eline);
                db.SaveChanges();
            }

            p.IdProvider = purchase.IdProvider;
            p.PurchaseTotal = purchase.PurchaseTotal;
            p.Comments = purchase.Comments;

            db.Entry(p).State = EntityState.Modified;
            db.SaveChanges();

            if (purchase.PurchaseLines != null)
            {
                foreach (var i in purchase.PurchaseLines)
                {
                    var pl = new PurchaseLine();

                    pl.IdPurchase = p.IdPurchase;
                    pl.IdProduct = i.IdProduct;
                    pl.LinePrice = i.LinePrice;
                    pl.LineQuantity = i.LineQuantity;
                    pl.LineTotal = i.LineTotal;

                    Product product = db.Products.Find(i.IdProduct);
                    product.ParcialStock = product.ParcialStock + i.LineQuantity;
                    product.Stock = product.Stock + i.LineQuantity;

                    if (product.Cost != i.LinePrice)
                    {
                        product.UploadDate = DateTime.Today;
                    }

                    product.Cost = i.LinePrice;

                    if (i.LinePrice > product.WholeSalePrice)
                    {
                        product.WholeSalePrice = i.LinePrice;

                        if (i.LinePrice > product.PublicPrice)
                        {
                            product.PublicPrice = i.LinePrice;
                        }
                    }

                    db.Entry(product).State = EntityState.Modified;

                    db.PurchaseLines.Add(pl);
                    db.SaveChanges();
                }
            }

            return new JsonResult { Data = new { status = true } };

        }


        // GET: Purchases/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CreatePurchaseVM purchase = new CreatePurchaseVM();
            purchase.Purchase = db.Purchases.Find(id);
            purchase.Providers = db.Providers.ToList();
            purchase.Products = db.Products.ToList();
            if (purchase == null)
            {
                return HttpNotFound();
            }
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
            List<int> list = new List<int>();

            foreach (var item in purchase.PurchaseLines.ToList())
            {
                list.Add(item.IdPurchaseLine);
            }

            foreach (var elim in list)
            {
                PurchaseLine pl = db.PurchaseLines.Find(elim);

                Product product = db.Products.Find(pl.IdProduct);
                product.ParcialStock = product.ParcialStock - pl.LineQuantity;
                product.Stock = product.Stock - pl.LineQuantity;

                db.Entry(product).State = EntityState.Modified;

                db.PurchaseLines.Remove(pl);
                db.SaveChanges();
            }

            db.Purchases.Remove(purchase);
            db.SaveChanges();
            return RedirectToAction("Index", "Purchases", new { message = 3 });
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
