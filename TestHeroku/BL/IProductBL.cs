using IntelliCRMAPIService.Model;

namespace IntelliCRMAPIService.BL
{
    public interface IProductBL
    {
        Task<bool> CreateProduct(List<Productmaster> productmasters);
        Task<IList<Productmaster>> GetAllProductDetails();
        Task<IList<Customerproduct>> GetCustomerProductDetails(string customerId);
    }
}
