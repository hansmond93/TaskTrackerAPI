using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities;

namespace Core.DataAccess.EfCore.Mapping
{
    public class AppUserTokenMap : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> builder)
        {
            builder.ToTable(nameof(UserToken));
            builder.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
        }
    }
}