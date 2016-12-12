namespace AutoMoto.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class x3 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Advertisements", "CarId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Advertisements", "CarId", c => c.Int(nullable: false));
        }
    }
}
