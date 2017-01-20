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
    public class ProvidersController : Controller
    {
        private AlfredoContext db = new AlfredoContext();

        // GET: Providers
        public ActionResult Index(bool? message)
        {
            if (message != null)
            {
                ViewBag.message = "El Proveedor ha sido eliminado correctamente.";
            }
            return View(db.Providers.ToList());
        }
        public JsonResult ExisteProv(string nombre)
        {

            var existe = db.Providers.ToList().Exists(a => a.ProviderName == nombre);


            return Json(existe, JsonRequestBehavior.AllowGet);
        }
        // GET: Providers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Provider provider = db.Providers.Find(id);
            if (provider == null)
            {
                return HttpNotFound();
            }
            return View(provider);
        }

        public JsonResult ExisteProveedor(string nombre, int? idprovider)
        {

            var existe = db.Providers.ToList().Exists(a => a.ProviderName == nombre & a.IdProvider != idprovider);


            return Json(existe, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExisteEmail(string nombre, int? idprovider)
        {

            var existe = db.Providers.ToList().Exists(a => a.ProviderEmail == nombre & a.IdProvider != idprovider);


            return Json(existe, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExisteCuil(string nombre, int? idprovider)
        {

            var existe = db.Providers.ToList().Exists(a => a.CuitCuil == nombre & a.IdProvider != idprovider);


            return Json(existe, JsonRequestBehavior.AllowGet);
        }

        // GET: Providers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Providers/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdProvider,ProviderName,CuitCuil,ProviderAddress,ProviderPhone,ProviderEmail")] Provider provider)
        {
            if (ModelState.IsValid)
            {
                db.Providers.Add(provider);
                db.SaveChanges();
                return RedirectToAction("Index");
                
            }

            return View(provider);
        }

        // GET: Providers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Provider provider = db.Providers.Find(id);
            if (provider == null)
            {
                return HttpNotFound();
            }
            return View(provider);
        }

        // POST: Providers/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdProvider,ProviderName,CuitCuil,ProviderAddress,ProviderPhone,ProviderEmail")] Provider provider)
        {
            if (ModelState.IsValid)
            {
                db.Entry(provider).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(provider);
        }

        // GET: Providers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Provider provider = db.Providers.Find(id);
            if (provider == null)
            {
                return HttpNotFound();
            }

            if (db.Purchases.ToList().Exists(p => p.IdProvider == id))
            {
                ViewBag.ErrorProvider = "Acción no permitida! Proveedor con Compras relacionadas.";
            }
            return View(provider);
        }

        // POST: Providers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {            
            if (db.Purchases.ToList().Exists(p => p.IdProvider == id))
            {
                return HttpNotFound();

            } else
            {
                Provider provider = db.Providers.Find(id);
                db.Providers.Remove(provider);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Providers", new { message = true });
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
