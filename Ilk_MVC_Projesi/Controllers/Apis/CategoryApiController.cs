using Ilk_MVC_Projesi.Models;
using Ilk_MVC_Projesi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ilk_MVC_Projesi.Controllers.Apis
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryApiController : ControllerBase
    {
        private readonly NorthwindContext _dbContext;
        public CategoryApiController(NorthwindContext dbContext)
        {
            _dbContext = dbContext;
        }
        //json viewer eklentisini chromea yükle
        [HttpGet]
        public IActionResult GetCategories()
        {
            try
            {
                var categories = _dbContext.Categories
                    .Include(x=> x.Products)
                    .OrderBy(x => x.CategoryName)
                    .Select(x=> new CategoryViewModel
                    {
                         CategoryId=x.CategoryId,
                         CategoryName = x.CategoryName,
                         Description = x.Description,
                         ProductCount = x.Products.Count
                    })
                    .ToList();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult AddCategory(CategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var category = new Category()
            {
                CategoryName = model.CategoryName,
                Description = model.Description
            };
            _dbContext.Categories.Add(category);
            try
            {
                _dbContext.SaveChanges();
                return Ok(new
                {
                    message = "Kategori ekleme işlemi başarılı.",
                    model = category
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("~/api/categoryapi/updatecategory/{id?}")]//custom route
        public IActionResult UpdateCategory(int? id,CategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var category = _dbContext.Categories.FirstOrDefault(x => x.CategoryId == id);
            if (category == null)
            {
                return NotFound("Kategori Bulunamadı");
            }
            category.CategoryName = model.CategoryName;
            category.Description = model.Description;
            try
            {
                _dbContext.Categories.Update(category);
                _dbContext.SaveChanges();
                return Ok(new {
                    message = "Kategori Güncelleme Başarılı.",
                    model = category
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        public IActionResult DeleteCategory(int? id)
        {
            //postman indir
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var category = _dbContext.Categories.FirstOrDefault(x => x.CategoryId == id);
            if (category == null)
            {
                return NotFound("Kategori Bulunamadı");
            }
            try
            {
                _dbContext.Categories.Remove(category);
                _dbContext.SaveChanges();
                return Ok(new
                {
                    message = $"{category.CategoryName}Kategorisi Silindi."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
