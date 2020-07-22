﻿using SMS.Models.EF;
using SMS.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
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
            var model = _context.Users.Include(u => u.Team).ToList();

            if (team != null & team != "")
            {
                model = model.Where(m => m.Team.Name.ToLower().Contains(team.ToLower())).ToList();
            }

            if (empcode != null && empcode != "")
            {
                model = model.Where(m => m.EmpCode.ToString().Contains(empcode)).ToList();
            }
            return Json(new { data = model, recordsTotal = model.Count(), recordsFiltered = model.Count() });
        }

        public ActionResult Delegate()
        {
            var user = _context.Users.Include(u => u.UserRoles).ToList();
            var roles = _context.Roles.ToList();

            List<UserRoleViewModel> userRoleViewModels = new List<UserRoleViewModel>();

            user.ForEach(u =>
            {
                userRoleViewModels.Add(new UserRoleViewModel()
                {
                    User = u,
                    Roles = roles.Where(r => u.UserRoles.Any(ur => ur.RoleID == r.ID)).ToList()
                });
            });

            return View(userRoleViewModels);
        }


        public ActionResult GetUserRole(string empCode)
        {
            var user = _context.Users.Include(u => u.Team).Single(u => u.EmpCode == empCode);
            var roles = _context.UserRoles.Where(ur => ur.UserID == user.ID).Select(r => r.Role).ToList();

            if (user != null)
            {
                return Json(new
                { 
                    data = new { user.ID, user.FullName, user.Position, Team = user.Team.Name, roles },
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    status = false
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UpdateRoles(string empCode,int[] roles)
        {
            var user = _context.Users.Single(u => u.EmpCode == empCode);
            var prevUserRoles = _context.UserRoles.Where(ur => ur.UserID == user.ID).ToList();

            _context.UserRoles.RemoveRange(prevUserRoles);
            foreach(var role in roles)
            {
                _context.UserRoles.Add(new UserRole() { UserID = user.ID, RoleID = role });
            }

            _context.SaveChanges();

            return Content("OK");
        }
    }
}