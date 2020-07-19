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

        private SMSDbContext _context;

        public DelegateController()
        {
            _context = new SMSDbContext();
        }

        // GET: Delegate
        public ActionResult ListEmployee()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FetchDataEmployee()
        {
            var model = _context.Users.OrderByDescending(x => x.ID).ToList();

            return Json(new { data = model, recordsTotal = _context.Users.Count(), recordsFiltered = model.Count() });
        }

        public ActionResult Delegate()
        {
            var user = _context.Users.ToList();
            return View(user);
        }
    }
}