using IntelliCRMAPIService.DBContext;
using IntelliCRMAPIService.Model;

namespace IntelliCRMAPIService.BL
{
    public interface IOrderBL
    {
        Task<List<string>> CreateOrder(IList<Orders> orders);
        Task<List<Orders>> GetOrderDetails(string customerId );
        Task<List<InvoiceResponse>> GetInvoiceDetails(InvoiceRequest request);
        Task<List<LableResponse>> GetLableDetails(LableRequest request);
    }
}
