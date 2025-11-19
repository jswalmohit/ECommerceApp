using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.EComm.Commons.Modals
{
    public class LoginRequest
    {
        [Required]
        public string LoginId { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}

