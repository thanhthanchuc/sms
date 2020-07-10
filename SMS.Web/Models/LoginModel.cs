using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SMS.Web.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Bạn phải nhập mã CNV")]
        public string EmpCode { get; set; }

        [Required(ErrorMessage = "Bạn phải nhập mật khẩu")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }

        public bool Check { get; set; }
    }
}