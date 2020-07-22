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
        public DbSet<Team> Teams { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<Guest> Guests { get; set; }
        public virtual DbSet<Guest_Item> Guest_Item { get; set; }
        public virtual DbSet<Leave_Early> Leave_Early { get; set; }
        public virtual DbSet<SystemConfig> SystemConfigs { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
