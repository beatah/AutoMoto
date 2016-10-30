namespace AutoMoto.Model.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Street = c.String(),
                    City = c.String(),
                    ZipCode = c.String(),
                    User_Id = c.String(maxLength: 128),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);

            CreateTable(
                "dbo.Advertisements",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Title = c.String(),
                    AddedDate = c.DateTime(nullable: false),
                    Description = c.String(),
                    AddressId = c.Int(nullable: false),
                    IsActive = c.Boolean(nullable: false),
                    CarId = c.Int(nullable: false),
                    User_Id = c.String(maxLength: 128),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.AddressId)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.AddressId)
                .Index(t => t.User_Id);

            CreateTable(
                "dbo.Cars",
                c => new
                {
                    AdvertisementId = c.Int(nullable: false),
                    EngineCap = c.Int(nullable: false),
                    Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    FuelType = c.Int(nullable: false),
                    Mileage = c.Int(nullable: false),
                    ModelId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.AdvertisementId)
                .ForeignKey("dbo.Advertisements", t => t.AdvertisementId)
                .ForeignKey("dbo.Models", t => t.ModelId)
                .Index(t => t.AdvertisementId)
                .Index(t => t.ModelId);

            CreateTable(
                "dbo.Models",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                    ManufacturerId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Manufacturers", t => t.ManufacturerId)
                .Index(t => t.ManufacturerId);

            CreateTable(
                "dbo.Manufacturers",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Photos",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Content = c.Binary(),
                    Extension = c.Int(nullable: false),
                    Advertisement_Id = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Advertisements", t => t.Advertisement_Id)
                .Index(t => t.Advertisement_Id);

            CreateTable(
                "dbo.AspNetUsers",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    FirstName = c.String(),
                    LastName = c.String(),
                    Email = c.String(maxLength: 256),
                    EmailConfirmed = c.Boolean(nullable: false),
                    PasswordHash = c.String(),
                    SecurityStamp = c.String(),
                    PhoneNumber = c.String(),
                    PhoneNumberConfirmed = c.Boolean(nullable: false),
                    TwoFactorEnabled = c.Boolean(nullable: false),
                    LockoutEndDateUtc = c.DateTime(),
                    LockoutEnabled = c.Boolean(nullable: false),
                    AccessFailedCount = c.Int(nullable: false),
                    UserName = c.String(nullable: false, maxLength: 256),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.AspNetRoles",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    Name = c.String(nullable: false, maxLength: 256),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    UserId = c.String(nullable: false, maxLength: 128),
                    ClaimType = c.String(),
                    ClaimValue = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                {
                    LoginProvider = c.String(nullable: false, maxLength: 128),
                    ProviderKey = c.String(nullable: false, maxLength: 128),
                    UserId = c.String(nullable: false, maxLength: 128),
                })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.Messages",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    FromUserId = c.String(nullable: false, maxLength: 128),
                    ToUserId = c.String(nullable: false, maxLength: 128),
                    Content = c.String(),
                    AdvertisementId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Advertisements", t => t.AdvertisementId)
                .ForeignKey("dbo.AspNetUsers", t => t.ToUserId)
                .ForeignKey("dbo.AspNetUsers", t => t.FromUserId)
                .Index(t => t.FromUserId)
                .Index(t => t.ToUserId)
                .Index(t => t.AdvertisementId);

            CreateTable(
                "dbo.UserNotifications",
                c => new
                {
                    NotificationId = c.Int(nullable: false),
                    UserId = c.String(nullable: false, maxLength: 128),
                    IsRead = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => new { t.NotificationId, t.UserId })
                .ForeignKey("dbo.Notifications", t => t.NotificationId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.NotificationId)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.Notifications",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    DateTime = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Ingredients",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 128),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                {
                    RoleId = c.String(nullable: false, maxLength: 128),
                    UserId = c.String(nullable: false, maxLength: 128),
                })
                .PrimaryKey(t => new { t.RoleId, t.UserId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.FollowingNotifications",
                c => new
                {
                    UserId = c.String(maxLength: 128),
                    AdvertisementId = c.Int(nullable: false),
                    Id = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Notifications", t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.Advertisements", t => t.AdvertisementId)
                .Index(t => t.UserId)
                .Index(t => t.AdvertisementId)
                .Index(t => t.Id);

            CreateTable(
                "dbo.MessageNotifications",
                c => new
                {
                    Id = c.Int(nullable: false),
                    MessageId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Notifications", t => t.Id)
                .ForeignKey("dbo.Messages", t => t.MessageId)
                .Index(t => t.Id)
                .Index(t => t.MessageId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.MessageNotifications", "MessageId", "dbo.Messages");
            DropForeignKey("dbo.MessageNotifications", "Id", "dbo.Notifications");
            DropForeignKey("dbo.FollowingNotifications", "AdvertisementId", "dbo.Advertisements");
            DropForeignKey("dbo.FollowingNotifications", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.FollowingNotifications", "Id", "dbo.Notifications");
            DropForeignKey("dbo.UserNotifications", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserNotifications", "NotificationId", "dbo.Notifications");
            DropForeignKey("dbo.Messages", "FromUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Messages", "ToUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Messages", "AdvertisementId", "dbo.Advertisements");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Advertisements", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Addresses", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Photos", "Advertisement_Id", "dbo.Advertisements");
            DropForeignKey("dbo.Cars", "ModelId", "dbo.Models");
            DropForeignKey("dbo.Models", "ManufacturerId", "dbo.Manufacturers");
            DropForeignKey("dbo.Cars", "AdvertisementId", "dbo.Advertisements");
            DropForeignKey("dbo.Advertisements", "AddressId", "dbo.Addresses");
            DropIndex("dbo.MessageNotifications", new[] { "MessageId" });
            DropIndex("dbo.MessageNotifications", new[] { "Id" });
            DropIndex("dbo.FollowingNotifications", new[] { "Id" });
            DropIndex("dbo.FollowingNotifications", new[] { "AdvertisementId" });
            DropIndex("dbo.FollowingNotifications", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserNotifications", new[] { "UserId" });
            DropIndex("dbo.UserNotifications", new[] { "NotificationId" });
            DropIndex("dbo.Messages", new[] { "AdvertisementId" });
            DropIndex("dbo.Messages", new[] { "ToUserId" });
            DropIndex("dbo.Messages", new[] { "FromUserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.Photos", new[] { "Advertisement_Id" });
            DropIndex("dbo.Models", new[] { "ManufacturerId" });
            DropIndex("dbo.Cars", new[] { "ModelId" });
            DropIndex("dbo.Cars", new[] { "AdvertisementId" });
            DropIndex("dbo.Advertisements", new[] { "User_Id" });
            DropIndex("dbo.Advertisements", new[] { "AddressId" });
            DropIndex("dbo.Addresses", new[] { "User_Id" });
            DropTable("dbo.MessageNotifications");
            DropTable("dbo.FollowingNotifications");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.Ingredients");
            DropTable("dbo.Notifications");
            DropTable("dbo.UserNotifications");
            DropTable("dbo.Messages");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Photos");
            DropTable("dbo.Manufacturers");
            DropTable("dbo.Models");
            DropTable("dbo.Cars");
            DropTable("dbo.Advertisements");
            DropTable("dbo.Addresses");
        }
    }
}
