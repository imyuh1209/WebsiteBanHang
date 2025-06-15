// Models/Order.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebsiteBanHang.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên khách hàng là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Tên khách hàng không được vượt quá 100 ký tự.")]
        [Display(Name = "Họ và tên khách hàng")]
        public string CustomerFullName { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự.")]
        [Display(Name = "Email khách hàng")]
        public string CustomerEmail { get; set; }

        [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        [StringLength(20, ErrorMessage = "Số điện thoại không được vượt quá 20 ký tự.")]
        [Display(Name = "Số điện thoại")]
        public string CustomerPhoneNumber { get; set; }

        [Required(ErrorMessage = "Địa chỉ giao hàng là bắt buộc.")]
        [StringLength(250, ErrorMessage = "Địa chỉ giao hàng không được vượt quá 250 ký tự.")]
        [Display(Name = "Địa chỉ giao hàng")]
        public string ShippingAddress { get; set; }

        [StringLength(500, ErrorMessage = "Ghi chú không được vượt quá 500 ký tự.")]
        [Display(Name = "Ghi chú")]
        public string? Notes { get; set; } // Có thể null

        [Required(ErrorMessage = "Phương thức thanh toán là bắt buộc.")]
        [StringLength(50, ErrorMessage = "Phương thức thanh toán không được vượt quá 50 ký tự.")]
        [Display(Name = "Phương thức thanh toán")]
        public string PaymentMethod { get; set; } // Ví dụ: "COD", "BankTransfer"

        [Required]
        [Display(Name = "Ngày đặt hàng")]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Tổng tiền hàng")]
        public decimal SubTotal { get; set; } // Tổng tiền các sản phẩm

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Phí vận chuyển")]
        public decimal ShippingFee { get; set; } = 30000m; // Ví dụ phí vận chuyển mặc định

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Tổng thanh toán")]
        public decimal GrandTotal { get; set; } // Tổng tiền cuối cùng = SubTotal + ShippingFee

        [Required]
        [StringLength(50)]
        [Display(Name = "Trạng thái đơn hàng")]
        // Ví dụ: "Pending", "Processing", "Shipped", "Delivered", "Cancelled"
        public string Status { get; set; } = "Pending";

        // Mối quan hệ 1-nhiều với OrderItem
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        // Thêm trường User/CustomerId nếu bạn có hệ thống người dùng đăng nhập
        // public string? UserId { get; set; }
        // [ForeignKey("UserId")]
        // public ApplicationUser? User { get; set; }
    }
}