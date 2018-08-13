using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityClaims.Controllers
{
    [Authorize(Roles = "Admin,MasterData,User")]
    public class CustomerPricingController : Controller
    {
        [Authorize (Policy = "User-CustomerPricingReport-View")]
        public IActionResult Index()
        {
            return View();
        }
    }
}