using BanSachWeb.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BanSachWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "UpdateAccount",
                url: "UpdateAccount",
                defaults: new { controller = "Account", action = "UpdateAccountInfo" }
                );

            routes.MapRoute(
                name: "Login",
                url: "Login",
                defaults: new { controller = "Account", action = "Login" }
            );

            routes.MapRoute(
                name: "Register",
                url: "Register",
                defaults: new { controller = "Account", action = "Register" }
            );

            routes.MapRoute(
                name: "Home",
                url: "Home",
                defaults: new { controller = "Home", action = "MainContent", id = UrlParameter.Optional }
            );

            routes.MapRoute(
            name: "Default",
            url: "{controller}/{action}/{id}",
            new { controller = "Home", action = "MainContent", id = UrlParameter.Optional }
            );
            routes.MapRoute(
            name: "ProductPagination",
            url: "Sach/Index/{page}",
            defaults: new { controller = "Sach", action = "Index", page = UrlParameter.Optional }
        );
        }
    }
}
