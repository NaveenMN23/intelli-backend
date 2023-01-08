namespace IntelliCRMAPIService.Model
{

    public class InvoiceResponse
    {
        public string PharmacyName { get; set; }
        public string PharmacyNumber { get; set; }
        public DateTime? OrderDate { get; set; }
        public string Refernce { get; set; }
        public string CustomerName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string CustomerId { get; set; }
        public string Address { get; set; }
        public string ClientRefernce { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string Notes { get; set; }
        public string BatchNo { get; set; }
        public string OrderId { get; set; }
        public string ShippingCost { set; get; }
        public string TotalCost { set; get; }
        public List<InvoiceProduct> InvoiceProducts { get; set; }

    }

    public class InvoiceProduct
    {
        public string Category { get; set; }
        public string USName { get; set; }
        public string ActiveIngredient { get; set; }
        public string NameonPackage { get; set; }
        public string Strength { get; set; }
        public string Origin { get; set; }
        public string Unitspack { get; set; }
        public string Totalpacks { get; set; }
        public string Subtotal { get; set; }
        public string Cost { get; set; }
        public string Batch { get; set; }
        public DateTime? ExpiryDate { get; set; }

    }
}
