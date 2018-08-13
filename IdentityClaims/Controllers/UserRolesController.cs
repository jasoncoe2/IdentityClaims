using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public class UserRolesController : Controller
    {
        private readonly ApplicationDbContext _appcontext;

        private readonly IdentityClaimsContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        public UserRolesController(
            ApplicationDbContext appcontext,
            IdentityClaimsContext context,
            UserManager<ApplicationUser> userManager)
        {
            _appcontext = appcontext;
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            List<UserRolesViewModel> model = new List<UserRolesViewModel>();
            model = _userManager.Users.Select(u => new UserRolesViewModel
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email
            }).OrderBy(u => u.UserName).ToList();

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(string Id)
        {
            ApplicationUser user = new ApplicationUser();
            UserRolesViewModel model = new UserRolesViewModel();
            user = await _userManager.FindByIdAsync(Id);
            //Get All Roles
            var roles = _appcontext.Roles.OrderBy(r => r.Name);

            foreach (var role in roles)
            {
                var allRoles = new RoleDefinitionViewModel
                {
                    Name = role.Name,
                    Id = role.Id,
                    Checked = await _userManager.IsInRoleAsync(user, role.Name)
                };
                model.RolesList.Add(allRoles);
            }
            model.Id = user.Id;
            model.UserName = user.UserName;
            model.Email = user.Email;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserRolesViewModel urvm)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                user = await _userManager.FindByIdAsync(urvm.Id);

                var userClaims = await _userManager.GetClaimsAsync(user);

                foreach (var role in urvm.RolesList)
                {
                    var assignedToRole = await _userManager.IsInRoleAsync(user, role.Name);
 
                    if ((assignedToRole) && (!role.Checked))
                    {
                        //Remove Claims for Role
                        var claimsForRole = userClaims.Where(c => c.Type == role.Name);
                        foreach (var userClaim in claimsForRole)
                        {
                            await _userManager.RemoveClaimAsync(user, userClaim);
                        }

                        var removeResult = await _userManager.RemoveFromRoleAsync(user, role.Name);
                    }
                    else if ((!assignedToRole) && (role.Checked))
                    {
                        //Remove Claims for Role
                        //Since there is a form to add claims manually, I always remove all from the role before adding them.
                        var claimsForRole = userClaims.Where(c => c.Type == role.Name);
 
                        foreach (var claimToRemove in claimsForRole)
                        {
                            await _userManager.RemoveClaimAsync(user, claimToRemove);
                        }
                        //Add Claims for Role
                        //The role Name is in Attribute3 of My Lookup Values Table.
                        var allClaimsInRole = _context.LookupValues.Where(lv => lv.ActiveFlag == GlobalData.gY && lv.Attribute3 == role.Name).Join(_context.Lookups.Where(lu => lu.ActiveFlag == GlobalData.gY && lu.LookupCode == GlobalData.gClaims), lv => lv.LookupID, lu => lu.LookupID, (lv, lu) => lv).ToList();

                        //I use the Role Name as the Type for the Claim. In this example, Attribute3.
                        foreach (var claimInRole in allClaimsInRole)
                        {
                            await _userManager.AddClaimAsync(user, new Claim(claimInRole.Attribute3, claimInRole.Attribute1));
                        }

                        var addResult = await _userManager.AddToRoleAsync(user, role.Name);
                    }
                }
                _appcontext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(urvm);
        }
    }
}