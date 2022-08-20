using IntelliCRMAPIService.Model;
using IntelliCRMAPIService.Repository;

namespace IntelliCRMAPIService.BL
{
    public class ProductBL : IProductBL
    {
        private readonly ILogger<ProductBL> _logger;
        private readonly IProductRepository _productRepository;

        public ProductBL(ILogger<ProductBL> logger, IProductRepository productRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
        }
        public async Task<bool> CreateProduct(List<Productmaster> productmasters)
        {
            try
            {
                var result = await _productRepository.SaveProduct(productmasters);
                return result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        
        public async Task<IList<Productmaster>> GetAllProductDetails()
        {
            try
            {
                var result = await _productRepository.GetAllProductDetails();
                return result;
            }
            catch (Exception ex)
            {
                return default;
            }
        }
    }
}
