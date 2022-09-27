using IntelliCRMAPIService.DBContext;
using IntelliCRMAPIService.Model;
using IntelliCRMAPIService.Repository;
using System.Linq;

namespace IntelliCRMAPIService.Services
{
    public class OrdersRepository : RepositoryBase<Orders>, IOrdersRepository
    {
        private readonly PostgresDBContext _applicationDBContext;
        public OrdersRepository(PostgresDBContext applicationDBContext)
            :base(applicationDBContext)
        {
            _applicationDBContext = applicationDBContext;
        }

        public async Task<List<string>> CreateOrder(IList<Orders> orders)
        {
            List<string> errorMessage = new List<string>();

            try
            {
                var customerProduct = _applicationDBContext.Customerproduct.Where(c => c.Email == orders.First().Emailaddress);

                var productAssigmentValidation = (from o in orders
                                         join p in customerProduct on o.Productid equals p.Productid
                                         into tempOrder
                                         from t in tempOrder.DefaultIfEmpty()
                                         where t == null
                                         select o.Productid
                                         ).ToList();

                if(productAssigmentValidation.Any())
                {
                    errorMessage.Add("Below are the products are not assigned to you. " + String.Join(",", productAssigmentValidation)); 
                }

                var productQuantityValidation = (from o in orders
                                         join p in _applicationDBContext.Productmaster on o.Productid equals p.Productid
                                         where o.Quantity > p.Qty
                                         select o.Productid
                                         ).ToList();

                if(productQuantityValidation.Any())
                {
                    errorMessage.Add("Below are the products are not in stack. " + String.Join(",", productQuantityValidation));
                }


                if(errorMessage.Any())
                {
                    return errorMessage;
                }
                await _applicationDBContext.Orders.AddRangeAsync(orders);
                _applicationDBContext.SaveChanges();
            }
            catch(Exception ex)
            {

            }

            return errorMessage;
        }

        public async Task<List<InvoiceResponse>> GetInvoiceDetails(InvoiceRequest request)
        {
            var result = new List<InvoiceResponse>();

            if (request.Orders != null && request.Orders.Count > 0)
            {
                var orders = _applicationDBContext.Orders.Where(o => request.Orders.Contains(o.Ordersid));


                result = orders.Join<Orders, Productmaster, int, InvoiceResponse>(
                        _applicationDBContext.Productmaster,
                        o => o.Productid,
                        p => p.Productid,
                        (o, p) => new InvoiceResponse
                        {
                            Address = o.Address1 + " " + o.Address2,
                            BatchNo = p.Batch,
                            ClientRefernce = o.Referencenumber,
                            CustomerName = o.Customername,
                            Notes = o.Rxwarningcautionarynote,
                            OrderDate = o.Date,
                            PharmacyName = o.OnlinepharmacyName,
                            Refernce = o.Referencenumber,
                            ExpiryDate = p.Expirydaterange
                        }
                    ).ToList();
            }
            else if (request.CustomerId != null && request.CustomerId.Count > 0)
            {
                var orders = _applicationDBContext.Orders.Where(o => request.CustomerId.Contains(o.Emailaddress));

                result = orders.Join<Orders, Productmaster, int, InvoiceResponse>(
                        _applicationDBContext.Productmaster,
                        o => o.Productid,
                        p => p.Productid,
                        (o, p) => new InvoiceResponse
                        {
                            Address = o.Address1 + " " + o.Address2,
                            BatchNo = p.Batch,
                            ClientRefernce = o.Referencenumber,
                            CustomerName = o.Customername,
                            Notes = o.Rxwarningcautionarynote,
                            OrderDate = o.Date,
                            PharmacyName = o.OnlinepharmacyName,
                            Refernce = o.Referencenumber,
                            ExpiryDate = p.Expirydaterange
                        }
                    ).ToList(); 
            }

            return result;
        }

        public async Task<List<LableResponse>> GetLableDetails(LableRequest request)
        {

            var result = _applicationDBContext.Orders.Where(o => request.Orders.Contains(o.Ordersid)).Select((o) => new LableResponse()
            {
                OrderDate = o.Date,
                Category = o.Category,
                Refernce = o.Referencenumber,
                CustomerName =o.Customername,
                Directionsofuse =o.Directionsofuse,
                DoctorName = o.DoctorName,
                DosageForm= o.Dosageform,
                NameonPackage =o.Nameonpackage,
                Onlinepharmacy = o.Onlinepharmacy,
                Onlinepharmacyphonenumber =o.Onlinepharmacyphonenumber,
                PharmacyName =o.OnlinepharmacyName,
                Quantity =o.Quantity,
                Refill = o.Refill,
                Rxwarningcautionarynote = o.Rxwarningcautionarynote,
                Strength =o.Strength,
                Unitsperpack =o.Unitsperpack
            }).ToList();

            return result;
        }

        public Task<List<Orders>> GetOrderDetails(string customerId)
        {
            var result = _applicationDBContext.Orders.Where(e => (e.Emailaddress == customerId || string.IsNullOrEmpty(customerId))).ToList();

            return Task.FromResult(result);
        }
    }
}
