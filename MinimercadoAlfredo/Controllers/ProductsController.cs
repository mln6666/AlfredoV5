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
    public class ProductsController : Controller
    {
        private AlfredoContext db = new AlfredoContext();

        // GET: Products
        public ActionResult Index()
        {
            var products = (from p in db.Products
                            where p.ProductState == true
                            select p);

            return View(products.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateModal([Bind(Include = "IdProduct,ProductDescription,ProductNumber,Cost,WholeSalePrice,PublicPrice,UploadDate,Stock,Minimum,ProductState,Image,idCategory")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.ParcialStock = product.Stock;
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("_NewProduct");
            }

            ViewBag.idCategory = new SelectList(db.Categories, "IdCategory", "CategoryName", product.idCategory);
            return RedirectToAction("_NewProduct");
        }

        public ActionResult _NewProduct()
        {
            var lista = db.Products.ToList();
            Product ultimo = lista.LastOrDefault();
            ViewBag.Products = db.Products.ToList();
            ViewBag.idultimoprod = ultimo.IdProduct;
            return View();
        }
        public ActionResult Minimum()
        {
            var products = (from p in db.Products
                        where p.Stock <= p.Minimum
                        select p);

            return View(products.ToList());
        }

        public ActionResult OffProducts()
        {
            var products = (from p in db.Products
                        where p.ProductState == false
                        select p);

            return View(products.ToList());
        }

        public ActionResult Record()
        {
            var products = db.Products.Include(p => p.Category);

            return View(products.ToList());
        }

        //GET
        public ActionResult Deactivate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.idCategory = new SelectList(db.Categories, "IdCategory", "CategoryName");
            return View();
        }

        // POST: Products/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdProduct,ProductDescription,ProductNumber,Cost,WholeSalePrice,PublicPrice,UploadDate,Stock,Minimum,ProductState,Image,idCategory")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.ParcialStock = product.Stock;
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idCategory = new SelectList(db.Categories, "IdCategory", "CategoryName", product.idCategory);
            return View(product);
        }

        [HttpPost, ActionName("Deactivate")]
        [ValidateAntiForgeryToken]
        public ActionResult DeactivateConfirmation(int? idProd)
        {
            Product prod = new Product();
            prod = db.Products.Find(idProd);

            if (ModelState.IsValid)
            {
                if (prod.ProductState)
                {
                    prod.ProductState = false;
                }
                else
                {
                    prod.ProductState = true;
                }
                db.Entry(prod).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.idCategory = new SelectList(db.Categories.OrderBy(c => c.CategoryName), "IdCategory", "CategoryName", product.idCategory);
            return View(product);
        }

        // POST: Products/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdProduct,ProductDescription,ProductNumber,Cost,WholeSalePrice,PublicPrice,UploadDate,Stock,Minimum,ProductState,Image,idCategory")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idCategory = new SelectList(db.Categories, "IdCategory", "CategoryName", product.idCategory);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Record");
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
