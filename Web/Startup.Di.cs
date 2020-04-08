using Application.Interfaces;
using Application.Services;
using AspNetCore.Totp;
using AspNetCore.Totp.Interface;
using Core.AspNetCore;
using Core.DataAccess.EfCore;
using Core.DataAccess.EfCore.Context;
using Core.DataAccess.UnitOfWork;
using Core.FileStorage;
using Core.Messaging.Email;
using Core.Permissions;
using Core.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace Web
{
    public partial class Startup
    {
        public void AddAppInsight(IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationInsightsTelemetry(configuration);
        }

        public void ConfigureDIService(IServiceCollection services)
        {

            services.AddTransient<DbContext, ApplicationDbContext>();

            services.AddScoped<IUnitOfWork, EfCoreUnitOfWork>();
            services.AddScoped(typeof(IDbContextProvider<>), typeof(UnitOfWorkDbContextProvider<>));

            services.RegisterGenericRepos(typeof(ApplicationDbContext));

            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

            services.AddScoped<IHttpUserService, HttpUserService>();
            services.AddHttpContextAccessor();

            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Path.Combine(
                          HostingEnvironment.ContentRootPath, Configuration.GetValue<string>("StoragePath"))));

            services.AddDataProtection();

            //services.AddScoped<BudgetExcelService, BudgetExcelService>();

            services.AddScoped<IProjectService, ProjectService>();
            
            services.AddTransient<ITotpGenerator, TotpGenerator>();

            services.AddSingleton<ITotpService, TotpService>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<ITaskService, TaskService>();

            services.AddScoped<IFileStorageService, FileStorageService>();

           
            services.AddTransient<IMailService, SmtpEmailService>();

            services.AddSingleton<IHostedService, ReportService>();

        }
    }
}