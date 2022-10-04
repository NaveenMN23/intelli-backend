using IntelliCRMAPIService.DBContext;
using IntelliCRMAPIService.Model;
using IntelliCRMAPIService.Repository;
using System.Linq;
using TestHeroku.Model;

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

        public async Task<List<string>> CreateOrder(IList<OrderDO> orders)
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


                //Orders newOrder = orders.Select(e => new Orders()
                //{
                //    Address1 = e.Address1,
                //    Address2 = e.Address2,
                //    City  = e.City,
                //    Createdby = e.RequestedBy,
                //    Createddate = DateTime.Now,
                //    Customername = e.Customername,
                //    Customerphonenumber = e.Customerphonenumber,
                //    Date = DateTime.Now,
                //    Directionsofuse = e.Directionsofuse,
                //    DoctorName = e.DoctorName,
                //    Emailaddress = e.Emailaddress,
                //    Onlinepharmacy = e.Onlinepharmacy,
                //    OnlinepharmacyName = e.OnlinepharmacyName,
                //    Onlinepharmacyphonenumber = e.Onlinepharmacyphonenumber,
                //    Prescribername = e.Prescribername,
                //    Prescriptionattached = e.Prescriptionattached,
                //    Province = e.Province,
                //    Referencenumber = e.Referencenumber,
                //    Remarks = e.Remarks,
                //    Rxwarningcautionarynote = e.Rxwarningcautionarynote,    
                //    Zipcode = e.Zipcode,
                //    Refill = e.Refill
                    
                //}).FirstOrDefault();


                var orderResult = await _applicationDBContext.Orders.AddAsync(new Orders(){
                
                    Address1 = orders[0].Address1,
                    Address2 = orders[0].Address2,
                    City = orders[0].City,
                    Createdby = orders[0].RequestedBy,
                    Createddate = DateTime.Now,
                    Customername = orders[0].Customername,
                    Customerphonenumber = orders[0].Customerphonenumber,
                    Date = DateTime.Now,
                    Directionsofuse = orders[0].Directionsofuse,
                    DoctorName = orders[0].DoctorName,
                    Emailaddress = orders[0].Emailaddress,
                    Onlinepharmacy = orders[0].Onlinepharmacy,
                    OnlinepharmacyName = orders[0].OnlinepharmacyName,
                    Onlinepharmacyphonenumber = orders[0].Onlinepharmacyphonenumber.ToString(),
                    Prescribername = orders[0].Prescribername,
                    Prescriptionattached = orders[0].Prescriptionattached,
                    Province = orders[0].Province,
                    Referencenumber = orders[0].Referencenumber.ToString(),
                    Remarks = orders[0].Remarks,
                    Rxwarningcautionarynote = orders[0].Rxwarningcautionarynote,
                    Zipcode = orders[0].Zipcode,
                    Refill = orders[0].Refill,
                    Status = "Confirmed"

                });
                _applicationDBContext.SaveChanges();


                var orderDetails = orders.Select(e => new OrdersProducts()
                {
                    OrdersID = orderResult.Entity.Ordersid,
                    Activeingredients = e.Activeingredients,
                    Category = e.Category,
                    Dosageform = e.Dosageform,
                    Equsbrandname = e.Equsbrandname,
                    Nameonpackage = e.Nameonpackage,
                    Priceperpackclientpays = e.Priceperpackclientpays.ToString(),
                    Productid = e.Productid,
                    Productsourcedfrom = e.Productsourcedfrom,
                    Shippingcostperorder = e.Shippingcostperorder.ToString(),
                    Totalpacksordered = e.Totalpacksordered.ToString(),
                    Totalpriceclientpays = e.Totalpriceclientpays.ToString(),
                    Totalpricecustomerpays = e.Totalpricecustomerpays.ToString(),
                    Quantity = e.Quantity,
                    Strength = e.Strength,
                    Unitsperpack = e.Unitsperpack.ToString(),
                    Createdby = e.RequestedBy,
                    Createddate = DateTime.Now,
                    
                });

                _applicationDBContext.OrdersProducts.AddRange(orderDetails);
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


                result = (from o in orders
                          select new InvoiceResponse()
                          {
                              Address = o.Address1 + " " + o.Address2,
                              ClientRefernce = o.Referencenumber,
                              CustomerName = o.Customername,
                              Notes = o.Rxwarningcautionarynote,
                              OrderDate = o.Date,
                              PharmacyName = o.OnlinepharmacyName,
                              Refernce = o.Referencenumber,
                              City = o.City,
                              CustomerId = o.Emailaddress,
                              OrderId = o.Ordersid,
                              PharmacyNumber = o.Onlinepharmacyphonenumber,
                              InvoiceProducts = (from op in _applicationDBContext.OrdersProducts.Where(e => e.OrdersID == o.Ordersid)
                                                 join p in _applicationDBContext.Productmaster on op.Productid equals p.Productid
                                                 select new InvoiceProduct()
                                                 {
                                                     ActiveIngredient = op.Activeingredients,
                                                     Category = op.Category,
                                                     Cost = op.Priceperpackclientpays,
                                                     NameonPackage = p.Nameonpackage,
                                                     Origin = null,
                                                     Strength = p.Strength,
                                                     Subtotal = op.Totalpricecustomerpays,
                                                     Unitspack = op.Unitsperpack,
                                                     Totalpacks = op.Totalpacksordered,
                                                     USName = p.Equsbrandname
                                                }
                               ).ToList()
                          }
                    ).ToList();
            }
            else if (request.CustomerId != null && request.CustomerId.Count > 0)
            {
                var orders = _applicationDBContext.Orders.Where(o => request.CustomerId.Contains(o.Emailaddress));

                result = (from o in orders 
                          select new InvoiceResponse()
                          {
                              Address = o.Address1 + " " + o.Address2,
                              ClientRefernce = o.Referencenumber,
                              CustomerName = o.Customername,
                              Notes = o.Rxwarningcautionarynote,
                              OrderDate = o.Date,
                              PharmacyName = o.OnlinepharmacyName,
                              Refernce = o.Referencenumber,
                              City = o.City,
                              CustomerId = o.Emailaddress,
                              OrderId = o.Ordersid,
                              PharmacyNumber = o.Onlinepharmacyphonenumber,
                              InvoiceProducts = (from op in _applicationDBContext.OrdersProducts.Where(e => e.OrdersID == o.Ordersid)
                                                 join p in _applicationDBContext.Productmaster on op.Productid equals p.Productid
                                                 select new InvoiceProduct()
                                                 {
                                                     ActiveIngredient = op.Activeingredients,
                                                     Category = op.Category,
                                                     Cost = op.Priceperpackclientpays,
                                                     NameonPackage = p.Nameonpackage,
                                                     Origin = null,
                                                     Strength = p.Strength,
                                                     Subtotal = op.Totalpricecustomerpays,
                                                     Unitspack = op.Unitsperpack,
                                                     Totalpacks = op.Totalpacksordered,
                                                     USName = p.Equsbrandname,
                                                     Batch = p.Batch,
                                                     ExpiryDate = p.Expirydaterange
                                                 }
                               ).ToList()
                          }
                    ).ToList(); 
            }

            return result;
        }

        public async Task<List<LableResponse>> GetLableDetails(LableRequest request)
        {

            var result = _applicationDBContext.Orders.Where(o => request.Orders.Contains(o.Ordersid))
                            .Join(
                            _applicationDBContext.OrdersProducts,
                            o => o.Ordersid,
                            p => p.OrdersID,
                            (o, p) =>
                            new LableResponse()
                            {
                                OrderDate = o.Date,
                                OrderNo = o.Ordersid.ToString(),
                                Category = p.Category,
                                Refernce = o.Referencenumber,
                                CustomerName = o.Customername,
                                Directionsofuse = o.Directionsofuse,
                                DoctorName = o.DoctorName,
                                DosageForm = p.Dosageform,
                                NameonPackage = p.Nameonpackage,
                                Onlinepharmacy = o.Onlinepharmacy,
                                CustomerId = o.Emailaddress,
                                Onlinepharmacyphonenumber = o.Onlinepharmacyphonenumber,
                                PharmacyName = o.OnlinepharmacyName,
                                Quantity = p.Quantity,
                                Refill = o.Refill,
                                Rxwarningcautionarynote = o.Rxwarningcautionarynote,
                                Strength = p.Strength,
                                Unitsperpack = p.Unitsperpack
                            }

                ).ToList();
                
            return result;
        }

        public Task<List<OrderDO>> GetOrderDetails(string customerId, string status)
        {

            var userRole = _applicationDBContext.Users.Where(u => u.Email == customerId).FirstOrDefault()?.Rolename?? "";



            var result = _applicationDBContext.Orders.Where(e =>  ((userRole.ToLower() != "customer") || e.Emailaddress == customerId || string.IsNullOrEmpty(customerId) && ( e.Status == status || string.IsNullOrEmpty(status)))).Join(
                            _applicationDBContext.OrdersProducts,
                            o => o.Ordersid,
                            p => p.OrdersID,
                            (o, p) => new OrderDO()
                            {
                                Address1 = o.Address1,
                                Address2 = o.Address2,
                                City = o.City,
                                Customername = o.Customername,
                                Customerphonenumber = o.Customerphonenumber,
                                Date = DateTime.Now,
                                Directionsofuse = o.Directionsofuse,
                                DoctorName = o.DoctorName,
                                Emailaddress = o.Emailaddress,
                                Onlinepharmacy = o.Onlinepharmacy,
                                OnlinepharmacyName = o.OnlinepharmacyName,
                                Onlinepharmacyphonenumber = Convert.ToInt32(o.Onlinepharmacyphonenumber),
                                Prescribername = o.Prescribername,
                                Prescriptionattached = o.Prescriptionattached,
                                Province = o.Province,
                                Referencenumber = o.Referencenumber.ToString(),
                                Remarks = o.Remarks,
                                Rxwarningcautionarynote = o.Rxwarningcautionarynote,
                                Zipcode = o.Zipcode,
                                Refill = o.Refill,
                                Ordernumber = o.Ordersid.ToString(),
                                Activeingredients = p.Activeingredients,
                                Category = p.Category,
                                Dosageform = p.Dosageform,
                                Equsbrandname = p.Equsbrandname,
                                Nameonpackage = p.Nameonpackage,
                                Priceperpackclientpays = p.Priceperpackclientpays,
                                Productid = p.Productid,
                                Productsourcedfrom = p.Productsourcedfrom,
                                Shippingcostperorder = p.Shippingcostperorder,
                                Totalpacksordered = p.Totalpacksordered,
                                Totalpriceclientpays = p.Totalpriceclientpays,
                                Totalpricecustomerpays = p.Totalpricecustomerpays,
                                Quantity = p.Quantity,
                                Strength = p.Strength,
                                Unitsperpack = p.Unitsperpack,
                                Status = o.Status
                            }).ToList();

            return Task.FromResult(result);
        }
    }
}
