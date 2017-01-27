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
                            where p.ProductState == true
                            select p);

            if (message != null)
            {
                if (message == true)
                {
                    ViewBag.message = "El Producto ha sido desactivado correctamente.";
                }
                else
                {
                    ViewBag.message = "Ha ocurrido un error, intente nuevamente.";
                }
            }
            return View(products.ToList());
        }

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

            return View(products.ToList().OrderBy(p => Tuple.Create(p.Category.CategoryName,p.ProductDescription)));
        }

        public JsonResult ExisteProd(string nombre, int? idproduct, string Trademark)
        {
            if (Trademark == "")
                Trademark = "[Producto sin Marca]";
            var existe = db.Products.ToList().Exists(a => a.ProductDescription == nombre & a.Trademark == Trademark & a.IdProduct != idproduct);

            return Json(existe, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExisteNro(int nombre, int? idproduct)
        {

            var existe = db.Products.ToList().Exists(a => a.ProductNumber == nombre & a.IdProduct != idproduct);


            return Json(existe, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateModal([Bind(Include = "IdProduct,Trademark,ProductDescription,ProductNumber,Cost,WholeSalePrice,PublicPrice,UploadDate,Stock,Minimum,ProductState,Image,idCategory")] Product product)
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
                            where p.ProductState == false
                            select p);

            if (message != null)
            {
                ViewBag.message = "El Producto ha sido activado correctamente.";
            }

            return View(products.ToList());
        }

        public ActionResult Record(bool? message)
        {
            var products = db.Products.Include(p => p.Category);

            if (message != null)
            {
                ViewBag.message = "El Producto se ha eliminado correctamente.";
            }

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
            ViewBag.idCategory = new SelectList(db.Categories.OrderBy(c => c.CategoryName), "IdCategory", "CategoryName");
            return View();
        }

        // POST: Products/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdProduct,Trademark,ProductDescription,ProductNumber,Cost,WholeSalePrice,PublicPrice,UploadDate,Stock,Minimum,ProductState,Image,idCategory")] Product product)
        {
            if (ModelState.IsValid)
            {
                if (product.Trademark == null)
                    product.Trademark = "[Producto sin Marca]";
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
                    db.Entry(prod).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Products", new { message = true });
                }
                else
                {
                    prod.ProductState = true;
                    db.Entry(prod).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("OffProducts", "Products", new { message = true });
                }

            }
            else
            {
                return RedirectToAction("Index", "Products", new { message = false });
            }


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
        public ActionResult Edit([Bind(Include = "IdProduct,Trademark,ProductDescription,ProductNumber,Cost,WholeSalePrice,PublicPrice,UploadDate,Stock,Minimum,ProductState,Image,idCategory")] Product product)
        {
            if (ModelState.IsValid)
            {
                if (product.Trademark == null)
                    product.Trademark = "[Producto sin Marca]";
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
            return RedirectToAction("Record", "Products", new { message = true });
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