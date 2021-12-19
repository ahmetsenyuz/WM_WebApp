using Ilk_MVC_Projesi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ilk_MVC_Projesi.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly NorthwindContext _context;
        public EmployeeController(NorthwindContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var model = _context.Employees.Include(x => x.EmployeeTerritories).OrderBy(x => x.FirstName).ToList();
            return View(model);
        }
    }
}
