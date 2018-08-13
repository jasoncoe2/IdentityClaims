using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityClaims.Data;
using IdentityClaims.Models;
using IdentityClaims.Models.UsersViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityClaims.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly ApplicationDbContext _appcontext;

        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(
            ApplicationDbContext appcontext,
            RoleManager<IdentityRole> roleManager)
        {
            _appcontext = appcontext;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            List<RolesListViewModel> model = new List<RolesListViewModel>();

            model = _appcontext.Roles.Select(r => new RolesListViewModel
            {
                Id = r.Id,
                RoleName = r.Name
            }).OrderBy(r => r.RoleName).ToList();

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RolesListViewModel rlvm)
        {
            if (ModelState.IsValid)
            {
                //Check for Duplicate
                var role = _appcontext.Roles.FirstOrDefault(r => r.NormalizedName == rlvm.RoleName.ToUpper());

                if(role != null)
                {
                    ModelState.AddModelError("RoleName", GlobalData.gDuplicateRecord);
                    return View(rlvm);
                }

                //Add Role
                _appcontext.Roles.Add(new IdentityRole()
                {
                    Name = rlvm.RoleName,
                    NormalizedName = rlvm.RoleName.ToUpper()
                });
                await _appcontext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(rlvm);
        }

        [HttpGet]
        public IActionResult Edit(string Id)
        {

            if(Id == null)
            {
                return NotFound();
            }

            //Get Current Role
            var currentRole = _appcontext.Roles.FirstOrDefault(r => r.Id == Id);

            RolesListViewModel model = new RolesListViewModel();
            RoleViewModel rvm = new RoleViewModel()
            {
                Id = currentRole.Id,
                RoleName = currentRole.Name
            };

            model.Id = currentRole.Id;
            model.RoleName = currentRole.Name;
            model.RolesList.Add(rvm);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RolesListViewModel rlvm)
        {
            if(ModelState.IsValid)
            {
                var currentRole = _appcontext.Roles.FirstOrDefault(r => r.Id == rlvm.Id);

                //Check for Duplicate
                var role = _appcontext.Roles.FirstOrDefault(r => r.Name.ToUpper() == rlvm.RoleName.ToUpper() && r.Id != rlvm.Id);

                if(role != null)
                {
                    ModelState.AddModelError("RoleName", GlobalData.gDuplicateRecord);
                    return View(rlvm);
                }

                //Update Role
                currentRole.Name = rlvm.RoleName;
                currentRole.NormalizedName = rlvm.RoleName.ToUpper();
                await _roleManager.UpdateAsync(currentRole);
                await _appcontext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}