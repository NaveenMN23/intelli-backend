using IntelliCRMAPIService.DBContext;
using IntelliCRMAPIService.Repository;

namespace IntelliCRMAPIService.Services
{
    public class ProductRepository : RepositoryBase<Productmaster>, IProductRepository
    {
        private readonly PostgresDBContext _applicationDBContext;
        public ProductRepository(PostgresDBContext applicationDBContext)
            : base(applicationDBContext)
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


        public Task<IList<Productmaster>> GetAllProductDetails(string customerId)
        {
            try
            {
                var userRole = _applicationDBContext.Users.Where(u => u.Email == customerId).FirstOrDefault()?.Rolename ?? "";


                IList<Productmaster> result = null;

                if (customerId != null && customerId.ToLower() != "customer")
                {
                    result= FindAll().ToList();
                }
                else
                {
                    result = _applicationDBContext.Productmaster.Join(
                            _applicationDBContext.Customerproduct.Where(e => e.Email == customerId),
                            p => p.Productid,
                            c => c.Productid,
                            (p,c) => p).ToList();
                }

                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        public Task<IList<Customerproduct>> GetCustomerProductDetails(string customerId)
        {
            try
            {
                var userRole = _applicationDBContext.Users.Where(u => u.Email == customerId).FirstOrDefault()?.Rolename ?? "";

                IList<Customerproduct> result = _applicationDBContext.Customerproduct.Where(e => string.IsNullOrEmpty(customerId)|| (userRole.ToLower() != "customer") || (e.Useridfk == Convert.ToInt32(customerId) || e.Email == customerId)).ToList();

                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                return default;
            }
        }

    }
}
