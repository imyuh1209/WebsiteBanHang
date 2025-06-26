using WebsiteBanHang.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebsiteBanHang.Repositories
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);
        Task<Order?> GetByIdAsync(int id);

        // Lấy tất cả đơn hàng
        Task<IEnumerable<Order>> GetAllAsync();

        // Cập nhật trạng thái đơn hàng
        Task UpdateStatusAsync(int id, string status);

        // Xoá đơn hàng
        Task DeleteAsync(int id);
        Task<IList<Order>> GetOrdersByUserIdAsync(string userId);

    }
}