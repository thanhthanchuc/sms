namespace SMS.Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public int ID { get; set; }

        [StringLength(10)]
        public string EmpCode { get; set; }

        [StringLength(50)]
        public string Password { get; set; }

        [StringLength(100)]
        public string FullName { get; set; }

        public int? Status { get; set; }

        [StringLength(50)]
        public string Position { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Cellphone { get; set; }

        public int TeamID { get; set; }
        public Team Team { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
