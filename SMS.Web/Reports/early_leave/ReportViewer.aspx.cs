using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace SMS.Web.Reports.early_leave
{
    public partial class ReportViewer : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadReport();
            }
        }

        private void LoadReport()
        {
            var reportParam = (dynamic) HttpContext.Current.Session["ReportParam"];
            if (reportParam != null & !string.IsNullOrEmpty(reportParam.RptFileName))
            {
                Page.Title = "Report|" + reportParam.ReportTitle;
                var dt = new DataTable();
                dt = reportParam.DataSource();
                if (dt.Rows.Count > 0)
                {
                    GenerateReportDocument(reportParam, reportParam.ReportType, dt);
                }
            }
        }

        public void GenerateReportDocument(dynamic reportParam, string reportType, DataTable data)
        {
            string dsName = reportParam.DataSetName;
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource(dsName, data));
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/" + "Reports//rpt//" + reportParam.RptFileName);
            ReportViewer1.DataBind();
            ReportViewer1.LocalReport.Refresh();
        }
    }
}