using PagedList;
using SMS.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMS.Web.ViewModels
{
    public class LeaveEarlyViewModel
    {
        public IPagedList<Leave_Early> Leave_Early { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string team { get; set; }
        public string empcode { get; set; }
        public int? shift { get; set; }
    }
}