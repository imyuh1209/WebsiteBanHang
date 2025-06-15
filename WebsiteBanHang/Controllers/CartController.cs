using Microsoft.AspNetCore.Mvc;
using WebsiteBanHang.Models;
using WebsiteBanHang.Models.ViewModels;
using WebsiteBanHang.Repositories;
using WebsiteBanHang.Services;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace WebsiteBanHang.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly CartService _cartService;
        private readonly IOrderRepository _orderRepository;

        public CartController(IProductRepository productRepository, CartService cartService, IOrderRepository orderRepository)
        {
            _productRepository = productRepository;
            _cartService = cartService;
            _orderRepository = orderRepository;
        }

        public IActionResult Index()
        {
            var cart = _cartService.GetCart();
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Sản phẩm không tồn tại.";
                return NotFound();
            }

            // Đã bỏ kiểm tra tồn kho vì không quản lý StockQuantity nữa
            _cartService.AddToCart(product, quantity);

            TempData["SuccessMessage"] = $"{product.Name} đã được thêm vào giỏ hàng.";

            return RedirectToAction("Display", "Product", new { id = productId });
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            _cartService.RemoveFromCart(productId);
            TempData["InfoMessage"] = "Sản phẩm đã được xóa khỏi giỏ hàng.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            if (quantity <= 0)
            {
                _cartService.RemoveFromCart(productId);
                TempData["InfoMessage"] = "Sản phẩm đã được xóa khỏi giỏ hàng.";
            }
            else
            {
                // Đã bỏ kiểm tra tồn kho vì không quản lý StockQuantity nữa
                _cartService.UpdateQuantity(productId, quantity);
                TempData["SuccessMessage"] = "Số lượng sản phẩm đã được cập nhật.";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            var cartItems = _cartService.GetCart();
            if (!cartItems.Any())
            {
                TempData["ErrorMessage"] = "Giỏ hàng của bạn đang trống. Vui lòng thêm sản phẩm vào giỏ hàng trước khi thanh toán.";
                return RedirectToAction(nameof(Index));
            }

            var checkoutItems = cartItems.Select(item => new CheckoutItemViewModel
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                Price = item.Price,
                ImageUrl = item.ImageUrl
            }).ToList();

            var viewModel = new CheckoutViewModel
            {
                CartItems = checkoutItems,
                CartTotal = checkoutItems.Sum(item => item.Total),
                // Có thể điền trước thông tin người dùng nếu đã đăng nhập
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(CheckoutViewModel model)
        {
            var cartItems = _cartService.GetCart();
            if (!cartItems.Any())
            {
                TempData["ErrorMessage"] = "Giỏ hàng của bạn đã bị trống. Vui lòng thêm sản phẩm vào giỏ hàng.";
                return RedirectToAction(nameof(Index));
            }

            model.CartItems = cartItems.Select(item => new CheckoutItemViewModel
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                Price = item.Price,
                ImageUrl = item.ImageUrl
            }).ToList();
            model.CartTotal = model.CartItems.Sum(item => item.Total);

            

            try
            {
                var order = new Order
                {
                    CustomerFullName = model.FullName,
                    CustomerEmail = model.Email,
                    CustomerPhoneNumber = model.PhoneNumber,
                    ShippingAddress = model.ShippingAddress,
                    Notes = model.Notes,
                    PaymentMethod = model.PaymentMethod,
                    OrderDate = DateTime.Now,
                    SubTotal = model.CartTotal,
                    ShippingFee = model.ShippingFee,
                    GrandTotal = model.GrandTotal,
                    Status = "Pending"
                };

                foreach (var item in model.CartItems)
                {
                    // Không cần lấy product từ DB để kiểm tra tồn kho hoặc cập nhật nữa
                    order.OrderItems.Add(new OrderItem
                    {
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        PriceAtOrder = item.Price,
                        Quantity = item.Quantity
                    });

                    // Đã bỏ logic cập nhật StockQuantity
                    // Nếu bạn có ProductRepository với UpdateAsync, bạn có thể xóa nó khỏi đây nếu không dùng nữa
                    // Hoặc xóa hoàn toàn phương thức UpdateAsync khỏi ProductRepository nếu nó chỉ dùng cho StockQuantity
                }

                await _orderRepository.AddAsync(order);

                _cartService.ClearCart();

                TempData["SuccessMessage"] = "Đơn hàng của bạn đã được đặt thành công!";
                return RedirectToAction("OrderConfirmation", new { orderId = order.Id });
            }
            catch (Exception ex)
            {
                // Vẫn giữ lại phần xử lý lỗi chung
                TempData["ErrorMessage"] = "Có lỗi xảy ra trong quá trình đặt hàng. Vui lòng thử lại sau.";
                return View("Checkout", model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> OrderConfirmation(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy thông tin đơn hàng xác nhận.";
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }
    }
}