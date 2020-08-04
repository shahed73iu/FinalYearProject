using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DevSkill.TenantPro.Web.Areas.Admin.Models;
using DevSkill.TenantPro.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace DevSkill.TenantPro.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            var model = new UserViewModel();
            return View(model);
        }

        public IActionResult GetUsers()
        {
            var tableModel = new DataTablesAjaxRequestModel(Request);
            var model = new UserViewModel();
            var data = model.GetUsers(tableModel);
            return Json(data);
        }

        [HttpGet]
        public IActionResult Add(string returnUrl = null)
        {
            var model = new UserUpdateModel();
            model.ReturnUrl = returnUrl;
            model.LoadRoles();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(UserUpdateModel model)
        {
            model.ReturnUrl = model.ReturnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                await model.AddUser();
                ViewBag.Message = "Success";
            }
            else
            {
               ViewBag.Message = "Invalid login attempt,wrong confirmation password";
            }
            model.LoadRoles();
            return View(model);
        }
        public async Task<IActionResult> Edit(string id)
        {
            var model = new UserUpdateModel();
            await model.Load(id);
            model.LoadRoles();
          //  await model.Load(id);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserUpdateModel model)
        {  
            await model.EditUser();
            model.LoadRoles();
            return View(model);
        }
        public async Task<IActionResult> Delete(string id)
        {
            var model = new UserViewModel();
            await model.Delete(id);
            return LocalRedirect("~/admin/User/Index");
        }
    }
}