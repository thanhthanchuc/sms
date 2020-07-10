namespace SMS.Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Go_Out
    {
        public int ID { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Bạn phải điền mã CNV")]
        public string EmpCode { get; set; }

        [StringLength(100)]
        public string FullName { get; set; }

        [StringLength(50)]
        public string Position { get; set; }

        [StringLength(10)]
        public string Team { get; set; }

        public int? Shift { get; set; }

        [Required(ErrorMessage = "Bạn phải điền lí do")]
        public string Reason { get; set; }

        public bool Purpose { get; set; }

        public string EstimatedDateOut { get; set; }

        [Required(ErrorMessage = "Bạn phải điền giờ ra")]
        public string EstimatedTimeOut { get; set; }

        public string EstimatedDateReturn { get; set; }

        [Required(ErrorMessage = "Bạn phải điền giờ quay lại")]
        public string EstimatedTimeReturn { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(50)]
        public string ApprovedBy { get; set; }

        public int? ApprovedStatus { get; set; }

        public string ApproverRemark { get; set; }

        public DateTime? ApprovedDate { get; set; }

        [StringLength(50)]
        public string GuardOut { get; set; }

        public int? GuardStatusOut { get; set; }

        public string GuardRemarkOut { get; set; }

        public DateTime? GuardDateOut { get; set; }

        [StringLength(50)]
        public string GuardReturn { get; set; }

        public int? GuardStatusReturn { get; set; }

        public string GuardRemarkReturn { get; set; }

        public DateTime? GuardDateReturn { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }
    }
}
