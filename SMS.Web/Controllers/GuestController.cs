using Newtonsoft.Json;
using SMS.Models.EF;
using SMS.Web.Common;
using SMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace SMS.Web.Controllers
{
    public class GuestController : BaseController
    {
        private SMSDbContext dbContext;

        public GuestController()
        {
            dbContext = new SMSDbContext();
        }
        public ActionResult History()
        {
            return View();
        }

        /// <summary>
        /// Lịch sử khách vào ra
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="company"></param>
        /// <param name="team"></param>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult FetchGuestDataHistory(string from = null, string to = null, string company = "", string team = "", string employee = "")
        {
            ViewBag.from = from;
            ViewBag.to = to;
            ViewBag.employee = employee;
            ViewBag.team = team;
            ViewBag.company = company;

            if (from == null || to == null)
            {
                from = DateTime.Now.ToString("dd/MM/yyyy");
                to = DateTime.Now.ToString("dd/MM/yyyy");
            }

            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            var tName = dbContext.Users.Include(t => t.Team).First(u => u.ID == user.ID).Team.Name;
            var currentRole = (HttpContext.User as CustomPrincipal).PriorityRole;
            var isGuard = (HttpContext.User as CustomPrincipal).RoleName == "Guard";

            var model = dbContext.Guests.Where(g => currentRole >= 4 || isGuard || g.Team == tName).OrderByDescending(x => x.EstimatedDateIn).ToList();

            return Json(new { data = model, currentRole, isGuard, recordsTotal = model.Count(), recordsFiltered = model.Count() });
        }

        /// <summary>
        /// Load danh sách phê duyệt hàng cho SMT, FST
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult FetchGuestData(string type)
        {
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            var tName = dbContext.Users.Include(t => t.Team).First(u => u.ID == user.ID).Team.Name;

            var currentRole = (HttpContext.User as CustomPrincipal).PriorityRole;

            var model = new List<Guest>();

            if (type == "SMT")
            {
                model = dbContext.Guests
                    .Where(g => dbContext.Guest_Item.Any(i => i.CatID == g.ID && i.AssetType == 1 && i.ITT_Status == null))
                    .OrderByDescending(x => x.CreatedDate).ToList();
            }
            else if (type == "FST")
            {
                model = dbContext.Guests
                   .Where(g => dbContext.Guest_Item.Any(i => i.CatID == g.ID && i.AssetType == 2 && i.FST_Status == null))
                   .OrderByDescending(x => x.CreatedDate).ToList();
            }

            if (type == "SMT" && currentRole < 4 && tName != "SMT")
            {
                model.Clear();
            }

            if (type == "FST" && currentRole < 4 && tName != "FST")
            {
                model.Clear();
            }

            return Json(new { data = model, currentRole, recordsTotal = model.Count(), recordsFiltered = model.Count() });
        }

        /// <summary>
        /// Load view tạo mới
        /// </summary>
        /// <returns></returns>
        /// 
        [AuthorizeUser(AccessLevel = 1)]
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Tạo mới
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        /// 
        [AuthorizeUser(AccessLevel = 1)]
        [HttpPost]
        public ActionResult Create(Guest m)
        {
            var guest = Request.Form.Get("guest");
            var excludeDate = Request.Form.Get("excludeDate");
            var model = JsonConvert.DeserializeObject<Guest>(guest);

            //file
            if (Request.Files.Count > 0)
            {
                var file = Request.Files.Get("file");
                var fileName = Path.GetFileName(file.FileName);
                model.FileRedirectURL = fileName;

                if (!Directory.Exists(Server.MapPath("~/Files")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Files"));
                }
                var filePath = Server.MapPath("~/Files/" + fileName);
                file.SaveAs(filePath);
            }

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(model);

            var guests = new List<Guest>();

            // Lưu khách nhiều ngày với định dạng dd/MM/yyyy
            saveGuest(model.EstimatedDateIn, model.EstimatedDateOut, excludeDate, json, ref guests);

            dbContext.Guests.AddRange(guests);

            dbContext.SaveChanges();

            return Content("Success");
        }


        private string FormatDate(string date)
        {
            var strs = date.Split('/');
            return strs[1] + "/" + strs[0] + "/" + strs[2];
        }

        /// <summary>
        /// Hàm lưu đăng ký khách (bao gồm cả ngắn hạn cả dài hạn)
        /// </summary>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="excludeDate"></param>
        /// <param name="model"></param>
        /// <param name="guests"></param>
        private void saveGuest(string StartDate, string EndDate, string excludeDate, string model, ref List<Guest> guests)
        {
            var newGuest = Newtonsoft.Json.JsonConvert.DeserializeObject<Guest>(model);

            var startdate = DateTime.ParseExact(StartDate, "dd/MM/yyyy", null);

            var enddate = DateTime.ParseExact(EndDate, "dd/MM/yyyy", null);

            var dateDiff = (enddate - startdate).TotalDays;

            var excludeDates = excludeDate.Split(',');

            if (excludeDates.Contains(StartDate))
            {
                if (StartDate == EndDate)
                {
                    return;
                }
                else
                {
                    var newStartDate = startdate.AddDays(1).ToString("dd/MM/yyyy");
                    saveGuest(newStartDate, EndDate, excludeDate, model, ref guests);
                }
            }
            else
            {
                var newStartDate = startdate.AddDays(1).ToString("dd/MM/yyyy");

                if(dateDiff >= 1)
                {
                    newGuest.EstimatedDateIn = StartDate;
                    newGuest.EstimatedDateOut = Int32.Parse(newGuest.EstimatedTimeOut.Split(':')[0]) < 8 ? newStartDate : StartDate;
                    newGuest.CreatedDate = System.DateTime.Now;
                    var user = (UserLogin)Session[CommonConstants.USER_SESSION];
                    newGuest.CreatedBy = user.EmpCode + "|" + user.FullName;
                    newGuest.Status = false;

                    guests.Add(newGuest);
                    saveGuest(newStartDate, EndDate, excludeDate, model, ref guests);
                }
                else
                {
                    newGuest.EstimatedDateIn = StartDate;
                    newGuest.EstimatedDateOut = StartDate;
                    newGuest.CreatedDate = System.DateTime.Now;
                    var user = (UserLogin)Session[CommonConstants.USER_SESSION];
                    newGuest.CreatedBy = user.EmpCode + "|" + user.FullName;
                    newGuest.Status = false;

                    guests.Add(newGuest);
                }
            }

        }

        /// <summary>
        /// Hàm show thông tin chi tiết đăng ký khách
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Detail(int id)
        {
            var guest = dbContext.Guests.Find(id);
            var guestitems = dbContext.Guest_Item.Where(t => t.CatID == id && t.Quantity != null && t.Item != null).ToList();
            guest.Guest_Item = guestitems;
            return View(guest);
        }

        /// <summary>
        /// Load view Sửa
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AuthorizeUser(AccessLevel = 1)]
        public ActionResult Edit(int id)
        {
            var guest = dbContext.Guests.Find(id);
            var guest_items = dbContext.Guest_Item.Where(t => t.CatID == id).ToList();
            guest.Guest_Item = guest_items;
            return View(guest);
        }

        /// <summary>
        /// Thực thi Sửa
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        [AuthorizeUser(AccessLevel = 1)]
        [HttpPost]
        public ActionResult Edit(Guest m)
        {
            try
            {
                var guestJSON = Request.Form.Get("guest");
                var model = JsonConvert.DeserializeObject<Guest>(guestJSON);

                var filePath = "";
                //file
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files.Get("file");
                    var fileName = Path.GetFileName(file.FileName);
                    model.FileRedirectURL = fileName;

                    if (!Directory.Exists(Server.MapPath("~/Files")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Files"));
                    }
                    filePath = Server.MapPath("~/Files/" + fileName);
                    file.SaveAs(filePath);

                }

                var user = (UserLogin)Session[CommonConstants.USER_SESSION];

                var guest = dbContext.Guests.FirstOrDefault(t => t.ID == model.ID);
                guest.ModifiedBy = user.EmpCode + "|" + user.FullName;
                guest.ModifiedDate = DateTime.Now;

                guest.Type = model.Type;
                guest.IO = model.IO;
                guest.FullName = model.FullName;
                guest.IDCard = model.IDCard;
                guest.Company = model.Company;
                guest.NumbersOfPerson = model.NumbersOfPerson;
                guest.Visa = model.Visa;
                guest.Hotel = model.Hotel;
                guest.Purpose = model.Purpose;
                guest.TransportNo = model.TransportNo;
                guest.EmployeeID = model.EmployeeID;
                guest.EmployeeName = model.EmployeeName;
                guest.Team = model.Team;
                guest.Position = model.Position;
                guest.EstimatedDateIn = model.EstimatedDateIn;
                guest.EstimatedTimeIn = model.EstimatedTimeIn;
                guest.EstimatedDateOut = model.EstimatedDateOut;
                guest.EstimatedTimeOut = model.EstimatedTimeOut;
                guest.KVPNo = model.KVPNo;
                guest.Check_Guest = model.Check_Guest;
                if (filePath != "")
                {
                    guest.FileRedirectURL = model.FileRedirectURL;
                }

                var listItemIDs = new List<int>();
                foreach (var item in model.Guest_Item)
                {
                    if (item.ID == 0)
                    {
                        dbContext.Guest_Item.Add(item);
                    }
                    else
                    {
                        listItemIDs.Add(item.ID);
                        var bringItem = dbContext.Guest_Item.FirstOrDefault(t => t.ID == item.ID);

                        bringItem.Item = item.Item;
                        bringItem.Serial = item.Serial;
                        bringItem.Quantity = item.Quantity;
                        bringItem.Unit = item.Unit;
                        bringItem.AssetType = item.AssetType;
                        bringItem.IsReturn = item.IsReturn;
                        bringItem.ReturnDate = item.ReturnDate;
                        bringItem.ReturnTime = item.ReturnTime;
                        bringItem.ModifiedBy = user.EmpCode;
                        bringItem.ModifiedDate = DateTime.Now;
                    }
                }
                var itemsDelete = dbContext.Guest_Item.Where(t => !listItemIDs.Contains(t.ID) && t.CatID == guest.ID);
                dbContext.Guest_Item.RemoveRange(itemsDelete);

                dbContext.SaveChanges();

                return Content("Success");
            }
            catch (System.Exception ex)
            {
                return Content("Lỗi trong quá trình xử lý");
            }
        }

        [AuthorizeUser(AccessLevel = 2)]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var guest_items = dbContext.Guest_Item.Where(t => t.CatID == id).ToList();
            dbContext.Guest_Item.RemoveRange(guest_items);

            var guest = dbContext.Guests.FirstOrDefault(t => t.ID == id);
            dbContext.Guests.Remove(guest);

            dbContext.SaveChanges();

            return Content("Success");
        }

        [AuthorizeUser(AccessLevel = 3)]
        [HttpGet]
        public ActionResult Cancel(int id)
        {

            var guest = dbContext.Guests.FirstOrDefault(t => t.ID == id);
            guest.Cancel = true;

            dbContext.SaveChanges();

            return Content("Success");
        }

        //Load view ITT phê duyệt đăng ký khách
        [AuthorizeUser(AccessLevel = 3, ExceptRoleName = "FM")]
        public ActionResult ITTApproveDetail(int id)
        {
            var guest = dbContext.Guests.Find(id);
            var guest_items = dbContext.Guest_Item.Where(t => t.CatID == id && t.AssetType == 1).OrderByDescending(t => t.CreatedDate).ToList();

            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            var tName = dbContext.Users.Include(t => t.Team).First(u => u.ID == user.ID).Team.Name;

            var isAdmin = (HttpContext.User as CustomPrincipal).PriorityRole >= 4;

            if (!isAdmin && tName != "SMT")
            {
                guest_items.Clear();
            }

            guest.Guest_Item = guest_items;
            return View(guest);
        }

        /// <summary>
        /// Load view chi tiết ITT phê duyệt
        /// </summary>
        /// <returns></returns>
        [AuthorizeUser(AccessLevel = 3, ExceptRoleName = "FM")]
        public ActionResult ITTApprove()
        {
            return View();
        }

        /// <summary>
        /// Thực thi ITT phê duyệt
        /// </summary>
        /// <param name="id"></param>
        /// <param name="itemId"></param>
        /// <param name="remark"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [AuthorizeUser(AccessLevel = 3, ExceptRoleName = "FM")]
        [HttpPost]
        public ActionResult ITTApprove(int id, int itemId, string remark, int status)
        {
            var guest = dbContext.Guests.Find(id);

            var guestItems = dbContext.Guest_Item.Find(itemId);
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            guestItems.ITT = user.EmpCode + "|" + user.FullName;
            guestItems.ITT_Date = DateTime.Now;
            guestItems.ITT_Remark = remark;
            guestItems.ITT_Status = status;

            var gStatus = false;
            foreach (var item in dbContext.Guest_Item.Where(t => t.CatID == id))
            {
                if (item.FST_Status != null && item.FST_Status.Value == 1)
                {
                    gStatus = true;
                }
                else
                {
                    gStatus = false;
                    break;
                }
            }
            guest.Status = gStatus;

            dbContext.SaveChanges();
            return Content(JsonConvert.SerializeObject(guestItems), "application/json");
        }

        [AuthorizeUser(AccessLevel = 3, ExceptRoleName = "FM")]
        //FST
        public ActionResult FSTApproveDetail(int id)
        {
            var guest = dbContext.Guests.Find(id);
            var guestItems = dbContext.Guest_Item.Where(t => t.CatID == id && t.AssetType == 2).OrderByDescending(t => t.CreatedDate).ToList();

            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            var tName = dbContext.Users.Include(t => t.Team).First(u => u.ID == user.ID).Team.Name;


            var isAdmin = (HttpContext.User as CustomPrincipal).PriorityRole >= 4;

            if (!isAdmin && tName != "FST")
            {
                guestItems.Clear();
            }

            guest.Guest_Item = guestItems;
            return View(guest);
        }

        [AuthorizeUser(AccessLevel = 3, ExceptRoleName = "FM")]
        public ActionResult FSTApprove()
        {
            return View();
        }

        [AuthorizeUser(AccessLevel = 3, ExceptRoleName = "FM")]
        [HttpPost]
        public ActionResult FSTApprove(int id, int itemId, string remark, int status)
        {
            var guest = dbContext.Guests.Find(id);

            var guestItems = dbContext.Guest_Item.Find(itemId);
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            guestItems.FST = user.EmpCode + "|" + user.FullName;
            guestItems.FST_Date = DateTime.Now;
            guestItems.FST_Remark = remark;
            guestItems.FST_Status = status;

            var gStatus = false;
            foreach (var item in dbContext.Guest_Item.Where(t => t.CatID == id))
            {
                if ((item.ITT_Status != null && item.ITT_Status.Value == 1) || (item.SMT_Status != null && item.SMT_Status.Value == 1))
                {
                    gStatus = true;
                }
                else
                {
                    gStatus = false;
                    break;
                }
            }
            guest.Status = gStatus;

            dbContext.SaveChanges();
            return Content(JsonConvert.SerializeObject(guestItems), "application/json");
        }

        /// <summary>
        /// In xác nhận gặp mặt khách
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GuestReport(int id)
        {
            var guest = dbContext.Guests.Find(id);
            var guestItems = dbContext.Guest_Item.Where(t => t.CatID == id).ToList();
            guest.Guest_Item = guestItems;
            return View(guest);
        }

        /// <summary>
        /// Dowload file đính kèm
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public ActionResult GuestFileDownload(string filename)
        {
            var file = Server.MapPath("~/Files/") + filename;
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
        }

        /// <summary>
        /// Dowload template đăng ký khách
        /// </summary>
        /// <returns></returns>
        public ActionResult TemplateGuest()
        {
            var file = Server.MapPath("~/Files/Form excel mẫu cho khách.xlsx");
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "File mẫu.xlsx");

        }
    }
}