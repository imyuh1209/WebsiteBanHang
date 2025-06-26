using WebsiteBanHang.Models;
using WebsiteBanHang.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebsiteBanHang.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index(string search, int? categoryId)
        {
            var products = _productRepository.GetAll().Cast<Product>();

            if (!string.IsNullOrEmpty(search))
                products = products.Where(p => p.Name.Contains(search, StringComparison.OrdinalIgnoreCase));

            // Lọc theo danh mục nếu có categoryId
            if (categoryId.HasValue && categoryId.Value > 0)
                products = products.Where(p => p.CategoryId == categoryId.Value);

            // Truyền danh mục cho dropdown lọc (nếu dùng ở view)
            var categories = _categoryRepository.GetAll().ToList();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            ViewBag.SelectedCategoryId = categoryId;
            ViewBag.Search = search;

            return View(products.ToList());
        }

        public async Task<IActionResult> Display(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }


    }
}