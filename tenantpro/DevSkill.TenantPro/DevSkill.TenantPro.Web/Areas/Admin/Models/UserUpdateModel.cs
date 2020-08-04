using Autofac;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace DevSkill.TenantPro.Web.Areas.Admin.Models
{
    public class UserUpdateModel : BaseModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserUpdateModel()
        {
            _userManager = Startup.AutofacContainer.Resolve<UserManager<IdentityUser>>();
            _roleManager = Startup.AutofacContainer.Resolve<RoleManager<IdentityRole>>();
        }

        public UserUpdateModel(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public string Id { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Role { get; set; }

        public List<SelectListItem> Roles { get; set; }

        public string ReturnUrl { get; set; }
        public IList<string> ErrorMessage { get; set; }

        public async Task<IdentityResult> AddUser()
        {
            try
            {
                var user = new IdentityUser
                {
                    UserName = this.Email,
                    Email = this.Email,
                    PhoneNumber = this.PhoneNumber
                };
                var result = await _userManager.CreateAsync(user, this.Password);
                if (result.Succeeded)
                {
                    var role = await _roleManager.FindByIdAsync(this.Role);
                    var roleResult = await _userManager.AddToRoleAsync(user, role.Name);
                    if (roleResult.Succeeded)
                    {
                        Notification = new NotificationModel("Success !!", "Successfully Added User", NotificationModel.NotificationType.Success);
                        return result;
                    }
                }
            }
            catch (Exception e)
            {
                Notification = new NotificationModel("Failed !!", "Failed to Add User", NotificationModel.NotificationType.Fail);
                throw e;
            }
            Notification = new NotificationModel("Failed !!", "Already registered", NotificationModel.NotificationType.Fail);
            return null;
        }
        public async Task EditUser()
        {
            try
            {
                var user = await _userManager.FindByIdAsync(Id);
                user.Email = this.Email;
                user.UserName = this.Email;
                user.PhoneNumber = this.PhoneNumber;
                IdentityResult result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    var roles =await _userManager.GetRolesAsync(user);
                    await _userManager.RemoveFromRolesAsync(user, roles);
                    var roleResult = await _userManager.AddToRoleAsync(user, Role);
                    if (roleResult.Succeeded)
                    {
                        Notification = new NotificationModel("Success !!", "Successfully Edited User", NotificationModel.NotificationType.Success);
                        
                    }
                }
            }
            catch (Exception e)
            {
                Notification = new NotificationModel("Failed !!", "Failed to Add User", NotificationModel.NotificationType.Fail);
                throw e;
            }
           // Notification = new NotificationModel("Failed !!", "Already registered", NotificationModel.NotificationType.Fail);
           
        }
        public void LoadRoles()
        {
            Roles = (from r in _roleManager.Roles
                     select new SelectListItem
                     {
                         Text = r.Name,
                         Value = r.Id
                     }).ToList();
        }
        public async Task Load(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = _roleManager.Roles.ToList();
            foreach(var role in roles)
            {
                if(await _userManager.IsInRoleAsync(user,role.Name))
                { 
                    Role = role.Name;
                }
            }
            if (user != null)
            {
                Id = user.Id;
                Email = user.Email;
                PhoneNumber = user.PhoneNumber;
                
            }
        }
    }
}
