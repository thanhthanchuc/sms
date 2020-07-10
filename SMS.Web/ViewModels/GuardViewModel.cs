
using SMS.Models.EF;
using SMS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMS.Web.ViewModels
{
    public class GuardViewModel
    {
        public UserLogin UserLogin { get; set; }
        public Leave_Early Leave_Early { get; set; }
    }
}