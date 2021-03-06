﻿using Newtonsoft.Json;
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

namespace SMS.Web.Controllers
{
    public class BringOutController : BaseController
    {
        // GET: BringOut
        private SMSDbContext dbContext;

        public BringOutController()
        {
            dbContext = new SMSDbContext();
        }

        /// <summary>
        /// Hàm show dữ liệu sử dụng ajax
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>

        public ActionResult History()
        {
            return View();
        }

        [AuthorizeUser(AccessLevel = 3)]
        [HttpGet]
        public ActionResult Cancel(int id)
        {
            var bringout = dbContext.Bring_Out.FirstOrDefault(t => t.ID == id);
            bringout.Cancel = true;

            dbContext.SaveChanges();

            return Content("Success");
        }

        [HttpPost]
        public ActionResult FetchBringOutData()
        {
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            var tName = dbContext.Users.Include(t => t.Team).First(u => u.ID == user.ID).Team.Name;

            var currentRole = (HttpContext.User as CustomPrincipal).PriorityRole;

            var model = dbContext.Bring_In.Where(b => currentRole >= 4 || b.Team == tName).OrderByDescending(x => x.EstimatedDate).ToList();

            return Json(new { data = model, currentRole, recordsTotal = dbContext.Bring_Out.Count(), recordsFiltered = model.Count() });
        }

        /// <summary>
        /// Show danh sách phê duyệt cho TM, ITT, FST
        /// </summary>
        /// <param name="name"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="team"></param>
        /// <param name="empcode"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult FetchBringOutApproveData(string name, int? from, int? to, string team = "", string empcode = "")
        {
            var res = new List<Bring_Out>();

            if (name == "itt")
            {
                res = dbContext.Bring_Out
                .Where(bi => dbContext.Bring_Out_Items.Any(i => i.CatID == bi.ID && i.AssetType == 1 && i.ITT_Status == null))
                .OrderByDescending(x => x.EstimatedDate)
                .ToList();
            }
            else if (name == "fst")
            {
                res = dbContext.Bring_Out
                .Where(bi => dbContext.Bring_Out_Items.Any(i => i.CatID == bi.ID && i.AssetType == 2 && i.FST_Status == null))
                .OrderByDescending(x => x.EstimatedDate)
                .ToList();
            }
            else
            {
                res = dbContext.Bring_Out
               .Where(bi => dbContext.Bring_Out_Items.Any(i => i.CatID == bi.ID && i.AssetType == 0 && i.ApprovedStatus == null))
               .OrderByDescending(x => x.EstimatedDate)
               .ToList();
            }

            var recordNumber = dbContext.Bring_Out.Count();

            //filter theo tiêu chí
            if (from != null)
            {
                res = res.Where(t => ((DateTimeOffset)t.CreatedDate.Value).ToUnixTimeSeconds() >= from).ToList();
            }

            if (to != null)
            {
                res = res.Where(t => ((DateTimeOffset)t.CreatedDate.Value).ToUnixTimeSeconds() <= to).ToList();
            }

            if (!string.IsNullOrEmpty(team))
            {
                res = res.Where(t => t.Team.Contains(team)).ToList();
            }

            if (!string.IsNullOrEmpty(empcode))
            {
                res = res.Where(t => t.EmpCode.Contains(empcode)).ToList();
            }

            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            var tName = dbContext.Users.Include(t => t.Team).First(u => u.ID == user.ID).Team.Name;

            var isAdmin = (HttpContext.User as CustomPrincipal).PriorityRole >= 4;

            if (name == "itt" && !isAdmin && tName != "SMT")
            {
                res.Clear();
            }

            if (name == "fst" && !isAdmin && tName != "FST")
            {
                res.Clear();
            }

            return Json(new { data = res, recordsTotal = dbContext.Bring_Out.Count(), recordsFiltered = recordNumber });
        }

        public ActionResult Detail(int id)
        {
            var BringOut = dbContext.Bring_Out.Find(id);
            var BringOutItems = dbContext.Bring_Out_Items.Where(t => t.CatID == id).ToList();
            BringOut.Bring_Out_Items = BringOutItems;
            return View(BringOut);
        }

        [AuthorizeUser(AccessLevel = 2)]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [AuthorizeUser(AccessLevel = 2)]
        [HttpPost]
        public ActionResult Create(Bring_Out model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Content("Dữ liệu nhập vào không đúng");
                }

                model.CreatedDate = DateTime.Now;
                var user = (UserLogin)Session[CommonConstants.USER_SESSION];
                model.CreatedBy = user.EmpCode + "|" + user.FullName;
                model.Status = false;

