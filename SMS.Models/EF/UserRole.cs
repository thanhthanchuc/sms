using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMS.Models.EF
{
    public class UserRole
    {
        [Key, Column(Order = 0)]
        public int UserID { get; set; }
        public User User { get; set; }


        [Key, Column(Order = 1)]
        public int RoleID { get; set; }
        public Role Role { get; set; }

    }
}
