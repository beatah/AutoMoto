using AutoMoto.Model.Models;
using System.Data.Entity.ModelConfiguration;

namespace AutoMoto.Model.Mappings
{
    public class AspNetUserMap : EntityTypeConfiguration<AspNetUser>
    {
        public AspNetUserMap()
        {
            HasKey(t => t.Id);


            Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(128);

            Property(t => t.Email)
                .HasMaxLength(256);

            Property(t => t.UserName)
                          .IsRequired()
                          .HasMaxLength(256);


            ToTable("AspNetUsers");
            Property(t => t.Id).HasColumnName("Id");
            Property(t => t.FirstName).HasColumnName("FirstName");
            Property(t => t.LastName).HasColumnName("LastName");

            Property(t => t.Email).HasColumnName("Email");
            Property(t => t.EmailConfirmed).HasColumnName("EmailConfirmed");
            Property(t => t.PasswordHash).HasColumnName("PasswordHash");
            Property(t => t.SecurityStamp).HasColumnName("SecurityStamp");
            Property(t => t.PhoneNumber).HasColumnName("PhoneNumber");
            Property(t => t.PhoneNumberConfirmed).HasColumnName("PhoneNumberConfirmed");
            Property(t => t.TwoFactorEnabled).HasColumnName("TwoFactorEnabled");
            Property(t => t.LockoutEndDateUtc).HasColumnName("LockoutEndDateUtc");
            Property(t => t.LockoutEnabled).HasColumnName("LockoutEnabled");
            Property(t => t.AccessFailedCount).HasColumnName("AccessFailedCount");
            Property(t => t.UserName).HasColumnName("UserName");

            HasMany(u => u.Followers)
    .WithRequired(f => f.Followee)
    .WillCascadeOnDelete(false);

            HasMany(u => u.Followees)
                .WithRequired(f => f.Follower)
                .WillCascadeOnDelete(false);


        }
    }
}
