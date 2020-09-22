using Newtonsoft.Json;
using SMS.Models.EF;
using SMS.Web.Common;
using SMS.Web.Models;
using SMS.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SMS.Web.Controllers
{
    public class GuardController : BaseController
    {
        private SMSDbContext dbcontext;
        public GuardController()
        {
            dbcontext = new SMSDbContext();
        }

        /// <summary>
        /// Lịch sử
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="guest"></param>
        /// <param name="company"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public ActionResult History(string employee = "", string guest = "", string company = "", string from = null, string to = null)
        {
            ViewBag.from = from;
            ViewBag.to = to;
            ViewBag.employee = employee;
            ViewBag.guest = guest;
            ViewBag.company = company;

            if (from == null || to == null)
            {
                from = DateTime.Now.ToString("dd/MM/yyyy");
                to = DateTime.Now.ToString("dd/MM/yyyy");
            }

            var now = DateTime.Now.ToString("dd/MM/yyyy");

            var le = dbcontext.Leave_Early.ToList();
            ViewBag.LeaveEarlies = le.Where(t => (
                                           DateTime.ParseExact(t.EstimatedDate, "dd/MM/yyyy", null) >= DateTime.ParseExact(from, "dd/MM/yyyy", null) &&
                                           DateTime.ParseExact(t.EstimatedDate, "dd/MM/yyyy", null) <= DateTime.ParseExact(to, "dd/MM/yyyy", null) &&
                                           t.EmpCode.ToLower().Contains(employee.ToLower()))).ToList();

            var go = dbcontext.Go_Out.ToList();
            ViewBag.GoOuts = go.Where(t => (
                                           DateTime.ParseExact(t.EstimatedDateOut, "dd/MM/yyyy", null) >= DateTime.ParseExact(from, "dd/MM/yyyy", null) &&
                                           DateTime.ParseExact(t.EstimatedDateReturn, "dd/MM/yyyy", null) <= DateTime.ParseExact(to, "dd/MM/yyyy", null) &&
                                           t.EmpCode.ToLower().Contains(employee.ToLower()))).ToList();

            var bi = dbcontext.Bring_In.ToList();
            var bt = dbcontext.Bring_In_Items.ToList();
            ViewBag.BringIns = bi.Where(t => (
                                          DateTime.ParseExact(t.EstimatedDate, "dd/MM/yyyy", null) >= DateTime.ParseExact(from, "dd/MM/yyyy", null) &&
                                          DateTime.ParseExact(t.EstimatedDate, "dd/MM/yyyy", null) <= DateTime.ParseExact(to, "dd/MM/yyyy", null) &&
                                          t.EmpCode.ToLower().Contains(employee.ToLower()))).ToList();

            var bo = dbcontext.Bring_Out.ToList();
            ViewBag.BringOuts = bo.Where(t => (
                                          DateTime.ParseExact(t.EstimatedDate, "dd/MM/yyyy", null) >= DateTime.ParseExact(from, "dd/MM/yyyy", null) &&
                                          DateTime.ParseExact(t.EstimatedDate, "dd/MM/yyyy", null) <= DateTime.ParseExact(to, "dd/MM/yyyy", null) &&
                                          t.EmpCode.ToLower().Contains(employee.ToLower()))).ToList();

            var fill_guest = dbcontext.Guests.ToList();
            var Guests = fill_guest.Where(t => (
                                           DateTime.ParseExact(t.EstimatedDateIn, "dd/MM/yyyy", null) >= DateTime.ParseExact(from, "dd/MM/yyyy", null) &&
                                           DateTime.ParseExact(t.EstimatedDateOut, "dd/MM/yyyy", null) <= DateTime.ParseExact(to, "dd/MM/yyyy", null) &&
                                           (string.IsNullOrEmpty(guest) || t.FullName.ToLower().Contains(guest.ToLower()))) && (string.IsNullOrEmpty(company) || t.Company.ToLower().Contains(company.ToLower()))
                                           ).ToList();

            var guest_no_items = new List<Guest>();
            var guest_has_items = new List<Guest>();
            var foreign_guest_no_items = new List<Guest>();
            var foreign_guest_has_items = new List<Guest>();
            foreach (var g in Guests)
            {
                if (g.Check_Guest == false)
                {
                    if (dbcontext.Guest_Item.Where(t => t.CatID == g.ID && t.Item != null).Count() > 0)
                    {
                        guest_has_items.Add(g);
                    }
                    else
                    {
                        guest_no_items.Add(g);
                    }
                }
                else
                {
                    if (dbcontext.Guest_Item.Where(t => t.CatID == g.ID && t.Item != null).Count() > 0)
                    {
                        foreign_guest_has_items.Add(g);
                    }
                    else
                    {
                        foreign_guest_no_items.Add(g);
                    }
                }
            }
            ViewBag.GuestNoItems = guest_no_items;
            ViewBag.GuestHasItems = guest_has_items;
            ViewBag.ForeignGuestNoItems = foreign_guest_no_items;
            ViewBag.ForeignGuestHasItems = foreign_guest_has_items;

            return View();
        }

        /// <summary>
        /// Chờ xử lý
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="guest"></param>
        /// <param name="company"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public ActionResult Queue(string employee = "", string guest = "", string company = "", string from = null, string to = null)
        {
            ViewBag.from = from;
            ViewBag.to = to;
            ViewBag.employee = employee;
            ViewBag.guest = guest;
            ViewBag.company = company;

            if (from == null || to == null)
            {
                from = DateTime.Now.ToString("dd/MM/yyyy");
                to = DateTime.Now.ToString("dd/MM/yyyy");
            }

            var le = dbcontext.Leave_Early.ToList();
            ViewBag.LeaveEarlies = le.Where(t => (
                                           DateTime.ParseExact(t.EstimatedDate, "dd/MM/yyyy", null) >= DateTime.ParseExact(from, "dd/MM/yyyy", null) &&
                                           DateTime.ParseExact(t.EstimatedDate, "dd/MM/yyyy", null) <= DateTime.ParseExact(to, "dd/MM/yyyy", null) &&
                                           t.EmpCode.ToLower().Contains(employee.ToLower()) &&
                                           t.GuardStatus == null &&
                                           t.ApprovedStatus != 4)).ToList();

            var go = dbcontext.Go_Out.ToList();
            ViewBag.GoOuts = go.Where(t => (
                                           DateTime.ParseExact(t.EstimatedDateOut, "dd/MM/yyyy", null) >= DateTime.ParseExact(from, "dd/MM/yyyy", null) &&
                                           DateTime.ParseExact(t.EstimatedDateReturn, "dd/MM/yyyy", null) <= DateTime.ParseExact(to, "dd/MM/yyyy", null) &&
                                           t.EmpCode.ToLower().Contains(employee.ToLower()) &&
                                           t.GuardStatusReturn == null &&
                                           t.ApprovedStatus != 4)).ToList();

            var bringIns = dbcontext.Bring_In.Where(t => t.Cancel != true && (string.IsNullOrEmpty(employee) || t.EmpCode.ToLower().Contains(employee.ToLower()))).ToList();
            var bringInItems = dbcontext.Bring_In_Items.ToList();
            ViewBag.BringInItems = bringInItems;
            ViewBag.BringIns = bringIns.Where(b => bringInItems.Where(i => i.CatID == b.ID && i.GuardStatusOut == null).Count() != 0).ToList();

            var bringOuts = dbcontext.Bring_Out.Where(t => t.Cancel != true && (string.IsNullOrEmpty(employee) || t.EmpCode.ToLower().Contains(employee.ToLower()))).ToList();
            var bringOutItems = dbcontext.Bring_Out_Items.ToList();
            ViewBag.BringOutItems = bringOutItems;
            ViewBag.BringOuts = bringOuts.Where(b => bringOutItems.Where(i => i.CatID == b.ID && i.GuardStatusReturn == null).Count() != 0).ToList();

            var fill_guest = dbcontext.Guests.ToList();
            var Guests = fill_guest.Where(t => (
                                           DateTime.ParseExact(t.EstimatedDateIn, "dd/MM/yyyy", null) >= DateTime.ParseExact(from, "dd/MM/yyyy", null) &&
                                           DateTime.ParseExact(t.EstimatedDateOut, "dd/MM/yyyy", null) <= DateTime.ParseExact(to, "dd/MM/yyyy", null) &&
                                           (string.IsNullOrEmpty(guest) || t.FullName.ToLower().Contains(guest.ToLower()))) && 
                                           (string.IsNullOrEmpty(company) || t.Company.ToLower().Contains(company.ToLower())) &&
                                           t.GuardStatusOut == null).ToList();

            var guest_no_items = new List<Guest>();
            var guest_has_items = new List<Guest>();
            var foreign_guest_no_items = new List<Guest>();
            var foreign_guest_has_items = new List<Guest>();
            foreach (var g in Guests)
            {
                if (g.Check_Guest == false || g.Check_Guest == null)
                {
                    if (dbcontext.Guest_Item.Where(t => t.CatID == g.ID && t.Item != null).Count() > 0)
                    {
                        guest_has_items.Add(g);
                    }
                    else
                    {
                        guest_no_items.Add(g);
                    }
                }

                if (g.Check_Guest == true)
                {
                    if (dbcontext.Guest_Item.Where(t => t.CatID == g.ID && t.Item != null).Count() > 0)
                    {
                        foreign_guest_has_items.Add(g);
                    }
                    else
                    {
                        foreign_guest_no_items.Add(g);
                    }
                }
            }
            ViewBag.GuestNoItems = guest_no_items;
            ViewBag.GuestHasItems = guest_has_items;
            ViewBag.ForeignGuestNoItems = foreign_guest_no_items;
            ViewBag.ForeignGuestHasItems = foreign_guest_has_items;

            return View();
        }

        /// <summary>
        /// Xác nhận về sớm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [AuthorizeUser(IncludeRoleLevels = new int[] { 5, 4, 1 })]
        public ActionResult ConfirmLE(int id)
        {
            var model = dbcontext.Leave_Early.FirstOrDefault(l => l.ID == id);
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];

            if (model == null)
            {
                return View(); // 404
            }

            var guardViewModel = new GuardViewModel()
            {
                UserLogin = user,
                Leave_Early = model
            };

            return View(guardViewModel);
        }

        /// <summary>
        /// Lịch sử Về sớm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AuthorizeUser(IncludeRoleLevels = new int[] { 5, 4, 1 })]
        public ActionResult HistoryLE(int id)
        {
            var model = dbcontext.Leave_Early.FirstOrDefault(l => l.ID == id);
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];

            if (model == null)
            {
                return View(); // 404
            }

            var guardViewModel = new GuardViewModel()
            {
                UserLogin = user,
                Leave_Early = model
            };

            return View(guardViewModel);
        }

        /// <summary>
        /// Xác nhận Ra ngoài
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AuthorizeUser(IncludeRoleLevels = new int[] { 5, 4, 1 })]
        public ActionResult ConfirmGO(int id)
        {
            var model = dbcontext.Go_Out.Find(id);
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];

            var guardViewModel = new GuardViewModel()
            {
                UserLogin = user,
                Go_Out = model
            };

            return View(guardViewModel);
        }

        /// <summary>
        /// Lịch sử Ra ngoài
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AuthorizeUser(IncludeRoleLevels = new int[] { 5, 4, 1 })]
        public ActionResult HistoryGO(int id)
        {
            var model = dbcontext.Go_Out.Find(id);
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];

            var guardViewModel = new GuardViewModel()
            {
                UserLogin = user,
                Go_Out = model
            };

            return View(guardViewModel);
        }

        /// <summary>
        /// Xác nhận khâu ra ngoài
        /// </summary>
        /// <param name="id"></param>
        /// <param name="remark"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [AuthorizeUser(IncludeRoleLevels = new int[] { 5, 4, 1 })]
        [HttpPost]
        public ActionResult ApproveGoOut(int id, string remark, int status)
        {
            var data = dbcontext.Go_Out.Find(id);
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            data.GuardOut = user.EmpCode + " | " + user.FullName;
            data.GuardStatusOut = status;
            data.GuardRemarkOut = remark;
            data.GuardDateOut = DateTime.Now;

            dbcontext.SaveChanges();
            return Content(JsonConvert.SerializeObject(data), "application/json");
        }

        /// <summary>
        /// Xác nhận khâu Quay lại
        /// </summary>
        /// <param name="id"></param>
        /// <param name="remark"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [AuthorizeUser(IncludeRoleLevels = new int[] { 5, 4, 1 })]
        [HttpPost]
        public ActionResult ApproveGoReturn(int id, string remark, int status)
        {
            var data = dbcontext.Go_Out.Find(id);
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            data.GuardReturn = user.EmpCode + " | " + user.FullName;
            data.GuardStatusReturn = status;
            data.GuardRemarkReturn = remark;
            data.GuardDateReturn = DateTime.Now;

            dbcontext.SaveChanges();
            return Content(JsonConvert.SerializeObject(data), "application/json");
        }

        /// <summary>
        /// Xác nhận Mang hàng vào
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AuthorizeUser(IncludeRoleLevels = new int[] { 5, 4, 1 })]
        public ActionResult ConfirmBringIn(int id)
        {
            var model = dbcontext.Bring_In.Find(id);
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            var items = dbcontext.Bring_In_Items.Where(i => i.CatID == model.ID).ToList();

            var guardViewModel = new GuardViewModel()
            {
                UserLogin = user,
                Bring_In = model,
                Bring_In_Items = items
            };

            return View(guardViewModel);
        }

        /// <summary>
        /// Xác nhận Mang hàng vào
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AuthorizeUser(IncludeRoleLevels = new int[] { 5, 4, 1 })]
        public ActionResult HistoryBI(int id)
        {
            var model = dbcontext.Bring_In.Find(id);
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            var items = dbcontext.Bring_In_Items.Where(i => i.CatID == model.ID).ToList();

            var guardViewModel = new GuardViewModel()
            {
                UserLogin = user,
                Bring_In = model,
                Bring_In_Items = items
            };

            return View(guardViewModel);
        }

        /// <summary>
        /// Bảo vệ xác nhận mang hàng vào
        /// </summary>
        /// <param name="id"></param>
        /// <param name="remark"></param>
        /// <param name="status"></param>
        /// <param name="ret"></param>
        /// <returns></returns>
        [AuthorizeUser(IncludeRoleLevels = new int[] { 5, 4, 1 })]
        [HttpPost]
        public ActionResult ApproveBringInItem(int id, string remark, int status, bool ret = false)
        {
            if (ret)
            {
                var item = dbcontext.Bring_In_Items.Find(id);
                var user = (UserLogin)Session[CommonConstants.USER_SESSION];
                item.GuardOut = user.EmpCode + " | " + user.FullName;
                item.GuardStatusOut = status;
                item.GuardRemarkOut = remark;
                item.GuardDateOut = DateTime.Now;

                dbcontext.SaveChanges();
                return Content(JsonConvert.SerializeObject(item), "application/json");
            }
            else
            {
                var item = dbcontext.Bring_In_Items.Find(id);
                var user = (UserLogin)Session[CommonConstants.USER_SESSION];
                item.GuardIn = user.EmpCode + " | " + user.FullName;
                item.GuardStatusIn = status;
                item.GuardRemarkIn = remark;
                item.GuardDateIn = DateTime.Now;
                if (item.IsReturn == false)
                {
                    item.GuardOut = null;
                    item.GuardStatusOut = 0;
                    item.GuardRemarkOut = null;
                    item.GuardDateOut = null;
                }

                dbcontext.SaveChanges();
                return Content(JsonConvert.SerializeObject(item), "application/json");
            }
        }

        [AuthorizeUser(IncludeRoleLevels = new int[] { 5, 4, 1 })]
        public ActionResult ConfirmBringOut(int id)
        {
            var model = dbcontext.Bring_Out.Find(id);
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            var items = dbcontext.Bring_Out_Items.Where(i => i.CatID == model.ID).ToList();

            var guardViewModel = new GuardViewModel()
            {
                UserLogin = user,
                Bring_Out = model,
                Bring_Out_Items = items
            };

            return View(guardViewModel);
        }

        [AuthorizeUser(IncludeRoleLevels = new int[] { 5, 4, 1 })]
        public ActionResult HistoryBO(int id)
        {
            var model = dbcontext.Bring_Out.Find(id);
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            var items = dbcontext.Bring_Out_Items.Where(i => i.CatID == model.ID).ToList();

            var guardViewModel = new GuardViewModel()
            {
                UserLogin = user,
                Bring_Out = model,
                Bring_Out_Items = items
            };

            return View(guardViewModel);
        }

        [AuthorizeUser(IncludeRoleLevels = new int[] { 5, 4, 1 })]
        [HttpPost]
        public ActionResult ApproveBringOutItem(int id, string remark, int status, bool ret = false)
        {
            if (ret)
            {
                var item = dbcontext.Bring_Out_Items.Find(id);
                var user = (UserLogin)Session[CommonConstants.USER_SESSION];
                item.GuardReturn = user.EmpCode + " | " + user.FullName;
                item.GuardStatusReturn = status;
                item.GuardRemarkReturn = remark;
                item.GuardDateReturn = DateTime.Now;

                dbcontext.SaveChanges();
                return Content(JsonConvert.SerializeObject(item), "application/json");
            }
            else
            {
                var item = dbcontext.Bring_Out_Items.Find(id);
                var user = (UserLogin)Session[CommonConstants.USER_SESSION];
                item.GuardOut = user.EmpCode + " | " + user.FullName;
                item.GuardStatusOut = status;
                item.GuardRemarkOut = remark;
                item.GuardDateOut = DateTime.Now;
                if (item.IsReturn == false)
                {
                    item.GuardReturn = null;
                    item.GuardStatusReturn = 0;
                    item.GuardRemarkReturn = null;
                    item.GuardDateReturn = null;
                }

                dbcontext.SaveChanges();
                return Content(JsonConvert.SerializeObject(item), "application/json");
            }
        }

        [AuthorizeUser(IncludeRoleLevels = new int[] { 5, 4, 1 })]
        public ActionResult ConfirmGuest(int id)
        {
            var model = dbcontext.Guests.Find(id);
            model.Guest_Item = dbcontext.Guest_Item.Where(t => t.CatID == id).ToList();
            return View(model);
        }

        /// <summary>
        /// Lich su Khach vao ra
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AuthorizeUser(IncludeRoleLevels = new int[] { 5, 4, 1 })]
        public ActionResult HistoryGuest(int id)
        {
            var model = dbcontext.Guests.Find(id);
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            var items = dbcontext.Guest_Item.Where(i => i.CatID == model.ID).ToList();

            var guardViewModel = new GuardViewModel()
            {
                UserLogin = user,
                Guest = model,
                Guest_Item = items
            };

            return View(guardViewModel);
        }

        [AuthorizeUser(IncludeRoleLevels = new int[] { 5, 4, 1 })]
        [HttpPost]
        public ActionResult ApproveGuest(int id, string remark, int status, bool gout = false)
        {
            if (gout)
            {
                var guest = dbcontext.Guests.Find(id);
                var user = (UserLogin)Session[CommonConstants.USER_SESSION];
                guest.GuardOut = user.EmpCode + " | " + user.FullName;
                guest.GuardStatusOut = status;
                guest.GuardRemarkOut = remark;
                guest.GuardDateOut = DateTime.Now;

                dbcontext.SaveChanges();
                return Content(JsonConvert.SerializeObject(guest), "application/json");
            }
            else
            {
                var guest = dbcontext.Guests.Find(id);
                var user = (UserLogin)Session[CommonConstants.USER_SESSION];
                guest.GuardIn = user.EmpCode + " | " + user.FullName;
                guest.GuardStatusIn = status;
                guest.GuardRemarkIn = remark;
                guest.GuardDateIn = DateTime.Now;

                dbcontext.SaveChanges();
                return Content(JsonConvert.SerializeObject(guest), "application/json");
            }
        }

        [AuthorizeUser(IncludeRoleLevels = new int[] { 5, 4, 1 })]
        [HttpPost]
        public ActionResult ApproveOut(int id, string remark, int status)
        {
            var leave = dbcontext.Leave_Early.Find(id);
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            leave.Guard = user.EmpCode + " | " + user.FullName;
            leave.GuardStatus = status;
            leave.GuardRemark = remark;
            leave.GuardDate = DateTime.Now;

            dbcontext.SaveChanges();
            return Content(JsonConvert.SerializeObject(leave), "application/json");
        }

        [AuthorizeUser(IncludeRoleLevels = new int[] { 5, 4, 1 })]
        public ActionResult ConfirmGuestItem(int id)
        {
            var model = dbcontext.Guests.Find(id);
            var items = dbcontext.Guest_Item.Where(t => t.CatID == id).ToList();
            model.Guest_Item = items;
            return View(model);
        }

        [AuthorizeUser(IncludeRoleLevels = new int[] { 5, 4, 1 })]
        [HttpPost]
        public ActionResult ApproveGuestItemIn(int id, int itemId, string remark, int status)
        {
            var guest = dbcontext.Guests.Find(id);

            var guestItems = dbcontext.Guest_Item.Find(itemId);
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            guestItems.GuardIn = user.EmpCode + " | " + user.FullName;
            guestItems.GuardDateIn = DateTime.Now;
            guestItems.GuardRemarkIn = remark;
            guestItems.GuardStatusIn = status;

            var guestStatus = false;
            foreach (var item in dbcontext.Guest_Item.Where(t => t.CatID == id))
            {
                if (item.GuardStatusIn != null && item.GuardStatusIn.Value == 1)
                {
                    guestStatus = true;
                }
                else
                {
                    guestStatus = false;
                    break;
                }
            }
            guest.Status = guestStatus;

            dbcontext.SaveChanges();
            return Content(JsonConvert.SerializeObject(guestItems), "application/json");
        }

        [AuthorizeUser(IncludeRoleLevels = new int[] { 5, 4, 1 })]
        [HttpPost]
        public ActionResult ApproveGuestItemOut(int id, int itemId, string remark, int status)
        {
            var guest = dbcontext.Guests.Find(id);

            var guestItems = dbcontext.Guest_Item.Find(itemId);
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            guestItems.GuardOut = user.EmpCode + " | " + user.FullName;
            guestItems.GuardDateOut = DateTime.Now;
            guestItems.GuardRemarkOut = remark;
            guestItems.GuardStatusOut = status;

            var guestStatus = false;
            foreach (var item in dbcontext.Guest_Item.Where(t => t.CatID == id))
            {
                if (item.GuardStatusOut != null && item.GuardStatusOut.Value == 1)
                {
                    guestStatus = true;
                }
                else
                {
                    guestStatus = false;
                    break;
                }
            }
            guest.Status = guestStatus;

            dbcontext.SaveChanges();
            return Content(JsonConvert.SerializeObject(guestItems), "application/json");
        }

        public ActionResult GuestFileDownload(string filename)
        {
            var file = Server.MapPath("~/Files/") + filename;
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
        }
    }
}