namespace AutoMoto.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OneAddressPerUser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Advertisements", "AddressId", "dbo.Addresses");
            DropIndex("dbo.Addresses", new[] { "User_Id" });
            DropIndex("dbo.Advertisements", new[] { "AddressId" });
            RenameColumn(table: "dbo.Addresses", name: "User_Id", newName: "UserId");
            DropPrimaryKey("dbo.Addresses");
            AddColumn("dbo.AspNetUsers", "AddressId", c => c.Int(nullable: false));
            AlterColumn("dbo.Addresses", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Photos", "Extension", c => c.String());
            AddPrimaryKey("dbo.Addresses", "UserId");
            CreateIndex("dbo.Addresses", "UserId");
            DropColumn("dbo.Addresses", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Addresses", "Id", c => c.Int(nullable: false, identity: true));
            DropIndex("dbo.Addresses", new[] { "UserId" });
            DropPrimaryKey("dbo.Addresses");
            AlterColumn("dbo.Photos", "Extension", c => c.Int(nullable: false));
            AlterColumn("dbo.Addresses", "UserId", c => c.String(maxLength: 128));
            DropColumn("dbo.AspNetUsers", "AddressId");
            AddPrimaryKey("dbo.Addresses", "Id");
            RenameColumn(table: "dbo.Addresses", name: "UserId", newName: "User_Id");
            CreateIndex("dbo.Advertisements", "AddressId");
            CreateIndex("dbo.Addresses", "User_Id");
            AddForeignKey("dbo.Advertisements", "AddressId", "dbo.Addresses", "Id");
        }
    }
}
