using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using PagedList;
using SMS.Models.DAO;
using SMS.Models.EF;
using SMS.Web.Common;
using SMS.Web.Models;
using SMS.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
        public ActionResult History(string employee ="", string guest="",string company = "")
        {
            ViewBag.LeaveEarlies = dbcontext.Leave_Early.Where(t => (string.IsNullOrEmpty(employee) || t.FullName.ToLower().Contains(employee.ToLower()))).ToList();
            ViewBag.GoOuts = dbcontext.Go_Out.Where(t => (string.IsNullOrEmpty(employee) || t.FullName.ToLower().Contains(employee.ToLower()))).ToList();
            ViewBag.BringIns = dbcontext.Bring_In.Where(t => (string.IsNullOrEmpty(employee) || t.FullName.ToLower().Contains(employee.ToLower()))).ToList();
            ViewBag.BringOuts = dbcontext.Bring_Out.Where(t => (string.IsNullOrEmpty(employee) || t.FullName.ToLower().Contains(employee.ToLower()))).ToList();
            var Guests = dbcontext.Guests.Where(t => ((string.IsNullOrEmpty(guest) || t.FullName.ToLower().Contains(guest.ToLower()))) && 
                                                (string.IsNullOrEmpty(company) || t.Company.ToLower().Contains(company.ToLower()))).ToList();
            var guest_no_items = new List<Guest>();
            var guest_has_items = new List<Guest>();
            var foreign_guest_no_items = new List<Guest>();
            var foreign_guest_has_items = new List<Guest>();
            foreach (var g in Guests)
            {
                if(g.Check_Guest == false)
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

        public ActionResult Queue(string employee = "", string guest = "", string company = "")
        {
            ViewBag.LeaveEarlies = dbcontext.Leave_Early.Where(t => (string.IsNullOrEmpty(employee) || t.FullName.ToLower().Contains(employee.ToLower())) && t.GuardStatus == null).ToList();
            ViewBag.GoOuts = dbcontext.Go_Out.Where(t => (string.IsNullOrEmpty(employee) || t.FullName.ToLower().Contains(employee.ToLower())) && t.GuardStatusReturn == null).ToList();
            ViewBag.BringIns = dbcontext.Bring_In.Where(t => (string.IsNullOrEmpty(employee) || t.FullName.ToLower().Contains(employee.ToLower()))).ToList();
            ViewBag.BringOuts = dbcontext.Bring_Out.Where(t => (string.IsNullOrEmpty(employee) || t.FullName.ToLower().Contains(employee.ToLower()))).ToList();
            var Guests = dbcontext.Guests.Where(t => ((string.IsNullOrEmpty(guest) || t.FullName.ToLower().Contains(guest.ToLower()))) &&
                                                (string.IsNullOrEmpty(company) || t.Company.ToLower().Contains(company.ToLower())) && (t.GuardStatusIn == null || t.GuardStatusOut == null)).ToList();
            var guest_no_items = new List<Guest>();
            var guest_has_items = new List<Guest>();
            var foreign_guest_no_items = new List<Guest>();
            var foreign_guest_has_items = new List<Guest>();
            foreach (var item in Guests)
            {
                if (item.Check_Guest == false)
                {
                    if (dbcontext.Guest_Item.Where(t => t.CatID == item.ID && t.Item != null).Count() > 0)
                    {
                        guest_has_items.Add(item);
                    }
                    else
                    {
                        guest_no_items.Add(item);
                    }
                }
                else
                {
                    if (dbcontext.Guest_Item.Where(t => t.CatID == item.ID && t.Item != null).Count() > 0)
                    {
                        foreign_guest_has_items.Add(item);
                    }
                    else
                    {
                        foreign_guest_no_items.Add(item);
                    }
                }
            }
            ViewBag.GuestNoItems = guest_no_items;
            ViewBag.GuestHasItems = guest_has_items;
            ViewBag.ForeignGuestNoItems = foreign_guest_no_items;
            ViewBag.ForeignGuestHasItems = foreign_guest_has_items;
            return View();
        }

        public ActionResult ConfirmLE(int id)
        {
            var model = dbcontext.Leave_Early.FirstOrDefault(l => l.ID == id);
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];

            if (model == null)
            {
                return View(); // 404
            }

            var guardViewModel = new GuardViewModel() {
                UserLogin = user,
                Leave_Early = model
            };

            return View(guardViewModel);
        }

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
        [HttpPost]
        public ActionResult ApproveGoOut(int id, string remark, int status)
        {
            var data = dbcontext.Go_Out.Find(id);
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            data.GuardOut = user.EmpCode;
            data.GuardStatusOut = status;
            data.GuardRemarkOut = remark;
            data.GuardDateOut = DateTime.Now;

            dbcontext.SaveChanges();
            return Content(JsonConvert.SerializeObject(data), "application/json");
        }
        [HttpPost]
        public ActionResult ApproveGoReturn(int id, string remark, int status)
        {
            var data = dbcontext.Go_Out.Find(id);
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            data.GuardReturn = user.EmpCode;
            data.GuardStatusReturn = status;
            data.GuardRemarkReturn = remark;
            data.GuardDateReturn = DateTime.Now;

            dbcontext.SaveChanges();
            return Content(JsonConvert.SerializeObject(data), "application/json");
        }

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

        [HttpPost]
        public ActionResult ApproveBringInItem(int id, string remark, int status, bool ret = false)
        {
            if (ret)
            {
                var item = dbcontext.Bring_In_Items.Find(id);
                var user = (UserLogin)Session[CommonConstants.USER_SESSION];
                item.GuardOut = user.EmpCode;
                item.GuardStatusOut = status;
                item.GuardRemarkOut = remark;
                item.GuardDateOut = DateTime.Now;

                dbcontext.SaveChanges();
                return Content(JsonConvert.SerializeObject(item), "application/json");
            } else
            {
                var item = dbcontext.Bring_In_Items.Find(id);
                var user = (UserLogin)Session[CommonConstants.USER_SESSION];
                item.GuardIn = user.EmpCode;
                item.GuardStatusIn = status;
                item.GuardRemarkIn = remark;
                item.GuardDateIn = DateTime.Now;

                dbcontext.SaveChanges();
                return Content(JsonConvert.SerializeObject(item), "application/json");
            }
        }

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
        
        [HttpPost]
        public ActionResult ApproveBringOutItem(int id, string remark, int status, bool ret = false)
        {
            if (ret)
            {
                var item = dbcontext.Bring_Out_Items.Find(id);
                var user = (UserLogin)Session[CommonConstants.USER_SESSION];
                item.GuardReturn = user.EmpCode;
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
                item.GuardOut = user.EmpCode;
                item.GuardStatusOut = status;
                item.GuardRemarkOut = remark;
                item.GuardDateOut = DateTime.Now;

                dbcontext.SaveChanges();
                return Content(JsonConvert.SerializeObject(item), "application/json");
            }
        }

        public ActionResult ConfirmGuest(int id)
        {
            var model = dbcontext.Guests.Find(id);
            model.Guest_Item = dbcontext.Guest_Item.Where(t => t.CatID == id).ToList();
            return View(model);
        }
        [HttpPost]
        public ActionResult ApproveIn(int id, string remark, int status)
        {
            var guest = dbcontext.Guests.Find(id);
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            guest.GuardIn = user.EmpCode;
            guest.GuardStatusIn = status;
            guest.GuardRemarkIn = remark;
            guest.GuardDateIn = DateTime.Now;

            dbcontext.SaveChanges();
            return Content(JsonConvert.SerializeObject(guest), "application/json");
        }
        [HttpPost]
        public ActionResult ApproveOut(int id, string remark, int status)
        {
            var leave = dbcontext.Leave_Early.Find(id);
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            leave.Guard = user.EmpCode;
            leave.GuardStatus = status;
            leave.GuardRemark = remark;
            leave.GuardDate = DateTime.Now;

            dbcontext.SaveChanges();
            return Content(JsonConvert.SerializeObject(leave), "application/json");
        }
        public ActionResult ConfirmGuestItem(int id)
        {
            var model = dbcontext.Guests.Find(id);
            var items = dbcontext.Guest_Item.Where(t => t.CatID == id).ToList();
            model.Guest_Item = items;
            return View(model);
        }
        [HttpPost]
        public ActionResult ApproveItemIn(int id, int itemId, string remark, int status)
        {
            var guest = dbcontext.Guests.Find(id);

            var guestItems = dbcontext.Guest_Item.Find(itemId);
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            guestItems.GuardIn = user.EmpCode;
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
        [HttpPost]
        public ActionResult ApproveItemOut(int id, int itemId, string remark, int status)
        {
            var guest = dbcontext.Guests.Find(id);

            var guestItems = dbcontext.Guest_Item.Find(itemId);
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            guestItems.GuardOut = user.EmpCode + "|" + user.FullName;
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
            var file = Server.MapPath("~/Files") + filename;
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}