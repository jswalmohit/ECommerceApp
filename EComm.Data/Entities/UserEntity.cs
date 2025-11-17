using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceApp.EComm.Data.Entities
{
    [Table("UserDetails")]
    public class UserEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string LoginId { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        public UserCredentialEntity Credentials { get; set; } = new();
    }

    [Table("UserCred")]
    public class UserCredentialEntity
    {
        [Key]
        [ForeignKey(nameof(User))]
        public int Id { get; set; }

        [Required]
        public string Password { get; set; } = string.Empty;

        public UserEntity? User { get; set; }
    }
}

