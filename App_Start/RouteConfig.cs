using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OnlineShopingStore
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
           name: "ProductDetails",
           url: "products/{productId}",
           defaults: new { controller = "Home", action = "Details", productId = UrlParameter.Optional }
       );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
    name: "Payment",
    url: "Payment/{action}/{id}",
    defaults: new { controller = "Payment", action = "CreatePayment", id = UrlParameter.Optional }
);
            routes.MapRoute(
       name: "Dashboard",
       url: "Dashboard/Index",
       defaults: new { controller = "Dashboard", action = "Index" }
   );

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    //defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //    defaults: new { controller = "Admin", action = "AdminLogin", id = UrlParameter.Optional }

            //    );
        }
    }
}
