namespace IntelliCRMAPIService.AuthModels
{
    public class AuthToken
    {
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string RefreshToken { get; set; }
    }
}
