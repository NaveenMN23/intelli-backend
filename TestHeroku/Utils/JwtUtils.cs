using Microsoft.IdentityModel.Tokens;
using IntelliCRMAPIService.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IntelliCRMAPIService.Attribute;

namespace IntelliCRMAPIService.Utility
{
    public interface IJwtUtils
    {
        public AuthResponse ValidateJwtToken(string token, bool isRefresh= false);
    }

    public class JwtUtils : IJwtUtils
    {
        private string _Secret;

        private readonly JwtSecurityTokenHandler tokenHandler;
        private readonly IConfiguration _configuration;
        private readonly ILogger<JwtUtils> _logger;
        public JwtUtils(IConfiguration configuration, ILogger<JwtUtils> logger)
        {
            this.tokenHandler = new JwtSecurityTokenHandler();
            _configuration = configuration;
            _logger = logger;
        }

        public AuthResponse ValidateJwtToken(string token, bool isRefresh=false)
        {
            if (token == null)
                return null;
            try
            {
                _Secret = _configuration.GetValue<string>("JwtTokenSettings:JwtSecretKey");

                var key = Encoding.ASCII.GetBytes(_Secret);

                _logger.LogInformation("JWT token - Validation started");
                
                var jwtToken =  tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Secret)),
                   ValidAudience = _configuration.GetValue<string>("JwtTokenSettings:Audience"),
                   ValidIssuer = _configuration.GetValue<string>("JwtTokenSettings:Issuer"),
                   ValidateIssuer = !isRefresh,
                   ValidateAudience = !isRefresh,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var userId = ToUserDetails(jwtToken);
                _logger.LogInformation("JWT token - Validation completed");

                // return user id from JWT token if validation successful
                return userId;
            }
            catch(Exception ex)
            {
                _logger.LogInformation("JWT token - Validation Failed due to : " +ex.Message);
                // return null if validation fails
                return null;
            }
        }

        private AuthResponse ToUserDetails(ClaimsPrincipal principal)
        {
            var username = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            var id = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
            var role = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;

            return new AuthResponse
            {
                Guid = id,
                Username = username,
                Role = (Role)Enum.Parse(typeof(Role), role)
            };
        }

    }
}