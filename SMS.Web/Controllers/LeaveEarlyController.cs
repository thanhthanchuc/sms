using ExcelLibrary.BinaryFileFormat;
using PagedList;
using SMS.Models.DAO;
using SMS.Models.EF;
using SMS.Web.Common;
using SMS.Web.Models;
using SMS.Web.ViewModels;
using System;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace SMS.Web.Controllers
{
    public class LeaveEarlyController : BaseController
    {
        private SMSDbContext _dbContext;

        public LeaveEarlyController()
        {
            _dbContext = new SMSDbContext();
        }
        // GET: LeaveEarly
        public ActionResult History(string searchString, int page = 1, int pageSize = 20)
        {
            var dao = new LeaveEarlyDAO();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }

        /// <summary>
        /// Phương thức load view form tạo mới
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Phương thức tạo mới
        /// </summary>
        /// <param name="leave_Early"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(Leave_Early leave_Early)
        {
            if (ModelState.IsValid)
            {
                var dao = new LeaveEarlyDAO();
                leave_Early.CreatedDate = DateTime.Now;
                var user = (UserLogin)Session[CommonConstants.USER_SESSION];
                leave_Early.CreatedBy = user.EmpCode + "|" + user.FullName;
                int id = dao.Insert(leave_Early, user.EmpCode);
                if (id > 0)
                {
                    return RedirectToAction("History", "LeaveEarly");
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
        /// Xuất báo cáo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult LEReport(int id)
        {
            var leaveEarly = new LeaveEarlyDAO().ViewDetail(id);
            return View(leaveEarly);
        }

        public ActionResult SummaryLE()
        {
            return View();
        }

        /// <summary>
        /// Phương thức load view sửa
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            var leaveEarly = new LeaveEarlyDAO().ViewDetail(id);
            return View(leaveEarly);
        }

        /// <summary>
        /// Phương thức sửa cho PIC
        /// </summary>
        /// <param name="leave_Early"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(Leave_Early leave_Early)
        {
            if (ModelState.IsValid)
            {
                var dao = new LeaveEarlyDAO();
                var user = (UserLogin)Session[CommonConstants.USER_SESSION];
                leave_Early.ModifiedBy = user.EmpCode + "|" + user.FullName;
                var res = dao.Update(leave_Early, user.EmpCode);
                if (res)
                {
                    SetAlert("Cập nhật yêu cầu thành công", "success");
                    return RedirectToAction("History", "LeaveEarly");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật thất bại");
                }
            }
            else
            {
                ModelState.AddModelError("", "Cập nhật thất bại");
                return View("Edit");
            }
            return RedirectToAction("History");
        }

        /// <summary>
        /// Phương thức load view Phê duyệt
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult Approve(int? from, int? to, string team, string empcode, int? shift, int page = 1, int pageSize = 20)
        {
            var res = _dbContext.Leave_Early.ToList();

            string new_from = null;
            string new_to = null;

            if (from != null)
            {
                res = res.Where(t => ((DateTimeOffset)DateTime.Parse(FormatDate(t.EstimatedDate))).ToUnixTimeSeconds() >= from).ToList();
                new_from = DateTimeOffset.FromUnixTimeSeconds((long)from).UtcDateTime.ToString("dd/MM/yyyy");
            }

            if (to != null)
            {
                res = res.Where(t => ((DateTimeOffset)DateTime.Parse(FormatDate(t.EstimatedDate))).ToUnixTimeSeconds() <= to).ToList();
                new_to = DateTimeOffset.FromUnixTimeSeconds((long)to).UtcDateTime.Date.ToString("dd/MM/yyyy");
            }

            if (shift != null)
            {
                res = res.Where(t => t.Shift == shift).ToList();
            }

            if (!string.IsNullOrEmpty(team))
            {
                res = res.Where(t => t.Team.Contains(team)).ToList();
            }

            if (!string.IsNullOrEmpty(empcode))
            {
                res = res.Where(t => t.EmpCode.Contains(empcode)).ToList();
            }

            var model = res.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);

            var viewModel = new LeaveEarlyViewModel()
            {
                Leave_Early = model,
                from = new_from,
                to = new_to,
                empcode = empcode,
                shift = shift,
                team = team
            };

            return View(viewModel);
        }

        private string FormatDate(string date)
        {
            var strs = date.Split('/');
            return strs[1] + "/" + strs[0] + "/" + strs[2];
        }

        /// <summary>
        /// Phương thức phê duyệt cho Admin, TM, GL
        /// </summary>
        /// <param name="leave_Early"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ApproveForAdmin(int id, string remark)
        {
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            var res = new LeaveEarlyDAO().ApproveForAdmin(id, remark, user.EmpCode + "|" + user.FullName);
            return Json(new
            {
                status = res
            });
        }

        /// <summary>
        /// Phương thức reject cho Admin, TM, GL
        /// </summary>
        /// <param name="leave_Early"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RejectForAdmin(int id, string remark)
        {
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            var res = new LeaveEarlyDAO().RejectForAdmin(id, remark, user.EmpCode + "|" + user.FullName);
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
        public ActionResult Delete(int id)
        {
            new LeaveEarlyDAO().Delete(id);
            return RedirectToAction("History");
        }

        /// <summary>
        /// Hàm hủy bỏ yêu cầu đã được phê duyệt
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Cancel(int id)
        {
            new LeaveEarlyDAO().Cancel(id);
            return RedirectToAction("History");
        }

        /// <summary>
        /// Phương thức lấy ra thông tin nhân viên theo mã
        /// </summary>
        /// <param name="empCode"></param>
        /// <returns></returns>
        public JsonResult GetByCodeLE(string empCode)
        {
            var dao = new LeaveEarlyDAO();
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