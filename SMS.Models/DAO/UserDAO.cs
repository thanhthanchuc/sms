using System;
using System.Collections.Generic;
using System.Linq;
using SMS.Models.EF;
using PagedList;
using System.Data.Entity;

namespace SMS.Models.DAO
{
    public class UserDAO
    {
        SMSDbContext dbContext = null;
        public UserDAO()
        {
            dbContext = new SMSDbContext();
        }

        public User GetByCode(string empCode)
        {
            return dbContext.Users.Include(u => u.Team).SingleOrDefault(x => x.EmpCode.Equals(empCode));
        }

        public int Insert(User entity)
        {
            dbContext.Users.Add(entity);
            dbContext.SaveChanges();
            return entity.ID;
        }

        /// <summary>
        /// Phương thức lấy ra mã CNV
        /// </summary>
        /// <param name="empCode"></param>
        /// <returns></returns>


        /// <summary>
        /// Phương thức đăng nhập
        /// </summary>
        /// <param name="empCode"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public int Login(string empCode, string password, bool check)
        {
            var authencationContext = new SMSDbContext();
            authencationContext.UserID = empCode;
            authencationContext.Password = password;
          

            if (!check)
            {
                if (authencationContext.IsValid())
                {
                    //Neu OK
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                var res = dbContext.Users.SingleOrDefault(x => x.EmpCode == empCode);
                if (res == null)
                {
                    return 0;
                }
                else if (res.Password == password)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }

        }
    }
}
