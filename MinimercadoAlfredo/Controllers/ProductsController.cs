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

        [HttpPost]
        public JsonResult GetProductData(int productid)
        {
            var cost = db.Products.Find(productid).Cost;
            var price = db.Products.Find(productid).WholeSalePrice;

            return new JsonResult { Data = new { cost = cost, price = price}};
        }

        public ActionResult PriceUpdate()
        {
            List<Product> products = new List<Product>();
            products = db.Products.ToList();

            return View(products);
        }

        [HttpPost]
        public JsonResult PriceUpdate(List<Product> products)
        {
            Product product = new Product();
            foreach (var item in products)
            {
                product = db.Products.Find(item.IdProduct);
                product.Cost = item.Cost;
                product.WholeSalePrice = item.WholeSalePrice;

                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
            }
            bool status = true;

            return new JsonResult { Data = new { status = status}};
        }


        public JsonResult Getproductstock(string pro)
        {
            var cero = 0;
            if (pro == "0")
            {

                return Json(cero, JsonRequestBehavior.AllowGet);
            }
            AlfredoContext db = new AlfredoContext();

            var proid = Int32.Parse(pro);


            Product productdata = db.Products.ToList().Find(u => u.IdProduct == proid);


            var midato = productdata.ParcialStock.ToString();

            return Json(midato, JsonRequestBehavior.AllowGet);
        }

        // GET: Products
        public ActionResult Index(int? message)
        {
            var products = (from p in db.Products
                            where p.ProductState
                            select p);

            if (message != null)
                TempData["message"] = message;

            ViewBag.Purchases = db.Purchases.ToList().Count();
            return View(products.ToList());
        }

        public ActionResult LastBought(int? id, bool? message)
        {
            List<Product> lastproducts = new List<Product>();
            Purchase lastpurchase = new Purchase();

            if (id != null)
            {
                lastpurchase = db.Purchases.Find(id);
            }
            else
            {
                lastpurchase = db.Purchases.ToList().LastOrDefault();
            }

            foreach (var item in lastpurchase.PurchaseLines.ToList())
            {
                lastproducts.Add(item.Product);
            }

            if (message != null)
            {
                TempData["message"] = message;
            }

            ViewBag.Purchase = lastpurchase.IdPurchase;
            return View(lastproducts);
        }

        [HttpPost]
        public JsonResult LastBought(List<LastProduct> prods)
        {
            Product product = new Product();

            foreach (var item in prods)
            {
                product = db.Products.Find(item.Productid);

                if (item.Wholesaleprice != null)
                {
                    product.UploadDate = DateTime.Today;
                }

                product.WholeSalePrice = item.Wholesaleprice;

                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
            }

            var status = true;

            return Json(status, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Catalog(bool personal)
        {
            var products = (from p in db.Products
                where p.ProductState
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

        public JsonResult ExisteProd(string nombre, int? idproduct, int trademark, int categ)
        {
            var existe = db.Products.ToList().Exists(a => a.ProductDescription == nombre & a.IdTrademark == trademark & a.IdProduct != idproduct & a.idCategory == categ);

            return Json(existe, JsonRequestBehavior.AllowGet);
        }


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

            ViewBag.Purchases = db.Purchases.ToList().Count();
            return View(products.ToList());
        }

        public ActionResult OffProducts(bool? message)
        {
            var products = (from p in db.Products
                            where !p.ProductState
                            select p);
            
            ViewBag.Purchases = db.Purchases.ToList().Count();
            return View(products.ToList());
        }

        public ActionResult Record()
        {
            var products = db.Products.Include(p => p.Category);

            ViewBag.Purchases = db.Purchases.ToList().Count();
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

        public ActionResult CreateProduct(int? message)
        {
            if (message != null)
                TempData["message"] = message;

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

        public ActionResult EditProduct(int id)
        {
            Product product = db.Products.Find(id);

            ViewBag.Trademarks = db.Trademarks.ToList().OrderBy(t => t.TrademarkName);
            ViewBag.Categories = db.Categories.ToList().OrderBy(c => c.CategoryName);

            return View(product);
        }

        [HttpPost]
        public JsonResult EditProduct(Product product)
        {
            Product prod = db.Products.Find(product.IdProduct);
            prod.ProductDescription = product.ProductDescription;
            prod.Cost = product.Cost;
            prod.WholeSalePrice = product.WholeSalePrice;
            prod.ProductState = product.ProductState;
            prod.idCategory = product.idCategory;
            prod.IdTrademark = product.IdTrademark;
            prod.Minimum = product.Minimum;
            prod.UploadDate = DateTime.Today;

            db.Entry(prod).State = EntityState.Modified;
            db.SaveChanges();

            //TempData["message"] = 6;
            return new JsonResult { Data = new { status = true } };
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