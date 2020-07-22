using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace SMS.Web.Models
{
    interface ICustomPrincipal : IPrincipal
    {
        string Id { get; set; }
        string FullName { get; set; }
        int PriorityRole { get; set; }
    }
    public class CustomPrincipal : ICustomPrincipal
    {
        public IIdentity Identity { get; private set; }
        public bool IsInRole(string role) { return false; }

        public CustomPrincipal(string email)
        {
            this.Identity = new GenericIdentity(email);
        }

        public string Id { get; set; }
        public string FullName { get; set; }
        public int PriorityRole { get; set; }
    }


    public class CustomPrincipalSerializeModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public int PriorityRole { get; set; }
    }
}