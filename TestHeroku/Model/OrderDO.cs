namespace TestHeroku.Model
{
    public class OrderDO
    {
        public string? RequestedBy { get; set; }
        public DateTime? Date { get; set; }
        public string? OrderDate => Date.Value.ToString("dd-MMM-yy HH:mm");

        public string Referencenumber { get; set; }
        public string? OnlinepharmacyName { get; set; }
        public string Onlinepharmacy { get; set; }
        public int Onlinepharmacyphonenumber { get; set; }
        public string Ordernumber { get; set; }
        public string Customername { get; set; }
        public string DoctorName { get; set; }
        public string Refill { get; set; }
        public string Customerphonenumber { get; set; }
        public string Emailaddress { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Zipcode { get; set; }
        public string Prescribername { get; set; }
        public string Prescriptionattached { get; set; }
        public string Directionsofuse { get; set; }
        public string Rxwarningcautionarynote { get; set; }
        public string Remarks { get; set; }
        public int Productid { get; set; }
        public string Equsbrandname { get; set; }
        public string Category { get; set; }
        public string Nameonpackage { get; set; }
        public string Strength { get; set; }
        public string Unitsperpack { get; set; }
        public int Quantity { get; set; }
        public string Dosageform { get; set; }
        public string Activeingredients { get; set; }
        public string? Status { get; set; }
        public string Productsourcedfrom { get; set; }
        public string Totalpacksordered { get; set; }
        public string Totalpricecustomerpays { get; set; }
        public string Priceperpackclientpays { get; set; }
        public string Shippingcostperorder { get; set; }
        public string Totalpriceclientpays { get; set; }
    }
}
