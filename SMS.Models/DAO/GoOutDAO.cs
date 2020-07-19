using System;
using System.Collections.Generic;
using System.Linq;
using SMS.Models.EF;
using PagedList;

namespace SMS.Models.DAO
{
    public class GoOutDAO
    {
        SMSDbContext dbContext = null;
        public GoOutDAO()
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
            return dbContext.Users.SingleOrDefault(x => x.EmpCode.Equals(empCode));
        }

        /// <summary>
        /// Phương thức thêm
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Insert(Go_Out entity, string createdBy)
        {
            dbContext.Go_Out.Add(entity);
            dbContext.SaveChanges();
            return entity.ID;
        }

        /// <summary>
        /// Phương thức trả về list danh sách
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IEnumerable<Go_Out> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Go_Out> model = dbContext.Go_Out;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.EmpCode.Contains(searchString) || x.FullName.Contains(searchString) || x.Team.Contains(searchString.ToLower()));
            }
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// Hàm update dành cho PIC
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(Go_Out entity, string modifiedBy)
        {
            try
            {
                var goOut = dbContext.Go_Out.Find(entity.ID);
                goOut.EmpCode = entity.EmpCode;
                goOut.Shift = entity.Shift;
                goOut.Reason = entity.Reason;
                goOut.EstimatedDateOut = entity.EstimatedDateOut;
                goOut.EstimatedTimeOut = entity.EstimatedTimeOut;
                goOut.EstimatedDateReturn = entity.EstimatedDateReturn;
                goOut.EstimatedTimeReturn = entity.EstimatedTimeReturn;
                goOut.ModifiedBy = entity.ModifiedBy;
                goOut.ModifiedDate = DateTime.Now;
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Phương thức trả về list danh sách cho mục phê duyệt
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IEnumerable<Go_Out> ListApprove(string searchString, int page, int pageSize)
        {
            IQueryable<Go_Out> model = dbContext.Go_Out;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.ApprovedStatus.Equals(null) && (x.EmpCode.Contains(searchString.ToLower()) || x.FullName.Contains(searchString.ToLower()) || x.Team.Contains(searchString.ToLower())));
            }
            else
            {
                model = model.Where(x => x.ApprovedStatus.Equals(null));
            }
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// Hàm update dành cho Admin, TM và GL khi phê duyệt
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool ApproveForAdmin(int id, string remark)
        {
            try
            {
                var goOut = dbContext.Go_Out.Find(id);
                goOut.ApprovedBy = "42001005 | Lưu Văn Phúc";
                goOut.ApprovedDate = DateTime.Now;
                //Chia case cho TM và GL
                goOut.ApprovedStatus = 1;
                goOut.ApproverRemark = remark;
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
        public bool RejectForAdmin(int id, string remark)
        {
            try
            {
                var leaveEarly = dbContext.Go_Out.Find(id);
                leaveEarly.ApprovedBy = "42001005";
                leaveEarly.ApprovedDate = DateTime.Now;
                //Chia case cho TM và GL
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
        /// Hàm lấy thông tin cho phần sửa
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Go_Out ViewDetail(int id)
        {
            return dbContext.Go_Out.Find(id);
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
                var goOut = dbContext.Go_Out.Find(id);
                dbContext.Go_Out.Remove(goOut);
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
        public bool Cancel(int id)
        {
            try
            {
                var goOut = dbContext.Go_Out.Find(id);
                goOut.ApprovedStatus = 4;
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
