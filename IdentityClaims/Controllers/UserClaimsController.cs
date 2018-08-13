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
    [Authorize (Roles = "Admin")]
    public class UserClaimsController : Controller
    {
        private readonly ApplicationDbContext _appcontext;

        private readonly IdentityClaimsContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        public UserClaimsController(
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
            List<UserClaimsViewModel> model = new List<UserClaimsViewModel>();
            model = _userManager.Users.Select(u => new UserClaimsViewModel
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email
            }).OrderBy(u => u.UserName).ToList();

            return View(model);
        }

        // GET: Elections/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(string Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            ApplicationUser user = new ApplicationUser();
            UserClaimsViewModel model = new UserClaimsViewModel();

            user = await _userManager.FindByIdAsync(Id);
            var userClaims = await _userManager.GetClaimsAsync(user);

            var allClaims = _context.LookupValues.Where(lv => lv.ActiveFlag == GlobalData.gY).Join(_context.Lookups.Where(lu => lu.ActiveFlag == GlobalData.gY && lu.LookupCode == GlobalData.gClaims), lv => lv.LookupID, lu => lu.LookupID, (lv, lu) => lv).ToList();

            var sortedClaims = allClaims.OrderBy(lv => lv.Attribute1);

            Boolean claimExists = false;
            foreach (var sortedClaim in sortedClaims)
            {
                claimExists = false;
                foreach (var existingClaim in userClaims)
                {
                    if (sortedClaim.Attribute1 == existingClaim.Value)
                    {
                        claimExists = true;
                        break;
                    }
                }

                var claimDefinition = new ClaimDefinitionViewModel
                {
                    Id = user.Id,
                    Name = sortedClaim.Attribute1,
                    Type = sortedClaim.Attribute3,
                    Checked = claimExists
                };
                model.ClaimsList.Add(claimDefinition);
            }

            model.Id = user.Id;
            model.UserName = user.UserName;
            model.Email = user.Email;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserClaimsViewModel vm)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                user = await _userManager.FindByIdAsync(vm.Id);

                Boolean claimExists = false;

                foreach (var claim in vm.ClaimsList)
                {
                    claimExists = false;

                    var userClaims = await _userManager.GetClaimsAsync(user);

                    foreach (var userClaim in userClaims)
                    {
                        if (claim.Name == userClaim.Value)
                        {
                            claimExists = true;
                            break;
                        }
                    }

                    if ((claim.Checked) && (!claimExists))
                    {
                        //add claim
                        await _userManager.AddClaimAsync(user, new Claim(claim.Type, claim.Name));
                    }
                    else if ((!claim.Checked) && (claimExists))
                    {
                        //Remove Claim
                        var claimToRemove = userClaims.SingleOrDefault(c => c.Value == claim.Name);
                        await _userManager.RemoveClaimAsync(user, claimToRemove);
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }
    }
}