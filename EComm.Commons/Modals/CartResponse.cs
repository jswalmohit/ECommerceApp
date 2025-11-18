namespace ECommerceApp.EComm.Commons.Modals
{
    public class CartResponse
    {
        public int UserId { get; set; }
        public List<CartItemResponse> Items { get; set; } = new();
        public decimal TotalAmount { get; set; }
        public int TotalItems { get; set; }
    }
}

