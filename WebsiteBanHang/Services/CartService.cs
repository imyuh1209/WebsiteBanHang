using System.Text.Json;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Services
{
    public class CartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // Lấy giỏ hàng từ session
        public List<CartItem> GetCart()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            string cartJson = session.GetString("Cart");
            if (string.IsNullOrEmpty(cartJson))
                return new List<CartItem>();

            return JsonSerializer.Deserialize<List<CartItem>>(cartJson);
        }

        // Lưu giỏ hàng vào session
        public void SaveCart(List<CartItem> cart)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            string cartJson = JsonSerializer.Serialize(cart);
            session.SetString("Cart", cartJson);
        }

        // Thêm sản phẩm vào giỏ hàng
        public void AddToCart(Product product, int quantity)
        {
            var cart = GetCart();
            var existingItem = cart.FirstOrDefault(item => item.ProductId == product.Id);

            if (existingItem != null)
            {
                // Nếu sản phẩm đã có trong giỏ hàng, tăng số lượng
                existingItem.Quantity += quantity;
            }
            else
            {
                // Nếu sản phẩm chưa có trong giỏ hàng, thêm mới
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = quantity,
                    ImageUrl = product.ImageUrl
                });
            }

            SaveCart(cart);
        }

        // Xóa sản phẩm khỏi giỏ hàng
        public void RemoveFromCart(int productId)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
            }
        }

        // Cập nhật số lượng sản phẩm trong giỏ hàng
        public void UpdateQuantity(int productId, int quantity)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                item.Quantity = quantity;
                SaveCart(cart);
            }
        }

        // Xóa toàn bộ giỏ hàng
        public void ClearCart()
        {
            _httpContextAccessor.HttpContext.Session.Remove("Cart");
        }

        // Tính tổng tiền giỏ hàng
        public decimal GetTotal()
        {
            return GetCart().Sum(item => item.Price * item.Quantity);
        }
    }
}