namespace SMS.Models.EF
{
    using SMS.Models.Utils;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Leave_Early : IValidatableObject
    {
        public int ID { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Bạn phải điền mã CNV")]
        public string EmpCode { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Bạn phải điền tên")]
        public string FullName { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Bạn phải điền vị trí")]
        public string Position { get; set; }

        [StringLength(10)]
        [Required(ErrorMessage = "Bạn phải điền phòng")]
        public string Team { get; set; }

        public int? Shift { get; set; }

        [Required(ErrorMessage = "Bạn phải điền lí do")]
        public string Reason { get; set; }

        public bool Purpose { get; set; }

        public string EstimatedDate { get; set; }

        [Required(ErrorMessage = "Bạn phải điền giờ đăng ký")]
        public string EstimatedTime { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? CreatedDate { get; set; }

        [StringLength(50)]
        public string ApprovedBy { get; set; }

        public int? ApprovedStatus { get; set; }

        public string ApproverRemark { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? ApprovedDate { get; set; }

        [StringLength(50)]
        public string Guard { get; set; }

        public int? GuardStatus { get; set; }

        [StringLength(250)]
        public string GuardRemark { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? GuardDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            if (DateTime.Parse(Util.FormatDate(EstimatedDate) + ' ' + EstimatedTime) <= DateTime.Now)
            {
                yield return
                 new ValidationResult("Vui lòng điền đúng ngày và giờ ra ngoài",
<<<<<<< HEAD
                                      new[] { "EstimatedDate" });
=======
                                      new[] { "EstimatedDateOut" });
>>>>>>> thanh
            }
        }
    }
}
