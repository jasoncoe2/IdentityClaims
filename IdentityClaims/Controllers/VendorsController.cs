using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityClaims.Controllers
{
    [Authorize(Roles = "Admin,MasterData,User")]
    public class VendorsController : Controller
    {
        [Authorize(Policy = "MasterData-Vendors-View")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "MasterData-Vendors-Manage")]
        public IActionResult Edit()
        {
            ViewData["Message"] = "This is the Edit Vendor View.";
            return View();
        }

        [Authorize(Policy = "MasterData-Vendors-View")]
        public IActionResult Details()
        {
            ViewData["Message"] = "This is the View Vendor View.";
            return View();
        }

        [Authorize(Policy = "MasterData-Vendors-Manage")]
        public IActionResult Create()
        {
            ViewData["Message"] = "This is the Create Vendor View.";
            return View();
        }

        [Authorize(Policy = "MasterData-Vendors-Delete")]
        public IActionResult Delete()
        {
            ViewData["Message"] = "This is the Delete Vendor View.";
            return View();
        }
    }
}