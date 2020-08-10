using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using Microsoft.Reporting.WebForms;

namespace SMS.Web.Reports.leave_early
{
    public partial class ReportLE : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Calendar1.Visible = false;
                TextBox1.Text = DateTime.Now.ToString("dd/MM/yyyy");
                Calendar2.Visible = false;
                TextBox2.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
        }

        protected void Button1_Click(object sender, ImageClickEventArgs e)
        {
            if (Calendar1.Visible)
            {
                Calendar1.Visible = false;
            }
            else
            {
                Calendar1.Visible = true;
            }
            Calendar1.Attributes.Add("style", "position:absolute");
        }

        protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        {
            if (Calendar2.Visible)
            {
                Calendar2.Visible = false;
            }
            else
            {
                Calendar2.Visible = true;
            }
            Calendar2.Attributes.Add("style", "position:absolute");
        }

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            TextBox1.Text = Calendar1.SelectedDate.ToString("dd/MM/yyyy");
            Calendar1.Visible = false;
        }

        protected void Calendar2_SelectionChanged(object sender, EventArgs e)
        {
            TextBox2.Text = Calendar2.SelectedDate.ToString("dd/MM/yyyy");
            Calendar2.Visible = false;
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SMSDbContext"].ConnectionString);
            SqlDataAdapter adp = new SqlDataAdapter("Select * from Leave_Early where EstimatedDate between '" + Convert.ToString(TextBox1.Text) + "'and'" + Convert.ToString(TextBox2.Text) + "' and Team = '" + Convert.ToString(TextBox3.Text) + "' and FullName = '" + Convert.ToString(TextBox4.Text) + "'", con);
            DataTable dt = new DataTable("table1");
            adp.Fill(dt);
            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/leave_early/Report_LE.rdlc");
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("dsLE", dt));
            ReportViewer1.LocalReport.Refresh();
        }
    }
}