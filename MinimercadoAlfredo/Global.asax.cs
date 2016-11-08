using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MinimercadoAlfredo.Context;
using MinimercadoAlfredo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MinimercadoAlfredo
{
    public class MvcApplication : System.Web.HttpApplication
    {
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

            if (!userManager.IsInRole(user.Id, "View Product"))
            {
                userManager.AddToRole(user.Id, "View Product");
            }

            if (!userManager.IsInRole(user.Id, "Create Product"))
            {
                userManager.AddToRole(user.Id, "Create Product");
            }

            if (!userManager.IsInRole(user.Id, "Edit Product"))
            {
                userManager.AddToRole(user.Id, "Edit Product");
            }

            if (!userManager.IsInRole(user.Id, "Delete Product"))
            {
                userManager.AddToRole(user.Id, "Delete Product");
            }

            if (!userManager.IsInRole(user.Id, "View Category"))
            {
                userManager.AddToRole(user.Id, "View Category");
            }

            if (!userManager.IsInRole(user.Id, "Create Category"))
            {
                userManager.AddToRole(user.Id, "Create Category");
            }

            if (!userManager.IsInRole(user.Id, "Edit Category"))
            {
                userManager.AddToRole(user.Id, "Edit Category");
            }

            if (!userManager.IsInRole(user.Id, "Delete Category"))
            {
                userManager.AddToRole(user.Id, "Delete Category");
            }

            if (!userManager.IsInRole(user.Id, "View Purchase"))
            {
                userManager.AddToRole(user.Id, "View Purchase");
            }

            if (!userManager.IsInRole(user.Id, "Create Purchase"))
            {
                userManager.AddToRole(user.Id, "Create Purchase");
            }

            if (!userManager.IsInRole(user.Id, "Edit Purchase"))
            {
                userManager.AddToRole(user.Id, "Edit Purchase");
            }

            if (!userManager.IsInRole(user.Id, "Delete Purchase"))
            {
                userManager.AddToRole(user.Id, "Delete Purchase");
            }

            if (!userManager.IsInRole(user.Id, "View Sale"))
            {
                userManager.AddToRole(user.Id, "View Sale");
            }

            if (!userManager.IsInRole(user.Id, "Create Sale"))
            {
                userManager.AddToRole(user.Id, "Create Sale");
            }

            if (!userManager.IsInRole(user.Id, "Edit Sale"))
            {
                userManager.AddToRole(user.Id, "Edit Sale");
            }

            if (!userManager.IsInRole(user.Id, "Delete Sale"))
            {
                userManager.AddToRole(user.Id, "Delete Sale");
            }

            if (!userManager.IsInRole(user.Id, "View Customer"))
            {
                userManager.AddToRole(user.Id, "View Customer");
            }

            if (!userManager.IsInRole(user.Id, "Create Customer"))
            {
                userManager.AddToRole(user.Id, "Create Customer");
            }

            if (!userManager.IsInRole(user.Id, "Edit Customer"))
            {
                userManager.AddToRole(user.Id, "Edit Customer");
            }

            if (!userManager.IsInRole(user.Id, "Delete Customer"))
            {
                userManager.AddToRole(user.Id, "Delete Customer");
            }

            if (!userManager.IsInRole(user.Id, "View Provider"))
            {
                userManager.AddToRole(user.Id, "View Provider");
            }

            if (!userManager.IsInRole(user.Id, "Create Provider"))
            {
                userManager.AddToRole(user.Id, "Create Provider");
            }

            if (!userManager.IsInRole(user.Id, "Edit Provider"))
            {
                userManager.AddToRole(user.Id, "Edit Provider");
            }

            if (!userManager.IsInRole(user.Id, "Delete Provider"))
            {
                userManager.AddToRole(user.Id, "Delete Provider");
            }
        }

        private void CreateRoles(ApplicationDbContext dc)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(dc));

            if (!roleManager.RoleExists("Administrador"))
            {
                roleManager.Create(new IdentityRole("Administrador"));
            }

            if (!roleManager.RoleExists("View Product"))
            {
                roleManager.Create(new IdentityRole("View Product"));
            }

            if (!roleManager.RoleExists("Create Product"))
            {
                roleManager.Create(new IdentityRole("Create Product"));
            }

            if (!roleManager.RoleExists("Edit Product"))
            {
                roleManager.Create(new IdentityRole("Edit Product"));
            }

            if (!roleManager.RoleExists("Delete Product"))
            {
                roleManager.Create(new IdentityRole("Delete Product"));
            }

            if (!roleManager.RoleExists("View Category"))
            {
                roleManager.Create(new IdentityRole("View Category"));
            }

            if (!roleManager.RoleExists("Create Category"))
            {
                roleManager.Create(new IdentityRole("Create Category"));
            }

            if (!roleManager.RoleExists("Edit Category"))
            {
                roleManager.Create(new IdentityRole("Edit Category"));
            }

            if (!roleManager.RoleExists("Delete Category"))
            {
                roleManager.Create(new IdentityRole("Delete Category"));
            }

            if (!roleManager.RoleExists("Delete Category"))
            {
                roleManager.Create(new IdentityRole("Delete Category"));
            }

            if (!roleManager.RoleExists("View Purchase"))
            {
                roleManager.Create(new IdentityRole("View Purchase"));
            }

            if (!roleManager.RoleExists("Create Purchase"))
            {
                roleManager.Create(new IdentityRole("Create Purchase"));
            }

            if (!roleManager.RoleExists("Edit Purchase"))
            {
                roleManager.Create(new IdentityRole("Edit Purchase"));
            }

            if (!roleManager.RoleExists("Delete Purchase"))
            {
                roleManager.Create(new IdentityRole("Delete Purchase"));
            }

            if (!roleManager.RoleExists("View Sale"))
            {
                roleManager.Create(new IdentityRole("View Sale"));
            }

            if (!roleManager.RoleExists("Create Sale"))
            {
                roleManager.Create(new IdentityRole("Create Sale"));
            }

            if (!roleManager.RoleExists("Edit Sale"))
            {
                roleManager.Create(new IdentityRole("Edit Sale"));
            }

            if (!roleManager.RoleExists("Delete Sale"))
            {
                roleManager.Create(new IdentityRole("Delete Sale"));
            }

            if (!roleManager.RoleExists("View Provider"))
            {
                roleManager.Create(new IdentityRole("View Provider"));
            }

            if (!roleManager.RoleExists("Create Provider"))
            {
                roleManager.Create(new IdentityRole("Create Provider"));
            }

            if (!roleManager.RoleExists("Edit Provider"))
            {
                roleManager.Create(new IdentityRole("Edit Provider"));
            }

            if (!roleManager.RoleExists("Delete Provider"))
            {
                roleManager.Create(new IdentityRole("Delete Provider"));
            }

            if (!roleManager.RoleExists("View Customer"))
            {
                roleManager.Create(new IdentityRole("View Customer"));
            }

            if (!roleManager.RoleExists("Create Customer"))
            {
                roleManager.Create(new IdentityRole("Create Customer"));
            }

            if (!roleManager.RoleExists("Edit Customer"))
            {
                roleManager.Create(new IdentityRole("Edit Customer"));
            }

            if (!roleManager.RoleExists("Delete Customer"))
            {
                roleManager.Create(new IdentityRole("Delete Customer"));
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
