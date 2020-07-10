using SMS.Models.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SMS.Web.Models
{
    public class UserViewModel
    {
        public int ID { get; set; }

        [StringLength(10)]
        public string EmpCode { get; set; }

        [StringLength(50)]
        public string Password { get; set; }

        [StringLength(100)]
        public string FullName { get; set; }

        [StringLength(100)]
        public string ImgProfile { get; set; }

        public int? GroupID { get; set; }

        public int? Status { get; set; }

        [StringLength(50)]
        public string Position { get; set; }

        [StringLength(50)]
        public string Team { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

       

    }
    public static class Extentions
    {
        public static void UpdateUser(this UserViewModel userViewModel, User user)
        {
            userViewModel.ID = user.ID;
            userViewModel.EmpCode = user.EmpCode;
            userViewModel.FullName = user.FullName;
            userViewModel.Position = user.Position;
            userViewModel.Team = user.Team;
            userViewModel.Email = user.Email;
            //userViewModel.Phone = user.Phone;
        }
    }
}