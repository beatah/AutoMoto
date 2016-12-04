namespace AutoMoto.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddActiveFlagToManufacturer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Manufacturers", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Manufacturers", "IsActive");
        }
    }
}
