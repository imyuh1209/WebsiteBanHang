using WebsiteBanHang.Models;

namespace WebsiteBanHang.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
        IEnumerable<object> GetAll();
        object GetById(int productId);
        void Update(Product product);
    }
}
