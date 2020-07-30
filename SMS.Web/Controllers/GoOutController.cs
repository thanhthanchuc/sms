using SMS.Models.DAO;
using SMS.Models.EF;
using SMS.Web.Common;
using SMS.Web.Models;
using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;

namespace SMS.Web.Controllers
{
    public class GoOutController : BaseController
    {
        private SMSDbContext _dbContext;

        public GoOutController()
        {
            _dbContext = new SMSDbContext();
        }

        // GET: GoOut
        public ActionResult History(string searchString, int page = 1, int pageSize = 20)
        {
            var dao = new GoOutDAO();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }

        /// <summary>
        /// Phương thức load view form tạo mới
        /// </summary>
        /// <returns></returns>
        /// 
        [AuthorizeUser(AccessLevel = 2)]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Phương thức tạo mới
        /// </summary>
        /// <param name="go_Out"></param>
        /// <returns></returns>
        /// 
        [AuthorizeUser(AccessLevel = 2)]
        [HttpPost]
        public ActionResult Create(Go_Out go_Out)
        {
            if (ModelState.IsValid)
            {
                var dao = new GoOutDAO();
                go_Out.CreatedDate = DateTime.Now;
                var user = (UserLogin)Session[CommonConstants.USER_SESSION];
                go_Out.CreatedBy = user.EmpCode + "|" + user.FullName;
                int id = dao.Insert(go_Out, user.EmpCode);
                if (id > 0)
                {
                    return RedirectToAction("History", "GoOut");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm mới thất bại");
                }
            }
            else
            {
                ModelState.AddModelError("", "Thêm mới thất bại");
                return View("Create");
            }
            return RedirectToAction("History");
        }

        /// <summary>
        /// Phương thức load view sửa
        /// </summary>
        /// <returns></returns>
        /// 
        [AuthorizeUser(AccessLevel = 2)]
        public ActionResult Edit(int id)
        {
            var goOut = new GoOutDAO().ViewDetail(id);
            return View(goOut);
        }

        /// <summary>
        /// Phương thức sửa cho PIC
        /// </summary>
        /// <param name="go_Out"></param>
        /// <returns></returns>
        /// 
        [AuthorizeUser(AccessLevel = 2)]
        [HttpPost]
        public ActionResult Edit(Go_Out go_Out)
        {
            if (ModelState.IsValid)
            {
                var dao = new GoOutDAO();
                var user = (UserLogin)Session[CommonConstants.USER_SESSION];
                go_Out.ModifiedBy = user.EmpCode + "|" + user.FullName;
                var res = dao.Update(go_Out, user.EmpCode);
                if (res /*&& (go_Out.EstimatedDateReturn >= go_Out.EstimatedDateOut)*/)
                {
                    return RedirectToAction("History", "GoOut");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật thất bại");
                }
            }
            return View("History");
        }

        /// <summary>
        /// Phương thức load view Phê duyệt
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        /// 
        [AuthorizeUser(AccessLevel = 3)]
        public ActionResult Approve(string searchString, int page = 1, int pageSize = 20)
        {
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            var tName = _dbContext.Users.Include(t => t.Team).First(u => u.ID == user.ID).Team.Name;

            bool unfilter = false;

            if ((HttpContext.User as CustomPrincipal).PriorityRole >= 4)
            {
                unfilter = true;
            }

            var dao = new GoOutDAO();
            var model = dao.ListApprove(searchString, page, pageSize, tName, unfilter);
            return View(model);
        }

        private string FormatDate(string date)
        {
            var strs = date.Split('/');
            return strs[1] + "/" + strs[0] + "/" + strs[2];
        }

        /// <summary>
        /// Phương thức phê duyệt cho Admin, TM, GL
        /// </summary>
        /// <param name="go_Out"></param>
        /// <returns></returns>
        /// 
        [AuthorizeUser(AccessLevel = 3)]
        [HttpPost]
        public JsonResult ApproveForAdmin(int id, string remark)
        {
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            var res = new GoOutDAO().ApproveForAdmin(id, remark, user.EmpCode + "|" + user.FullName);
            return Json(new
            {
                status = res
            });
        }

        /// <summary>
        /// Phương thức reject cho Admin, TM, GL
        /// </summary>
        /// <param name="go_Out"></param>
        /// <returns></returns>
        /// 
        [AuthorizeUser(AccessLevel = 3)]
        [HttpPost]
        public JsonResult RejectForAdmin(int id, string remark)
        {
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            var res = new GoOutDAO().RejectForAdmin(id, remark, user.EmpCode + "|" + user.FullName);
            return Json(new
            {
                status = res
            });
        }

        /// <summary>
        /// Phương thức xóa
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [AuthorizeUser(AccessLevel = 2)]
        public ActionResult Delete(int id)
        {
            new GoOutDAO().Delete(id);
            return RedirectToAction("History");
        }

        /// <summary>
        /// Hàm hủy bỏ yêu cầu đã được phê duyệt
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [AuthorizeUser(AccessLevel = 2)]
        public ActionResult Cancel(int id)
        {
            new GoOutDAO().Cancel(id);
            return RedirectToAction("History");
        }

        public ActionResult GOReport(int id)
        {
            var goOut = new GoOutDAO().ViewDetail(id);
            return View(goOut);
        }

        public ActionResult SummaryGO()
        {
            var go = _dbContext.Go_Out.OrderByDescending(g => g.CreatedDate).ToList();
            return View(go);
        }

        /// <summary>
        /// Phương thức lấy ra thông tin nhân viên theo mã
        /// </summary>
        /// <param name="empCode"></param>
        /// <returns></returns>
        public JsonResult GetByCodeGO(string empCode)
        {
            var dao = new GoOutDAO();
            User user = new User();
            UserViewModel userViewModel = new UserViewModel();
            user = dao.GetByCode(empCode);
            userViewModel.UpdateUser(user);
            if (userViewModel.EmpCode != null)
            {
                return Json(new
                {
                    data = userViewModel,
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
    }
}