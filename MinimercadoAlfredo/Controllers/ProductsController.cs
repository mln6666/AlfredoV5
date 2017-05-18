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
    public class ProductsController : Controller
    {
        private AlfredoContext db = new AlfredoContext();


        public ActionResult RemoveStock()
        {
            ViewBag.Products = db.Products.ToList();
            return View();
        }

        [HttpPost]
        public JsonResult RemoveStock(SaleVM O)
        {
            //CustomerName contiene el id del cliente
            bool status = false;

                foreach (var i in O.SaleLines)
                {
                    Product prod = new Product();
                    prod = db.Products.Find(i.IdProduct);
                    prod.ParcialStock = prod.ParcialStock - i.LineQuantity;
                    prod.Stock = prod.Stock - i.LineQuantity;
                    db.Entry(prod).State = EntityState.Modified;
                    db.SaveChanges();


                }
                status = true;

            
            
            
            return new JsonResult { Data = new { status = status } };
        }

        public ActionResult AddStock()
        {
            ViewBag.Products = db.Products.ToList();
            return View();
        }

        [HttpPost]
        public JsonResult AddStock(SaleVM O)
        {
            //CustomerName contiene el id del cliente
            bool status = false;

            foreach (var i in O.SaleLines)
            {
                Product prod = new Product();
                prod = db.Products.Find(i.IdProduct);
                prod.ParcialStock = prod.ParcialStock + i.LineQuantity;
                prod.Stock = prod.Stock + i.LineQuantity;
                db.Entry(prod).State = EntityState.Modified;
                db.SaveChanges();


            }
            status = true;




            return new JsonResult { Data = new { status = status } };
        }


        public JsonResult Getproductstock(string pro)
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


            var midato = productdata.ParcialStock.ToString();

            return Json(midato, JsonRequestBehavior.AllowGet);
        }

        // GET: Products
        public ActionResult Index(bool? message)
        {
            var products = (from p in db.Products
                            where p.ProductState
                            select p);

            if (message != null)
                TempData["message"] = 1;

            return View(products.ToList());
        }
        [HttpGet]
        public ActionResult Catalog(bool personal)
        {
            var products = (from p in db.Products
                where p.ProductState == true
                select p);

            if (personal)
            {
                ViewBag.personal = true;
            }else
            {
                ViewBag.personal = false;
            }

            ViewBag.Products = db.Products.Count();
            return View(products.ToList().OrderBy(p => Tuple.Create(p.Category.CategoryName, p.IdTrademark)));
        }

        public JsonResult ExisteProd(string nombre, int? idproduct, int Trademark)
        {
            //if (Trademark == "")
            //    Trademark = "[Producto sin Marca]";
            var existe = db.Products.ToList().Exists(a => a.ProductDescription == nombre & a.IdTrademark == Trademark & a.IdTrademark != idproduct);

            return Json(existe, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult ExisteNro(int nombre, int? idproduct)
        //{

        //    var existe = db.Products.ToList().Exists(a => a.ProductNumber == nombre & a.IdProduct != idproduct);


        //    return Json(existe, JsonRequestBehavior.AllowGet);
        //}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateModal([Bind(Include = "IdProduct,IdTrademark,ProductDescription,Cost,WholeSalePrice,PublicPrice,UploadDate,Stock,Minimum,ProductState,Image,idCategory")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.ParcialStock = product.Stock;
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("_NewProduct");
            }

            ViewBag.idCategory = new SelectList(db.Categories, "IdCategory", "CategoryName", product.idCategory);
            ViewBag.IdTrademark = new SelectList(db.Trademarks, "IdTrademark", "TrademarkName", product.IdTrademark);

            return RedirectToAction("_NewProduct");
        }

        public ActionResult _NewProduct()
        {
            var lista = db.Products.ToList();
            ViewBag.Products = lista;
            ViewBag.idultimoprod = lista.LastOrDefault().IdProduct;
            return View();
        }

        public ActionResult Minimum()
        {
            var products = (from p in db.Products
                            where p.Stock <= p.Minimum
                            select p);

            return View(products.ToList());
        }

        public ActionResult OffProducts(bool? message)
        {
            var products = (from p in db.Products
                            where !p.ProductState
                            select p);

            if (message != null)
            {
                ViewBag.message = "El Producto ha sido activado correctamente.";
            }

            return View(products.ToList());
        }

        public ActionResult Record()
        {
            var products = db.Products.Include(p => p.Category);

            return View(products.ToList());
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

        [HttpPost]
        public JsonResult Deactivate(int? id)
        {
            var status = false;
            Product prod = new Product();
            prod = db.Products.Find(id);

            prod.ProductState = !prod.ProductState;
            db.Entry(prod).State = EntityState.Modified;
            db.SaveChanges();
            status = true;
            return Json(status, JsonRequestBehavior.AllowGet);

        }

        public ActionResult CreateProduct(bool? message)
        {
            if (message != null)
                TempData["message"] = 1;

            ViewBag.Trademarks = db.Trademarks.ToList().OrderBy(t => t.TrademarkName);
            ViewBag.Categories = db.Categories.ToList().OrderBy(c => c.CategoryName);

            return View();
        }

        [HttpPost]
        public JsonResult CreateProduct(CreateProductVM product)
        {
            Product prod = new Product();
            prod.idCategory = Int32.Parse(product.ProductCategory);
            prod.IdTrademark = Int32.Parse(product.ProductTrademark);
            prod.ProductDescription = product.ProductDescription;
            prod.Cost = product.ProductCost;
            prod.WholeSalePrice = product.ProductWholeSalePrice;
            prod.PublicPrice = product.ProductPublicPrice;
            prod.UploadDate = DateTime.Today;
            prod.ProductState = product.ProductState;
            prod.ParcialStock = product.ProductStock;
            prod.Stock = product.ProductStock;
            prod.Minimum = product.ProductMinimum;

            db.Products.Add(prod);
            db.SaveChanges();

            TempData["message"] = 6;
            return new JsonResult { Data = new { status = true}};
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.idCategory = new SelectList(db.Categories.OrderBy(c => c.CategoryName), "IdCategory", "CategoryName");
            ViewBag.IdTrademark = new SelectList(db.Trademarks.OrderBy(c => c.TrademarkName), "IdTrademark", "TrademarkName");
            ViewBag.Trademarks = db.Trademarks.ToList().OrderBy(t => t.TrademarkName);

            return View();
        }

        // POST: Products/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdProduct,IdTrademark,ProductDescription,Cost,WholeSalePrice,PublicPrice,Stock,Minimum,ProductState,idCategory")] Product product)
        {
            if (ModelState.IsValid)
            {
                //if (product.Trademark == null)
                //    product.Trademark = "[Producto sin Marca]";
                product.UploadDate = DateTime.Now.Date;
                product.ParcialStock = product.Stock;
                db.Products.Add(product);
                db.SaveChanges();
                TempData["message"] = 1;
                return RedirectToAction("Index");
            }

            ViewBag.idCategory = new SelectList(db.Categories, "IdCategory", "CategoryName", product.idCategory);
            ViewBag.IdTrademark = new SelectList(db.Trademarks, "IdTrademark", "TrademarkName", product.IdTrademark);

            return View(product);
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
            ViewBag.IdTrademark = new SelectList(db.Trademarks.OrderBy(c => c.TrademarkName), "IdTrademark", "TrademarkName", product.IdTrademark);

            return View(product);
        }

        // POST: Products/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdProduct,IdTrademark,ProductDescription,Cost,WholeSalePrice,PublicPrice,Stock,Minimum,ProductState,idCategory,ParcialStock,UploadDate,Image")] Product product)
        {
            if (ModelState.IsValid)
            {
                //if (product.Trademark == null)
                //    product.Trademark = "[Producto sin Marca]";
                
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                TempData["message"] = 4;
                return RedirectToAction("Index");
            }
            ViewBag.idCategory = new SelectList(db.Categories, "IdCategory", "CategoryName", product.idCategory);
            ViewBag.IdTrademark = new SelectList(db.Trademarks, "IdTrademark", "TrademarkName", product.IdTrademark);

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
            TempData["message"] = 5;
            return RedirectToAction("Record", "Products");
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