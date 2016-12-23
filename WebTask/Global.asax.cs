using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using WebApplication1;
using WebTask.Controllers.Access;
using WebTask.Extensions;

namespace WebTask
{
    public class MvcApplication : HttpApplication
    {
        private readonly SessionAccess sessionAccess = new SessionAccess();
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_PreSendRequestContent(object sender, EventArgs e)
        {
            var path = Request.Url.AbsolutePath;
            if (path.StartsWith("/__browserLink") || path.Contains("?")) return;
            var identity = User.Identity;
            Guid? userId = null;
            if (identity.IsAuthenticated)
            {
                userId = Guid.Parse(identity.GetUserId());
            }
            var userIp = Request.GetClientIp();
            sessionAccess.AddSession(userId, userIp, path);
        }
    }
}
