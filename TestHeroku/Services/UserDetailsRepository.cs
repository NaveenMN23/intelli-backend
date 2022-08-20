using IntelliCRMAPIService.DBContext;
using IntelliCRMAPIService.Repository;

namespace IntelliCRMAPIService.Services
{
    public class UserDetailsRepository : RepositoryBase<Userdetails>, IUserDetailsRepository
    {
        private readonly PostgresDBContext _applicationDBContext;
        public UserDetailsRepository(PostgresDBContext applicationDBContext)
            :base(applicationDBContext)
        {
            _applicationDBContext = applicationDBContext;
            //_appSettings = appSettings.Value;
        }
        
    }
}
