namespace SMS.Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Bring_Out
    {
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        //public Bring_Out()
        //{
        //    Bring_Out_Items = new HashSet<Bring_Out_Items>();
        //}

        public int ID { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Bạn phải điền mã CNV")]
        public string EmpCode { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Bạn phải điền mã CNV")]
        public string FullName { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Bạn phải điền mã CNV")]
        public string Position { get; set; }

        [StringLength(10)]
        [Required(ErrorMessage = "Bạn phải điền mã CNV")]
        public string Team { get; set; }

        [Required(ErrorMessage = "Bạn phải điền lí do")]
        public string Reason { get; set; }

        [Required(ErrorMessage = "Bạn phải điền ngày mang ra")]
        public string EstimatedDate { get; set; }

        [Required(ErrorMessage = "Bạn phải điền giờ mang ra")]
        public string EstimatedTime { get; set; }

        public bool? Status { get; set; }

        public bool? Cancel { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        public DateTime? CreatedDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        public ICollection<Bring_Out_Items> Bring_Out_Items { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Bring_Out_Items> Bring_Out_Items { get; set; }
    }
}
