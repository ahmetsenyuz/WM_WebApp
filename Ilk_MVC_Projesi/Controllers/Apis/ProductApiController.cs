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
    public class ProductApiController : ControllerBase
    {
        private readonly NorthwindContext _dbContext;
        public ProductApiController(NorthwindContext context)
        {
            _dbContext = context;
        }
        [HttpPost]
        public IActionResult Add(ProductViewModel model)
        {
            var product = new Product()
            {
                CategoryId = model.CategoryId,
                ProductName = model.ProductName,
                UnitPrice = model.UnitPrice
            };
            try
            {
                _dbContext.Products.Add(product);
                _dbContext.SaveChanges();
                return Ok(new
                {
                    Message = "Ürün Ekleme İşlemi Başarılı.",
                    Model = product
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Bir Hata Oluştu: {ex.Message}");
            }
        }
        [HttpPost]
        [Route("~/api/productapi/delete/{id?}")]
        public IActionResult Delete(int id=0)
        {
            var product = _dbContext.Products.FirstOrDefault(x => x.ProductId == id);
            if (product == null)
            {
                return NotFound("Ürün Bulunamadı");
            }
            try
            {
                _dbContext.Products.Remove(product);
                _dbContext.SaveChanges();
                return Ok(new
                {
                    message = $"{product.ProductName}  Ürünü Silindi."
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Bir hata oluştu: {ex.Message}");
            }
        }
    }
}
