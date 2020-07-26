using AutoMapper;
using CrystalDecisions.CrystalReports.Engine;
using Microsoft.Ajax.Utilities;
using SMS.Models.EF;
using SMS.Web.Common;
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
    }
}