using Autofac;
using DevSkill.TenantPro.Web.Data;
using DevSkill.TenantPro.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevSkill.TenantPro.Web.Areas.Admin.Models
{
    public class UserViewModel:BaseModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        //private readonly UserManager<IdentityUserRole<string>> _userManager2;
        private readonly ApplicationDbContext _db;
        public UserViewModel() 
        {
            _roleManager = Startup.AutofacContainer.Resolve<RoleManager<IdentityRole>>();
            _userManager = Startup.AutofacContainer.Resolve<UserManager<IdentityUser>>();
            //_userManager2 = Startup.AutofacContainer.Resolve<UserManager<IdentityUserRole<string>>>();
            _db = Startup.AutofacContainer.Resolve<ApplicationDbContext>();
        }

        public object GetUsers(DataTablesAjaxRequestModel tableModel)
        {
            int total = 0;
            int totalFiltered = 0;
            var start = (tableModel.PageIndex - 1) * tableModel.PageSize;
            IEnumerable<IdentityUser> records = null;

            if (string.IsNullOrWhiteSpace(tableModel.SearchText))
                records = _userManager.Users.Skip(start).Take(tableModel.PageSize);
            else
                records = _userManager.Users.Where(x => x.Email.Contains(tableModel.SearchText));
           

            total = _userManager.Users.AsQueryable().Count();
            totalFiltered = _userManager.Users.AsQueryable().Where(x => x.Email.Contains(tableModel.SearchText)).Count();
            
            
            var userRole = _db.UserRoles.ToList();
            var roles = _roleManager.Roles.ToList();
            
            
            return new
            {
                recordsTotal = total,
                recordsFiltered = totalFiltered,
                data = (from record in records
                        select new string[]
                        {
                            record.Id.ToString(),
                            
                            roles.Where(x => userRole.Any(y => y.RoleId == x.Id && y.UserId == record.Id)).Select(z => z.Name).FirstOrDefault(),

                            record.UserName,
                            record.Email,
                            record.PhoneNumber,
                            record.Id.ToString()
                        }
                    ).ToArray()
            };
        }

        public async Task Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _userManager.DeleteAsync(user);
        }
    }
}
