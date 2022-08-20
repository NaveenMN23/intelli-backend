using IntelliCRMAPIService.DBContext;
using IntelliCRMAPIService.Repository;

namespace IntelliCRMAPIService.Services
{
    public class CustomerProductRepository : RepositoryBase<Customerproduct>, ICustomerProductRepository
    {
        private readonly PostgresDBContext _applicationDBContext;
        public CustomerProductRepository(PostgresDBContext applicationDBContext)
            :base(applicationDBContext)
        {
            _applicationDBContext = applicationDBContext;
        }
        
    }
}
