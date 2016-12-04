namespace AutoMoto.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeInCar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cars", "Year", c => c.Int(nullable: false));
            DropTable("dbo.Ingredients");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Ingredients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.Cars", "Year");
        }
    }
}
