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
    public class CategoriesController : Controller
    {
        private AlfredoContext db = new AlfredoContext();

        // GET: Categories
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }

        public JsonResult ValRubro(string dato, int? idcategory)
        {
            
            var category = db.Categories.ToList().Exists(u => u.CategoryName == dato & u.IdCategory != idcategory);


            return Json(category, JsonRequestBehavior.AllowGet);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            ViewBag.Categories = db.Categories.ToList();
            return View();
        }

        // POST: Categories/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdCategory,CategoryName,CategoryDescription")] Category category)
        {
            if (ModelState.IsValid)
            {
                //IEnumerable<int> query = (from c in db.Categories
                //                          where c.CategoryName == category.CategoryName
                //                          select c.IdCategory);

                //if (query.Count() != 0)
                //{
                //    ViewBag.RubroExistente = "Rubro Existente.";
                //    return View("Index", db.Categories.ToList());
                //}

                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdCategory,CategoryName,CategoryDescription")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                TempData["message"] = 2;
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            if (category.Products.Count() != 0)
            {
                ViewBag.errorcategory = "Acción no permitida! Rubro con productos relacionados.";
            }



            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            if (category.Products.Count() != 0)
            {
                return HttpNotFound();
            }
            db.Categories.Remove(category);
            db.SaveChanges();
            TempData["message"] = 1;
            return RedirectToAction("Index", "Categories");
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
