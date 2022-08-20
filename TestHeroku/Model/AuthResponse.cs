using IntelliCRMAPIService.Attribute;

namespace IntelliCRMAPIService.Models
{
    public class AuthResponse
    {
        public string Username { get; set; }
        public Role Role { get; set; }
        public string Guid { get; set; }
    }
}
