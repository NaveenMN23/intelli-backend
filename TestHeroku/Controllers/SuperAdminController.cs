using IntelliCRMAPIService.BL;
using IntelliCRMAPIService.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestHeroku.Model;

namespace IntelliCRMAPIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperAdminController : ControllerBase
    {
        private readonly ILogger<SuperAdminController> _logger;
        private readonly ISuperAdminBL _superAdminBL;

        public SuperAdminController(ILogger<SuperAdminController> logger, ISuperAdminBL superAdminBL)
        {
            _logger = logger;
            _superAdminBL = superAdminBL;
        }

        [HttpPost]
        [Route("CreateCustomer")]
        public async Task<bool> CreatCustomer([FromForm] UserResponse userResponse)
        {
            var result = await _superAdminBL.CreateCustomer(userResponse);

            return result;
        }

        [HttpPost]
        [Route("CreatSuperAdmin")]
        public async Task<bool> CreatSuperAdmin([FromForm] SubAdminResponse userResponse)
        {

            var result = await _superAdminBL.CreateSubAdmin(userResponse);

            return result;
        }

        [HttpGet]
        [Route("GetCustomerDetails")]
        public async Task<UserResponse> GetCustomerDetails([FromQuery]string email)
        {
            var result = await _superAdminBL.GetCustomer(email);

            return result;
        }

        [HttpGet]
        [Route("DeleteUserDetails")]
        public async Task<bool> DeleteUserDetails([FromQuery] string email)
        {
            var result = await _superAdminBL.DeleteUserDetails(email);

            return result;
        }

        [HttpGet]
        [Route("GetSubAdminDetails")]
        public async Task<UserResponse> GetSubAdminDetails([FromQuery] string email)
        {

            var result = await _superAdminBL.GetSubAdmin(email);

            return result;
        }

        [HttpGet]
        [Route("GetAllUserDetails/{userType}")]
        public async Task<IList<UserResponse>> GetAllUserDetails(int userType)
        {

            var result = await _superAdminBL.GetAllUserDetails(userType);

            return result;
        }
        
        [HttpGet]
        [Route("GetAllUserPriority")]
        public async Task<IList<CustomerPriorityResponse>> GetAllUserPriority()
        {

            var result = await _superAdminBL.GetAllUserPriority();

            return result;
        }

        [HttpPost]
        [Route("UpdateUserPriority")]
        public async Task<bool> UpdateUserPriority(List<CustomerPriorityResponse> customerPriorityResponse)
        {

            var result = await _superAdminBL.UpdateUserPriority(customerPriorityResponse);

            return result;
        }

        [HttpGet]
        [Route("GetSOADetails")]
        public async Task<List<SOADetails>> GetSOADetails(string customerId)
        {

            var result = await _superAdminBL.GetSOADetails(customerId);

            return result;
        }
    }
}
