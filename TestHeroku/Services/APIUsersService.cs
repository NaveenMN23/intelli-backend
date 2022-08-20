using IntelliCRMAPIService.AuthModels;
using IntelliCRMAPIService.DBContext;
using IntelliCRMAPIService.Model;
using IntelliCRMAPIService.Repository;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace IntelliCRMAPIService.Services
{
    public class APIUsersService : IAPIUsersRepository
    {
        protected PostgresDBContext _applicationDBContext { get; set; }
        private DbContext _dbContext { get; set; }
        public APIUsersService(PostgresDBContext repositoryContext)
        {
            _applicationDBContext = repositoryContext;
        }

        public Task<Users> ValidUser(AuthUser authUser)
        {
            var result = _applicationDBContext.Users.SingleOrDefault(a => a.Email == authUser.Username );

            if (result == null || result == default)
                return Task.FromResult<Users>(default);

            bool verified = BCrypt.Net.BCrypt.Verify(authUser.Password, result.Password);

            return Task.FromResult<Users>(result);
        }

        public Task<Users> GetUserByEmail(string email)
        {
            var result = _applicationDBContext.Users.SingleOrDefault(a => a.Email == email);

            if (result == null || result == default)
                return Task.FromResult<Users>(default);

            return Task.FromResult<Users>(result);
        }

    }
}
