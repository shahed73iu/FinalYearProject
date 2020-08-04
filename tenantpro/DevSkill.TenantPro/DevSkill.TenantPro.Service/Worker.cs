using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using DevSkill.TenantPro.Service.EmailService;
using DevSkill.TenantPro.Tenantship.Contexts;
using DevSkill.TenantPro.Tenantship.Repositories;
using DevSkill.TenantPro.Tenantship.Services;
using DevSkill.TenantPro.Tenantship.UnitOfWorks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DevSkill.TenantPro.Service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        public IConfiguration Configuration { get; }

        public static IContainer AutofacContainer;
        public string ConnectionString { get; set; }
        public string MigrationAssemblyName { get; set; }
        public Worker(ILogger<Worker> logger, IConfiguration config)
        {
            _logger = logger;
            Configuration = config;
            ConnectionString = Configuration.GetConnectionString("DefaultConnection");
            MigrationAssemblyName = typeof(Worker).Assembly.FullName;
            var builder = new ContainerBuilder();
            builder.RegisterModule(new WorkerServiceModule(ConnectionString, MigrationAssemblyName));
            AutofacContainer = builder.Build();
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            { 
                var sendmail = new EmailSendingOperation(Configuration);
                sendmail.SendEmail();
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(86400000, stoppingToken);
            }
        }
    }
}
