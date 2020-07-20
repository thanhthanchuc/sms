namespace SMS.Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Serializable]
    public class User
    {
        public int ID { get; set; }

        [StringLength(10)]
        public string EmpCode { get; set; }

        [StringLength(50)]
        public string Password { get; set; }

        [StringLength(100)]
        public string FullName { get; set; }

        //[StringLength(100)]
        //public string ImgProfile { get; set; }

        public int? GroupID { get; set; }

        public int? Status { get; set; }

        [StringLength(50)]
        public string Position { get; set; }

        [StringLength(50)]
        public string Team { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Cellphone { get; set; }

        public int RoleId { get; set; }

        public bool? Admin { get; set; }

        public bool? SubAdmin { get; set; }

        public bool? PIC { get; set; }

        public bool? ITT_TM { get; set; }

        public bool? SMT_TM { get; set; }

        public bool? FST_TM { get; set; }

        public bool? PIC_TM { get; set; }

        public bool? Group_Leader { get; set; }

        public bool? Guard { get; set; }

        public bool? Read_Only { get; set; }

        //[StringLength(20)]
        //public string Phone { get; set; }

        [ForeignKey("GroupID")]
        public virtual GroupUser GroupUser { get; set; }
    }
}
