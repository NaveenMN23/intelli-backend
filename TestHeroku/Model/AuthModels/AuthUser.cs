using System.ComponentModel.DataAnnotations;

namespace IntelliCRMAPIService.AuthModels
{
    public class AuthUser
    {
        [Required]
        public string Username { get; set; }
        public string? Password { get; set; }
    }
}
