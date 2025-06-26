using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebsiteBanHang.Models;
using WebsiteBanHang.Repositories;
using System.Threading.Tasks;

namespace WebsiteBanHang.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IProductRepository _productRepository;

        public OrderController(IOrderRepository orderRepository, UserManager<ApplicationUser> userManager, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _userManager = userManager;
            _productRepository = productRepository;
        }

        // Hiển thị danh sách đơn hàng của user hiện tại
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var orders = await _orderRepository.GetOrdersByUserIdAsync(user.Id);
            return View(orders);
        }

        // Hiển thị chi tiết đơn hàng (chỉ cho phép xem đơn của chính mình)
        public async Task<IActionResult> Details(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null || order.UserId != user.Id)
                return NotFound();
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null || order.UserId != user.Id)
                return NotFound();

            // Chỉ cho phép hủy nếu đơn chưa giao hoặc chưa hoàn thành
            if (order.Status == "Pending" || order.Status == "Chờ xử lý" || order.Status == "Processing")
            {
                // Cập nhật trạng thái
                await _orderRepository.UpdateStatusAsync(id, "Cancelled");

                // Hoàn trả tồn kho
                foreach (var item in order.OrderItems)
                {
                    // Sử dụng phương thức async đúng kiểu Product
                    var product = await _productRepository.GetByIdAsync(item.ProductId);
                    if (product != null)
                    {
                        product.Stock += item.Quantity;
                        await _productRepository.UpdateAsync(product);
                    }
                }

            }
            else
            {
                TempData["ErrorMessage"] = "Đơn hàng không thể hủy ở trạng thái hiện tại.";
            }
            return RedirectToAction("Index");
        }
    }
}