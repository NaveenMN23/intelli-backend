using IntelliCRMAPIService.Model;

namespace IntelliCRMAPIService.BL
{
    public interface IProductBL
    {
        Task<bool> CreateProduct(List<Productmaster> productmasters);
        Task<IList<Productmaster>> GetAllProductDetails(string customerId);
        Task<IList<Customerproduct>> GetCustomerProductDetails(string customerId);
    }
}
