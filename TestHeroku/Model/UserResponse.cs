using IntelliCRMAPIService.Attribute;

namespace IntelliCRMAPIService.Model
{
    public class UserResponse : SubAdminResponse
    {
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public double? CreditLimit { get; set; }
        public double? SoareceviedAmount { get; set; }

        [ValidateFileAttribute]
        public IFormFile? UploadFile { get; set; }

    }

}
