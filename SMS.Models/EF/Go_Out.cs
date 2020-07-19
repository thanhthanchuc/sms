namespace SMS.Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using Utils;

<<<<<<< HEAD
    public partial class Go_Out : IValidatableObject
=======
    public partial class Go_Out: IValidatableObject
>>>>>>> thanh
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

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
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

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DateTime.Parse(Util.FormatDate(EstimatedDateOut) + ' ' + EstimatedTimeOut) < DateTime.Now)
            {
                yield return
                 new ValidationResult("Vui lòng điền đúng ngày và giờ ra ngoài",
                                      new[] { "EstimatedDateOut" });
            }

            if (DateTime.Parse(Util.FormatDate(EstimatedDateOut) + ' ' + EstimatedTimeOut) > DateTime.Parse(Util.FormatDate(EstimatedDateReturn) + ' ' + EstimatedTimeReturn))
            {
                yield return
                  new ValidationResult("Vui lòng điền đúng ngày và giờ trở lại",
                                       new[] { "EstimatedDateReturn" });
            }
        }
    }
}
