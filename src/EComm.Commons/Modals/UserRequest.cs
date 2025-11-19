using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.EComm.Commons.Modals
{
    public class UserRequest
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string LoginId { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        public LoginModel Credentials { get; set; } = new();
    }

    public class LoginModel
    {
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
