using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.EComm.Commons.Modals
{
    public class CartItemRequest
    {
        [Required]
        public string ProductId { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
    }
}

