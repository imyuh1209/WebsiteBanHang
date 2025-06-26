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
            // Ép kiểu về List<Product>
            var products = _productRepository.GetAll().Cast<Product>().ToList();
            return View(products);
        }


    }
}