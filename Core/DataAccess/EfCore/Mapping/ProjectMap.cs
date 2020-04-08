using Core.ViewModel.Enums;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DataAccess.EfCore.Mapping
{
    public class ProjectMap: IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ToTable<Project>(nameof(Project));
            builder.HasKey(k => k.Id);

            SetupData(builder);
        }

        private void SetupData(EntityTypeBuilder<Project> builder)
        {
            var projects = new List<Project>
            {
                new Project
                {
                    Id = 2,
                    Name = "Learning Management System",
                    Code = "LMS-AFP001",
                    Description = "blah2!, blah2!!, blah2!!!",
                    IsRecurring = false,
                    Status = ProjectState.Active,
                    CreatorUserId = Utils.UserUtils.AdministratorAccountId
                    
                },
                new Project
                {
                    Id = 3,
                    Name = "Time Tracker",
                    Code = "TT-IH001",
                    Description = "blah3!, blah3!!, blah3!!!",
                    IsRecurring = true,
                    Status = ProjectState.Active,
                    CreatorUserId = Utils.UserUtils.AdministratorAccountId
                },
                new Project
                {
                    Id = 4,
                    Name = "Work Tracker",
                    Code = "TT-WT001",
                    Description = "Hey!!!3!, Hey!!!3!!, Hey3!!!",
                    Status = ProjectState.Closed,
                    CreatorUserId = Utils.UserUtils.AdministratorAccountId
                }
            };

            builder.HasData(projects);
        }
    }
}
