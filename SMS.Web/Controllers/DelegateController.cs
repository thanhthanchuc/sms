using SMS.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMS.Web.Controllers
{
    public class DelegateController : BaseController
    {

        private SMSDbContext dbContext;

        public DelegateController()
        {
            dbContext = new SMSDbContext();
        }

        // GET: Delegate
        public ActionResult ListEmployee()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FetchDataEmployee()
        {
            var model = dbContext.Users.OrderByDescending(x => x.ID).ToList();

            return Json(new { data = model, recordsTotal = dbContext.Users.Count(), recordsFiltered = model.Count() });
        }

        public ActionResult Delegate()
        {
            return View();
        }
    }
}