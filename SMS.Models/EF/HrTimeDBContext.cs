namespace SMS.Models.EF
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class HrTimeDBContext : DbContext
    {
        public HrTimeDBContext()
            : base("name=HrTimeDBContext")
        {
        }

        public virtual DbSet<Profile_Employee> Profile_Employee { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
