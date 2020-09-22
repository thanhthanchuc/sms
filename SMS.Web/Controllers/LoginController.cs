using Microsoft.Ajax.Utilities;
using SMS.Models.DAO;
using SMS.Models.EF;
using SMS.Web.Common;
using SMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace SMS.Web.Controllers
{
    public class LoginController : Controller
    {
        private SMSDbContext _context;

        public LoginController()
        {
            _context = new SMSDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var userDAO = new UserDAO();

                if (loginModel.Check)
                {
                    var res = userDAO.Login(loginModel.EmpCode, Encryptor.MD5Hash(loginModel.Password), loginModel.Check);

                    if (res == 1)
                    {
                        var userSession = new UserLogin();

                        var user = _context.Users.Single(u => u.EmpCode == loginModel.EmpCode);
                        var roles = _context.Roles.Where(r => _context.UserRoles.Any(ur => ur.RoleID == r.ID && ur.UserID == user.ID)).OrderByDescending(r => r.Priority).ToList();

                        CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel();
                        serializeModel.Id = user.EmpCode;
                        serializeModel.FullName = user.FullName;
                        serializeModel.PriorityRole = roles[0].Priority;
                        serializeModel.RoleName = roles[0].Name;

                        JavaScriptSerializer serializer = new JavaScriptSerializer();

                        string userData = serializer.Serialize(serializeModel);

                        FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                             1,
                             user.EmpCode,
                             DateTime.Now,
                             DateTime.Now.AddHours(24),
                             false,
                             userData
                         );

                        string encTicket = FormsAuthentication.Encrypt(authTicket);
                        HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                        Response.Cookies.Add(faCookie);

                        userSession.ID = user.ID;
                        userSession.EmpCode = user.EmpCode;
                        userSession.FullName = user.FullName;
                        ViewBag.userSS = userSession.FullName;
                        //userSession.RoleName = userDAO.GetRoleName(user.RoleId);
                        Session["Login"] = user.EmpCode;
                        Session.Add(CommonConstants.USER_SESSION, userSession);
                        return RedirectToAction("Index", "Home");
                    }
                    else if (res == 0)
                    {
                        ModelState.AddModelError("", "Tài khoản không tồn tại");
                    }
                    else if (res == -1)
                    {
                        ModelState.AddModelError("", "Mật khẩu không chính xác");
                    }
                }
                else
                {
                    // sua o day
                    var res = userDAO.Login(loginModel.EmpCode, loginModel.Password, loginModel.Check);
                    if (res == 1)
                    {
                        var userSession = new UserLogin();
                        var user = _context.Users.Single(u => u.EmpCode == loginModel.EmpCode);
                        var roles = _context.Roles.Where(r => _context.UserRoles.Any(ur => ur.RoleID == r.ID && ur.UserID == user.ID)).OrderByDescending(r => r.Priority).ToList();

                        CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel();
                        serializeModel.Id = user.EmpCode;
                        serializeModel.FullName = user.FullName;
                        serializeModel.PriorityRole = roles[0].Priority;
                        serializeModel.RoleName = roles[0].Name;

                        JavaScriptSerializer serializer = new JavaScriptSerializer();

                        string userData = serializer.Serialize(serializeModel);

                        FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                             1,
                             user.EmpCode,
                             DateTime.Now,
                             DateTime.Now.AddHours(24),
                             false,
                             userData
                         );

                        string encTicket = FormsAuthentication.Encrypt(authTicket);
                        HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                        Response.Cookies.Add(faCookie);

                        ViewBag.userSS = (UserLogin)Session[CommonConstants.USER_SESSION];
                        userSession.ID = user.ID;
                        userSession.EmpCode = user.EmpCode;
                        userSession.FullName = user.FullName;
                        //userSession.RoleName = userDAO.GetRoleName(user.RoleId);
                        Session["Login"] = user.EmpCode;
                        Session.Add(CommonConstants.USER_SESSION, userSession);
                        return RedirectToAction("Index", "Home");
                    }
                    else if (res == 0)
                    {
                        ModelState.AddModelError("", "Tài khoản không tồn tại");
                    }
                    else if (res == -1)
                    {
                        ModelState.AddModelError("", "Mật khẩu không chính xác");
                    }
                }
            }
            return View("Index");
        }

        //Ham dang xuat
        public ActionResult Logout()
        {
            Session.Abandon();

            Session.Clear();
            Response.Cookies.Clear();
            Session.RemoveAll();

            Session["Login"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }
    }
}