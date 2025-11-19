namespace ECommerceApp.EComm.Commons.Modals
{
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public int UserId { get; set; }
        public string LoginId { get; set; } = string.Empty;
    }
}

