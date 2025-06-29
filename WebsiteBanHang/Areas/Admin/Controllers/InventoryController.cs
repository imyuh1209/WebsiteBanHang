using Microsoft.AspNetCore.Mvc;
using WebsiteBanHang.Repositories;
using WebsiteBanHang.Models;
using System.Collections.Generic;
using System.Linq;

namespace WebsiteBanHang.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class InventoryController : Controller
    {
        private readonly IProductRepository _productRepository;

        public InventoryController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // Hiển thị danh sách sản phẩm và tồn kho
        public IActionResult Index()
        {
            var products = _productRepository.GetAll().Cast<Product>().ToList();
            return View(products);
        }

        // Tăng số lượng tồn kho
        [HttpPost]
        public IActionResult IncreaseStock(int id)
        {
            var product = _productRepository.GetById(id) as Product;
            if (product != null)
            {
                product.Stock += 1;
                _productRepository.Update(product);
            }
            return RedirectToAction(nameof(Index));
        }

        // Giảm số lượng tồn kho
        [HttpPost]
        public IActionResult DecreaseStock(int id)
        {
            var product = _productRepository.GetById(id) as Product;
            if (product != null && product.Stock > 0)
            {
                product.Stock -= 1;
                _productRepository.Update(product);
            }
            return RedirectToAction(nameof(Index));
        }

        // ✅ Cập nhật số lượng tồn kho theo input
        [HttpPost]
        public IActionResult UpdateStock(int productId, int newStock)
        {
            var product = _productRepository.GetById(productId) as Product;
            if (product != null)
            {
                product.Stock = newStock;
                _productRepository.Update(product);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
