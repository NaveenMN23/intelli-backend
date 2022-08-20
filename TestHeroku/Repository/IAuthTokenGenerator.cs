using IntelliCRMAPIService.AuthModels;

namespace IntelliCRMAPIService.Repository
{
    public interface IAuthTokenGenerator
    {
        Task<AuthToken> GenerateToken(AuthUser user);
        Task<AuthToken> RefreshToken(AuthToken tokenModel);
    }
}
