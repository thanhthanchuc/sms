using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SMS.Web.Models.Report
{
    public class BringInRPModel
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string EmpCode { get; set; }

        [StringLength(100)]
        public string FullName { get; set; }

        [StringLength(50)]
        public string Position { get; set; }

        [StringLength(10)]
        public string Team { get; set; }

        [StringLength(500)]
        public string Reason { get; set; }

        public string EstimatedDate { get; set; }
        public string EstimatedTime { get; set; }

        public string Status { get; set; }

        public string Cancel { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public string CreatedDate { get; set; }

        public string ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public List<BringInItemRPModel> Bring_In_Items { get; set; }
    }
}