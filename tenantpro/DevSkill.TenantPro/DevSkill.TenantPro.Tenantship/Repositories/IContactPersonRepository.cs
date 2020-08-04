using DevSkill.Data;
using DevSkill.TenantPro.Tenantship.Contexts;
using DevSkill.TenantPro.Tenantship.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevSkill.TenantPro.Tenantship.Repositories
{
    public interface IContactPersonRepository:IRepository<ContactPerson,int , TenantContext>
    {
    }
}
