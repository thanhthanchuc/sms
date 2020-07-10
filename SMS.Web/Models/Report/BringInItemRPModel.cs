using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SMS.Web.Models.Report
{
    public class BringInItemRPModel
    {
        [ForeignKey("BringInRPModel")]
        public string CatID { get; set; }

        [StringLength(250)]
        public string Item { get; set; }

        [StringLength(250)]
        public string Serial { get; set; }

        public string Quantity { get; set; }

        [StringLength(50)]
        public string Unit { get; set; }

        public string AssetType { get; set; }

        public string IsReturn { get; set; }

        public string ReturnDate { get; set; }
        public string ReturnTime { get; set; }

        [StringLength(50)]
        public string ApprovedBy { get; set; }

        public string ApprovedStatus { get; set; }

        [StringLength(250)]
        public string ApproverRemark { get; set; }

        public string ApprovedDate { get; set; }

        [StringLength(50)]
        public string GuardIn { get; set; }

        public string GuardStatusIn { get; set; }

        [StringLength(250)]
        public string GuardRemarkIn { get; set; }

        public string GuardDateIn { get; set; }

        [StringLength(50)]
        public string GuardOut { get; set; }

        public string GuardStatusOut { get; set; }

        [StringLength(250)]
        public string GuardRemarkOut { get; set; }

        public string GuardDateOut { get; set; }

        [StringLength(50)]
        public string SMT { get; set; }

        public string SMT_Status { get; set; }

        [StringLength(50)]
        public string SMT_Remark { get; set; }

        public string SMT_Date { get; set; }

        [StringLength(50)]
        public string ITT { get; set; }

        public string ITT_Status { get; set; }

        [StringLength(50)]
        public string ITT_Remark { get; set; }

        public string ITT_Date { get; set; }

        [StringLength(50)]
        public string FST { get; set; }

        public string FST_Status { get; set; }

        [StringLength(50)]
        public string FST_Remark { get; set; }

        public string FST_Date { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public string CreatedDate { get; set; }

        public string ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }
    }
}