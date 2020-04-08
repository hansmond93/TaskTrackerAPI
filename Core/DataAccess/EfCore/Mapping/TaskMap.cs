using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DataAccess.EfCore.Mapping
{
    public class TaskMap: IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
            builder.ToTable<Task>(nameof(Task));
            builder.HasKey(k => k.Id);

            SetupData(builder);
        }

        private void SetupData(EntityTypeBuilder<Task> builder)
        {
            var tasks = new List<Task>
            {

                   new Task
                {
                    Id = 2,
                    Description = "Added Project and Task Entities with CRUD operations",
                    UserId = Utils.UserUtils.AdministratorAccountId,
                    CreatorUserId = Utils.UserUtils.AdministratorAccountId,
                    Hours = 4,
                    Minutes = 30,
                    ProjectId = 2
                },
                  new Task
                {
                    Id = 3,
                    Description = "Added Authentication Service to the Application",
                    UserId = Utils.UserUtils.AdministratorAccountId,
                    CreatorUserId = Utils.UserUtils.AdministratorAccountId,
                    Hours = 2,
                    Minutes = 45,
                    ProjectId = 3
                },
                  new Task
                {
                    Id = 4,
                    Description = "Added Upload Seed User with Excel Manager",
                    UserId = Utils.UserUtils.AdministratorAccountId,
                    CreatorUserId = Utils.UserUtils.AdministratorAccountId,
                    Hours = 3,
                    Minutes = 0,
                    ProjectId = 3
                },
               new Task
                {
                    Id = 5,
                    Description = "Password Reset Feature",
                    UserId = Utils.UserUtils.AdministratorAccountId,
                    CreatorUserId = Utils.UserUtils.AdministratorAccountId,
                    Hours = 2,
                    Minutes = 25,
                    ProjectId = 2
                }
            };

            builder.HasData(tasks);
        }
    }
}
