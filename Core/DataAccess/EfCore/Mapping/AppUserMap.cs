using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Timing;
using Core.Utils;
using Entities;
using System.Collections.Generic;

namespace Core.DataAccess.EfCore.Mapping
{
    public class AppUserMap : IEntityTypeConfiguration<User>
    {
        public PasswordHasher<User> Hasher { get; set; } = new PasswordHasher<User>();

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(User));

            builder.Property(b => b.FirstName).HasMaxLength(150);
            builder.Property(b => b.LastName).HasMaxLength(150);
            builder.Property(b => b.MiddleName).HasMaxLength(150);
            builder.Property(b => b.Designation).HasMaxLength(150);
            builder.Property(b => b.Designation).HasMaxLength(150);
            builder.Property(b => b.Birthday).HasMaxLength(150);
            builder.Property(b => b.EmployeeID).HasMaxLength(150);

            SetupAdmin(builder);
        }

        private void SetupAdmin(EntityTypeBuilder<User> builder)
        {
            var admin = new User
            {
                CreationTime = Clock.Now,
                FirstName = "SBSC",
                LastName = "Admin",
                Id = UserUtils.AdministratorAccountId,
                LastLoginDate = Clock.Now,
                Email = UserUtils.AdministratorAccount,
                EmailConfirmed = true,
                NormalizedEmail = UserUtils.AdministratorAccount.ToUpper(),
                UserName = UserUtils.AdministratorAccount,
                NormalizedUserName = UserUtils.AdministratorAccount.ToUpper(),
                PasswordHash = Hasher.HashPassword(null, "micr0s0ft_"),
                SecurityStamp = "99ae0c45-d682-4542-9ba7-1281e471916b",
            };

            var users = new List<User>() { admin };

            builder.HasData(users);
        }
    }
}