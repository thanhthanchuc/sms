using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SMS.Web.Models
{
    public class ReportParams
    {
        public string RptFileName { get; set; }
        public string ReportTitle { get; set; }
        public string ReportType { get; set; }
        public DataTable DataSource { get; set; }
        public bool IsHasParams { get; set; }
        public string DataSetName { get; internal set; }
    }
}