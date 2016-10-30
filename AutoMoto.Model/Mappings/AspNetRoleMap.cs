using System.Data.Entity.ModelConfiguration;
using AutoMoto.Model.Models;

namespace AutoMoto.Model.Mappings
{
    public class AspNetRoleMap : EntityTypeConfiguration<AspNetRole>
    {
        public AspNetRoleMap()
        {
            HasKey(t => t.Id);

            Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(128);

            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(256);

            ToTable("AspNetRoles");
            Property(t => t.Id).HasColumnName("Id");
            Property(t => t.Name).HasColumnName("Name");

            HasMany(t => t.AspNetUsers)
                .WithMany(t => t.AspNetRoles)
                .Map(m =>
                {
                    m.ToTable("AspNetUserRoles");
                    m.MapLeftKey("RoleId");
                    m.MapRightKey("UserId");
                });


        }
    }
}
