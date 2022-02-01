using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItServiceApp.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")] //sadece rolü admin olan kullancının girişine izin veriliyr.
    public class ManageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Users()
        {
            return View();
        }
        public IActionResult SubscriptionTypes()
        {
            return View();
        }
    }
}
