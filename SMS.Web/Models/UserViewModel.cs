using SMS.Models.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SMS.Web.Models
{
    public class UserViewModel : User
    {
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
            
            userViewModel.Admin = user.Admin;

            userViewModel.SubAdmin = user.SubAdmin;

            userViewModel.PIC = user.PIC;

            userViewModel.ITT_TM = user.ITT_TM;

            userViewModel.SMT_TM = user.SMT_TM;

            userViewModel.FST_TM = user.FST_TM;

            userViewModel.PIC_TM = user.PIC_TM;

            userViewModel.Group_Leader = user.Group_Leader;

            userViewModel.Guard = user.Guard;

            userViewModel.Read_Only = user.Read_Only;
            //userViewModel.Phone = user.Phone;
        }
    }
}