using IntelliCRMAPIService.Model;
using IntelliCRMAPIService.Repository;
using TestHeroku.Model;

namespace IntelliCRMAPIService.BL
{
    public class SuperAdminBL : ISuperAdminBL
    {
        private readonly ILogger<SuperAdminBL> _logger;
        private readonly ISuperAdminRepository _superAdminRepository;

        public SuperAdminBL(ILogger<SuperAdminBL> logger, ISuperAdminRepository superAdminRepository)
        {
            _logger = logger;
            _superAdminRepository = superAdminRepository;
        }
        public async Task<bool> CreateCustomer(UserResponse userResponse)
        {
            try
            {
                var result = await _superAdminRepository.CreateCustomer(userResponse);
                return result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> CreateSubAdmin(SubAdminResponse userResponse)
        {
            try
            {
                var result = await _superAdminRepository.CreateSubAdmin(userResponse);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("CreateSubAdmin -- " + ex.Message);
                return false;
            }
        }

        public async Task<UserResponse> GetCustomer(string email)
        {
            try
            {
                var result = await _superAdminRepository.GetCustomer(email);
                return result;
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        public async Task<UserResponse> GetSubAdmin(string email)
        {
            try
            {
                var result = await _superAdminRepository.GetSubAdmin(email);
                return result;
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        public async Task<IList<UserResponse>> GetAllUserDetails(int userType)
        {
            try
            {
                IList<UserResponse> result = new List<UserResponse>();

                if (userType == 1)                
                     result = await _superAdminRepository.GetAllUserDetails(userType);
                else
                    result = await _superAdminRepository.GetAllSubAdminUserDetails(userType);

                return result;
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        public async Task<IList<CustomerPriorityResponse>> GetAllUserPriority()
        {
            try
            {
                IList<CustomerPriorityResponse> result = new List<CustomerPriorityResponse>();

                result = await _superAdminRepository.GetAllUserPriority();

                return result;
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        public async Task<bool> UpdateUserPriority(CustomerPriorityResponse customerPriorityResponse)
        {
            try
            {
                var result = await _superAdminRepository.UpdateUserPriority(customerPriorityResponse);

                return result;
            }
            catch (Exception ex)
            {
                return default;
            }
        }
    }
}
