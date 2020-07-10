namespace SMS.Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Profile_Employee
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string EmpCode { get; set; }

        [StringLength(50)]
        public string FullName { get; set; }

        public bool? Sex { get; set; }

        [StringLength(50)]
        public string Cost_Center { get; set; }

        [StringLength(50)]
        public string TeamCode { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(50)]
        public string JobGrade { get; set; }

        [Column(TypeName = "date")]
        public DateTime? HiredDate { get; set; }

        public int? IDTeam { get; set; }

        public int? IDJobTitle { get; set; }

        public bool? Status_Working { get; set; }

        public int? Line { get; set; }

        public int? GroupID { get; set; }

        [StringLength(50)]
        public string JobTitle { get; set; }
    }
}
