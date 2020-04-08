using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public abstract class ScopedServicesResolver : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        protected readonly IConfiguration _configuration;

        public ScopedServicesResolver(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration) : base()
        {
            _serviceScopeFactory = serviceScopeFactory;
            _configuration = configuration;
        }

        protected override async Task Process()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                await ProcessInScope(scope.ServiceProvider);
            }
        }

        public abstract Task ProcessInScope(IServiceProvider serviceProvider);
    }
}
