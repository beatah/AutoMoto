namespace AutoMoto.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class x1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FollowingNotifications", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.FollowingNotifications", new[] { "UserId" });
            DropColumn("dbo.FollowingNotifications", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FollowingNotifications", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.FollowingNotifications", "UserId");
            AddForeignKey("dbo.FollowingNotifications", "UserId", "dbo.AspNetUsers", "Id");
        }
    }
}
