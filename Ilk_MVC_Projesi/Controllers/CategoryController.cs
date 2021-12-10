using Ilk_MVC_Projesi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ilk_MVC_Projesi.Controllers
{
    public class CategoryController : Controller
    {   
        //actionlar defauult olarak [HttpGet] tipindedir.(mozilla.org http request methods)
        private readonly NorthwindContext _context;
        public CategoryController(NorthwindContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            //var context = new NorthwindContext();bunun yerine startuppta tanımladık. ve yukarısını ekledik.
            var data = _context.Categories.Include(x => x.Products).OrderBy(x => x.CategoryName).ToList();
            return View(data);
        }
        public IActionResult Detail(int? id)
        {
            var category = _context.Categories
                .Include(x => x.Products)
                .ThenInclude(x => x.OrderDetails)
                .ThenInclude(x => x.Order)
                .FirstOrDefault(x => x.CategoryId == id);//her x bir öncekilin içine girer. left join yapmak gibi

            var category2 = from cat in _context.Categories
                            join prod in _context.Products on cat.CategoryId equals prod.CategoryId
                            join ordet in _context.OrderDetails on prod.ProductId equals ordet.ProductId
                            where cat.CategoryId == id
                            select cat;
            var model = category2.FirstOrDefault();
            { if (category == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                return View(category);
            }
            
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category model)
        {
            var category = new Category()
            {
                CategoryName = model.CategoryName,
                Description = model.Description
            };
            _context.Categories.Add(category);
            try
            {
                _context.SaveChanges();
                return RedirectToAction("Detail", new { id = category.CategoryId });
            }
            catch (Exception ex)
            {
            }

            return View();
        }
    }
}
