namespace IntelliCRMAPIService.Model
{
    public class LableRequest
    {
        public List<string>? CustomerId { get; set; }
        public List<long>? Orders { get; set; }
        public string? TrackingNo
        {
            get; set;
        }

        public string? OrderStatus
        {
            get; set;
        }
    }
}
