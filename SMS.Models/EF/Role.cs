namespace SMS.Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Role
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        public int Priority { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
    }
}
