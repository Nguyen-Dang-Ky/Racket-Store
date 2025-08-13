using System.ComponentModel.DataAnnotations;

namespace Serberus_Racket_Store.DTOs.RacketDTOs
{
    public class CreateRacketDto
    {
        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Tên sản phẩm không được quá 100 ký tự.")]
        public string ProductName { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Mô tả không được quá 500 ký tự.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Giá là bắt buộc.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Số lượng tồn kho là bắt buộc.")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn kho không được âm.")]
        public int StockQuantity { get; set; }

        [Url(ErrorMessage = "URL hình ảnh không hợp lệ.")]
        [StringLength(255, ErrorMessage = "URL hình ảnh không được quá 255 ký tự.")]
        public string? ImageURL { get; set; }

        [Required(ErrorMessage = "Mã thương hiệu là bắt buộc.")]
        [Range(1, int.MaxValue, ErrorMessage = "Mã thương hiệu phải là một số dương.")]
        public int BrandId { get; set; }
    }
}
