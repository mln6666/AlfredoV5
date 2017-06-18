using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MinimercadoAlfredo.Context;
using MinimercadoAlfredo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MinimercadoAlfredo.Controllers
{
    [Authorize(Users = "luisalfredopiriz@yahoo.com.ar")]
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Users
        [Authorize(Roles = "Administrador")]
        public ActionResult Index()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var users = userManager.Users.ToList();
            var usersView = new List<UserView>();
            foreach (var user in users)
            {
                var userView = new UserView
                {
                    Email = user.Email,
                    Name = user.UserName,
                    UserID = user.Id
                };
                usersView.Add(userView);
            }

            return View(usersView);
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult ViewPermissions()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var roles = roleManager.Roles.ToList();
            var rolesView = new List<RoleView>();

            foreach (var item in roles)
            {

                var roleView = new RoleView
                {
                    RoleID = item.Id,
                    Name = item.Name
                };
                rolesView.Add(roleView);
            }

            var userView = new UserView
            {
                Roles = rolesView.OrderBy(name => name.Name).ToList()
            };

            ViewBag.descAdmin = "Posee todos los permisos y además cuenta con la posibilidad de Agregar/Eliminar usuarios y Asignar/Quitar los permisos que tendrán con el sistema.";
            ViewBag.ViewProd = "Permite al usuario acceder a todas las listas de Productos";
            ViewBag.CreateProd = "Permite al usuario agregar un nuevo Producto a su Stock";
            ViewBag.EditProd = "Permite al usuario modificar la información de un Producto";
            ViewBag.DeleteProd = "Permite al usuario eliminar un Producto de su Stock";
            ViewBag.ViewCateg = "Permite al usuario acceder a la lista de Rubros";
            ViewBag.CreateCateg = "Permite al usuario agregar un nuevo Rubro";
            ViewBag.EditCateg = "Permite al usuario modificar un Rubro";
            ViewBag.DeleteCateg = "Permite al usuario eliminar un Rubro";
            ViewBag.ViewPurch = "Permite al usuario acceder a la lista de Compras y al detalle de cada una";
            ViewBag.CreatePurch = "Permite al usuario registrar una Compra";
            ViewBag.EditPurch = "Permite al usuario modificar tanto la información de una Compra, como su detalle";
            ViewBag.DeletePurch = "Permite al usuario eliminar una Compra";
            ViewBag.ViewSale = "Permite al usuario acceder a todas las listas de Ventas y al detalle de cada una";
            ViewBag.CreateSale = "Permite al usuario registrar una Venta";
            ViewBag.EditSale = "Permite al usuario modificar la información de una Venta, modificar el detalle de Venta y finalizar una Venta";
            ViewBag.DeleteSale = "Permite al usuario eliminar una Venta";
            ViewBag.ViewCust = "Permite al usuario acceder a la lista de Clientes y a la información de cada uno";
            ViewBag.CreateCust = "Permite al usuario agregar un nuevo Cliente al sistema";
            ViewBag.EditCust = "Permite al usuario modificar la información de un Cliente";
            ViewBag.DeleteCust = "Permite al usuario eliminar un Cliente del sistema";
            ViewBag.ViewProv = "Permite al usuario acceder a la lista de Proveedores y a la información de cada uno";
            ViewBag.CreateProv = "Permite al usuario agregar un nuevo Proveedor al sistema";
            ViewBag.EditProv = "Permite al usuario modificar la información de un Proveedor";
            ViewBag.DeleteProv = "Permite al usuario eliminar un Proveedor del sistema";

            return View(userView);
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult Roles (string userID)
        {
            if (string.IsNullOrEmpty(userID))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var users = userManager.Users.ToList();
            var user = users.Find(u => u.Id == userID);

            if (user == null)
            {
                return HttpNotFound();
            }

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var roles = roleManager.Roles.ToList();
            var rolesView = new List<RoleView>();

            foreach (var item in user.Roles)
            {
                var role = roles.Find(r => r.Id == item.RoleId);
                var roleView = new RoleView
                {
                    RoleID = role.Id,
                    Name = role.Name
                };
                rolesView.Add(roleView);
            }


            var userView = new UserView
            {
                Email = user.Email,
                Name = user.UserName,
                UserID = user.Id,
                Roles = rolesView.OrderBy(name => name.Name).ToList()
            };

            ViewBag.descAdmin = "Posee todos los permisos y además cuenta con la posibilidad de Agregar/Eliminar usuarios y Asignar/Quitar los permisos que tendrán con el sistema.";
            ViewBag.ViewProd = "Permite al usuario acceder a todas las listas de Productos";
            ViewBag.CreateProd = "Permite al usuario agregar un nuevo Producto a su Stock";
            ViewBag.EditProd = "Permite al usuario modificar la información de un Producto";
            ViewBag.DeleteProd = "Permite al usuario eliminar un Producto de su Stock";
            ViewBag.ViewCateg = "Permite al usuario acceder a la lista de Rubros";
            ViewBag.CreateCateg = "Permite al usuario agregar un nuevo Rubro";
            ViewBag.EditCateg = "Permite al usuario modificar un Rubro";
            ViewBag.DeleteCateg = "Permite al usuario eliminar un Rubro";
            ViewBag.ViewPurch = "Permite al usuario acceder a la lista de Compras y al detalle de cada una";
            ViewBag.CreatePurch = "Permite al usuario registrar una Compra";
            ViewBag.EditPurch = "Permite al usuario modificar tanto la información de una Compra, como su detalle";
            ViewBag.DeletePurch = "Permite al usuario eliminar una Compra";
            ViewBag.ViewSale = "Permite al usuario acceder a todas las listas de Ventas y al detalle de cada una";
            ViewBag.CreateSale = "Permite al usuario registrar una Venta";
            ViewBag.EditSale = "Permite al usuario modificar la información de una Venta, modificar el detalle de Venta y finalizar una Venta";
            ViewBag.DeleteSale = "Permite al usuario eliminar una Venta";
            ViewBag.ViewCust = "Permite al usuario acceder a la lista de Clientes y a la información de cada uno";
            ViewBag.CreateCust = "Permite al usuario agregar un nuevo Cliente al sistema";
            ViewBag.EditCust = "Permite al usuario modificar la información de un Cliente";
            ViewBag.DeleteCust = "Permite al usuario eliminar un Cliente del sistema";
            ViewBag.ViewProv = "Permite al usuario acceder a la lista de Proveedores y a la información de cada uno";
            ViewBag.CreateProv = "Permite al usuario agregar un nuevo Proveedor al sistema";
            ViewBag.EditProv = "Permite al usuario modificar la información de un Proveedor";
            ViewBag.DeleteProv = "Permite al usuario eliminar un Proveedor del sistema";

            return View(userView);
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult MyRoles (string name1)
        {
            if (string.IsNullOrEmpty(name1))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var users = userManager.Users.ToList();

            var user = users.Find(u => u.UserName == name1);

            if (user == null)
            {
                return HttpNotFound();
            }

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var roles = roleManager.Roles.ToList();
            var rolesView = new List<RoleView>();

            foreach (var item in user.Roles)
            {
                var role = roles.Find(r => r.Id == item.RoleId);
                var roleView = new RoleView
                {
                    RoleID = role.Id,
                    Name = role.Name
                };
                rolesView.Add(roleView);
            }


            var userView = new UserView
            {
                Email = user.Email,
                Name = user.UserName,
                UserID = user.Id,
                Roles = rolesView.OrderBy(name => name.Name).ToList()
            };

            ViewBag.descAdmin = "Posee todos los permisos y además cuenta con la posibilidad de Agregar/Eliminar usuarios y Asignar/Quitar los permisos que tendrán con el sistema.";
            ViewBag.ViewProd = "Permite al usuario acceder a todas las listas de Productos";
            ViewBag.CreateProd = "Permite al usuario agregar un nuevo Producto a su Stock";
            ViewBag.EditProd = "Permite al usuario modificar la información de un Producto";
            ViewBag.DeleteProd = "Permite al usuario eliminar un Producto de su Stock";
            ViewBag.ViewCateg = "Permite al usuario acceder a la lista de Rubros";
            ViewBag.CreateCateg = "Permite al usuario agregar un nuevo Rubro";
            ViewBag.EditCateg = "Permite al usuario modificar un Rubro";
            ViewBag.DeleteCateg = "Permite al usuario eliminar un Rubro";
            ViewBag.ViewPurch = "Permite al usuario acceder a la lista de Compras y al detalle de cada una";
            ViewBag.CreatePurch = "Permite al usuario registrar una Compra";
            ViewBag.EditPurch = "Permite al usuario modificar tanto la información de una Compra, como su detalle";
            ViewBag.DeletePurch = "Permite al usuario eliminar una Compra";
            ViewBag.ViewSale = "Permite al usuario acceder a todas las listas de Ventas y al detalle de cada una";
            ViewBag.CreateSale = "Permite al usuario registrar una Venta";
            ViewBag.EditSale = "Permite al usuario modificar la información de una Venta, modificar el detalle de Venta y finalizar una Venta";
            ViewBag.DeleteSale = "Permite al usuario eliminar una Venta";
            ViewBag.ViewCust = "Permite al usuario acceder a la lista de Clientes y a la información de cada uno";
            ViewBag.CreateCust = "Permite al usuario agregar un nuevo Cliente al sistema";
            ViewBag.EditCust = "Permite al usuario modificar la información de un Cliente";
            ViewBag.DeleteCust = "Permite al usuario eliminar un Cliente del sistema";
            ViewBag.ViewProv = "Permite al usuario acceder a la lista de Proveedores y a la información de cada uno";
            ViewBag.CreateProv = "Permite al usuario agregar un nuevo Proveedor al sistema";
            ViewBag.EditProv = "Permite al usuario modificar la información de un Proveedor";
            ViewBag.DeleteProv = "Permite al usuario eliminar un Proveedor del sistema";

            return View("MyPermissions", userView);
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult AddRole(string userID)
        {
            if (string.IsNullOrEmpty(userID))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var users = userManager.Users.ToList();
            var user = users.Find(u => u.Id == userID);

            if (user == null)
            {
                return HttpNotFound();
            }

            var userView = new UserView
            {
                Email = user.Email,
                Name = user.UserName,
                UserID = user.Id
            };

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var list = roleManager.Roles.ToList();

            list.Remove(list.Single(x => x.Name == "Administrador"));
            list = list.OrderBy(r => r.Name).ToList();

            ViewBag.permisos = list;
            ViewBag.RoleID = new SelectList(list, "Id", "Name");

            return View(userView);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public ActionResult AddRole(string userID, FormCollection form)
        {
            var roleID = Request["RoleID"];

            ViewBag.descAdmin = "Posee todos los permisos y además cuenta con la posibilidad de Agregar/Eliminar usuarios y Asignar/Quitar los permisos que tendrán con el sistema.";
            ViewBag.ViewProd = "Permite al usuario acceder a todas las listas de Productos";
            ViewBag.CreateProd = "Permite al usuario agregar un nuevo Producto a su Stock";
            ViewBag.EditProd = "Permite al usuario modificar la información de un Producto";
            ViewBag.DeleteProd = "Permite al usuario eliminar un Producto de su Stock";
            ViewBag.ViewCateg = "Permite al usuario acceder a la lista de Rubros";
            ViewBag.CreateCateg = "Permite al usuario agregar un nuevo Rubro";
            ViewBag.EditCateg = "Permite al usuario modificar un Rubro";
            ViewBag.DeleteCateg = "Permite al usuario eliminar un Rubro";
            ViewBag.ViewPurch = "Permite al usuario acceder a la lista de Compras y al detalle de cada una";
            ViewBag.CreatePurch = "Permite al usuario registrar una Compra";
            ViewBag.EditPurch = "Permite al usuario modificar tanto la información de una Compra, como su detalle";
            ViewBag.DeletePurch = "Permite al usuario eliminar una Compra";
            ViewBag.ViewSale = "Permite al usuario acceder a todas las listas de Ventas y al detalle de cada una";
            ViewBag.CreateSale = "Permite al usuario registrar una Venta";
            ViewBag.EditSale = "Permite al usuario modificar la información de una Venta, modificar el detalle de Venta y finalizar una Venta";
            ViewBag.DeleteSale = "Permite al usuario eliminar una Venta";
            ViewBag.ViewCust = "Permite al usuario acceder a la lista de Clientes y a la información de cada uno";
            ViewBag.CreateCust = "Permite al usuario agregar un nuevo Cliente al sistema";
            ViewBag.EditCust = "Permite al usuario modificar la información de un Cliente";
            ViewBag.DeleteCust = "Permite al usuario eliminar un Cliente del sistema";
            ViewBag.ViewProv = "Permite al usuario acceder a la lista de Proveedores y a la información de cada uno";
            ViewBag.CreateProv = "Permite al usuario agregar un nuevo Proveedor al sistema";
            ViewBag.EditProv = "Permite al usuario modificar la información de un Proveedor";
            ViewBag.DeleteProv = "Permite al usuario eliminar un Proveedor del sistema";

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var users = userManager.Users.ToList();
            var user = users.Find(u => u.Id == userID);
            var userView = new UserView
            {
                Email = user.Email,
                Name = user.UserName,
                UserID = user.Id
            };

            if (string.IsNullOrEmpty(roleID))
            {
                ViewBag.Error = "Debe Seleccionar un permiso";

                var list = roleManager.Roles.ToList();
                list.Add(new IdentityRole { Id = "", Name = "[Seleccione un permiso...]" });
                list = list.OrderBy(r => r.Name).ToList();
                ViewBag.RoleID = new SelectList(list, "Id", "Name");

                return View(userView);
            }

            var roles = roleManager.Roles.ToList();
            var role = roles.Find(r => r.Id == roleID);
            if (!userManager.IsInRole(userID, role.Name))
            {
                userManager.AddToRole(userID, role.Name);
                db.SaveChanges();
            }

            var rolesView = new List<RoleView>();
            foreach (var item in user.Roles)
            {
                role = roles.Find(r => r.Id == item.RoleId);
                var roleView = new RoleView
                {
                    Name = role.Name,
                    RoleID = role.Id
                };
                rolesView.Add(roleView);
            }


            userView = new UserView
            {
                Email = user.Email,
                Name = user.UserName,
                Roles = rolesView,
                UserID = user.Id
            };

            return View("Roles", userView);
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult Delete(string userID, string roleID)
        {
            if (string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(roleID))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

            var user = userManager.Users.ToList().Find(u => u.Id == userID);
            var role = roleManager.Roles.ToList().Find(r => r.Id == roleID);

            //Delete
            if (userManager.IsInRole(user.Id, role.Name))
            {
                userManager.RemoveFromRole(user.Id, role.Name);
                db.SaveChanges();
            }

            //Prepara la vista de vuelta
            var users = userManager.Users.ToList();
            var roles = roleManager.Roles.ToList();
            var rolesView = new List<RoleView>();

            foreach (var item in user.Roles)
            {
                role = roles.Find(r => r.Id == item.RoleId);
                var roleView = new RoleView
                {
                    RoleID = role.Id,
                    Name = role.Name
                };
                rolesView.Add(roleView);
            }


            var userView = new UserView
            {
                Email = user.Email,
                Name = user.UserName,
                UserID = user.Id,
                Roles = rolesView
            };

            return View("Roles", userView);
        }

        // POST: /Users/Delete/5
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> DeleteUser(string id)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var user = userManager.Users.ToList().Find(u => u.Id == id);
            await userManager.DeleteAsync(user);
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