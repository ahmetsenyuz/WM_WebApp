using Ilk_MVC_Projesi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ilk_MVC_Projesi.Controllers
{
    public class ProductController : Controller
    {
        private readonly NorthwindContext _context;
        public ProductController(NorthwindContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var data = _context.Products.Include(x => x.Category).ToList();
            return View(data);
        }
    }
}
