using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityClaims.Controllers
{
    [Authorize(Roles = "Admin,MasterData,User")]
    public class CustomersController : Controller
    {
        [Authorize (Policy = "MasterData-Customers-View")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "MasterData-Customers-Manage")]
        public IActionResult Edit()
        {
            ViewData["Message"] = "This is the Edit Customer View.";
            return View();
        }

        [Authorize(Policy = "MasterData-Customers-View")]
        public IActionResult Details()
        {
            ViewData["Message"] = "This is the View Customer View.";
            return View();
        }

        [Authorize(Policy = "MasterData-Customers-Manage")]
        public IActionResult Create()
        {
            ViewData["Message"] = "This is the Create Customer View.";
            return View();
        }

        [Authorize(Policy = "MasterData-Customers-Delete")]
        public IActionResult Delete()
        {
            ViewData["Message"] = "This is the Delete Customer View.";
            return View();
        }
    }
}