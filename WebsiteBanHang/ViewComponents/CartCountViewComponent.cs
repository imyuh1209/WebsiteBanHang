using Microsoft.AspNetCore.Mvc;
using WebsiteBanHang.Services;

namespace WebsiteBanHang.ViewComponents
{
    public class CartCountViewComponent : ViewComponent
    {
        private readonly CartService _cartService;

        public CartCountViewComponent(CartService cartService)
        {
            _cartService = cartService;
        }

        public IViewComponentResult Invoke()
        {
            // Lấy giỏ hàng từ session và đếm tổng số lượng sản phẩm
            var cart = _cartService.GetCart();
            int itemCount = cart.Sum(item => item.Quantity);
            
            // Trả về số lượng sản phẩm dưới dạng chuỗi
            return Content(itemCount.ToString());
        }
    }
}