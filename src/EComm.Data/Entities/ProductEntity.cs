using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceApp.EComm.Data.Entities
{
    [Table("Products")]
    public class ProductEntity : BaseEntity
    {

        [Key, MaxLength(10)]
        public string ProductId { get; set; } =string.Empty;

        [Required, MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [MaxLength(100)]
        public string Category { get; set; } = string.Empty;

        [MaxLength(500)]
        public string ImageUrl { get; set; } = string.Empty;

        public int StockQuantity { get; set; } = 0;

        public bool IsActive { get; set; } = true;
    }
}

