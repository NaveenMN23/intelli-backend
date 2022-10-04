namespace IntelliCRMAPIService.AuthModels
{
    public class AuthToken
    {
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string RefreshToken { get; set; }
        public string Role { get; set; }
        public bool? canEditCustomer { get; set; }
        public bool? canEditProducts { get; set; }
        public bool? canEditOrders { get; set; }
    }
}
