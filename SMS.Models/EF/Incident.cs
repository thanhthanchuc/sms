namespace SMS.Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [Table("Incidents")]
    public partial class Incident
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Bạn phải điền tiêu đề")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Bạn phải điền file đính kèm")]
        public string AttachedFile { get; set; }

        public string SMTAttachedFile { get; set; }

        public string TeamAttachedFile { get; set; }

        public string CreatedBy { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        public DateTime? CreatedDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        public DateTime? ModifiedDate { get; set; }

        public string ModifiedBy { get; set; }
    }
}
