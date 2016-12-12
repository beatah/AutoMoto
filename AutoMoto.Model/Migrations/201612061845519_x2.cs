namespace AutoMoto.Model.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class x2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Addresses", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Addresses", "Id");
            CreateIndex("dbo.AspNetUsers", "AddressId");
        }

        public override void Down()
        {
            AddColumn("dbo.Addresses", "UserId", c => c.String(nullable: false, maxLength: 128));
            DropForeignKey("dbo.AspNetUsers", "AddressId", "dbo.Addresses");
            DropIndex("dbo.AspNetUsers", new[] { "AddressId" });
            DropPrimaryKey("dbo.Addresses");
            DropColumn("dbo.Addresses", "Id");
            AddPrimaryKey("dbo.Addresses", "UserId");
            RenameColumn(table: "dbo.AspNetUsers", name: "AddressId", newName: "UserId");
            CreateIndex("dbo.Addresses", "UserId");
        }
    }
}
