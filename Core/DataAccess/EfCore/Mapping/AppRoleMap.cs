using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Utils;
using Entities;
using System.Collections.Generic;

namespace Core.DataAccess.EfCore.Mapping
{
    public class AppRoleMap : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable(nameof(Role));

            SetupData(builder);
        }

        private void SetupData(EntityTypeBuilder<Role> builder)
        {
            var roles = new List<Role>
            {
                   new Role
                {
                    Id = 2,
                    Name = "ADMINISTRATOR",
                    NormalizedName = "ADMINISTRATOR"
                }
            };

            builder.HasData(roles);
        }
    }
}