using AutoMoto.Model.Models;
using System.Data.Entity.Migrations;

namespace AutoMoto.Model.Migrations
{
    public class AutoDbMigrationConfiguration : DbMigrationsConfiguration<AutoDbContext>
    {
        public AutoDbMigrationConfiguration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "AutoMoto.Model.Models.AutoDbContext";

            TargetDatabase = new System.Data.Entity.Infrastructure.DbConnectionInfo("DefaultConnection");
        }
        protected override void Seed(AutoDbContext context)
        {

        }

    }
}