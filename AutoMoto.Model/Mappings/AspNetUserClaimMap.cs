using System.Data.Entity.ModelConfiguration;
using AutoMoto.Model.Models;

namespace AutoMoto.Model.Mappings
{
    public class AspNetUserClaimMap : EntityTypeConfiguration<AspNetUserClaim>
    {
        public AspNetUserClaimMap()
        {
            HasKey(t => t.Id);


            Property(t => t.UserId)
                .IsRequired()
                .HasMaxLength(128);


            ToTable("AspNetUserClaims");
            Property(t => t.Id).HasColumnName("Id");
            Property(t => t.UserId).HasColumnName("UserId");
            Property(t => t.ClaimType).HasColumnName("ClaimType");
            Property(t => t.ClaimValue).HasColumnName("ClaimValue");

            HasRequired(t => t.AspNetUser)
                .WithMany(t => t.AspNetUserClaims)
                .HasForeignKey(d => d.UserId).WillCascadeOnDelete();
        }
    }
}
