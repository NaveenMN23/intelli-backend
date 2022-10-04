

using IntelliCRMAPIService.DBContext;

namespace IntelliCRMAPIService.Repository
{
    public interface IProductRepository : IRepositoryBase<Productmaster>
    {
        Task<IList<Productmaster>> GetAllProductDetails(string customerId);
        Task<bool> SaveProduct(List<Productmaster> productmasters);
        Task<IList<Customerproduct>> GetCustomerProductDetails(string customerId);

    }
}
