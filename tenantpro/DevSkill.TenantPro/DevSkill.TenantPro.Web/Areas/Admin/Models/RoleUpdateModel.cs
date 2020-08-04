using Autofac;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevSkill.TenantPro.Web.Areas.Admin.Models
{
    public class RoleUpdateModel : BaseModel
    {
        public string RoleName { get; set; }
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleUpdateModel()
        {
            _roleManager = Startup.AutofacContainer.Resolve<RoleManager<IdentityRole>>();
        }
        public async Task AddNewRole()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(RoleName)) 
                {
                    throw new NullReferenceException();
                }
                await _roleManager.CreateAsync(new IdentityRole() { Name = RoleName });
                Notification = new NotificationModel("Success", "Successfully Added Role", NotificationModel.NotificationType.Success);
            }
            catch (Exception)
            {
                Notification = new NotificationModel("Failed", "Failed to Add Role", NotificationModel.NotificationType.Fail);
            }

        }
    }
}
