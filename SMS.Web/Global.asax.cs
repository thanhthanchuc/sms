using SMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace SMS.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            HttpConfiguration config = GlobalConfiguration.Configuration;
        }

        protected void Application_BeginRequest()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                CustomPrincipalSerializeModel serializeModel = serializer.Deserialize<CustomPrincipalSerializeModel>(authTicket.UserData);

                CustomPrincipal newUser = new CustomPrincipal(authTicket.Name);
                newUser.Id = serializeModel.Id;
                newUser.FullName = serializeModel.FullName;
                newUser.Admin = serializeModel.Admin;

                newUser.SubAdmin = serializeModel.SubAdmin;

                newUser.PIC = serializeModel.PIC;

                newUser.ITT_TM = serializeModel.ITT_TM;

                newUser.SMT_TM = serializeModel.SMT_TM;

                newUser.FST_TM = serializeModel.FST_TM;

                newUser.PIC_TM = serializeModel.PIC_TM;

                newUser.Group_Leader = serializeModel.Group_Leader;

                newUser.Guard = serializeModel.Guard;

                newUser.Read_Only = serializeModel.Read_Only;

                HttpContext.Current.User = newUser;
            }
        }
    }
}
