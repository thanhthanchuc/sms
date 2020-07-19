using AutoMapper;
using CrystalDecisions.CrystalReports.Engine;
using Microsoft.Ajax.Utilities;
using SMS.Models.EF;
using SMS.Web.Common;
using SMS.Web.Models.Report;
using SMS.Web.Reports.bring_in;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;

namespace SMS.Web.Controllers
{
    public class HomeController : BaseController
    {
        private SMSDbContext dbcontext;
        public HomeController()
        {
            dbcontext = new SMSDbContext();
        }
        public ActionResult Index()
        {
            var userSession = new UserLogin();
            ViewBag.userSS = userSession.FullName;
            return View();
        }
        //public ActionResult ViewReport(int id)
        //{
        //    var bringin = dbcontext.Bring_In.Find(id);
        //    bringin.Bring_In_Items = dbcontext.Bring_In_Items.Where(t => t.CatID == id).ToList();


        //    var rpdata = Mapper.Map<Bring_In, ListBringInRPModel>(bringin);

        //    ReportDocument rd = new ReportDocument();
        //    var test = System.IO.File.Exists(Path.Combine(Server.MapPath("~/Reports/bring_in"), "BringInReport.rpt"));
        //    rd.Load(Path.Combine(Server.MapPath("~/Reports/bring_in"),"BringInReport.rpt"));
        //    rd.SetDataSource(rpdata);

        //    Response.Buffer = false;
        //    Response.ClearContent();
        //    Response.ClearHeaders();

        //    Stream str = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
        //    str.Seek(0, SeekOrigin.Begin);

        //    return File(str, "application/pdf", "bring_in.pdf");
        //}
        public HttpResponseBase ViewListBringIn()
        {
            var bringins = dbcontext.Bring_In.ToList();
            var rpDatas = new List<ListBringInRPModel>();
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            foreach (var bringin in bringins)
            {
                var bringInItems = dbcontext.Bring_In_Items.Where(t => t.CatID == bringin.ID).ToList();
                var data = new ListBringInRPModel();
                foreach(var item in bringInItems)
                {
                    data.Type = "In";
                    data.FullName = bringin.FullName;
                    data.UserName = bringin.EmpCode;
                    data.Team = bringin.Team;
                    data.Pos = bringin.Position;
                    data.Reason = bringin.Reason;

                    var CreateDate = bringin.CreatedDate != null ? bringin.CreatedDate.Value.ToString("ddMMyyy") : "";

                    // "RBI" + bringin.ID + bringin.Team + CreateDate
                    data.CatID = $"RBI{bringin.ID}{bringin.Team}{CreateDate}";
                    data.Item = item.Item;
                    data.Quantity = item.Quantity != null ? item.Quantity.Value : 0;
                    data.AssetType = item.AssetType != 0 ? (item.AssetType != 1 ? "Độc hại" : "IT") : "Thường";
                    data.Unit = item.Unit;
                    data.Return = item.ReturnDate != null ? "Y" : "N";
                    data.ReturnDate = item.ReturnDate;
                    data.ReturnTime = item.ReturnTime;
                    data.MN = item.ApprovedStatus != null ? (item.ApprovedStatus == 0 ? "Y" : "N") : "?";
                    data.ITT = item.ITT_Status != null ? (item.ITT_Status != 0 ? "Y" : "N") : "?";
                    data.GAT = item.FST_Status != null ? (item.FST_Status != 0 ? "Y" : "N") : "?";
                    data.GuardReturn = item.IsReturn != null ? (item.IsReturn.Value ? "Y" : "N") : "?";
                    data.GuardIn = item.GuardStatusIn != null ? (item.GuardStatusIn != 0 ? "Y" : "N") : "?";
                    data.GuardOut = item.GuardStatusOut != null ? (item.GuardStatusOut != 0 ? "Y" : "N") : "?";
                    rpDatas.Add(data);
                }
            }


            ReportDocument rd = new ReportDocument();
            //var test = System.IO.File.Exists(Path.Combine(Server.MapPath("~/Reports/bring_in"), "ListBringInReport.rpt"));
            rd.Load(Path.Combine(Server.MapPath("~/Reports/bring_in"), "ListBringInReport.rpt"));
            rd.SetDataSource(rpDatas);

            //////////Show file in brower
            Stream str = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            var pdfbytes = new byte[str.Length - 1];

            str.Position = 0;
            str.Read(pdfbytes, 0, Convert.ToInt32(str.Length - 1));
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("content-dispostion", "filename=bringin.pdf");
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-length", pdfbytes.Length.ToString());
            Response.BinaryWrite(pdfbytes);


            //////////Download file pdf
            //Response.Buffer = false;
            //Response.ClearContent();
            //Response.ClearHeaders();

            //Stream str = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            //str.Seek(0, SeekOrigin.Begin);

            //return File(str, "application/pdf", "lst_bring_in.pdf");
            return Response;
        }

        public HttpResponseBase ViewBringDetail(int id)
        {
            var bringin = dbcontext.Bring_In.Find(id);
            var bringInIteam = dbcontext.Bring_In_Items.Where(t => t.CatID == id).ToList();
            //Theem condition filter CreatedDate = noww();


            //var rpData = Mapper.Map<Bring_In, BringInRPModel>(bringin);
            //var rpDataItem = Mapper.Map<List<Bring_In_Items>, List<BringInItemRPModel>>(bringInIteam);
            var rpData = new BringInRPModel();
            rpData.ID = bringin.ID;
            rpData.FullName = bringin.FullName;
            rpData.Reason = bringin.Reason;
            rpData.Position = bringin.Position;
            rpData.Status = bringin.Status != null ? (bringin.Status.Value ? "Y" : "N") : "?";
            rpData.Team = bringin.Team;
            rpData.CreatedBy = bringin.CreatedBy;
            rpData.CreatedDate = bringin.CreatedDate != null ? bringin.CreatedDate.Value.ToString("dd/MM/yyyy") : "";
            rpData.EstimatedDate = bringin.EstimatedDate;
            rpData.EstimatedTime = bringin.EstimatedTime;

            //...

            var rpDataItem = new List<BringInItemRPModel>();
            foreach(var item in bringInIteam)
            {
                var rpItem = new BringInItemRPModel();
                rpItem.CatID = item.CatID.Value.ToString();
                //...
                rpDataItem.Add(rpItem);
            }

            BringInReport detailRP = new BringInReport();
            detailRP.Database.Tables["SMS_Web_Models_Report_BringInItemRPModel"].SetDataSource(rpDataItem);
            detailRP.Database.Tables["SMS_Web_Models_Report_BringInRPModel"].SetDataSource(new List<BringInRPModel>() { rpData });

            //ReportDocument rd = new ReportDocument();
            ////var test = System.IO.File.Exists(Path.Combine(Server.MapPath("~/Reports/bring_in"), "ListBringInReport.rpt"));
            //rd.Load(Path.Combine(Server.MapPath("~/Reports/bring_in"), "BringInReport.rpt"));
            //rd.SetDataSource(rpDataItem);
            //rd.SetDataSource(new List<BringInRPModel>() { rpData });

            Stream str = detailRP.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            var pdfbytes = new byte[str.Length - 1];

            str.Position = 0;
            str.Read(pdfbytes, 0, Convert.ToInt32(str.Length - 1));
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("content-dispostion", "filename=bringin.pdf");
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-length", pdfbytes.Length.ToString());
            Response.BinaryWrite(pdfbytes);
            return Response;
        }
    }
}