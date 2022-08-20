using IntelliCRMAPIService.AuthModels;
using IntelliCRMAPIService.DBContext;

namespace IntelliCRMAPIService.Repository
{
    public interface IAPIUsersRepository
    {
        Task<Users> ValidUser(AuthUser authUser);
        Task<Users> GetUserByEmail(string email);
    }
}
