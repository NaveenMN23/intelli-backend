using IntelliCRMAPIService.Attribute;
using IntelliCRMAPIService.AuthModels;
using IntelliCRMAPIService.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntelliCRMAPIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly ILogger<AuthController> _logger;
        private readonly IAuthTokenGenerator _authTokenGenerator;
        public AuthController(ILogger<AuthController> logger, IAuthTokenGenerator authTokenGenerator)
        {
            _logger = logger;
            _authTokenGenerator = authTokenGenerator;
        }

        [HttpPost]
        [Route("Authenticate")]
        [CustomAllowAnonymousAttribute]
        public async Task<ActionResult<AuthToken>> GetAuthenticationToken([FromBody] AuthUser user)
        {
            _logger.LogInformation("AuthController - GetAuthenticationToken is invoked");

            var result = await _authTokenGenerator.GenerateToken(user);

            if(result is null)
            {
                 return BadRequest(new { message = "Username or Password is invalid" });
            }

            _logger.LogInformation("AuthController - GetAuthenticationToken is completed");

            return result;
        }

        [HttpPost]
        [Route("RefreshToken")]
        [CustomAllowAnonymousAttribute]
        public async Task<ActionResult<AuthToken>> RefreshToken(AuthToken tokenModel)
        {
            if (tokenModel is null)
            {
                return BadRequest("Invalid client request");
            }

            var result = await _authTokenGenerator.RefreshToken(tokenModel);

            if (result is null)
            {
                return BadRequest(new { message = "Username or Password is invalid" });
            }

            _logger.LogInformation("AuthController - RefreshToken is completed");

            return result;
        }
    }
}
