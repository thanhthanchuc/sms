using System;
using System.Collections.Generic;
using System.Linq;
using SMS.Models.EF;
using PagedList;
using System.Data.Entity;

namespace SMS.Models.DAO
{
    public class LeaveEarlyDAO
    {
        SMSDbContext dbContext = null;
        public LeaveEarlyDAO()
        {
            dbContext = new SMSDbContext();
        }

        /// <summary>
        /// Phương thức lấy ra thông tin CNV theo mã
        /// </summary>
        /// <param name="empCode"></param>
        /// <returns></returns>
        public User GetByCode(string empCode)
        {
            return dbContext.Users.Include(u => u.Team).SingleOrDefault(x => x.EmpCode.Equals(empCode));
        }

        /// <summary>
        /// Phương thức thêm
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Insert(Leave_Early entity,string createdby)
        {
            dbContext.Leave_Early.Add(entity);
            dbContext.SaveChanges();
            return entity.ID;
        }

        /// <summary>
        /// Phương thức trả về list danh sách
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IEnumerable<Leave_Early> ListAllPaging(string searchString, int page, int pageSize, int userID, int role)
        {
            var team = dbContext.Users.Include(t => t.Team).Single(u => u.ID == userID).Team.Name;

            IQueryable<Leave_Early> model = dbContext.Leave_Early.Where(t => role >= 4 || t.Team == team);
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.EmpCode.Contains(searchString.ToLower()) || x.FullName.Contains(searchString.ToLower()) || x.Team.Contains(searchString.ToLower()));
            }
            return model.OrderByDescending(x => x.EstimatedDate).ThenByDescending(x => x.EstimatedTime).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// Phương thức trả về list danh sách cho mục phê duyệt
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IEnumerable<Leave_Early> ListApprove(string searchString, int page, int pageSize, string tName, bool unfilter)
        {
            IQueryable<Leave_Early> model = dbContext.Leave_Early.Where(t => unfilter || t.Team == tName); ;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.ApprovedStatus.Equals(null) && (x.EmpCode.Contains(searchString.ToLower()) || x.FullName.Contains(searchString.ToLower()) || x.Team.Contains(searchString.ToLower())));
            }
            else
            {
                model = model.Where(x => x.ApprovedStatus.Equals(null));
            }
            return model.OrderByDescending(x => x.EstimatedDate).ThenByDescending(x => x.EstimatedTime).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// Lấy thông tin cho phần sửa
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Leave_Early ViewDetail(int id)
        {
            return dbContext.Leave_Early.Find(id);
        }

        /// <summary>
        /// Hàm update dành cho PIC
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(Leave_Early entity, string modifiedBy)
        {
            try
            {
                var leaveEarly = dbContext.Leave_Early.Find(entity.ID);
                leaveEarly.EmpCode = entity.EmpCode;
                leaveEarly.Shift = entity.Shift;
                leaveEarly.Reason = entity.Reason;
                leaveEarly.EstimatedDate = entity.EstimatedDate;
                leaveEarly.EstimatedTime = entity.EstimatedTime;
                leaveEarly.ModifiedDate = DateTime.Now;
                leaveEarly.ModifiedBy = entity.ModifiedBy;
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Hàm update dành cho Admin, TM và GL khi phê duyệt
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool ApproveForAdmin(int id, string remark, string approveBy)
        {
            try
            {
                var leaveEarly = dbContext.Leave_Early.Find(id);
                leaveEarly.ApprovedDate = DateTime.Now;
                //Chia case cho TM và GL
                leaveEarly.ApprovedBy = approveBy;
                leaveEarly.ApprovedStatus = 1;
                leaveEarly.ApproverRemark = remark;
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Hàm reject dành cho Admin, TM và GL khi phê duyệt
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool RejectForAdmin(int id, string remark, string approveBy)
        {
            try
            {
                var leaveEarly = dbContext.Leave_Early.Find(id);
                leaveEarly.ApprovedDate = DateTime.Now;
                //Chia case cho TM và GL
                leaveEarly.ApprovedBy = approveBy;
                leaveEarly.ApprovedStatus = 0;
                leaveEarly.ApproverRemark = remark;
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Hàm xóa
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            try
            {
                var leaveEarly = dbContext.Leave_Early.Find(id);
                dbContext.Leave_Early.Remove(leaveEarly);
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Hàm thay trạng thái hủy bỏ cho PIC
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Cancel(int id, string modifiedBy)
        {
            try
            {
                var leaveEarly = dbContext.Leave_Early.Find(id);
                leaveEarly.ApprovedStatus = 4;
                leaveEarly.ModifiedDate = DateTime.Now;
                leaveEarly.ModifiedBy = modifiedBy;
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
