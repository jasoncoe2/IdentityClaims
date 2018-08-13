using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityClaims.Controllers
{
    [Authorize(Roles = "Admin,MasterData,User")]
    public class PurchaseOrdersController : Controller
    {
        [Authorize(Policy = "User-PurchaseOrders-View")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "User-PurchaseOrders-Manage")]
        public IActionResult Edit()
        {
            ViewData["Message"] = "This is the Edit Order View.";
            return View();
        }

        [Authorize(Policy = "User-PurchaseOrders-View")]
        public IActionResult Details()
        {
            ViewData["Message"] = "This is the View Order View.";
            return View();
        }

        [Authorize(Policy = "User-PurchaseOrders-Manage")]
        public IActionResult Create()
        {
            ViewData["Message"] = "This is the Create Order View.";
            return View();
        }

        [Authorize(Policy = "User-PurchaseOrders-Delete")]
        public IActionResult Delete()
        {
            ViewData["Message"] = "This is the Delete Order View.";
            return View();
        }
    }
}