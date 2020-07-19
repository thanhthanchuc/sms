namespace SMS.Models.EF
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.DirectoryServices.AccountManagement;

    public partial class SMSDbContext : DbContext
    {
        public SMSDbContext()
            : base("name=SMSDbContext")
        {
        }
        #region ConnectDomain
        public string Host = System.Configuration.ConfigurationManager.AppSettings["AuthencationHost"];
        public string Domain = System.Configuration.ConfigurationManager.AppSettings["AuthencationDomain"];

        public string UserID { get; set; }
        public string Password { get; set; }

        public bool IsValid()
        {
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, this.Host))
            {
                // validate the credentials
                bool isValid = pc.ValidateCredentials(this.UserID, this.Password);

                return isValid;
            }
        }
        #endregion 
        public virtual DbSet<Bring_In> Bring_In { get; set; }
        public virtual DbSet<Bring_In_Items> Bring_In_Items { get; set; }
        public virtual DbSet<Bring_Out> Bring_Out { get; set; }
        public virtual DbSet<Bring_Out_Items> Bring_Out_Items { get; set; }
        public virtual DbSet<Go_Out> Go_Out { get; set; }
        public virtual DbSet<GroupUser> GroupUsers { get; set; }
        public virtual DbSet<Guest> Guests { get; set; }
        public virtual DbSet<Guest_Item> Guest_Item { get; set; }
        public virtual DbSet<Leave_Early> Leave_Early { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<SystemConfig> SystemConfigs { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bring_In>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<Bring_In>()
                .Property(e => e.Position)
                .IsUnicode(false);

            modelBuilder.Entity<Bring_In>()
                .Property(e => e.Team)
                .IsUnicode(false);

            modelBuilder.Entity<Bring_In>()
                .Property(e => e.CreatedBy)
                .IsUnicode(true);

            modelBuilder.Entity<Bring_In>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(true);

            //modelBuilder.Entity<Bring_In>()
            //    .HasMany(e => e.Bring_In_Items)
            //    .WithOptional(e => e.Bring_In)
            //    .HasForeignKey(e => e.CatID);

            modelBuilder.Entity<Bring_In>()
            .HasMany(o => o.Bring_In_Items)
            .WithOptional()
            .HasForeignKey(c => c.CatID);

            modelBuilder.Entity<Bring_In_Items>()
                .Property(e => e.Quantity)
                .HasPrecision(18, 1);

            modelBuilder.Entity<Bring_In_Items>()
                .Property(e => e.ApprovedBy)
                .IsUnicode(true);

            modelBuilder.Entity<Bring_In_Items>()
                .Property(e => e.GuardIn)
                .IsUnicode(true);

            modelBuilder.Entity<Bring_In_Items>()
                .Property(e => e.GuardOut)
                .IsUnicode(true);

            modelBuilder.Entity<Bring_In_Items>()
                .Property(e => e.SMT)
                .IsUnicode(true);

            modelBuilder.Entity<Bring_In_Items>()
                .Property(e => e.ITT)
                .IsUnicode(true);

            modelBuilder.Entity<Bring_In_Items>()
                .Property(e => e.FST)
                .IsUnicode(true);

            modelBuilder.Entity<Bring_In_Items>()
                .Property(e => e.CreatedBy)
                .IsUnicode(true);

            modelBuilder.Entity<Bring_In_Items>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(true);

            modelBuilder.Entity<Bring_Out>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<Bring_Out>()
                .Property(e => e.Position)
                .IsUnicode(false);

            modelBuilder.Entity<Bring_Out>()
                .Property(e => e.Team)
                .IsUnicode(false);

            modelBuilder.Entity<Bring_Out>()
                .Property(e => e.CreatedBy)
                .IsUnicode(true);

            modelBuilder.Entity<Bring_Out>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(true);

            //modelBuilder.Entity<Bring_Out>()
            //    .HasMany(e => e.Bring_Out_Items)
            //    .WithOptional(e => e.Bring_Out)
            //    .HasForeignKey(e => e.CatID);

            modelBuilder.Entity<Bring_Out>()
            .HasMany(o => o.Bring_Out_Items)
            .WithOptional()
            .HasForeignKey(c => c.CatID);

            modelBuilder.Entity<Bring_Out_Items>()
                .Property(e => e.Quantity)
                .HasPrecision(18, 1);

            modelBuilder.Entity<Bring_Out_Items>()
                .Property(e => e.ApprovedBy)
                .IsUnicode(true);

            modelBuilder.Entity<Bring_Out_Items>()
                .Property(e => e.GuardOut)
                .IsUnicode(true);

            modelBuilder.Entity<Bring_Out_Items>()
                .Property(e => e.GuardReturn)
                .IsUnicode(true);

            modelBuilder.Entity<Bring_Out_Items>()
                .Property(e => e.SMT)
                .IsUnicode(true);

            modelBuilder.Entity<Bring_Out_Items>()
                .Property(e => e.ITT)
                .IsUnicode(true);

            modelBuilder.Entity<Bring_Out_Items>()
                .Property(e => e.FST)
                .IsUnicode(true);

            modelBuilder.Entity<Bring_Out_Items>()
                .Property(e => e.CreatedBy)
                .IsUnicode(true);

            modelBuilder.Entity<Bring_Out_Items>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(true);

            modelBuilder.Entity<Go_Out>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<Go_Out>()
                .Property(e => e.Position)
                .IsUnicode(false);

            modelBuilder.Entity<Go_Out>()
                .Property(e => e.Team)
                .IsUnicode(false);

            modelBuilder.Entity<Go_Out>()
                .Property(e => e.CreatedBy)
                .IsUnicode(true);

            modelBuilder.Entity<Go_Out>()
                .Property(e => e.ApprovedBy)
                .IsUnicode(true);

            modelBuilder.Entity<Go_Out>()
                .Property(e => e.GuardOut)
                .IsUnicode(true);

            modelBuilder.Entity<Go_Out>()
                .Property(e => e.GuardReturn)
                .IsUnicode(true);

            modelBuilder.Entity<Go_Out>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(true);

            modelBuilder.Entity<GroupUser>()
                .HasMany(e => e.Users)
                .WithOptional(e => e.GroupUser)
                .HasForeignKey(e => e.GroupID);

            modelBuilder.Entity<Guest>()
                .Property(e => e.EmployeeID)
                .IsUnicode(false);

            modelBuilder.Entity<Guest>()
                .Property(e => e.Team)
                .IsUnicode(false);

            modelBuilder.Entity<Guest>()
                .Property(e => e.Position)
                .IsUnicode(false);

            modelBuilder.Entity<Guest>()
                .Property(e => e.KVPNo)
                .IsUnicode(false);

            modelBuilder.Entity<Guest>()
                .Property(e => e.GuardIn)
                .IsUnicode(true);

            modelBuilder.Entity<Guest>()
                .Property(e => e.GuardOut)
                .IsUnicode(true);

            modelBuilder.Entity<Guest>()
                .Property(e => e.CreatedBy)
                .IsUnicode(true);

            modelBuilder.Entity<Guest>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(true);

            //modelBuilder.Entity<Guest>()
            //    .HasMany(e => e.Guest_Item)
            //    .WithOptional(e => e.Guest)
            //    .HasForeignKey(e => e.CatID);

            modelBuilder.Entity<Guest>()
            .HasMany(o => o.Guest_Item)
            .WithOptional()
            .HasForeignKey(c => c.CatID);

            modelBuilder.Entity<Guest_Item>()
                .Property(e => e.Quantity)
                .HasPrecision(18, 1);

            modelBuilder.Entity<Guest_Item>()
                .Property(e => e.SMT)
                .IsUnicode(true);

            modelBuilder.Entity<Guest_Item>()
                .Property(e => e.ITT)
                .IsUnicode(true);

            modelBuilder.Entity<Guest_Item>()
                .Property(e => e.FST)
                .IsUnicode(true);

            modelBuilder.Entity<Guest_Item>()
                .Property(e => e.GuardIn)
                .IsUnicode(true);

            modelBuilder.Entity<Guest_Item>()
                .Property(e => e.GuardOut)
                .IsUnicode(true);

            modelBuilder.Entity<Guest_Item>()
                .Property(e => e.CreatedBy)
                .IsUnicode(true);

            modelBuilder.Entity<Guest_Item>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(true);

            modelBuilder.Entity<Leave_Early>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<Leave_Early>()
                .Property(e => e.Position)
                .IsUnicode(false);

            modelBuilder.Entity<Leave_Early>()
                .Property(e => e.Team)
                .IsUnicode(false);

            modelBuilder.Entity<Leave_Early>()
                .Property(e => e.CreatedBy)
                .IsUnicode(true);

            modelBuilder.Entity<Leave_Early>()
                .Property(e => e.ApprovedBy)
                .IsUnicode(true);

            modelBuilder.Entity<Leave_Early>()
                .Property(e => e.Guard)
                .IsUnicode(true);

            modelBuilder.Entity<Leave_Early>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(true);

            modelBuilder.Entity<Role>()
                .HasMany(e => e.GroupUsers)
                .WithOptional(e => e.Role)
                .HasForeignKey(e => e.RolesID);

            modelBuilder.Entity<SystemConfig>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Password)
                .IsUnicode(false);

            //modelBuilder.Entity<User>()
            //    .Property(e => e.Phone)
            //    .IsUnicode(false);
        }
    }
}
