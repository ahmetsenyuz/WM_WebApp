using Ilk_MVC_Projesi.Models;
using Ilk_MVC_Projesi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private int _pageSize = 10;
        public IActionResult Index(int? page = 1)//null gelirse default 1 alır öte türlü geleni verir.
        {
            var model = _context.Products
                .Include(x => x.Category)
                .Include(x=> x.Supplier)//include'da product içinde içer yapıyoruz. Eğer theninclude olsaydı category'nin içine girerdi.
                .OrderBy(x=> x.Category.CategoryName)
                .ThenBy(x=> x.ProductName)
                .Skip((page.GetValueOrDefault()-1) * _pageSize)//içindeki değer kadar sayıyı baştan atlayıp kalanları getirir.
                .Take(_pageSize)//pagesize kadar ürünü ekrana getir.
                .ToList();
            //ViewBag.Categories = _context.Categories.OrderBy(x => x.CategoryName).ToList();
            //ViewBag.Suppliers = _context.Suppliers.OrderBy(x => x.CompanyName).ToList();//viewa birden fazla model göndermeye yarıyor.
            ViewBag.Page = page.GetValueOrDefault(1);
            ViewBag.Limit = (int)Math.Ceiling(_context.Products.Count() / (double)_pageSize);
            return View(model);
        }
        public IActionResult Detail(int? id)
        {
            var data = _context.Products
                .Include(x => x.Category)
                .Include(x => x.Supplier)
                .FirstOrDefault(x => x.ProductId == id);//her x bir öncekilin içine girer. left join yapmak gibi
            {
                if (data == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                var model = new ProductViewModel()
                {
                    ProductId = data.ProductId,
                    CategoryName = data.Category?.CategoryName,
                    CategoryId = data.CategoryId,
                    CompanyName = data.Supplier?.CompanyName,//nullable olması lazım
                    ProductName = data.ProductName,
                    UnitPrice = data.UnitPrice,
                    SupplierId = data.SupplierId
                };
                return View(model);
            }
        }
        public IActionResult Update(int? id)
        {
            var data = _context.Products
                .Include(x => x.Category)
                .Include(x => x.Supplier)
                .FirstOrDefault(x => x.ProductId == id);//her x bir öncekilin içine girer. left join yapmak gibi
            {
                if (data == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                var model = new ProductViewModel()
                {
                    ProductId = data.ProductId,
                    CategoryName = data.Category?.CategoryName,
                    CategoryId = data.CategoryId,
                    CompanyName = data.Supplier?.CompanyName,
                    ProductName = data.ProductName,
                    UnitPrice = data.UnitPrice,
                    SupplierId = data.SupplierId
                };
               
                ViewBag.CategoryList = GetCategoryList();
                ViewBag.SupplierList = GetSupplierList();
                return View(model);
            }
        }
        [HttpPost]
        public IActionResult Update(ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CategoryList = GetCategoryList();
                ViewBag.SupplierList = GetSupplierList();
                return View(model);
            }
            var product = _context.Products.FirstOrDefault(x => x.ProductId == model.ProductId);
            if (product == null)
            {
                return RedirectToAction(nameof(Index));
            }else
            {
                product.ProductName = model.ProductName;
                product.UnitPrice = model.UnitPrice;
                product.CategoryId = model.CategoryId;
                product.SupplierId = model.SupplierId;
            }
            try
            {
                _context.Products.Update(product);
                _context.SaveChanges();
                TempData["Mesaj"] = $"{product.ProductName} ürünü başarıyla güncellenmiştir.";
                return RedirectToAction(nameof(Detail), new { id = product.ProductId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewBag.CategoryList = GetCategoryList();
                ViewBag.SupplierList = GetSupplierList();
                return View(model);
            }
        }
        private List<SelectListItem> GetCategoryList()
        {
            var categories = _context.Categories.OrderBy(x => x.CategoryName).ToList();
            var categoryList = new List<SelectListItem>();
            categoryList.Add(new SelectListItem("Kategory Yok", null));//default değer yaptık.
            foreach (var category in categories)
            {
                categoryList.Add(new SelectListItem(category.CategoryName, category.CategoryId.ToString()));
            }
            return categoryList;
        }
        private List<SelectListItem> GetSupplierList()
        {
            var suppliers = _context.Suppliers.OrderBy(x => x.CompanyName).ToList();
            var supplierList = new List<SelectListItem>();
            supplierList.Add(new SelectListItem("Tedarikçi Yok", null));//default değer yaptık.
            foreach (var supplier in suppliers)
            {
                supplierList.Add(new SelectListItem(supplier.CompanyName, supplier.SupplierId.ToString()));
            }
            return supplierList;
        }
    }
}
