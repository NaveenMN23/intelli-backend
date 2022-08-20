using IntelliCRMAPIService.Model;

namespace IntelliCRMAPIService.Repository
{
    public interface ISuperAdminRepository
    {
        Task<bool> CreateCustomer(UserResponse userResponse);
        Task<bool> CreateSubAdmin(SubAdminResponse userResponse);
        Task<UserResponse> GetCustomer(string email);
        Task<UserResponse> GetSubAdmin(string email);
        Task<IList<UserResponse>> GetAllUserDetails(int userType);

        Task<IList<UserResponse>> GetAllSubAdminUserDetails(int userType);
    }
}
