using Newtonsoft.Json;
using SMS.Models.EF;
using SMS.Web.Common;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

namespace SMS.Web.Controllers
{
    public class BringInController : BaseController
    {
        private SMSDbContext dbContext;

        public BringInController()
        {
            dbContext = new SMSDbContext();
        }

        public ActionResult History()
        {
            return View();
        }

        /// <summary>
        /// Hàm show lịch sử
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult FetchBringInData()
        {
            var model = dbContext.Bring_In.OrderByDescending(x => x.CreatedDate).ToList();
            
            return Json(new { data = model, recordsTotal = dbContext.Bring_In.Count(), recordsFiltered = model.Count() });
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
        public ActionResult FetchBringInApproveData(string name, int? from, int? to, string team = "", string empcode = "")
        {
            var assetType = 0;
            switch (name.ToLower().Trim())
            {
                case "itt":
                    assetType = 1;
                    break;
                case "fst":
                    assetType = 2;
                    break;
            }

            var res = dbContext.Bring_In
                .Where(bi => dbContext.Bring_In_Items.Any(i => i.CatID == bi.ID && i.AssetType == assetType && i.ApprovedStatus == null))
                .OrderByDescending(x => x.CreatedDate)
                .ToList();

            var recordNumber = dbContext.Bring_In.Count();

            //filter theo tiêu chí
            if(from != null)
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

            return Json(new { data = res, recordsTotal = dbContext.Bring_In.Count(), recordsFiltered = recordNumber });
        }

        public ActionResult Detail(int id)
        {
            var bringin = dbContext.Bring_In.Find(id);
            var bringinItems = dbContext.Bring_In_Items.Where(t => t.CatID == id).ToList();
            bringin.Bring_In_Items = bringinItems;
            return View(bringin);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Bring_In model)
        {
                if (!ModelState.IsValid)
                {
                    return Content("Dữ liệu nhập vào không đúng");
                }

                model.CreatedDate = DateTime.Now;
                var user = (UserLogin)Session[CommonConstants.USER_SESSION];
                model.CreatedBy = user.EmpCode + "|" + user.FullName;
                model.Status = false;

                var bringIn = dbContext.Bring_In.Add(model);
                dbContext.SaveChanges();

                //if (bringIn.ID != 0)
                //{
                //    //chuẩn hóa
                //    foreach (var item in model.Bring_In_Items)
                //    {
                //        //Trường này k nên có ở bảng item
                //        item.CreatedBy = user.EmpCode;
                //        item.CreatedDate = DateTime.Now;
                //    }
                //    //

                //    dbContext.Bring_In_Items.AddRange(model.Bring_In_Items);
                //    dbContext.SaveChanges();
                //}
                return Content("Success");
        }

        public ActionResult Edit(int id)
        {
            var bringin = dbContext.Bring_In.Find(id);
            var bringinItems = dbContext.Bring_In_Items.Where(t => t.CatID == id).ToList();
            bringin.Bring_In_Items = bringinItems;
            return View(bringin);
        }

        [HttpPost]
        public ActionResult Edit(Bring_In model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Content("Dữ liệu nhập vào không đúng");
                }

                var user = (UserLogin)Session[CommonConstants.USER_SESSION];

                var bringin = dbContext.Bring_In.FirstOrDefault(t => t.ID == model.ID);
                bringin.ModifiedBy = user.EmpCode + "|" + user.FullName;
                bringin.ModifiedDate = DateTime.Now;
                bringin.Reason = model.Reason;
                bringin.EstimatedDate = model.EstimatedDate;
                bringin.EstimatedTime = model.EstimatedTime;

                var listItemIDs = new List<int>();
                foreach(var item in model.Bring_In_Items)
                {
                    if(item.ID == 0)
                    {
                        dbContext.Bring_In_Items.Add(item);
                    }
                    else
                    {
                        listItemIDs.Add(item.ID);
                        var bringItem = dbContext.Bring_In_Items.FirstOrDefault(t => t.ID == item.ID);
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
                var itemsDelete = dbContext.Bring_In_Items.Where(t => !listItemIDs.Contains(t.ID) && t.CatID == bringin.ID);
                dbContext.Bring_In_Items.RemoveRange(itemsDelete);

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
            var bringinItems = dbContext.Bring_In_Items.Where(t => t.CatID == id).ToList();
            dbContext.Bring_In_Items.RemoveRange(bringinItems);

            var bringin = dbContext.Bring_In.FirstOrDefault(t => t.ID == id);
            dbContext.Bring_In.Remove(bringin);

            dbContext.SaveChanges();

            return Content("Success");
        }

        [HttpGet]
        public ActionResult Cancel(int id)
        {
            var bringin = dbContext.Bring_In.FirstOrDefault(t => t.ID == id);
            bringin.Cancel = true;
            dbContext.SaveChanges();
            return Content("Success");
        }

        public ActionResult ApproveDetail(int id)
        {
            var bringin = dbContext.Bring_In.Find(id);
            var bringinItems = dbContext.Bring_In_Items.Where(t => t.CatID == id && t.AssetType == 0).OrderByDescending(t=>t.CreatedDate).ToList();
            bringin.Bring_In_Items = bringinItems;
            return View(bringin);
        }

        public ActionResult Approve()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Approve(int id, int itemId, string remark, int status)
        {
            var bringin = dbContext.Bring_In.Find(id);

            var bringinItems = dbContext.Bring_In_Items.Find(itemId);
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            bringinItems.ApprovedBy = user.EmpCode;
            bringinItems.ApprovedDate = DateTime.Now;
            bringinItems.ApproverRemark = remark;
            bringinItems.ApprovedStatus = status;

            var bringStatus = false;
            foreach (var item in dbContext.Bring_In_Items.Where(t => t.CatID == id))
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
            bringin.Status = bringStatus;

            dbContext.SaveChanges();
            return Content(JsonConvert.SerializeObject(bringinItems),"application/json");
        }

        //ITT
        public ActionResult ITTApproveDetail(int id)
        {
            var bringin = dbContext.Bring_In.Find(id);
            var bringinItems = dbContext.Bring_In_Items.Where(t => t.CatID == id && t.AssetType == 1).OrderByDescending(t => t.ID).ToList();
            bringin.Bring_In_Items = bringinItems;
            return View(bringin);
        }

        public ActionResult ITTApprove()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ITTApprove(int id, int itemId, string remark, int status)
        {
            var bringin = dbContext.Bring_In.Find(id);

            var bringinItems = dbContext.Bring_In_Items.Find(itemId);
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            bringinItems.ITT = user.EmpCode;
            bringinItems.ITT_Date = DateTime.Now;
            bringinItems.ITT_Remark = remark;
            bringinItems.ITT_Status = status;
            bringinItems.ApprovedStatus = status;

            var bringStatus = false;
            foreach(var item in dbContext.Bring_In_Items.Where(t=>t.CatID == id))
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
            bringin.Status = bringStatus;

            dbContext.SaveChanges();
            return Content(JsonConvert.SerializeObject(bringinItems), "application/json");
        }

        //FST
        public ActionResult FSTApproveDetail(int id)
        {
            var bringin = dbContext.Bring_In.Find(id);
            var bringinItems = dbContext.Bring_In_Items.Where(t => t.CatID == id && t.AssetType == 2).OrderByDescending(t => t.ID).ToList();
            bringin.Bring_In_Items = bringinItems;
            return View(bringin);
        }

        public ActionResult FSTApprove()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FSTApprove(int id, int itemId, string remark, int status)
        {
            var bringin = dbContext.Bring_In.Find(id);

            var bringinItems = dbContext.Bring_In_Items.Find(itemId);
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            bringinItems.FST = user.EmpCode;
            bringinItems.FST_Date = DateTime.Now;
            bringinItems.FST_Remark = remark;
            bringinItems.FST_Status = status;
            bringinItems.ApprovedStatus = status;

            var bringStatus = false;
            foreach (var item in dbContext.Bring_In_Items.Where(t => t.CatID == id))
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
            bringin.Status = bringStatus;

            dbContext.SaveChanges();
            return Content(JsonConvert.SerializeObject(bringinItems), "application/json");
        }

        public ActionResult BIReport()
        {
            return View();
        }
    }
}