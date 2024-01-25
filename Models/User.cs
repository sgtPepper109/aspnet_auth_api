using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ASPNETAuthAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [NotNull]
        public string Email { get; set; }
        [NotNull]
        public string Password { get; set; }
        [AllowNull]
        public string Token { get; set; }
        [AllowNull]
        public string Role { get; set; }
    }
}
