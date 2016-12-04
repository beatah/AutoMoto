using AutoMoto.Model.Models;
using System.Data.Entity.ModelConfiguration;

namespace AutoMoto.Model.Mappings
{
    public class AspNetUserLoginMap : EntityTypeConfiguration<AspNetUserLogin>
    {
        public AspNetUserLoginMap()
        {

            HasKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId });


            Property(t => t.LoginProvider)
                .IsRequired()
                .HasMaxLength(128);

            Property(t => t.ProviderKey)
                .IsRequired()
                .HasMaxLength(128);

            Property(t => t.UserId)
                .IsRequired()
                .HasMaxLength(128);


            ToTable("AspNetUserLogins");
            Property(t => t.LoginProvider).HasColumnName("LoginProvider");
            Property(t => t.ProviderKey).HasColumnName("ProviderKey");
            Property(t => t.UserId).HasColumnName("UserId");


            HasRequired(t => t.AspNetUser)
                .WithMany(t => t.AspNetUserLogins)
                .HasForeignKey(d => d.UserId).WillCascadeOnDelete();



        }
    }
}
