using IntelliCRMAPIService.DBContext;
using IntelliCRMAPIService.Model;
using IntelliCRMAPIService.Repository;

namespace IntelliCRMAPIService.BL
{
    public class OrderBL : IOrderBL
    {
        private readonly IOrdersRepository _orderRepository;

        public OrderBL(IOrdersRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public Task<List<string>> CreateOrder(IList<Orders> orders)
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

        public Task<List<Orders>> GetOrderDetails(string customerId)
        {
            try
            {
                return _orderRepository.GetOrderDetails(customerId);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
