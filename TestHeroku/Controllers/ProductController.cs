using IntelliCRMAPIService.BL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IntelliCRMAPIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductBL _productBL;

        public ProductController(ILogger<ProductController> logger, IProductBL productBL)
        {
            _logger = logger;
            _productBL = productBL;
        }

        [HttpGet]
        [Route("GetProductDetails")]
        public async Task<IList<Productmaster>> GetProductDetails(string customerId)
        {
            var result = await _productBL.GetAllProductDetails(customerId);

            return result;
        }

        [HttpGet]
        [Route("GetCustomerProductDetails")]
        public async Task<IList<Customerproduct>> GetCustomerProductDetails([FromQuery]string customerId="")
        {
            var result = await _productBL.GetCustomerProductDetails(customerId);

            return result;
        }

        [HttpPost]
        [Route("CreateProductDetails")]
        public async Task<bool> CreateProduct(List<Productmaster> productmasters)
        {
            var result = await _productBL.CreateProduct(productmasters);
            return result;
        }
    }
}
