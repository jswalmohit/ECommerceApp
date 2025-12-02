using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceApp.EComm.Data.Entities
{
    [Table("CartItems")]
    public class CartItemEntity : BaseEntity
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        [Required]
        [ForeignKey(nameof(Product))]
        public string ProductId { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        // Navigation properties
        public UserEntity? User { get; set; }
        public ProductEntity? Product { get; set; }
    }
}

