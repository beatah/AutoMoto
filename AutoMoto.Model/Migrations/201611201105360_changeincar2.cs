namespace AutoMoto.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeincar2 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Cars", name: "AdvertisementId", newName: "Id");
            RenameIndex(table: "dbo.Cars", name: "IX_AdvertisementId", newName: "IX_Id");
            AddColumn("dbo.Advertisements", "CarId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Advertisements", "CarId");
            RenameIndex(table: "dbo.Cars", name: "IX_Id", newName: "IX_AdvertisementId");
            RenameColumn(table: "dbo.Cars", name: "Id", newName: "AdvertisementId");
        }
    }
}
