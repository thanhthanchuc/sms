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
        bool? Admin { get; set; }

        bool? SubAdmin { get; set; }

        bool? PIC { get; set; }

        bool? ITT_TM { get; set; }

        bool? SMT_TM { get; set; }

        bool? FST_TM { get; set; }

        bool? PIC_TM { get; set; }

        bool? Group_Leader { get; set; }

        bool? Guard { get; set; }

        bool? Read_Only { get; set; }
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
        public bool? Admin { get; set; }

        public bool? SubAdmin { get; set; }

        public bool? PIC { get; set; }

        public bool? ITT_TM { get; set; }

        public bool? SMT_TM { get; set; }

        public bool? FST_TM { get; set; }

        public bool? PIC_TM { get; set; }

        public bool? Group_Leader { get; set; }

        public bool? Guard { get; set; }

        public bool? Read_Only { get; set; }
    }


    public class CustomPrincipalSerializeModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public bool? Admin { get; set; }

        public bool? SubAdmin { get; set; }

        public bool? PIC { get; set; }

        public bool? ITT_TM { get; set; }

        public bool? SMT_TM { get; set; }

        public bool? FST_TM { get; set; }

        public bool? PIC_TM { get; set; }

        public bool? Group_Leader { get; set; }

        public bool? Guard { get; set; }

        public bool? Read_Only { get; set; }
    }
}