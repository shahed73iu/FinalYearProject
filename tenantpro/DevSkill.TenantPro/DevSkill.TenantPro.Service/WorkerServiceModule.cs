using Autofac;
using DevSkill.TenantPro.Tenantship.Contexts;
using DevSkill.TenantPro.Tenantship.Repositories;
using DevSkill.TenantPro.Tenantship.Services;
using DevSkill.TenantPro.Tenantship.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevSkill.TenantPro.Service
{
    public class WorkerServiceModule : Module
    {
        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;
       // private readonly IConfiguration _configuration;

        public WorkerServiceModule(string connectionStringName, string migrationAssemblyName)
        {
            _connectionString = connectionStringName;
            _migrationAssemblyName = migrationAssemblyName;
        }
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TenantContext>()
                    .WithParameter("connectionString", _connectionString)
                    .WithParameter("migrationAssemblyName", _migrationAssemblyName)
                    .InstancePerLifetimeScope();

            builder.RegisterType<TenantContext>().As<ITenantContext>()
                   .WithParameter("connectionString", _connectionString)
                   .WithParameter("migrationAssemblyName", _migrationAssemblyName)
                   .InstancePerLifetimeScope();

            builder.RegisterType<TenantUnitOfWork>().As<ITenantUnitOfWork>()
                   .WithParameter("connectionString", _connectionString)
                   .WithParameter("migrationAssemblyName", _migrationAssemblyName)
                   .InstancePerLifetimeScope();

            builder.RegisterType<TenantRepository>().As<ITenantRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TenantService>().As<ITenantService>()
               .InstancePerLifetimeScope();

           // AutofacContainer = builder.Build();

            base.Load(builder);
        }
    }
}