                var BringOut = dbContext.Bring_Out.Add(model);
                dbContext.SaveChanges();

                //if (BringOut.ID != 0)
                //{
                //    //chuẩn hóa
                //    foreach (var item in model.Bring_Out_Items)
                //    {
                //        //Trường này k nên có ở bảng item
                //        item.CreatedBy = user.EmpCode;
                //        item.CreatedDate = DateTime.Now;
                //    }
                //    //

                //    dbContext.Bring_Out_Items.AddRange(model.Bring_Out_Items);
                //    dbContext.SaveChanges();
                //}
                return Content("Success");
            }
            catch (System.Exception ex)
            {
                return Content("Lỗi trong quá trình xử lý");
            }
        }

        [AuthorizeUser(AccessLevel = 2)]
        public ActionResult Edit(int id)
        {
            var BringOut = dbContext.Bring_Out.Find(id);
            var BringOutItems = dbContext.Bring_Out_Items.Where(t => t.CatID == id).ToList();
            BringOut.Bring_Out_Items = BringOutItems;
            return View(BringOut);
        }

        [AuthorizeUser(AccessLevel = 2)]
        [HttpPost]
        public ActionResult Edit(Bring_Out model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Content("Dữ liệu nhập vào không đúng");
                }

                var user = (UserLogin)Session[CommonConstants.USER_SESSION];

                var BringOut = dbContext.Bring_Out.FirstOrDefault(t => t.ID == model.ID);
                BringOut.ModifiedBy = user.EmpCode + "|" + user.FullName;
                BringOut.ModifiedDate = DateTime.Now;
                BringOut.Reason = model.Reason;
                BringOut.EstimatedDate = model.EstimatedDate;
                BringOut.EstimatedTime = model.EstimatedTime;

                var listItemIDs = new List<int>();
                if (model.Bring_Out_Items != null)
                {
                    foreach (var item in model.Bring_Out_Items)
                    {
                        if (item.ID == 0)
                        {
                            dbContext.Bring_Out_Items.Add(item);
                        }
                        else
                        {
                            listItemIDs.Add(item.ID);
                            var bringItem = dbContext.Bring_Out_Items.FirstOrDefault(t => t.ID == item.ID);
                            bringItem.Item = item.Item;
                            bringItem.Serial = item.Serial;
                            bringItem.Quantity = item.Quantity;
                            bringItem.Unit = item.Unit;
                            bringItem.AssetType = item.AssetType;
                            bringItem.IsReturn = item.IsReturn;
                            bringItem.ReturnDate = item.ReturnDate;
                            bringItem.ReturnTime = item.ReturnTime;
                        }
                    }
                }
                var itemsDelete = dbContext.Bring_Out_Items.Where(t => !listItemIDs.Contains(t.ID) && t.CatID == BringOut.ID);
                dbContext.Bring_Out_Items.RemoveRange(itemsDelete);

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
            var BringOutItems = dbContext.Bring_Out_Items.Where(t => t.CatID == id).ToList();
            dbContext.Bring_Out_Items.RemoveRange(BringOutItems);

            var BringOut = dbContext.Bring_Out.FirstOrDefault(t => t.ID == id);
            dbContext.Bring_Out.Remove(BringOut);

            dbContext.SaveChanges();

            return Content("Success");
        }

        [AuthorizeUser(AccessLevel = 3)]
        public ActionResult ApproveDetail(int id)
        {
            var BringOut = dbContext.Bring_Out.Find(id);
            var BringOutItems = dbContext.Bring_Out_Items.Where(t => t.CatID == id && t.AssetType == 0).OrderByDescending(t => t.CreatedDate).ToList();
            BringOut.Bring_Out_Items = BringOutItems;
            return View(BringOut);
        }

        [AuthorizeUser(AccessLevel = 3)]
        public ActionResult Approve()
        {
            return View();
        }

        [AuthorizeUser(AccessLevel = 3)]
        [HttpPost]
        public ActionResult Approve(int id, int itemId, string remark, int status)
        {
            var BringOut = dbContext.Bring_Out.Find(id);

            var BringOutItems = dbContext.Bring_Out_Items.Find(itemId);
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            BringOutItems.ApprovedBy = user.EmpCode + "|" + user.FullName;
            BringOutItems.ApprovedDate = DateTime.Now;
            BringOutItems.ApproverRemark = remark;
            BringOutItems.ApprovedStatus = status;

            var bringStatus = false;
            foreach (var item in dbContext.Bring_Out_Items.Where(t => t.CatID == id))
            {
                if (item.FST_Status != null && item.FST_Status.Value == 1 && ((item.ITT_Status != null && item.ITT_Status.Value == 1) || (item.SMT_Status != null && item.SMT_Status.Value == 1)))
                {
                    bringStatus = true;
                }
                else
                {
                    bringStatus = false;
                    break;
                }
            }
            BringOut.Status = bringStatus;

            dbContext.SaveChanges();
            return Content(JsonConvert.SerializeObject(BringOutItems), "application/json");
        }

        [AuthorizeUser(AccessLevel = 3)]
        //ITT
        public ActionResult ITTApproveDetail(int id)
        {
            var BringOut = dbContext.Bring_Out.Find(id);
            var BringOutItems = dbContext.Bring_Out_Items.Where(t => t.CatID == id && t.AssetType == 1).OrderByDescending(t => t.CreatedDate).ToList();
            BringOut.Bring_Out_Items = BringOutItems;
            return View(BringOut);
        }

        [AuthorizeUser(AccessLevel = 3)]
        public ActionResult ITTApprove()
        {
            return View();
        }

        [AuthorizeUser(AccessLevel = 3)]
        [HttpPost]
        public ActionResult ITTApprove(int id, int itemId, string remark, int status)
        {
            var BringOut = dbContext.Bring_Out.Find(id);

            var BringOutItems = dbContext.Bring_Out_Items.Find(itemId);
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            BringOutItems.ITT = user.EmpCode + "|" + user.FullName;
            BringOutItems.ITT_Date = DateTime.Now;
            BringOutItems.ITT_Remark = remark;
            BringOutItems.ITT_Status = status;

            var bringStatus = false;
            foreach (var item in dbContext.Bring_Out_Items.Where(t => t.CatID == id))
            {
                if ((item.ApprovedStatus != null && item.ApprovedStatus.Value == 1) && (item.FST_Status != null && item.FST_Status.Value == 1))
                {
                    bringStatus = true;
                }
                else
                {
                    bringStatus = false;
                    break;
                }
            }
            BringOut.Status = bringStatus;

            dbContext.SaveChanges();
            return Content(JsonConvert.SerializeObject(BringOutItems), "application/json");
        }

        [AuthorizeUser(AccessLevel = 3)]
        //FST
        public ActionResult FSTApproveDetail(int id)
        {
            var BringOut = dbContext.Bring_Out.Find(id);
            var BringOutItems = dbContext.Bring_Out_Items.Where(t => t.CatID == id && t.AssetType == 2).OrderByDescending(t => t.ID).ToList();
            BringOut.Bring_Out_Items = BringOutItems;
            return View(BringOut);
        }

        [AuthorizeUser(AccessLevel = 3)]
        public ActionResult FSTApprove()
        {
            return View();
        }

        [AuthorizeUser(AccessLevel = 3)]
        [HttpPost]
        public ActionResult FSTApprove(int id, int itemId, string remark, int status)
        {
            var BringOut = dbContext.Bring_Out.Find(id);

            var BringOutItems = dbContext.Bring_Out_Items.Find(itemId);
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            BringOutItems.FST = user.EmpCode + "|" + user.FullName;
            BringOutItems.FST_Date = DateTime.Now;
            BringOutItems.FST_Remark = remark;
            BringOutItems.FST_Status = status;

            var bringStatus = false;
            foreach (var item in dbContext.Bring_Out_Items.Where(t => t.CatID == id))
            {
                if (item.ApprovedStatus != null && item.ApprovedStatus.Value == 1 && (item.ITT_Status != null && item.ITT_Status.Value == 1) || (item.SMT_Status != null && item.SMT_Status.Value == 1))
                {
                    bringStatus = true;
                }
                else
                {
                    bringStatus = false;
                    break;
                }
            }
            BringOut.Status = bringStatus;

            dbContext.SaveChanges();
            return Content(JsonConvert.SerializeObject(BringOutItems), "application/json");
        }

        public ActionResult BOReport(int id)
        {
            var bringout = dbContext.Bring_Out.Find(id);
            var bringoutItems = dbContext.Bring_Out_Items.Where(t => t.CatID == id).ToList();
            bringout.Bring_Out_Items = bringoutItems;
            return View(bringout);
        }

        public ActionResult SummaryBO()
        {
            var bringout = dbContext.Bring_Out.ToList();
            var bos = dbContext.Bring_Out_Items.Where(b => b.Quantity != null && b.Item != null).ToList();

            for (var b = 0; b < bringout.Count(); b++)
            {
                var bringoutItems = bos.Where(t => t.CatID == bringout[b].ID).ToList();
                bringout[b].Bring_Out_Items = bringoutItems;
            }

            return View(bringout);
        }
    }
}