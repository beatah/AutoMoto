namespace AutoMoto.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class x : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Advertisements", name: "User_Id", newName: "UserId");
            RenameIndex(table: "dbo.Advertisements", name: "IX_User_Id", newName: "IX_UserId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Advertisements", name: "IX_UserId", newName: "IX_User_Id");
            RenameColumn(table: "dbo.Advertisements", name: "UserId", newName: "User_Id");
        }
    }
}
