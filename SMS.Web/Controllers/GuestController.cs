using Newtonsoft.Json;
using SMS.Models.EF;
using SMS.Web.Common;
using System;
using System.Collections.Generic;
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

        [HttpPost]
        public ActionResult FetchGuestData()
        {
            var model = dbContext.Guests.OrderByDescending(x => x.CreatedDate).ToList();
            return Json(new { data = model, recordsTotal = dbContext.Bring_In.Count(), recordsFiltered = model.Count() });
        }

        /// <summary>
        /// Load view tạo mới
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Tạo mới
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(Guest m)
        {
            var guest = Request.Form.Get("guest");
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

            model.CreatedDate = System.DateTime.Now;
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            model.CreatedBy = user.EmpCode;
            model.Status = false;

            var bringIn = dbContext.Guests.Add(model);
            dbContext.SaveChanges();

            return Content("Success");
        }

        public ActionResult Detail(int id)
        {
            var guest = dbContext.Guests.Find(id);
            var guestitems = dbContext.Guest_Item.Where(t => t.CatID == id).ToList();
            guest.Guest_Item = guestitems;
            return View(guest);
        }

        public ActionResult Edit(int id)
        {
            var guest = dbContext.Guests.Find(id);
            var guest_items = dbContext.Guest_Item.Where(t => t.CatID == id).ToList();
            guest.Guest_Item = guest_items;
            return View(guest);
        }

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
                guest.ModifiedBy = user.EmpCode;
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

        [HttpGet]
        public ActionResult Cancel(int id)
        {

            var guest = dbContext.Guests.FirstOrDefault(t => t.ID == id);
            guest.Cancel = true;

            dbContext.SaveChanges();

            return Content("Success");
        }

        //ITT
        public ActionResult ITTApproveDetail(int id)
        {
            var guest = dbContext.Guests.Find(id);
            var guest_items = dbContext.Guest_Item.Where(t => t.CatID == id).OrderByDescending(t => t.CreatedDate).ToList();
            guest.Guest_Item = guest_items;
            return View(guest);
        }

        public ActionResult ITTApprove()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ITTApprove(int id, int itemId, string remark, int status)
        {
            var guest = dbContext.Guests.Find(id);

            var guestItems = dbContext.Guest_Item.Find(itemId);
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            guestItems.ITT = user.EmpCode;
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

        //FST
        public ActionResult FSTApproveDetail(int id)
        {
            var guest = dbContext.Guests.Find(id);
            var guestItems = dbContext.Guest_Item.Where(t => t.CatID == id).OrderByDescending(t => t.CreatedDate).ToList();
            guest.Guest_Item = guestItems;
            return View(guest);
        }

        public ActionResult FSTApprove()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FSTApprove(int id, int itemId, string remark, int status)
        {
            var guest = dbContext.Guests.Find(id);

            var guestItems = dbContext.Guest_Item.Find(itemId);
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            guestItems.FST = user.EmpCode;
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
    }
}