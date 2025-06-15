// Repositories/OrderRepository.cs
using WebsiteBanHang.Models;
using Microsoft.EntityFrameworkCore;

namespace WebsiteBanHang.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders
                                 .Include(o => o.OrderItems) // Đảm bảo load cả các item trong order
                                 .FirstOrDefaultAsync(o => o.Id == id);
        }
    }
}