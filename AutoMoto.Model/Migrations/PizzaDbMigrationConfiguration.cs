using AutoMoto.Model.Models;
using System.Data.Entity.Migrations;

namespace AutoMoto.Model.Migrations
{
    public class PizzaDbMigrationConfiguration : DbMigrationsConfiguration<PizzaDbContext>
    {
        public PizzaDbMigrationConfiguration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "AutoMoto.Model.Models.PizzaDbContext";

            TargetDatabase = new System.Data.Entity.Infrastructure.DbConnectionInfo("DefaultConnection");
        }
        protected override void Seed(PizzaDbContext context)
        {

        }

    }
}