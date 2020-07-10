namespace SMS.Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Guest")]
    public partial class Guest
    {
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        //public Guest()
        //{
        //    Guest_Item = new HashSet<Guest_Item>();
        //}

        public int ID { get; set; }

        public int? Type { get; set; }

        public bool? IO { get; set; }

        [StringLength(150)]
        public string FullName { get; set; }

        [StringLength(250)]
        public string IDCard { get; set; }

        [StringLength(250)]
        public string Company { get; set; }

        public int? NumbersOfPerson { get; set; }

        [StringLength(200)]
        public string Visa { get; set; }

        [StringLength(300)]
        public string Hotel { get; set; }

        [StringLength(250)]
        public string Purpose { get; set; }

        [StringLength(100)]
        public string TransportNo { get; set; }

        [StringLength(10)]
        [Required]
        public string EmployeeID { get; set; }

        [StringLength(250)]
        [Required]
        public string EmployeeName { get; set; }

        [StringLength(4)]
        public string Team { get; set; }

        [StringLength(3)]
        public string Position { get; set; }

        public string EstimatedDateIn { get; set; }

        public string EstimatedTimeIn { get; set; }

        public string EstimatedDateOut { get; set; }
        public string EstimatedTimeOut { get; set; }

        [StringLength(50)]
        public string KVPNo { get; set; }

        [StringLength(50)]
        public string GuardIn { get; set; }

        public int? GuardStatusIn { get; set; }

        [Column(TypeName = "ntext")]
        public string GuardRemarkIn { get; set; }

        public DateTime? GuardDateIn { get; set; }

        [StringLength(50)]
        public string GuardOut { get; set; }

        public int? GuardStatusOut { get; set; }

        [Column(TypeName = "ntext")]
        public string GuardRemarkOut { get; set; }

        public DateTime? GuardDateOut { get; set; }

        public bool? Status { get; set; }

        public bool? Cancel { get; set; }

        public bool? Check_Guest { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }
        public string FileRedirectURL { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Guest_Item> Guest_Item { get; set; }
    }
}
