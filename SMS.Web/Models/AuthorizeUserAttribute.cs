using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMS.Web.Models
{
    public class AuthorizeUserAttribute: AuthorizeAttribute
    {
        public int AccessLevel { get; set; }
        public string RoleName { get; set; }

        public int[] ExceptRoleLevels { get; set; }

        public int[] IncludeRoleLevels { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                return false;
            }



            string roleName = (httpContext.User as CustomPrincipal).RoleName;

            if(RoleName != null || RoleName != "")
            {
                return roleName.Contains(RoleName);
            }

            int privilegeLevels = (httpContext.User as CustomPrincipal).PriorityRole; // Call another method to get rights of the user from DB

            if (ExceptRoleLevels.Length != 0)
            {
                return !ExceptRoleLevels.Contains(privilegeLevels);
            }

            if (IncludeRoleLevels.Length != 0)
            {
                return IncludeRoleLevels.Contains(privilegeLevels);
            }


            return AccessLevel >= privilegeLevels;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {

            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {

                filterContext.Result = new RedirectToRouteResult(
                            new System.Web.Routing.RouteValueDictionary(
                                new
                                {
                                    controller = "Delegate",
                                    action = "Unauthorised"
                                })
                            );
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}