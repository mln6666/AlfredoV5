using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using MinimercadoAlfredo.Context;
using MinimercadoAlfredo.Models;

namespace MinimercadoAlfredo.Controllers
{
    public class TrademarksController : Controller
    {
        private AlfredoContext db = new AlfredoContext();

        // GET: Trademarks
        public ActionResult Index()
        {
            return View(db.Trademarks.ToList());
        }
        

        // GET: Trademarks/Create
        public ActionResult Create()
        {
            return View();
        }

        public JsonResult ExisteMarca(string nombre, int? trademark)
        {
            var existe = db.Trademarks.ToList().Exists(t => t.TrademarkName == nombre & t.IdTrademark != trademark);

            return Json(existe, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddTrademark(string newtrademark)
        {
            var exist = db.Trademarks.ToList().Exists(t => t.TrademarkName == newtrademark);
            
            if (!exist)
            {
                Trademark trademark = new Trademark
                {
                    TrademarkName = newtrademark
                };
                db.Trademarks.Add(trademark);
                db.SaveChanges();
            }
            return new JsonResult { Data = new { exist = exist, x = db.Trademarks.ToList().Last().IdTrademark }};
        }

        // POST: Trademarks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdTrademark,TrademarkName,TrademarkDescription")] Trademark trademark)
        {
            if (ModelState.IsValid)
            {
                db.Trademarks.Add(trademark);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(trademark);
        }

        // GET: Trademarks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trademark trademark = db.Trademarks.Find(id);
            if (trademark == null)
            {
                return HttpNotFound();
            }
            return View(trademark);
        }

        // POST: Trademarks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdTrademark,TrademarkName,TrademarkDescription")] Trademark trademark)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trademark).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(trademark);
        }

        [HttpPost]
        public JsonResult DeleteTrademark(int dato)
        {
            var status = false;
            Trademark trademark = db.Trademarks.Find(dato);
            db.Trademarks.Remove(trademark);
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
