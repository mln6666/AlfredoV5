using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MinimercadoAlfredo.Context;
using MinimercadoAlfredo.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MinimercadoAlfredo
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_BeginRequest()
        {
            var currentCulture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            currentCulture.NumberFormat.NumberDecimalSeparator = ".";
            currentCulture.NumberFormat.NumberGroupSeparator = " ";
            currentCulture.NumberFormat.CurrencyDecimalSeparator = ".";

            Thread.CurrentThread.CurrentCulture = currentCulture;
            //Thread.CurrentThread.CurrentUICulture = currentCulture;
        }

        protected void Application_Start()
        {
            AlfredoContext db = new AlfredoContext();
            ApplicationDbContext dc = new ApplicationDbContext();
            if (dc.Users == null | dc.Users.Count() == 0)
            {
                CreateRoles(dc);
                CreateSuperuser(dc);
                AddPermisionsToSuperuser(dc);
            }
            dc.Dispose();
            //Mapper.Initialize(c => c.AddProfile<MappingProfile>());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }

        private void AddPermisionsToSuperuser(ApplicationDbContext dc)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(dc));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(dc));
            var user = userManager.FindByName("alfredo@admin.com");

            if (!userManager.IsInRole(user.Id, "Ver Productos"))
            {
                userManager.AddToRole(user.Id, "Ver Productos");
            }

            if (!userManager.IsInRole(user.Id, "Agregar Producto"))
            {
                userManager.AddToRole(user.Id, "Agregar Producto");
            }

            if (!userManager.IsInRole(user.Id, "Editar Producto"))
            {
                userManager.AddToRole(user.Id, "Editar Producto");
            }

            if (!userManager.IsInRole(user.Id, "Eliminar Producto"))
            {
                userManager.AddToRole(user.Id, "Eliminar Producto");
            }

            if (!userManager.IsInRole(user.Id, "Ver Rubros"))
            {
                userManager.AddToRole(user.Id, "Ver Rubros");
            }

            if (!userManager.IsInRole(user.Id, "Agregar Rubro"))
            {
                userManager.AddToRole(user.Id, "Agregar Rubro");
            }

            if (!userManager.IsInRole(user.Id, "Editar Rubro"))
            {
                userManager.AddToRole(user.Id, "Editar Rubro");
            }
            
            if (!userManager.IsInRole(user.Id, "Ver Compras"))
            {
                userManager.AddToRole(user.Id, "Ver Compras");
            }

            if (!userManager.IsInRole(user.Id, "Registrar Compra"))
            {
                userManager.AddToRole(user.Id, "Registrar Compra");
            }

            if (!userManager.IsInRole(user.Id, "Editar Compra"))
            {
                userManager.AddToRole(user.Id, "Editar Compra");
            }

            if (!userManager.IsInRole(user.Id, "Eliminar Compra"))
            {
                userManager.AddToRole(user.Id, "Eliminar Compra");
            }

            if (!userManager.IsInRole(user.Id, "Ver Ventas"))
            {
                userManager.AddToRole(user.Id, "Ver Ventas");
            }

            if (!userManager.IsInRole(user.Id, "Registrar Venta"))
            {
                userManager.AddToRole(user.Id, "Registrar Venta");
            }

            if (!userManager.IsInRole(user.Id, "Editar Venta"))
            {
                userManager.AddToRole(user.Id, "Editar Venta");
            }

            if (!userManager.IsInRole(user.Id, "Eliminar Venta"))
            {
                userManager.AddToRole(user.Id, "Eliminar Venta");
            }

            if (!userManager.IsInRole(user.Id, "Ver Clientes"))
            {
                userManager.AddToRole(user.Id, "Ver Clientes");
            }

            if (!userManager.IsInRole(user.Id, "Agregar Cliente"))
            {
                userManager.AddToRole(user.Id, "Agregar Cliente");
            }

            if (!userManager.IsInRole(user.Id, "Editar Cliente"))
            {
                userManager.AddToRole(user.Id, "Editar Cliente");
            }

            if (!userManager.IsInRole(user.Id, "Eliminar Cliente"))
            {
                userManager.AddToRole(user.Id, "Eliminar Cliente");
            }

            if (!userManager.IsInRole(user.Id, "Ver Proveedores"))
            {
                userManager.AddToRole(user.Id, "Ver Proveedores");
            }

            if (!userManager.IsInRole(user.Id, "Agregar Proveedor"))
            {
                userManager.AddToRole(user.Id, "Agregar Proveedor");
            }

            if (!userManager.IsInRole(user.Id, "Editar Proveedor"))
            {
                userManager.AddToRole(user.Id, "Editar Proveedor");
            }

            if (!userManager.IsInRole(user.Id, "Eliminar Proveedor"))
            {
                userManager.AddToRole(user.Id, "Eliminar Proveedor");
            }
        }

        private void CreateRoles(ApplicationDbContext dc)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(dc));

            if (!roleManager.RoleExists("Administrador"))
            {
                roleManager.Create(new IdentityRole("Administrador"));
            }

            if (!roleManager.RoleExists("Ver Productos"))
            {
                roleManager.Create(new IdentityRole("Ver Productos"));
            }

            if (!roleManager.RoleExists("Agregar Producto"))
            {
                roleManager.Create(new IdentityRole("Agregar Producto"));
            }

            if (!roleManager.RoleExists("Editar Producto"))
            {
                roleManager.Create(new IdentityRole("Editar Producto"));
            }

            if (!roleManager.RoleExists("Eliminar Producto"))
            {
                roleManager.Create(new IdentityRole("Eliminar Producto"));
            }

            if (!roleManager.RoleExists("Ver Rubros"))
            {
                roleManager.Create(new IdentityRole("Ver Rubros"));
            }

            if (!roleManager.RoleExists("Agregar Rubro"))
            {
                roleManager.Create(new IdentityRole("Agregar Rubro"));
            }

            if (!roleManager.RoleExists("Editar Rubro"))
            {
                roleManager.Create(new IdentityRole("Editar Rubro"));
            }
            
            if (!roleManager.RoleExists("Ver Compras"))
            {
                roleManager.Create(new IdentityRole("Ver Compras"));
            }

            if (!roleManager.RoleExists("Registrar Compra"))
            {
                roleManager.Create(new IdentityRole("Registrar Compra"));
            }

            if (!roleManager.RoleExists("Editar Compra"))
            {
                roleManager.Create(new IdentityRole("Editar Compra"));
            }

            if (!roleManager.RoleExists("Eliminar Compra"))
            {
                roleManager.Create(new IdentityRole("Eliminar Compra"));
            }

            if (!roleManager.RoleExists("Ver Ventas"))
            {
                roleManager.Create(new IdentityRole("Ver Ventas"));
            }

            if (!roleManager.RoleExists("Registrar Venta"))
            {
                roleManager.Create(new IdentityRole("Registrar Venta"));
            }

            if (!roleManager.RoleExists("Editar Venta"))
            {
                roleManager.Create(new IdentityRole("Editar Venta"));
            }

            if (!roleManager.RoleExists("Eliminar Venta"))
            {
                roleManager.Create(new IdentityRole("Eliminar Venta"));
            }

            if (!roleManager.RoleExists("Ver Proveedores"))
            {
                roleManager.Create(new IdentityRole("Ver Proveedores"));
            }

            if (!roleManager.RoleExists("Agregar Proveedor"))
            {
                roleManager.Create(new IdentityRole("Agregar Proveedor"));
            }

            if (!roleManager.RoleExists("Editar Proveedor"))
            {
                roleManager.Create(new IdentityRole("Editar Proveedor"));
            }

            if (!roleManager.RoleExists("Eliminar Proveedor"))
            {
                roleManager.Create(new IdentityRole("Eliminar Proveedor"));
            }

            if (!roleManager.RoleExists("Ver Clientes"))
            {
                roleManager.Create(new IdentityRole("Ver Clientes"));
            }

            if (!roleManager.RoleExists("Agregar Cliente"))
            {
                roleManager.Create(new IdentityRole("Agregar Cliente"));
            }

            if (!roleManager.RoleExists("Editar Cliente"))
            {
                roleManager.Create(new IdentityRole("Editar Cliente"));
            }

            if (!roleManager.RoleExists("Eliminar Cliente"))
            {
                roleManager.Create(new IdentityRole("Eliminar Cliente"));
            }
        }

        private void CreateSuperuser (ApplicationDbContext dc)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(dc));
            var user = userManager.FindByName("alfredo@admin.com");

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = "alfredo@admin.com",
                    Email = "alfredo@admin.com"
                };
                userManager.Create(user, "Abc123.");
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            Response.Clear();

            HttpException httpException = exception as HttpException;

            int error = httpException != null ? httpException.GetHttpCode() : 0;

            Server.ClearError();
            Response.Redirect(String.Format("~/Error/?error={0}", error, exception.Message));
        }

    }
}
