namespace AutoMoto.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFeatureToCar : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Features",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FeatureCars",
                c => new
                    {
                        Feature_Id = c.Int(nullable: false),
                        Car_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Feature_Id, t.Car_Id })
                .ForeignKey("dbo.Features", t => t.Feature_Id, cascadeDelete: true)
                .ForeignKey("dbo.Cars", t => t.Car_Id, cascadeDelete: true)
                .Index(t => t.Feature_Id)
                .Index(t => t.Car_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FeatureCars", "Car_Id", "dbo.Cars");
            DropForeignKey("dbo.FeatureCars", "Feature_Id", "dbo.Features");
            DropIndex("dbo.FeatureCars", new[] { "Car_Id" });
            DropIndex("dbo.FeatureCars", new[] { "Feature_Id" });
            DropTable("dbo.FeatureCars");
            DropTable("dbo.Features");
        }
    }
}
