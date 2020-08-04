using DevSkill.Data;
using DevSkill.TenantPro.Tenantship.Contexts;
using DevSkill.TenantPro.Tenantship.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevSkill.TenantPro.Tenantship.Repositories
{
    public class ContactPersonRepository:Repository<ContactPerson,int,TenantContext>,IContactPersonRepository
    {
        public ContactPersonRepository(TenantContext tenantContext) : base(tenantContext)
        {

        }
    }
}
