using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMS.Models.EF;

namespace SMS.Models.DAO
{
    public class UserDAO
    {
        SMSDbContext dbContext = null;
        public UserDAO()
        {
            dbContext = new SMSDbContext();
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
        public User GetByCode(string empCode)
        {
            return dbContext.Users.SingleOrDefault(x => x.EmpCode == empCode);
        }

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
