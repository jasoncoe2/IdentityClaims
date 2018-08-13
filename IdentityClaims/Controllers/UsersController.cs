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
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ApplicationDbContext _appcontext;

        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext appcontext,
            RoleManager<IdentityRole> roleManager
            )
        {
            _userManager = userManager;
            _appcontext = appcontext;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            List<UsersViewModel> model = new List<UsersViewModel>();
            model = _userManager.Users.Select(u => new UsersViewModel
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email
            }).OrderBy(u => u.UserName).ToList();

            return View(model);
        }

        // GET: Users/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsersViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, EmailConfirmed = model.EmailConfirmed, LockoutEnd = model.LockoutEnd, LockoutEnabled = true };
                await _userManager.CreateAsync(user, model.Password);
                await _appcontext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.UserName = model.UserName;
            ViewBag.Email = model.Email;
            ViewBag.LockoutEnd = model.LockoutEnd;
            return View(model);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }


            ApplicationUser model = new ApplicationUser();
            model = await _userManager.FindByIdAsync(id);

            UsersViewModel uvm = new UsersViewModel();
            uvm.Id = model.Id;
            uvm.UserName = model.UserName;
            uvm.Email = model.Email;
            uvm.LockoutEnabled = model.LockoutEnabled;
            uvm.LockoutEnd = model.LockoutEnd;
            uvm.EmailConfirmed = model.EmailConfirmed;
            return View(uvm);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ApplicationUser model = new ApplicationUser();
            model = await _userManager.FindByIdAsync(id);
            UsersEditViewModel uvm = new UsersEditViewModel();
            uvm.Id = model.Id;
            uvm.UserName = model.UserName;
            uvm.Email = model.Email;
            uvm.LockoutEnabled = model.LockoutEnabled;
            uvm.LockoutEnd = model.LockoutEnd;
            uvm.EmailConfirmed = model.EmailConfirmed;
            return View(uvm);
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UsersEditViewModel uvm)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(uvm.Id);

                if (user == null)
                {
                    return NotFound();
                }

                user.UserName = uvm.UserName;
                user.Email = uvm.Email;
                user.LockoutEnabled = uvm.LockoutEnabled;
                user.LockoutEnd = uvm.LockoutEnd;
                user.EmailConfirmed = uvm.EmailConfirmed;

                if ((user.LockoutEnabled) && (user.LockoutEnd == null))
                {
                    var unlockResult = await _userManager.SetLockoutEndDateAsync(user, null);
                    if (unlockResult.Succeeded)
                    {
                        await _userManager.ResetAccessFailedCountAsync(user);
                    }
                }

                if ((user.LockoutEnd != null) && (user.LockoutEnabled))
                {
                    var lockResult = await _userManager.SetLockoutEndDateAsync(user, user.LockoutEnd);
                }

                await _userManager.UpdateAsync(user);
                await _appcontext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                
            }
            return View(uvm);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            ApplicationUser model = new ApplicationUser();
            model = await _userManager.FindByIdAsync(Id);
            UsersEditViewModel uvm = new UsersEditViewModel();
            uvm.Id = model.Id;
            uvm.UserName = model.UserName;
            uvm.Email = model.Email;

            return View(uvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string Id)
        {
            ApplicationUser model = new ApplicationUser();
            model = await _userManager.FindByIdAsync(Id);

            var roles = _roleManager.Roles.ToList();

            var userClaims = await _userManager.GetClaimsAsync(model);

            await _userManager.RemoveClaimsAsync(model, userClaims);

            foreach (IdentityRole role in roles)
            {
                if (await _userManager.IsInRoleAsync(model, role.Name))
                {
                    await _userManager.RemoveFromRoleAsync(model, role.Name);
                }
            }

            _appcontext.SaveChanges();
            var result = await _userManager.DeleteAsync(model);

            if (result.Succeeded)
            {
                _appcontext.SaveChanges();
                return RedirectToAction("Index");
            }

            UsersEditViewModel uvm = new UsersEditViewModel();
            uvm.Id = model.Id;
            uvm.UserName = model.UserName;
            uvm.Email = model.Email;

            return View(uvm);
        }
    }
}