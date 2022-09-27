
using IntelliCRMAPIService.DBContext;
using IntelliCRMAPIService.Model;

namespace IntelliCRMAPIService.Repository
{
    public interface IOrdersRepository : IRepositoryBase<Orders>
    {
        Task<List<string>> CreateOrder(IList<Orders> orders);
        Task<List<Orders>> GetOrderDetails(string customerId);
        Task<List<InvoiceResponse>> GetInvoiceDetails(InvoiceRequest request);
        Task<List<LableResponse>> GetLableDetails(LableRequest request);
    }
}
