using IntelliCRMAPIService.DBContext;
using IntelliCRMAPIService.Model;
using IntelliCRMAPIService.Repository;
using Microsoft.EntityFrameworkCore;
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
                var email = orders.First().Emailaddress?.ToLower();
                var customerProduct = _applicationDBContext.Customerproduct.Where(c => c.Email == email);

                var productAssigmentValidation = (from o in orders
                                         join p in customerProduct on o.Productid equals p.Productid
                                         into tempOrder
                                         from t in tempOrder.DefaultIfEmpty()
                                         where t == null
                                         select o.Productid
                                         ).Distinct().ToList();


                if (productAssigmentValidation.Any())
                {
                    errorMessage.Add($"Below are the products are not assigned to {email}. " + String.Join(",", productAssigmentValidation)); 
                }

                var productQuantityValidation = (from o in orders
                                         join p in _applicationDBContext.Productmaster on o.Productid equals p.Productid
                                         where o.Quantity > p.Qty
                                         select o.Productid
                                         ).Distinct().ToList();

                var invalidProduct = (from o in orders
                                                  join p in _applicationDBContext.Productmaster on o.Productid equals p.Productid
                                                  into tempOrder
                                                  from t in tempOrder.DefaultIfEmpty()
                                                  where t == null
                                                  select o.Productid
                                         ).Distinct().ToList();
                if (invalidProduct.Any())
                {
                    errorMessage.Add("Below are the invalid products. " + String.Join(",", productAssigmentValidation));
                }

                if (productQuantityValidation.Any())
                {
                    errorMessage.Add("Below are the products are not in stack. " + String.Join(",", invalidProduct));
                }


                if(errorMessage.Any())
                {
                    return errorMessage;
                }




                var emailList = orders.Select(e => e.Emailaddress).Distinct().ToList();

                for (int i = 0; i <= emailList.Count(); i++)
                {

                    Orders newOrder = orders.Where(e => e.Emailaddress == emailList[i]).Select(e => new Orders()
                    {
                        Address1 = e.Address1,
                        Address2 = e.Address2,
                        City = e.City,
                        Createdby = e.RequestedBy,
                        Createddate = DateTime.Now,
                        Customername = e.Customername,
                        Customerphonenumber = e.Customerphonenumber,
                        Date = DateTime.Now,
                        Directionsofuse = e.Directionsofuse,
                        DoctorName = e.DoctorName,
                        Emailaddress = e.Emailaddress,
                        Onlinepharmacy = e.Onlinepharmacy,
                        OnlinepharmacyName = e.OnlinepharmacyName,
                        Onlinepharmacyphonenumber = e.Onlinepharmacyphonenumber,
                        Prescribername = e.Prescribername,
                        Prescriptionattached = e.Prescriptionattached,
                        Province = e.Province,
                        Referencenumber = e.Referencenumber,
                        Remarks = e.Remarks,
                        Rxwarningcautionarynote = e.Rxwarningcautionarynote,
                        Zipcode = e.Zipcode,
                        Refill = e.Refill

                    }).FirstOrDefault();

                    var orderResult = await _applicationDBContext.Orders.AddAsync(newOrder);

                    //    new Orders()
                    //{

                    //    Address1 = newOrder.Address1,
                    //    Address2 = newOrder.Address2,
                    //    City = newOrder.City,
                    //    Createdby = newOrder.Createdby,
                    //    Createddate = DateTime.Now,
                    //    Customername = newOrder.Customername,
                    //    Customerphonenumber = newOrder.Customerphonenumber,
                    //    Date = DateTime.Now,
                    //    Directionsofuse = newOrder.Directionsofuse,
                    //    DoctorName = newOrder.DoctorName,
                    //    Emailaddress = newOrder.Emailaddress,
                    //    Onlinepharmacy = newOrder.Onlinepharmacy,
                    //    OnlinepharmacyName = newOrder.OnlinepharmacyName,
                    //    Onlinepharmacyphonenumber = newOrder.Onlinepharmacyphonenumber.ToString(),
                    //    Prescribername = newOrder.Prescribername,
                    //    Prescriptionattached = newOrder.Prescriptionattached,
                    //    Province = newOrder.Province,
                    //    Referencenumber = newOrder.Referencenumber.ToString(),
                    //    Remarks = newOrder.Remarks,
                    //    Rxwarningcautionarynote = newOrder.Rxwarningcautionarynote,
                    //    Zipcode = newOrder.Zipcode,
                    //    Refill = newOrder.Refill,
                    //    Status = "Confirmed",
                    //    Shippingcostperorder = newOrder.Shippingcostperorder.ToString(),
                    //    Totalpriceclientpays = newOrder.Totalpriceclientpays.ToString()

                    //});
                    _applicationDBContext.SaveChanges();


                    var orderDetails = orders.Where(e => e.Emailaddress == orderResult.Entity.Emailaddress).Select(e => new OrdersProducts()
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
                        Totalpacksordered = e.Totalpacksordered.ToString(),
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
                              Country = o.Country,
                              CustomerId = o.Emailaddress,
                              OrderId = o.Ordersid,
                              PharmacyNumber = o.Onlinepharmacyphonenumber,
                              ShippingCost = o.Shippingcostperorder,
                              TotalCost = o.Totalpriceclientpays,
                              InvoiceProducts = (from op in _applicationDBContext.OrdersProducts.Where(e => e.OrdersID == o.Ordersid)
                                                 join p in _applicationDBContext.Productmaster on op.Productid equals p.Productid
                                                 select new InvoiceProduct()
                                                 {
                                                     ActiveIngredient = op.Activeingredients,
                                                     Category = op.Category,
                                                     Cost = op.Priceperpackclientpays,
                                                     NameonPackage = p.Nameonpackage,
                                                     Origin = op.Productsourcedfrom,
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
                              PharmacyName = o.Onlinepharmacy,
                              Refernce = o.Referencenumber,
                              City = o.City,
                              Country = o.Country,
                              CustomerId = o.Emailaddress,
                              OrderId = o.Ordersid,
                              PharmacyNumber = o.Onlinepharmacyphonenumber,
                              ShippingCost = o.Shippingcostperorder,
                              TotalCost = o.Totalpriceclientpays,
                              InvoiceProducts = (from op in _applicationDBContext.OrdersProducts.Where(e => e.OrdersID == o.Ordersid)
                                                 join p in _applicationDBContext.Productmaster on op.Productid equals p.Productid
                                                 select new InvoiceProduct()
                                                 {
                                                     ActiveIngredient = op.Activeingredients,
                                                     Category = op.Category,
                                                     Cost = op.Priceperpackclientpays,
                                                     NameonPackage = p.Nameonpackage,
                                                     Origin = op.Productsourcedfrom,
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
                                Unitsperpack = p.Unitsperpack,
                                TotalPacks = "3"
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
                                Onlinepharmacyphonenumber = o.Onlinepharmacyphonenumber,
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
                                Shippingcostperorder = o.Shippingcostperorder,
                                Totalpacksordered = p.Totalpacksordered,
                                Totalpriceclientpays = o.Totalpriceclientpays,
                                Totalpricecustomerpays = p.Totalpricecustomerpays,
                                Quantity = p.Quantity,
                                Strength = p.Strength,
                                Unitsperpack = p.Unitsperpack,
                                Status = o.Status,
                                TrackingNo = o.TrackingNo,
                                Country = o.Country
                            }).ToList();

            return Task.FromResult(result);
        }

        public async Task<bool> UpdateOrderTracking(LableRequest request)
        {
            using var transaction = _applicationDBContext.Database.BeginTransaction();

            try
            {
                var result = _applicationDBContext.Orders.Where(o => request.Orders.Contains(o.Ordersid)).ToList();

                var productdetails = _applicationDBContext.OrdersProducts.Where(o => request.Orders.Contains(o.OrdersID)).Select(
                    e => new
                        {
                            Productid = e.Productid,
                            Quantity = e.Quantity,
                            Orderid = e.OrdersID
                        }
                    ).ToList();


                foreach (var u in productdetails)
                {
                    var product = _applicationDBContext.Productmaster.Where(p => p.Productid == u.Productid).FirstOrDefault();

                    if (product != default && product != null)
                    {
                        product.Qty = product.Qty - u.Quantity;

                        _applicationDBContext.Productmaster.Update(product);
                        _applicationDBContext.SaveChanges();
                    }
                }

                for (int i=0;i<result.Count;i++)
                {
                    if (!string.IsNullOrEmpty(request.TrackingNo))
                    {
                        result[i].TrackingNo = request.TrackingNo;

                    }
                    result[i].Status = request.OrderStatus;
                }

                _applicationDBContext.Orders.UpdateRange(result);
                _applicationDBContext.SaveChanges();

                transaction.Commit();

                return true;

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return false;
            }
        }

        public async Task<List<Orders>> GetOrderDetails(DataRange request)
        {
            var orders = (from order in _applicationDBContext.Orders
                          where order.Date <= request.To && order.Date > request.From
                          select order
                          ).ToList();

            return orders;
        }
    }
}
