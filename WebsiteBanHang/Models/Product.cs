using System.ComponentModel.DataAnnotations;

namespace WebsiteBanHang.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string Name { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0.")]

        public decimal Price { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        public List<string>? ImageUrls { get; set; }    
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public int Stock { get; set; }
        // Thông số kỹ thuật cho máy tính/PC
        [StringLength(100)]
        public string? CPU { get; set; } // Bộ vi xử lý
        [StringLength(50)]
        public string? RAM { get; set; } // Bộ nhớ RAM
        [StringLength(100)]
        public string? Storage { get; set; } // Ổ cứng
        [StringLength(100)]
        public string? GPU { get; set; } // Card đồ họa
        [StringLength(100)]
        public string? OperatingSystem { get; set; } // Hệ điều hành
        [StringLength(50)]
        public string? ScreenSize { get; set; }
    }
}
