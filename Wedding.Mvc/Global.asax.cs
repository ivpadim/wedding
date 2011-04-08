using System;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Wedding.Mvc.Models;

namespace Wedding.Mvc
{

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            this.PostAuthenticateRequest += Application_PostAuthenticateRequest;
        }

        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated &&
                HttpContext.Current.User.Identity.AuthenticationType == "Forms")
            {
                var formsIdentity = HttpContext.Current.User.Identity as FormsIdentity;
                var customIdentity = new UserIdentity() { Ticket = formsIdentity.Ticket };
                var customPrincipal = new UserPrincipal(customIdentity);

                HttpContext.Current.User = customPrincipal;
                Thread.CurrentPrincipal = customPrincipal;
            }
        }

    }
}