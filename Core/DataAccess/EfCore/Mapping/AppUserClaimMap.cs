using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Permissions;
using Core.Utils;
using Entities;
using System.Collections.Generic;
using System.Linq;

namespace Core.DataAccess.EfCore.Mapping
{
    public class AppUserClaimMap : IEntityTypeConfiguration<UserClaim>
    {
        private static int counter = 1;

        public void Configure(EntityTypeBuilder<UserClaim> builder)
        {
            builder.ToTable(nameof(UserClaim));
            builder.HasData(SystemPermissionData());
        }

        private static IEnumerable<UserClaim> SystemPermissionData()
        {

            var systemClaims = new List<UserClaim>();

            var adminPermissions = PermisionProvider.GetSystemDefaultRoles()
                .Where(x => x.Key == RoleHelper.ADMINISTRATOR)
                .SelectMany(x => x.Value);


            foreach (var item in adminPermissions)
            {
                systemClaims.Add(
                    new UserClaim { Id = counter++, ClaimType = nameof(Permission), ClaimValue = item.ToString(), UserId = UserUtils.AdministratorAccountId });
            }
            

            return systemClaims;
        }
    }
}