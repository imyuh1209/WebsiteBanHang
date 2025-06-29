using Microsoft.AspNetCore.Mvc;
using WebsiteBanHang.Repositories;
using WebsiteBanHang.Models;
using System.Globalization;

namespace WebsiteBanHang.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StatisticsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;

        public StatisticsController(IProductRepository productRepository, IOrderRepository orderRepository)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }

        public async Task<IActionResult> Index()
        {
            // 🔹 Lấy thống kê sản phẩm
            var products = _productRepository.GetAll().Cast<Product>().ToList();
            ViewBag.TotalProducts = products.Count;
            ViewBag.TotalStock = products.Sum(p => p.Stock);
            ViewBag.ProductNames = products.Select(p => p.Name).ToList();
            ViewBag.ProductStocks = products.Select(p => p.Stock).ToList();

            // 🔹 Lấy thống kê doanh thu theo ngày
            var orders = (await _orderRepository.GetAllAsync()).ToList();

            var revenueByDay = orders
                .GroupBy(o => o.OrderDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture))
                .Select(g => new
                {
                    Day = g.Key,
                    Revenue = g.Sum(o => o.TotalAmount)
                })
                .OrderBy(x => x.Day)
                .ToList();

            ViewBag.RevenueDays = revenueByDay.Select(r => r.Day).ToList();
            ViewBag.DailyRevenues = revenueByDay.Select(r => r.Revenue).ToList();
            ViewBag.TotalRevenue = revenueByDay.Sum(r => r.Revenue);

            return View();
        }
    }
}
