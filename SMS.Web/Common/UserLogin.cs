using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMS.Web.Common
{
    [Serializable]
    public class UserLogin
    {
        public int ID { get; set; }
        public string EmpCode { get; set; }
        public string FullName { get; set; }

    }
}