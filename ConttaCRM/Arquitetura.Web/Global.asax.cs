using Arquitetura.Web.Helpers;
using Microsoft.Practices.Unity;
using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Arquitetura.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());
            ModelBinders.Binders.Add(typeof(decimal?), new DecimalModelBinder());
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            DependencyResolverConfig.RegisterDependency();
        }

        protected void Application_End()
        {
            //Session.Abandon();
            //Session.Clear();
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            ActiveSessions.Sessions.Add(Session.SessionID);
        }

        protected void Session_End(object sender, EventArgs e)
        {
            Session.Abandon();
            Session.Clear();
            ActiveSessions.Sessions.Remove(Session.SessionID);
        }

    }
}
