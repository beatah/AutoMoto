namespace AutoMoto.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeinadv : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Advertisements", "AddressId");
            DropColumn("dbo.Advertisements", "CarId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Advertisements", "CarId", c => c.Int(nullable: false));
            AddColumn("dbo.Advertisements", "AddressId", c => c.Int(nullable: false));
        }
    }
}
