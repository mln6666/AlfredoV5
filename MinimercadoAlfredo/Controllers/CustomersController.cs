﻿using System;
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
    [Authorize(Users = "luisalfredopiriz@yahoo.com.ar")]
    public class CustomersController : Controller
    {
        private AlfredoContext db = new AlfredoContext();

        // GET: Customers
        public ActionResult Index()
        {
            return View(db.Customers.ToList());
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
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

        public JsonResult ExisteCliente(string nombre, int? idcustomer)
        {

            var existe = db.Customers.ToList().Exists(a => a.CustomerName == nombre & a.IdCustomer != idcustomer);


            return Json(existe, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExisteEmail(string nombre, int? idcustomer)
        {

            var existe = db.Customers.ToList().Exists(a => a.CustomerEmail == nombre & a.IdCustomer != idcustomer);


            return Json(existe, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExisteCuil(string nombre, int? idcustomer)
        {

            var existe = db.Customers.ToList().Exists(a => a.CuitCuil == nombre & a.IdCustomer != idcustomer);


            return Json(existe, JsonRequestBehavior.AllowGet);
        }
        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdCustomer,CustomerName,CustomerAddress,CustomerPhone,CustomerEmail,CuitCuil")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                TempData["message"] = 1;
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Customers/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdCustomer,CustomerName,CustomerAddress,CustomerPhone,CustomerEmail,CuitCuil")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                TempData["message"] = 2;
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
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

        [HttpPost]
        public JsonResult DeleteCustomer(int dato)
        {
            var status = false;
            Customer customer = db.Customers.Find(dato);
            db.Customers.Remove(customer);
            db.SaveChanges();
            status = true;

            return Json(status, JsonRequestBehavior.AllowGet);
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
