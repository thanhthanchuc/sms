using Newtonsoft.Json;
using SMS.Models.EF;
using SMS.Web.Common;
using SMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace SMS.Web.Controllers
{
    public class IncidentController : BaseController
    {
        private SMSDbContext dbContext;

        public IncidentController()
        {
            dbContext = new SMSDbContext();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FetchDataHistory()
        {
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            var isGuard = (HttpContext.User as CustomPrincipal).RoleName == "Guard";
            var isAdmin = (HttpContext.User as CustomPrincipal).RoleName == "Admin";
            var isSubAdmin = (HttpContext.User as CustomPrincipal).RoleName == "Sub Admin";
            var currentRole = (HttpContext.User as CustomPrincipal).PriorityRole;

            var model = dbContext.Incidents.OrderByDescending(t => t.ID).ToList();

            return Json(new { data = model, currentRole, isAdmin, isSubAdmin, isGuard, recordsTotal = model.Count(), recordsFiltered = model.Count() });
        }

        public ActionResult FileDownload(string filename)
        {
            var file = Server.MapPath("~/Files/") + filename;
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
        }

        /// <summary>
        /// Dowload template báo cáo
        /// </summary>
        /// <returns></returns>
        public ActionResult TemplateReport()
        {
            var file = Server.MapPath("~/Files/Mẫu báo cáo.xlsx");
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Mẫu báo cáo.xlsx");
        }

        /// <summary>
        /// Dowload template xác nhận của SMT
        /// </summary>
        /// <returns></returns>
        public ActionResult TemplateSMTReport()
        {
            var file = Server.MapPath("~/Files/Mẫu SMT xác nhận.xlsx");
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Mẫu SMT xác nhận.xlsx");
        }

        /// <summary>
        /// Dowload template xác nhận của phòng ban
        /// </summary>
        /// <returns></returns>
        public ActionResult TemplateTeamReport()
        {
            var file = Server.MapPath("~/Files/Mẫu Team xác nhận.xlsx");
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Mẫu Team xác nhận.xlsx");
        }

        [AuthorizeUser(IncludeRoleLevels = new[] { 5, 4, 1 })]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var items = dbContext.Incidents.Where(t => t.ID == id).ToList();
            dbContext.Incidents.RemoveRange(items);

            var incident = dbContext.Incidents.FirstOrDefault(t => t.ID == id);
            dbContext.Incidents.Remove(incident);

            dbContext.SaveChanges();

            return Content("Success");
        }

        [HttpPost]
        public ActionResult Create(Incident m)
        {
            var incident = Request.Form.Get("incident");
            var model = JsonConvert.DeserializeObject<Incident>(incident);

            model.CreatedDate = DateTime.Now;
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            model.CreatedBy = user.EmpCode + "|" + user.FullName;

            try
            {
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files.Get("file");
                    var SMTFile = Request.Files.Get("SMTfile");
                    var PICFile = Request.Files.Get("PICfile");
                    var fileName = Path.GetFileName(file.FileName);
                    model.AttachedFile = fileName;
                    var SMTFileName = "";
                    var PICFileName = "";
                    if (SMTFile != null)
                    {
                        SMTFileName = Path.GetFileName(SMTFile.FileName);
                        model.SMTAttachedFile = SMTFileName;
                    }
                    if (PICFile != null)
                    {
                        PICFileName = Path.GetFileName(PICFile.FileName);
                        model.TeamAttachedFile = PICFileName;
                    }


                    if (!Directory.Exists(Server.MapPath("~/Files")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Files"));
                    }
                    var filePath = Server.MapPath("~/Files/" + fileName);
                    if (SMTFileName != "")
                    {
                        var SMTFilePath = Server.MapPath("~/Files/" + SMTFileName);
                        SMTFile.SaveAs(SMTFilePath);
                    }
                    if (PICFileName != "")
                    {
                        var PICFilePath = Server.MapPath("~/Files/" + PICFileName);
                        PICFile.SaveAs(PICFilePath);
                    }
                    file.SaveAs(filePath);
                }

                string json = Newtonsoft.Json.JsonConvert.SerializeObject(model);

                var incidents = new List<Incident>();
                incidents.Add(model);

                dbContext.Incidents.AddRange(incidents);

                dbContext.SaveChanges();
                return Content("Success");
            }

            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting  
                        // the current instance as InnerException  
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
        }

        /// <summary>
        /// Load view Sửa
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AuthorizeUser(AccessLevel = 1)]
        public ActionResult Edit(int id)
        {
            var incident = dbContext.Incidents.Find(id);
            return View(incident);
        }

        /// <summary>
        /// Thực thi Sửa
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        [AuthorizeUser(AccessLevel = 1)]
        [HttpPost]
        public ActionResult Edit(Incident i)
        {
            try
            {
                var incidentObject = Request.Form.Get("incident");
                var model = JsonConvert.DeserializeObject<Incident>(incidentObject);

                var filePath = "";
                var SMTFilePath = "";
                var PICFilePath = "";

                //file
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files.Get("file");
                    var SMTFile = Request.Files.Get("SMTfile");
                    var PICFile = Request.Files.Get("PICfile");
                    var fileName = "";
                    var SMTFileName = "";
                    var PICFileName = "";

                    if (file != null)
                    {
                        fileName = Path.GetFileName(file.FileName);
                        model.AttachedFile = fileName;
                    }
                    if (SMTFile != null)
                    {
                        SMTFileName = Path.GetFileName(SMTFile.FileName);
                        model.SMTAttachedFile = SMTFileName;
                    }
                    if (PICFile != null)
                    {
                        PICFileName = Path.GetFileName(PICFile.FileName);
                        model.TeamAttachedFile = PICFileName;
                    }


                    if (!Directory.Exists(Server.MapPath("~/Files")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Files"));
                    }

                    if (fileName != "")
                    {
                        filePath = Server.MapPath("~/Files/" + fileName);
                        file.SaveAs(filePath);
                    }


                    if (SMTFileName != "")
                    {
                        SMTFilePath = Server.MapPath("~/Files/" + SMTFileName);
                        SMTFile.SaveAs(SMTFilePath);
                    }

                    if (PICFileName != "")
                    {
                        PICFilePath = Server.MapPath("~/Files/" + PICFileName);
                        PICFile.SaveAs(PICFilePath);
                    }
                }

                var user = (UserLogin)Session[CommonConstants.USER_SESSION];

                var incident = dbContext.Incidents.FirstOrDefault(t => t.ID == model.ID);
                incident.ModifiedBy = user.EmpCode + "|" + user.FullName;
                incident.ModifiedDate = DateTime.Now;
                incident.Title = model.Title;

                if (filePath != "")
                {
                    incident.AttachedFile = model.AttachedFile;
                }
                if (SMTFilePath != null)
                {
                    incident.SMTAttachedFile = model.SMTAttachedFile;
                }
                if (PICFilePath != null)
                {
                    incident.TeamAttachedFile = model.TeamAttachedFile;
                }
                dbContext.SaveChanges();
                return Content("Success");
            }

            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting  
                        // the current instance as InnerException  
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
        }
    }
}