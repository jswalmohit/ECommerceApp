using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ECommerceApp.EComm.Commons.Modals
{
    [Table("UserDetails")]
    public class UserRequest
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string LoginId { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        // One-to-one navigation
        public LoginModel Credentials { get; set; } = default!;
    }

    [Table("UserCred")]
    public class LoginModel
    {
        [Key]
        [ForeignKey(nameof(User))] // Foreign key = UserModel.Id
        public int Id { get; set; }

        [Required]
        public string Password { get; set; } = string.Empty;

        // Navigation back to User
        [JsonIgnore]
        public UserRequest? User { get; set; }

    }
}