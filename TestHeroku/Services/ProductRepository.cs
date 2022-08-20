using IntelliCRMAPIService.DBContext;
using IntelliCRMAPIService.Repository;

namespace IntelliCRMAPIService.Services
{
    public class ProductRepository : RepositoryBase<Productmaster>, IProductRepository
    {
        private readonly PostgresDBContext _applicationDBContext;
        public ProductRepository(PostgresDBContext applicationDBContext)
            :base(applicationDBContext)
        {
            _applicationDBContext = applicationDBContext;
        }

        public async Task<bool> SaveProduct(List<Productmaster> productmasters)
        {
            try
            {

                await _applicationDBContext.Productmaster.AddRangeAsync(productmasters);
                await _applicationDBContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public Task<IList<Productmaster>> GetAllProductDetails()
        {
            try
            {
                IList<Productmaster> result = FindAll().ToList();
                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                return default;
            }
        }

    }
}
