using Autofac;
using DevSkill.DocumentPro.Documentship.Services;
using DevSkill.TenantPro.Tenantship.Contexts;
using DevSkill.TenantPro.Tenantship.Repositories;
using DevSkill.TenantPro.Tenantship.Services;
using DevSkill.TenantPro.Tenantship.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevSkill.TenantPro.Tenantship
{
    public class TenantModule: Module
    {
        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;

        public TenantModule(string connectionStringName, string migrationAssemblyName)
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

            //builder.RegisterType<TenantContext>().As<ITenantContext>()
            //       .WithParameter("connectionString", _connectionString)
            //       .WithParameter("migrationAssemblyName", _migrationAssemblyName)
            //       .InstancePerLifetimeScope();

            builder.RegisterType<TenantUnitOfWork>().As<ITenantUnitOfWork>()
                   //.WithParameter("connectionString", _connectionString)
                   //.WithParameter("migrationAssemblyName", _migrationAssemblyName)
                   .InstancePerLifetimeScope();

            builder.RegisterType<TenantRepository>().As<ITenantRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<DocumentRepository>().As<IDocumentRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ContactPersonRepository>().As<IContactPersonRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TenantService>().As<ITenantService>()
               .InstancePerLifetimeScope();

            builder.RegisterType<DocumentService>().As<IDocumentService>()
              .InstancePerLifetimeScope();

            builder.RegisterType<PaymentRepository>().As<IPaymentRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<PrintingRepository>().As<IPrintingRepository>()
                .InstancePerLifetimeScope();


            base.Load(builder);
        }
    }
}
