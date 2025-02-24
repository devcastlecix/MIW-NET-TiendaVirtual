using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TiendaVirtual.Models;
using TiendaVirtual.Models.Binders;

namespace TiendaVirtual
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_BeginRequest()
        {
            var cultureInfo = new CultureInfo("en-US");
            cultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ModelBinders.Binders.Add(typeof(CarritoCompra), new CarritoCompraModelBinder());
            CrearRolesConUsuarios();
        }

        private void CrearRolesConUsuarios()
        {
            var context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!roleManager.RoleExists("Admin"))
            {
                CrearAdmin(roleManager, userManager);
            }

            if (!roleManager.RoleExists("User"))
            {
                CrearUser(roleManager, userManager);                
            }
        }
        private void CrearAdmin(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager) {
            var role = new IdentityRole("Admin");
            roleManager.Create(role);
            var userAdmin = new ApplicationUser { UserName = "admin@admin.com", Email = "admin@admin.com" };
            string userPassword = "#123Admin";
            var chkUser = userManager.Create(userAdmin, userPassword);

            if (chkUser.Succeeded)
            {
                userManager.AddToRole(userAdmin.Id, "Admin");
            }
        }

        private void CrearUser(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            var role = new IdentityRole("User");            
            roleManager.Create(role);
            var user = new ApplicationUser { UserName = "usuario1@test.com", Email = "usuario1@test.com" };
            string userPassword = "#123User";
            var chkUser = userManager.Create(user, userPassword);

            if (chkUser.Succeeded)
            {
                userManager.AddToRole(user.Id, "User");
            }
        }
    }
}
