// Models/OrderItem.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebsiteBanHang.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; } // Foreign Key đến Order

        [ForeignKey("OrderId")]
        public Order Order { get; set; } // Navigation property

        [Required]
        public int ProductId { get; set; } // ID của sản phẩm tại thời điểm đặt hàng

        // Bạn có thể không cần Navigation Property Product ở đây để tránh eager loading lớn,
        // hoặc nếu bạn muốn lưu snapshot thông tin sản phẩm.
        // [ForeignKey("ProductId")]
        // public Product Product { get; set; }

        // Lưu trữ thông tin sản phẩm tại thời điểm đặt hàng (snapshot)
        [Required]
        [StringLength(200)]
        [Display(Name = "Tên sản phẩm")]
        public string ProductName { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Giá tại thời điểm mua")]
        public decimal PriceAtOrder { get; set; } // Giá sản phẩm tại thời điểm đặt hàng

        [Required]
        [Display(Name = "Số lượng")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0.")]
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Tổng tiền mặt hàng")]
        public decimal Total => PriceAtOrder * Quantity;
    }
}