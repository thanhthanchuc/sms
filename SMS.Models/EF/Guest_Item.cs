namespace SMS.Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Guest_Item
    {
        public int ID { get; set; }

        public int CatID { get; set; }

        [StringLength(250)]
        public string Item { get; set; }

        [StringLength(250)]
        public string Serial { get; set; }

        public decimal? Quantity { get; set; }

        [StringLength(50)]
        public string Unit { get; set; }

        public int? AssetType { get; set; }

        public bool? IsReturn { get; set; }

        public string ReturnDate { get; set; }

        public string ReturnTime { get; set; }

        [StringLength(50)]
        public string SMT { get; set; }

        public int? SMT_Status { get; set; }

        [StringLength(50)]
        public string SMT_Remark { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        public DateTime? SMT_Date { get; set; }

        [StringLength(50)]
        public string ITT { get; set; }

        public int? ITT_Status { get; set; }

        [StringLength(50)]
        public string ITT_Remark { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        public DateTime? ITT_Date { get; set; }

        [StringLength(50)]
        public string FST { get; set; }

        public int? FST_Status { get; set; }

        [StringLength(50)]
        public string FST_Remark { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        public DateTime? FST_Date { get; set; }

        [StringLength(50)]
        public string GuardIn { get; set; }

        public int? GuardStatusIn { get; set; }

        [StringLength(250)]
        public string GuardRemarkIn { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        public DateTime? GuardDateIn { get; set; }

        [StringLength(50)]
        public string GuardOut { get; set; }

        public int? GuardStatusOut { get; set; }

        [StringLength(250)]
        public string GuardRemarkOut { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        public DateTime? GuardDateOut { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        public DateTime? CreatedDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        //public virtual Guest Guest { get; set; }
    }
}
