using SMS.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMS.Web.ViewModels
{
    public class UserRoleViewModel
    {
        public User User { get; set; }
        public List<Role> Roles { get; set; }
    }
}