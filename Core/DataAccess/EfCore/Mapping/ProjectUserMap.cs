using Core.Utils;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DataAccess.EfCore.Mapping
{
    public class ProjectUserMap : IEntityTypeConfiguration<ProjectUser>
    {
        public void Configure(EntityTypeBuilder<ProjectUser> builder)
        {
            builder.ToTable<ProjectUser>(nameof(ProjectUser));
            builder.HasKey(pu => new { pu.UserId, pu.ProjectId});

            SetupData(builder);
        }

        private void SetupData(EntityTypeBuilder<ProjectUser> builder)
        {
            var projectUser = new List<ProjectUser>
            {

                new ProjectUser
                {
                    UserId = UserUtils.AdministratorAccountId,
                    ProjectId = 2
                },
                new ProjectUser
                {
                    UserId = UserUtils.AdministratorAccountId,
                    ProjectId = 3
                }
            };

            builder.HasData(projectUser);
        }
    }
}
