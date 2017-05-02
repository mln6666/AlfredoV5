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
    public class TrademarksController : Controller
    {
        private AlfredoContext db = new AlfredoContext();

        // GET: Trademarks
        public ActionResult Index()
        {
            return View(db.Trademarks.ToList());
        }

        // GET: Trademarks/Details/5
        public ActionResult Details(int? id)
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

        // GET: Trademarks/Create
        public ActionResult Create()
        {
            return View();
        }

        public JsonResult ExisteMarca(string nombre)
        {
            var existe = db.Trademarks.ToList().Exists(t => t.TrademarkName == nombre);

            return Json(existe, JsonRequestBehavior.AllowGet);
        }
        
        //public void NewTrademark([Bind(Include = "IdTrademark,TrademarkName,TrademarkDescription")] Trademark trademark)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Trademarks.Add(trademark);
        //        db.SaveChanges();
        //    }
        //}

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

        // GET: Trademarks/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Trademarks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Trademark trademark = db.Trademarks.Find(id);
            db.Trademarks.Remove(trademark);
            db.SaveChanges();
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
