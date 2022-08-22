namespace TestHeroku.Model
{
    public class CustomerPriorityResponse
    {
        public int Userid { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Email { get; set; }  
        public int? Priority { get; set; }
        public string? RequestedBy { get; set; }
    }
}
