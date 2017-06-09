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
        public ActionResult Index(int? message)
        {
            var sales = db.Sales.Include(s => s.Customer);

            if (message != null)
            {
                TempData["message"] = message;
            }

            return View(sales.ToList());
        }
        public ActionResult Pending(int? message)
        {
            var sales = (from s in db.Sales
                            where s.SaleState == SaleState.Pendiente
                            select s);

            if (message != null)
            {
                TempData["message"] = message;
            }

            return View(sales);
        }
        public ActionResult Finalized(int? message)
        {
            var sales = (from s in db.Sales
                         where s.SaleState == SaleState.Finalizada
                         select s);

            if (message != null)
            {
                TempData["message"] = message;
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

        public JsonResult Getproductdescription(int pro)
        {
            var productdesc = db.Products.ToList().Find(p => p.IdProduct == pro).ProductDescription;

            return Json(productdesc, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTrademarkName(int pro)
        {
            var product = db.Products.ToList().Find(p => p.IdProduct == pro);
            var trademarkname = String.Empty;

            if (product.IdTrademark != null)
            {
                trademarkname = product.Trademark.TrademarkName;
            }

            return Json(trademarkname, JsonRequestBehavior.AllowGet);
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


        public ActionResult PrintSale(int? id, int? x)
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
            var a = id;
            if (x != null)
            {
                if (x == 1)
                {
                    TempData["mimsg"] = 1;
                    return RedirectToAction("PrintSale", new { id = a });

                }

            }


            return View(sale);
        }
        //Vista de ventas Mayoristas, el precio que se carga por defecto en los productos//
        //es el precio mayorista, aunque se lo puede cambiar en la vista.//
        public ActionResult CreateSale(int? x)
        {
            if (x != null)
            {
                if (x == 1)
                {
                    TempData["mimsg"] = 1;
                    return RedirectToAction("CreateSale");

                }

            }
            ViewBag.Customers = db.Customers.ToList();
            
            var products = (from p in db.Products
                        where p.ProductState
                        select p);

            ViewBag.Products = products;
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
            Bill bill = new Bill();

            var cusid = Int32.Parse(O.CustomerName);


            if (ModelState.IsValid)
            {
               if (final) { sale.SaleState = SaleState.Finalizada; } else { sale.SaleState = SaleState.Pendiente; }
                bill.SaleTotal = O.SaleTotal;
                bill.Comments = O.Comments;
                bill.SaleDate = O.SaleDate;
                bill.SaleAddress = O.SaleAddress;
                bill.LinesTotal = O.SaleTotal;
                bill.IdCustomer = cusid;
                db.Bills.Add(bill);
                db.SaveChanges();

                sale.SaleDate = O.SaleDate;
                sale.SaleAddress = O.SaleAddress;
                sale.Comments = O.Comments;
                sale.SaleTotal = O.SaleTotal;
                sale.LinesTotal = O.SaleTotal;
                sale.IdBill = bill.IdBill;
                sale.IdCustomer = cusid;
               
                db.Sales.Add(sale);
                    db.SaveChanges();

                    foreach (var i in O.SaleLines)
                    {
                    BillLine billline = new BillLine();
                    billline.IdProduct = i.IdProduct;
                    billline.LinePrice = i.LinePrice;
                    billline.LineDiscount = i.LineDiscount;
                    billline.LineQuantity = i.LineQuantity;
                    billline.LineTotal = i.LineTotal;
                    billline.IdBill = bill.IdBill;
                    db.BillLines.Add(billline);
                    db.SaveChanges();

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

        [HttpPost]
        public JsonResult AddReturns(SaleVM O)
        {
            bool status = false;


            Sale sale1 = db.Sales.Find(O.IdSale);
            if (sale1.SaleState == SaleState.Finalizada)
            {
                
                return new JsonResult { Data = new { status = status } };
            }


            Sale sale2 = sale1;
            sale1.Comments = O.Comments;
            sale1.SaleTotal = O.SaleTotal;
            sale1.LinesTotal = O.LinesTotal;
            sale1.ReturnsTotal = O.ReturnsTotal;
            int count = 0;
            foreach (var i in O.SaleLines)
            {
                //sale1.SaleLines.ElementAt(count).LineQuantity = i.LineQuantity;
                sale1.SaleLines.ElementAt(count).LinePrice = i.LinePrice;
                sale1.SaleLines.ElementAt(count).LineDiscount = i.LineDiscount;
                sale1.SaleLines.ElementAt(count).LineTotal = i.LineTotal;
                sale1.SaleLines.ElementAt(count).Return = i.Return;
                sale1.SaleLines.ElementAt(count).LineTotalReturn = i.LineTotalReturn;

                count++;
            }
            db.Entry(sale1).State = EntityState.Modified;
            db.SaveChanges();

            Sale saledata = sale1;
            foreach (var item in saledata.SaleLines)
            {
                Product prod = new Product();
                prod = db.Products.Find(item.IdProduct);
                prod.Stock = prod.Stock - item.LineQuantity + item.Return;
                prod.ParcialStock = prod.ParcialStock + item.Return;

                db.Entry(prod).State = EntityState.Modified;
                db.SaveChanges();

            }
            saledata.SaleState = SaleState.Finalizada;
            db.Entry(saledata).State = EntityState.Modified;
            db.SaveChanges();
            status = true;


            return new JsonResult { Data = new { status = status, id = sale1.IdSale } };
        }

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

        //GET
        public ActionResult EditSales(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Sale sale = db.Sales.Find(id);
            CreateSaleVM sale = new CreateSaleVM();
            sale.Sale = db.Sales.Find(id);
            sale.Customers = db.Customers.ToList();
            sale.Products = db.Products.ToList().FindAll(p => p.ProductState);
            if (sale.Sale == null)
            {
                return HttpNotFound();
            }

            if (sale.Sale.SaleState == SaleState.Finalizada)
            {
                return HttpNotFound();
                //ViewBag.pendiente = "No se permiten modificaciones en Ventas Finalizadas.";
                //return View("Delete", db.Sales.Find(id));
            }
            
            //ViewBag.Customers = db.Customers.ToList().OrderBy(c => c.CustomerName);
            //ViewBag.Products = db.Products.ToList().FindAll(p => p.ProductState);
            return View(sale);
        }

        [HttpPost]
        public JsonResult EditSales(Sale sale)
        {
            Sale s = db.Sales.Find(sale.IdSale);
            List<int> list = new List<int>();

            foreach (var item in s.SaleLines.ToList())
            {
                list.Add(item.IdSaleLine);
            }

            foreach (var elim in list)
            {
                SaleLine eline = db.SaleLines.Find(elim);

                Product product = db.Products.Find(eline.IdProduct);
                product.ParcialStock = product.ParcialStock + eline.LineQuantity;
                if (sale.SaleState == SaleState.Finalizada)
                {
                    product.Stock = product.Stock + eline.LineQuantity;
                }
                
                db.Entry(product).State = EntityState.Modified;

                db.SaleLines.Remove(eline);
                db.SaveChanges();
            }

            if (ModelState.IsValid)
            {
                s.IdCustomer = sale.IdCustomer;
                s.SaleState = sale.SaleState;
                s.SaleTotal = sale.SaleTotal;
                s.Comments = sale.Comments;
                
                db.Entry(s).State = EntityState.Modified;
                db.SaveChanges();

                if (sale.SaleLines != null)
                {
                    foreach (var i in sale.SaleLines)
                    {
                        var sl = new SaleLine();

                        sl.IdSale = s.IdSale;
                        sl.IdProduct = i.IdProduct;
                        sl.LinePrice = i.LinePrice;
                        sl.LineQuantity = i.LineQuantity;
                        sl.LineDiscount = i.LineDiscount;
                        sl.LineTotal = i.LineTotal;
                        sl.LineTotalReturn = i.LineTotal;

                        Product product = db.Products.Find(i.IdProduct);
                        product.ParcialStock = product.ParcialStock - i.LineQuantity;
                        if (sale.SaleState == SaleState.Finalizada)
                        {
                            product.Stock = product.Stock - i.LineQuantity;
                        }
                        db.Entry(product).State = EntityState.Modified;

                        db.SaleLines.Add(sl);
                        db.SaveChanges();
                    }
                }

                return new JsonResult { Data = new { status = true }};
            }
            else
            {
                return new JsonResult { Data = new { status = false } };
            }

            
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
            TempData["View"] = view;
            return View(sale);
        }

        // POST: Sales/Delete/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteSale(int id, int view)
        {
            Sale sale = db.Sales.Find(id);

            if (sale != null & sale.SaleState == SaleState.Pendiente)
            {
                foreach (var item in sale.SaleLines)
                {
                    Product prod = new Product();
                    prod = db.Products.Find(item.IdProduct);
                    prod.ParcialStock = prod.ParcialStock + item.LineQuantity;
                    db.Entry(prod).State = EntityState.Modified;
                    db.SaveChanges();

                }

                db.Sales.Remove(sale);
                db.SaveChanges();
                TempData["message"] = 3;
                if (view == 2)
                {
                    return RedirectToAction("Pending");
                }
                else
                {
                    if (view != 1)
                    {
                        TempData["message"] = 0;
                    }
                }

                
            }
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
