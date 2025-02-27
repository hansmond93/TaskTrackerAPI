﻿using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;

namespace Core.DataAccess.EfCore.Context
{
    /// <Note>
    /// DbSet properties are being used by generic repository
    /// </Note>
    public class ApplicationDbContext : BaseDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Task> Tasks { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectUser> ProjectUsers { get; set; }





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.UseOpenIddict();
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }

    /// <summary>
    /// Migration only
    /// </summary>
    public class AppDbContextMigrationFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public static readonly IConfigurationRoot ConfigBuilder = new ConfigurationBuilder()
                 .SetBasePath(AppContext.BaseDirectory)
                 .AddJsonFile("appsettings.json", true, true)
                 .AddJsonFile("appsettings.Development.json", false).Build();

        public ApplicationDbContext CreateDbContext(string[] args)
        {
            return new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
                                   .UseSqlServer(ConfigBuilder.GetConnectionString("Default"), b => b.MigrationsAssembly(GetType().Assembly.FullName))
                                   .Options);
        }
    }
}