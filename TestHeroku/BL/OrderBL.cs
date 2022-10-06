using IntelliCRMAPIService.DBContext;
using IntelliCRMAPIService.Model;
using IntelliCRMAPIService.Repository;
using TestHeroku.Model;

namespace IntelliCRMAPIService.BL
{
    public class OrderBL : IOrderBL
    {
        private readonly IOrdersRepository _orderRepository;

        public OrderBL(IOrdersRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public Task<List<string>> CreateOrder(IList<OrderDO> orders)
        {
            try
            {
                return _orderRepository.CreateOrder(orders);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public Task<List<InvoiceResponse>> GetInvoiceDetails(InvoiceRequest request)
        {
            try
            {
                return _orderRepository.GetInvoiceDetails(request);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Task<List<LableResponse>> GetLableDetails(LableRequest request)
        {
            try
            {
                return _orderRepository.GetLableDetails(request);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Task<List<OrderDO>> GetOrderDetails(string customerId, string status)
        {
            try
            {
                return _orderRepository.GetOrderDetails(customerId, status);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> UpdateOrderTracking(LableRequest request)
        {
            try
            {
                return await _orderRepository.UpdateOrderTracking(request); 

            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
