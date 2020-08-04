using Autofac;
using DevSkill.TenantPro.Billing.Contexts;
using DevSkill.TenantPro.Billing.Repositories;
using DevSkill.TenantPro.Billing.Services;
using DevSkill.TenantPro.Billing.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevSkill.TenantPro.Billing
{
    public class BillModule:Module
    {
        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;

        public BillModule(string connectionStringName, string migrationAssemblyName)
        {

            _connectionString = connectionStringName;
            _migrationAssemblyName = migrationAssemblyName;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BillingContext>()
                   .WithParameter("connectionString", _connectionString)
                   .WithParameter("migrationAssemblyName", _migrationAssemblyName)
                   .InstancePerLifetimeScope();

            //builder.RegisterType<BillingContext>().As<IBillingContext>()
            //       .WithParameter("connectionString", _connectionString)
            //       .WithParameter("migrationAssemblyName", _migrationAssemblyName)
            //       .InstancePerLifetimeScope();

            builder.RegisterType<BillUnitOfWork>().As<IBillUnitOfWork>()
                   //.WithParameter("connectionString", _connectionString)
                   //.WithParameter("migrationAssemblyName", _migrationAssemblyName)
                   .InstancePerLifetimeScope();

            builder.RegisterType<BillRepository>().As<IBillRepository>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<ReadingRepository>().As<IReadingRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ReadingService>().As<IReadingService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<BillService>().As<IBillService>()
                .InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
