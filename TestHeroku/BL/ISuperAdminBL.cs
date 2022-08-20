using IntelliCRMAPIService.Model;

namespace IntelliCRMAPIService.BL
{
    public interface ISuperAdminBL
    {
        Task<bool> CreateCustomer(UserResponse userResponse);
        Task<bool> CreateSubAdmin(SubAdminResponse userResponse);
        Task<UserResponse> GetCustomer(string email);
        Task<UserResponse> GetSubAdmin(string email);
        Task<IList<UserResponse>> GetAllUserDetails(int userType);
    }
}
