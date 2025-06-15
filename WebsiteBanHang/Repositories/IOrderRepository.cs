// Repositories/IOrderRepository.cs
using WebsiteBanHang.Models;
using System.Threading.Tasks;

namespace WebsiteBanHang.Repositories
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);
        Task<Order?> GetByIdAsync(int id);
        // Thêm các phương thức khác nếu cần (GetAll, Update, Delete)
    }
}