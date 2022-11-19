
using IntelliCRMAPIService.DBContext;
using IntelliCRMAPIService.Model;
using TestHeroku.Model;

namespace IntelliCRMAPIService.Repository
{
    public interface IOrdersRepository : IRepositoryBase<Orders>
    {
        Task<List<string>> CreateOrder(IList<OrderDO> orders);
        Task<List<OrderDO>> GetOrderDetails(string customerId, string status);
        Task<List<InvoiceResponse>> GetInvoiceDetails(InvoiceRequest request);
        Task<List<LableResponse>> GetLableDetails(LableRequest request);
        Task<bool> UpdateOrderTracking(LableRequest request);
        Task<List<Orders>> GetOrderDetails(DataRange request);
    }
}
