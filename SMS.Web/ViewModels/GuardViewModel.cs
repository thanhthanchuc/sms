
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
        public Go_Out Go_Out { get; set; }
        public Bring_In Bring_In { get; set; }
        public ICollection<Bring_In_Items> Bring_In_Items { get; set; }
        public Bring_Out Bring_Out { get; set; }
        public ICollection<Bring_Out_Items> Bring_Out_Items { get; set; }
        public Guest Guest { get; set; }
    }
}