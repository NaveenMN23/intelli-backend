namespace  IntelliCRMAPIService.Model
{
    public class InvoiceRequest
    {
        public List<string>? CustomerId { get; set; }
        public List<long>? Orders { get; set; }
    }
}
