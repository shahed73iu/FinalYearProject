using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevSkill.TenantPro.Core
{
    public class CoreModule : Module
    {
        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;

        public CoreModule(string connectionStringName, string migrationAssemblyName)
        {

            _connectionString = connectionStringName;
            _migrationAssemblyName = migrationAssemblyName;
        }

        protected override void Load(ContainerBuilder builder)
        {
         
            base.Load(builder);
        }
    }
}
