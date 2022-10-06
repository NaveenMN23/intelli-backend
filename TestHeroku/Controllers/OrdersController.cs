using IntelliCRMAPIService.BL;
using IntelliCRMAPIService.DBContext;
using IntelliCRMAPIService.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestHeroku.Model;

namespace IntelliCRMAPIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly IOrderBL _orderBL;

        public OrdersController(ILogger<OrdersController> logger, IOrderBL orderBL)
        {
            _logger = logger;
            _orderBL = orderBL;
        }

        [HttpPost]
        [Route("CreateOrder")]
        public async Task<ActionResult<List<string>>> CreateOrder(IList<OrderDO> orders)
        {
            var result = await _orderBL.CreateOrder(orders);

            return Ok(result);
        }


        [HttpPost]
        [Route("GetOrderDetails")]
        public async Task<ActionResult<List<OrderDO>>> GetOrderDetails(OrderRequest orderRequest)
        {
            var result = await _orderBL.GetOrderDetails(orderRequest.customerId,orderRequest.status);

            return Ok(result);
        }

        [HttpPost]
        [Route("GetInvoiceDetails")]
        public async Task<ActionResult<List<InvoiceResponse>>> GetInvoiceDetails(InvoiceRequest request)
        {
            var result = await _orderBL.GetInvoiceDetails(request);

            return Ok(result);
        }

        [HttpPost]
        [Route("GetLableDetails")]
        public async Task<ActionResult<List<LableResponse>>> GetLableDetails(LableRequest request)
        {
            var result = await _orderBL.GetLableDetails(request);

            return Ok(result);
        }
    }
}
