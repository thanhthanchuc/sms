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
        public ActionResult ListEmployee(string team, string empcode)
        {
            ViewBag.team = team;
            ViewBag.empcode = empcode;
            return View();
        }

        [HttpPost]
        public ActionResult FetchDataEmployee(string team, string empcode)
        {
            _context.Configuration.ProxyCreationEnabled = false;
            var model = _context.Users.ToList();

            if(team != null & team != "")
            {
                model = model.Where(m => m.Team.ToLower().Contains(team.ToLower())).ToList();
            }

            if(empcode != null && empcode !="")
            {
                model = model.Where(m => m.EmpCode.ToString().Contains(empcode)).ToList();
            }    
                

            return Json(new { data = model, recordsTotal = model.Count(), recordsFiltered = model.Count() });
        }

        public ActionResult Delegate()
        {
            var user = _context.Users.ToList();
            return View(user);
        }
    }
}