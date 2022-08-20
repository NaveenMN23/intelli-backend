using IntelliCRMAPIService.DBContext;
using IntelliCRMAPIService.Repository;

namespace IntelliCRMAPIService.Services
{
    public class UserRepository : RepositoryBase<Users>, IUserRepository
    {
        private readonly PostgresDBContext _applicationDBContext;
        public UserRepository(PostgresDBContext applicationDBContext)
            :base(applicationDBContext)
        {
            _applicationDBContext = applicationDBContext;
            //_appSettings = appSettings.Value;
        }
        
    }
}
