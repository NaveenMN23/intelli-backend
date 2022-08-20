using Microsoft.IdentityModel.Tokens;
using IntelliCRMAPIService.AuthModels;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using IntelliCRMAPIService.Repository;
using System.Security.Cryptography;

namespace IntelliCRMAPIService.Services
{
    public class AuthTokenGenerator : IAuthTokenGenerator
    {
        private readonly ILogger<AuthTokenGenerator> _logger;
        private readonly IConfiguration _configuration;
        private readonly IAPIUsersRepository _usersRepository;
        public AuthTokenGenerator(ILogger<AuthTokenGenerator> logger, IConfiguration configuration, IAPIUsersRepository aPIUsersRepository)
        {
            _logger = logger;
            _configuration = configuration;
            _usersRepository = aPIUsersRepository;
        }

        public async Task<AuthToken> GenerateToken(AuthUser user)
        {
            try
            {
                _logger.LogInformation("Token generation Invoked");

                var userresult = await _usersRepository.ValidUser(user);
                if (userresult == null && userresult == default)
                {
                    return null;
                }
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, userresult.Email.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, userresult.Email.ToString()),
                    new Claim(ClaimTypes.Email, userresult.Email.ToString()),
                    new Claim(ClaimTypes.Role, userresult.Role.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };


                _logger.LogInformation("Token generation completed");

                return CreateToken(claims.ToList());
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Token generation failed due to :" + ex.Message);

                return null;
            }
        }

        private AuthToken CreateToken(List<Claim> authClaims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JwtTokenSettings:JwtSecretKey")));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var expirationDate = DateTime.Now.AddHours(2);

            var token = new JwtSecurityToken(audience: _configuration.GetValue<string>("JwtTokenSettings:Audience"),
                                              issuer: _configuration.GetValue<string>("JwtTokenSettings:Issuer"),
                                              claims: authClaims,
                                              expires: expirationDate,
                                              signingCredentials: credentials);

            var authToken = new AuthToken();
            authToken.Token = new JwtSecurityTokenHandler().WriteToken(token);
            authToken.ExpirationDate = expirationDate;
            authToken.RefreshToken = GenerateRefreshToken();

            return authToken;
        }

        public async Task<AuthToken> RefreshToken(AuthToken tokenModel)
        {
            List<Claim> authClaims;

            string? accessToken = tokenModel.Token;
            string? refreshToken = tokenModel.RefreshToken;

            var principal = GetPrincipalFromExpiredToken(accessToken);
            
            if (principal == null)
            {
                return null;
            }

            var user = await _usersRepository.GetUserByEmail(principal.Identity.Name);

            if (user == null)
            {
                return null;
            }

            var newAccessToken = CreateToken(principal.Claims.ToList());

            return newAccessToken;

        }
        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JwtTokenSettings:JwtSecretKey"))),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken)
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}
