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
    public class SalesController : Controller
    {
        private AlfredoContext db = new AlfredoContext();

        // GET: Sales
        public ActionResult Index(bool? message)
        {
            var sales = db.Sales.Include(s => s.Customer);

            if (message != null)
            {
                ViewBag.message = "La Venta ha sido eliminada correctamente.";
            }

            return View(sales.ToList());
        }
        public ActionResult Pending(bool? message)
        {
            var sales = (from s in db.Sales
                            where s.SaleState == SaleState.Pendiente
                            select s);

            if (message != null)
            {
                ViewBag.message = "La Venta ha sido eliminada correctamente.";
            }

            return View(sales);
        }
        public ActionResult Finalized(bool? message)
        {
            var sales = (from s in db.Sales
                         where s.SaleState == SaleState.Finalizada
                         select s);

            if (message != null)
            {
                ViewBag.message = "La Venta ha sido eliminada correctamente.";
            }

            return View(sales);
        }

        public JsonResult Getcustomerdata(string cus)
        {
            if (cus == "0")
            {
                Customer customerdata2 = new Customer();
                customerdata2.CustomerAddress = "";
                customerdata2.CuitCuil = "";
                return Json(customerdata2, JsonRequestBehavior.AllowGet);

            }
          
            var cusid = Int32.Parse(cus);
            AlfredoContext db = new AlfredoContext();
            Customer customerdata = db.Customers.ToList().Find(u => u.IdCustomer == cusid);
            Customer customerdata3 = new Customer();

            if (customerdata.CustomerAddress != null)
            {
                customerdata3.CustomerAddress = customerdata.CustomerAddress.ToString();
            }
            else
            {
                customerdata3.CustomerAddress = ""; }

            if (customerdata.CuitCuil != null)
            {
                customerdata3.CuitCuil = customerdata.CuitCuil.ToString();
            }
            else
            {
                customerdata3.CuitCuil = ""; }



            return Json(customerdata3, JsonRequestBehavior.AllowGet);
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


            var midato = productdata.WholeSalePrice.ToString();

            return Json(midato, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ValStockSale(string pro)
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


            var midato = productdata.ParcialStock;

            return Json(midato, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FinalizeSale(string misale)
        {
            var saleid = Int32.Parse(misale);
            AlfredoContext db = new AlfredoContext();
            Sale saledata = db.Sales.ToList().Find(u => u.IdSale == saleid);

            foreach (var item in saledata.SaleLines)
            {
                Product prod = new Product();
                prod = db.Products.Find(item.IdProduct);
                prod.Stock = prod.Stock - item.LineQuantity;
                db.Entry(prod).State = EntityState.Modified;
                db.SaveChanges();

            }
            saledata.SaleState=SaleState.Finalizada;
            db.Entry(saledata).State = EntityState.Modified;
            db.SaveChanges();

            var midato = "Ok";

            return Json(midato, JsonRequestBehavior.AllowGet);
        }

        //Vista de ventas Mayoristas, el precio que se carga por defecto en los productos//
        //es el precio mayorista, aunque se lo puede cambiar en la vista.//
        public ActionResult CreateSale()
        {
            ViewBag.Customers = db.Customers.ToList();
            ViewBag.Products = db.Products.ToList();
            var nsale = 0;
            if (db.Sales != null & db.Sales.Count() != 0)
            {
                nsale = db.Sales.ToList().LastOrDefault().IdSale;
            }

            ViewBag.nsale = nsale + 1;

            
            return View();
        }


        [HttpPost]
        public JsonResult CreateSale(SaleVM O)
        {
            //CustomerName contiene el id del cliente
            bool status = false;
            bool final = false;

            if (O.SaleState == "1")final = true; 
            
                 

            Sale sale = new Sale();
            var cusid = Int32.Parse(O.CustomerName);


            if (ModelState.IsValid)
            {
               if (final) { sale.SaleState = SaleState.Finalizada; } else { sale.SaleState = SaleState.Pendiente; }
                sale.SaleDate = O.SaleDate;
                    sale.SaleAddress = O.SaleAddress;
                    sale.Comments = O.Comments;
                    sale.SaleTotal = O.SaleTotal;
                    sale.IdCustomer = cusid;
                    
                    db.Sales.Add(sale);
                    db.SaveChanges();

                    foreach (var i in O.SaleLines)
                    {
                        SaleLine saleline=new SaleLine();
                        saleline.IdProduct = i.IdProduct;
                        saleline.LinePrice = i.LinePrice;
                        saleline.LineDiscount = i.LineDiscount;
                        saleline.LineQuantity = i.LineQuantity;
                        saleline.LineTotal = i.LineTotal;
                        saleline.IdSale = sale.IdSale;
                        db.SaleLines.Add(saleline);
                        db.SaveChanges();

                    Product prod = new Product();
                    prod = db.Products.Find(i.IdProduct);
                    if (final) { prod.Stock = prod.Stock - i.LineQuantity; }
                    prod.ParcialStock = prod.ParcialStock - i.LineQuantity;
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


        //[HttpPost]
        //public JsonResult CreateSale(SaleVM O)
        //{
        //    bool status = false;
        //    if (ModelState.IsValid)
        //    {
        //        using (AlfredoContext dc = new AlfredoContext())
        //        {
        //            Sale order = new Sale { CustomerName = O.CustomerName, SaleAddress = O.SaleAddress, SaleDate = O.SaleDate, SaleTotal = O.SaleTotal, Comments = O.Comments };
        //            foreach (var i in O.SaleLines)
        //            {
        //                order.SaleLines.Add(i);
        //            }
        //            dc.Sales.Add(order);
        //            dc.SaveChanges();
        //            status = true;
        //        }
        //    }
        //    else
        //    {
        //        status = false;
        //    }
        //    return new JsonResult { Data = new { status = status } };
        //}

        // GET: Sales/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = db.Sales.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            return View(sale);
        }

        public ActionResult modalCustomer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Sales/Create
        public ActionResult Create()
        {
            ViewBag.IdCustomer = new SelectList(db.Customers, "IdCustomer", "CustomerName");
            return View();
        }

        // POST: Sales/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdSale,SaleDate,Discount,Comments,SaleTotal,IdCustomer")] Sale sale)
        {
            if (ModelState.IsValid)
            {
                db.Sales.Add(sale);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdCustomer = new SelectList(db.Customers, "IdCustomer", "CustomerName", sale.IdCustomer);
            return View(sale);
        }

        // GET: Sales/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = db.Sales.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdCustomer = new SelectList(db.Customers, "IdCustomer", "CustomerName", sale.IdCustomer);
            return View(sale);
        }

        // POST: Sales/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdSale,SaleDate,Discount,Comments,SaleTotal,IdCustomer")] Sale sale)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sale).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdCustomer = new SelectList(db.Customers, "IdCustomer", "CustomerName", sale.IdCustomer);
            return View(sale);
        }

        // GET: Sales/Delete/5
        public ActionResult Delete(int? id,int? view)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = db.Sales.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            if (sale.SaleState == SaleState.Finalizada)
            {
                ViewBag.pendiente = "Solo es posible eliminar Ventas Pendientes";
            }
            return View(sale);
        }

        // POST: Sales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id,int? view)
        {
            Sale sale = db.Sales.Find(id);
            bool finalized = false;
            if (sale.SaleState == SaleState.Finalizada)
            {finalized = true;}

            foreach (var item in sale.SaleLines)
            {
                Product prod = new Product();
                prod = db.Products.Find(item.IdProduct);
                if (finalized) { prod.Stock = prod.Stock + item.LineQuantity; }
                prod.ParcialStock = prod.ParcialStock + item.LineQuantity;
                db.Entry(prod).State = EntityState.Modified;
                db.SaveChanges();

            }

            db.Sales.Remove(sale);
            db.SaveChanges();
            if (view == 0) { return RedirectToAction("Index", new { message = true }); }
            if (view == 1) { return RedirectToAction("Pending", new { message = true }); }
            if (view == 2) { return RedirectToAction("Finalized", new { message = true }); }

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
