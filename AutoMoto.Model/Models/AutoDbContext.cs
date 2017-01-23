using AutoMoto.Model.Mappings;
using AutoMoto.Model.Migrations;
using AutoMoto.Models;
using Repository.Pattern.Ef6;
using System.Data.Entity;

namespace AutoMoto.Model.Models
{
    public partial class AutoDbContext : DataContext
    {
        static AutoDbContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AutoDbContext, AutoDbMigrationConfiguration>());
        }
        public AutoDbContext() : base("Name=DefaultConnection")
        { }



        public DbSet<AspNetRole> AspNetRoles { get; set; }
        public DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public DbSet<AspNetUser> AspNetUsers { get; set; }
        public DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Advertisement> Advertisements { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Following> Followings { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<AutoMoto.Models.Model> Modele { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Car> Cars { get; set; }

        public DbSet<FollowingNotification> FollowingNotifications { get; set; }
        public DbSet<MessageNotification> MessageNotifications { get; set; }

        public DbSet<UserNotification> UserNotifications { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.OneToManyCascadeDeleteConvention>();
            modelBuilder.Configurations.Add(new AspNetRoleMap());
            modelBuilder.Configurations.Add(new AspNetUserClaimMap());
            modelBuilder.Configurations.Add(new AspNetUserLoginMap());
            modelBuilder.Configurations.Add(new AspNetUserMap());
            modelBuilder.Entity<AspNetUser>().HasMany(u => u.MessagesReceived).WithRequired(u => u.ToUser).WillCascadeOnDelete(false);
            modelBuilder.Entity<AspNetUser>().HasMany(u => u.MessagesSent).WithRequired(u => u.FromUser).WillCascadeOnDelete(false);




        }
    }
}
