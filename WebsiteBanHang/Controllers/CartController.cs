using Microsoft.AspNetCore.Mvc;
using WebsiteBanHang.Models;
using WebsiteBanHang.Repositories;
using WebsiteBanHang.Services;

namespace WebsiteBanHang.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly CartService _cartService;

        public CartController(IProductRepository productRepository, CartService cartService)
        {
            _productRepository = productRepository;
            _cartService = cartService;
        }

        public IActionResult Index()
        {
            // Lấy giỏ hàng từ session và hiển thị
            var cart = _cartService.GetCart();
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            // Lấy thông tin sản phẩm từ repository
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            // Thêm sản phẩm vào giỏ hàng
            _cartService.AddToCart(product, quantity);
            
            // Hiển thị thông báo thành công
            TempData["SuccessMessage"] = $"{product.Name} đã được thêm vào giỏ hàng";
            
            // Chuyển hướng trở lại trang sản phẩm
            return RedirectToAction("Display", "Product", new { id = productId });
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            // Xóa sản phẩm khỏi giỏ hàng
            _cartService.RemoveFromCart(productId);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            // Cập nhật số lượng sản phẩm trong giỏ hàng
            if (quantity > 0)
            {
                _cartService.UpdateQuantity(productId, quantity);
            }
            else
            {
                _cartService.RemoveFromCart(productId);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Checkout()
        {
            // Xử lý thanh toán (sẽ triển khai sau)
            // Xóa giỏ hàng sau khi thanh toán
            _cartService.ClearCart();
            TempData["SuccessMessage"] = "Đặt hàng thành công!";
            return RedirectToAction("Index", "Home");
        }
    }
}