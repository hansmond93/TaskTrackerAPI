using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Utils;
using Entities;
using System.Collections.Generic;

namespace Core.DataAccess.EfCore.Mapping
{
    public class AppUserRoleMap : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable(nameof(UserRole));

            builder.HasKey(p => new { p.UserId, p.RoleId });
            SetupData(builder);
        }

        private void SetupData(EntityTypeBuilder<UserRole> builder)
        {
            List<UserRole> dataList = new List<UserRole>()
            {
                new UserRole()
                {
                    UserId =UserUtils.AdministratorAccountId,
                    RoleId = 2,
                }
            };

            builder.HasData(dataList);
        }
    }
}