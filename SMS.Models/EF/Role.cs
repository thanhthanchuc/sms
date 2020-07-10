namespace SMS.Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Role
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Role()
        {
            GroupUsers = new HashSet<GroupUser>();
        }

        public int ID { get; set; }

        [StringLength(50)]
        public string RoleName { get; set; }

        public int? IDParent { get; set; }

        [StringLength(50)]
        public string Link { get; set; }

        [StringLength(50)]
        public string DropDown { get; set; }

        public int? Hiden { get; set; }

        [StringLength(50)]
        public string Active { get; set; }

        [StringLength(50)]
        public string SubActive { get; set; }

        public int? IndexA { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GroupUser> GroupUsers { get; set; }
    }
}
