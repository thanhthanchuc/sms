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
        [HttpPost]
        public ActionResult FetchBringInData()
        {
            var model = dbContext.Bring_In.OrderByDescending(x => x.CreatedDate).ToList();
            
            return Json(new { data = model, recordsTotal = dbContext.Bring_In.Count(), recordsFiltered = model.Count() });
        }

        [HttpPost]
        public ActionResult FetchBringInApproveData(string name,DateTime from, DateTime to, string team, string empcode)
        {
            var model = dbContext.Bring_In.OrderByDescending(x => x.CreatedDate).ToList();
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
            foreach(var item in model)
            {
                item.Bring_In_Items = dbContext.Bring_In_Items.Where(t => t.CatID == item.ID && t.AssetType == assetType).ToList();
            }
            var res = model.Where(t => t.Bring_In_Items.Count > 0);

            //filter
            if(from != null)
            {
                res = res.Where(t => DateTime.Compare(t.CreatedDate.Value, from) <= 0);
            }
            if (to != null)
            {
                res = res.Where(t => DateTime.Compare(t.CreatedDate.Value, to) >= 0);
            }
            if (!string.IsNullOrEmpty(team))
            {
                res = res.Where(t => t.Team.Contains(team));
            }
            if (!string.IsNullOrEmpty(empcode))
            {
                res = res.Where(t => t.EmpCode.Contains(empcode));
            }
            //

            return Json(new { data = res, recordsTotal = dbContext.Bring_In.Count(), recordsFiltered = model.Count() });
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
            try
            {
                if (!ModelState.IsValid)
                {
                    return Content("Dữ liệu nhập vào không đúng");
                }

                model.CreatedDate = System.DateTime.Now;
                var user = (UserLogin)Session[CommonConstants.USER_SESSION];
                model.CreatedBy = user.EmpCode;
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
            catch (System.Exception ex)
            {
                return Content("Lỗi trong quá trình xử lý");
            }
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
                bringin.ModifiedBy = user.EmpCode;
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
            //var bringinItems = dbContext.Bring_In_Items.Where(t => t.CatID == id).ToList();
            //dbContext.Bring_In_Items.RemoveRange(bringinItems);

            var bringin = dbContext.Bring_In.FirstOrDefault(t => t.ID == id);
            bringin.Cancel = true;
            //dbContext.Bring_In.Remove(bringin);

            dbContext.SaveChanges();

            return Content("Success");
        }

        public ActionResult ApproveDetail(int id)
        {
            var bringin = dbContext.Bring_In.Find(id);
            var bringinItems = dbContext.Bring_In_Items.Where(t => t.CatID == id && t.AssetType == 0).OrderByDescending(t=>t.ID).ToList();
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
    }
}