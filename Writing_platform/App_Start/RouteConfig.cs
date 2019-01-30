﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Writing_platform
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "User",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "User", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Book",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Book", id = UrlParameter.Optional }
            );
        }
    }
}
