namespace IntelliCRMAPIService.Model
{
    public class SubAdminResponse
    {
        public int? UserId { get; set; }
        public string? Password { get; set; }
        public string? Salt { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? ContactNumber { get; set; }
        public string? RequestedBy { get; set; }
        public string? Role { get; set; }
        public string? AccountStatus { get; set; }
        public int? AccountType { get; set; }
        public bool? RightsForCustomerAccount { get; set; }


    }
}
