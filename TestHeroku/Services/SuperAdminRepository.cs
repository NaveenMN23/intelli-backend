using IntelliCRMAPIService.Attribute;
using IntelliCRMAPIService.BL;
using IntelliCRMAPIService.DBContext;
using IntelliCRMAPIService.Model;
using IntelliCRMAPIService.Repository;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using TestHeroku.Model;

namespace IntelliCRMAPIService.Services
{
    public class SuperAdminRepository : ISuperAdminRepository
    {
        private readonly IUserDetailsRepository _userDetailsRepository;
        private readonly IUserRepository _userRepository;
        private readonly PostgresDBContext _applicationDBContext;
        private readonly ExcelConverter _excelConverter;
        private readonly ICustomerProductRepository _customerProductRepository;

        public SuperAdminRepository(IUserDetailsRepository userDetailsRepository, IUserRepository userRepository, PostgresDBContext applicationDBContext,
            ExcelConverter excelConverter, ICustomerProductRepository customerProductRepository)
        {
            _userDetailsRepository = userDetailsRepository;
            _userRepository = userRepository;
            _applicationDBContext = applicationDBContext;
            _excelConverter = excelConverter;
            _customerProductRepository = customerProductRepository;
        }
        public async Task<bool> CreateCustomer(UserResponse userResponse)
        {
            var result = await SaveUserDetails(userResponse);

            if (result != 0 && userResponse.UploadFile != null)
            {
                var productTable = await _excelConverter.ConvertToDataTable(userResponse.UploadFile.OpenReadStream());

                foreach (DataRow row in productTable.Rows)
                {
                    var values = row.ItemArray;

                    if (Convert.ToString(values[1]) == "")
                        continue;

                    var customerproduct = new Customerproduct()
                    {
                        Productid = Convert.ToString(values[0]),
                        Productname = Convert.ToString(values[1]),
                        Productprice = Convert.ToString(values[2]),
                        Qtyassign = Convert.ToString(values[3]),
                        Email = userResponse.Email,
                        Useridfk = result
                    };

                    var existingCust = _customerProductRepository.FindByCondition(e => e.Useridfk == result && e.Productid == customerproduct.Productid).FirstOrDefault();
                    if (existingCust != null && existingCust != default)
                    {
                        existingCust.Productprice = customerproduct.Productprice;
                        existingCust.Qtyassign = customerproduct.Qtyassign;
                        existingCust.Modifiedby = userResponse.RequestedBy;
                        existingCust.Modifieddate = DateTime.Now;
                        existingCust.Email = userResponse.Email;
                        _customerProductRepository.Update(existingCust);
                    }
                    else
                    {
                        customerproduct.Createdby = userResponse.RequestedBy;
                        customerproduct.Createddate = DateTime.Now;
                        _customerProductRepository.Create(customerproduct);
                    }

                }

            }

            return true;
        }

        public async Task<bool> CreateSubAdmin(SubAdminResponse userResponse)
        {
            return await SaveSubAdminDetails(userResponse);
        }

        public async Task<UserResponse> GetCustomer(string email)
        {
            return await GetUserDetails(email);
        }

        public async Task<UserResponse> GetSubAdmin(string email)
        {
            return await GetUserDetails(email);
        }

        private async Task<int> SaveUserDetails(UserResponse userResponse)
        {
            var checkExistinguser = _userRepository.FindByCondition(e => e.Email == userResponse.Email).FirstOrDefault();
            int userId;

            if (checkExistinguser == null)
            {
                var customer = new Users()
                {
                    Accountstatus = userResponse.AccountStatus.ToLower() == "active" ? 1 : 0,
                    Contactnumber = userResponse.ContactNumber,
                    Email = userResponse.Email,
                    Firstname = userResponse.FirstName,
                    Lastname = userResponse.LastName,
                    Password = userResponse.Password,
                    Salt = userResponse.Salt,
                    Accounttype = userResponse.AccountType,
                    Rightsforcustomeraccount = userResponse.canEditCustomer,
                    Createdby = userResponse.RequestedBy,
                    Createddate = DateTime.Now,
                    Role = (Role)Enum.Parse(typeof(Role), userResponse.Role),
                    Rolename = userResponse.Role

                };
                var user = _userRepository.Create(customer);
                userId = user.Userid;
            }
            else
            {
                checkExistinguser.Accountstatus = userResponse.AccountStatus.ToLower() == "active" ? 1 : 0;
                checkExistinguser.Contactnumber = userResponse.ContactNumber;
                checkExistinguser.Email = userResponse.Email;
                checkExistinguser.Firstname = userResponse.FirstName;
                checkExistinguser.Salt = userResponse.Salt;
                checkExistinguser.Lastname = userResponse.LastName;
                //checkExistinguser.Password = userResponse.Password;
                checkExistinguser.Accounttype = userResponse.AccountType;
                checkExistinguser.Rightsforcustomeraccount = userResponse.canEditCustomer;
                checkExistinguser.RightsForOrder = userResponse.canEditOrders;
                checkExistinguser.RightsForProduct = userResponse.canEditProducts;
                checkExistinguser.Modifieddate = DateTime.Now;
                checkExistinguser.Modifiedby = userResponse.RequestedBy;

                checkExistinguser.Role = (Role)Enum.Parse(typeof(Role), userResponse.Role);
                _userRepository.Update(checkExistinguser);

                userId = checkExistinguser.Userid;
            }

            var checkExistinguserdetails = _userDetailsRepository.FindByCondition(e => e.UseridFk == userId).FirstOrDefault();

            if (checkExistinguserdetails == null)
            {

                var customerDetails = new Userdetails()
                {
                    Address = userResponse.Address,
                    City = userResponse.City,
                    Coutry = userResponse.Country,
                    Createddate = DateTime.Now,
                    Createdby = userResponse.RequestedBy,
                    Creditlimit = userResponse.CreditLimit,
                    Soareceviedamount = userResponse.SoareceviedAmount,
                    State = userResponse.State,
                    UseridFk = userId
                };

                _userDetailsRepository.Create(customerDetails);
            }
            else
            {
                checkExistinguserdetails.Address = userResponse.Address;
                checkExistinguserdetails.City = userResponse.City;
                checkExistinguserdetails.Coutry = userResponse.Country;
                checkExistinguserdetails.Creditlimit = userResponse.CreditLimit;
                checkExistinguserdetails.Soareceviedamount = userResponse.SoareceviedAmount;
                checkExistinguserdetails.State = userResponse.State;
                checkExistinguserdetails.Modifieddate = DateTime.Now;
                checkExistinguserdetails.Modifiedby = userResponse.RequestedBy;

                _userDetailsRepository.Update(checkExistinguserdetails);
            }

            return userId;
        }
        private async Task<bool> SaveSubAdminDetails(SubAdminResponse userResponse)
        {
            var checkExistinguser = _userRepository.FindByCondition(e => e.Email == userResponse.Email).FirstOrDefault();
            int userId;

            if (checkExistinguser == null)
            {
                var customer = new Users()
                {
                    Accountstatus = userResponse?.AccountStatus.ToLower() == "active" ? 1 : 0,
                    Contactnumber = userResponse.ContactNumber,
                    Email = userResponse.Email,
                    Firstname = userResponse.FirstName,
                    Lastname = userResponse.LastName,
                    Password = userResponse.Password,
                    Salt = userResponse.Salt,
                    Accounttype = userResponse.AccountType,
                    Rightsforcustomeraccount = userResponse.canEditCustomer,
                    RightsForOrder = userResponse.canEditOrders,
                    RightsForProduct = userResponse.canEditProducts,
                    Createdby = userResponse.RequestedBy,
                    Createddate = DateTime.Now,
                    Role = (Role)Enum.Parse(typeof(Role), userResponse.Role),
                    Rolename = userResponse.Role

                };
                var user = _userRepository.Create(customer);
                userId = user.Userid;
            }
            else
            {
                checkExistinguser.Accountstatus = userResponse.AccountStatus.ToLower() == "active" ? 1 : 0;
                checkExistinguser.Contactnumber = userResponse.ContactNumber;
                checkExistinguser.Email = userResponse.Email;
                checkExistinguser.Firstname = userResponse.FirstName;
                //checkExistinguser.Salt = userResponse.Salt;
                checkExistinguser.Lastname = userResponse.LastName;
                //checkExistinguser.Password = userResponse.Password;
                checkExistinguser.Accounttype = userResponse.AccountType;
                checkExistinguser.Rightsforcustomeraccount = userResponse.canEditCustomer;
                checkExistinguser.RightsForOrder = userResponse.canEditOrders;
                checkExistinguser.RightsForProduct = userResponse.canEditProducts;
                checkExistinguser.Modifieddate = DateTime.Now;
                checkExistinguser.Modifiedby = userResponse.RequestedBy;
                checkExistinguser.Rolename = userResponse.Role;

                checkExistinguser.Role = (Role)Enum.Parse(typeof(Role), userResponse.Role);
                _userRepository.Update(checkExistinguser);

                userId = checkExistinguser.Userid;
            }

            return true;
        }
        private async Task<UserResponse> GetUserDetails(string email)
        {
            var checkExistinguser = _userRepository.FindByCondition(e => e.Email == email).FirstOrDefault();
            var checkExistinguserdetails = _userDetailsRepository.FindByCondition(e => e.UseridFk == checkExistinguser.Userid).FirstOrDefault();


            var customerResponse = new UserResponse
            {
                UserId = checkExistinguser.Userid,
                AccountStatus = checkExistinguser.Accountstatus == 1 ? "Active" : "Hold",
                ContactNumber = checkExistinguser.Contactnumber,
                Email = checkExistinguser.Email,
                FirstName = checkExistinguser.Firstname,
                LastName = checkExistinguser.Lastname,
                AccountType = checkExistinguser.Accounttype,
                Address = checkExistinguserdetails?.Address,
                City = checkExistinguserdetails?.City,
                Country = checkExistinguserdetails?.Coutry,
                canEditCustomer = checkExistinguser.Rightsforcustomeraccount,
                canEditOrders = checkExistinguser.RightsForOrder,
                canEditProducts = checkExistinguser.RightsForProduct,
                CreditLimit = checkExistinguserdetails?.Creditlimit,
                SoareceviedAmount = checkExistinguserdetails?.Soareceviedamount,
                State = checkExistinguserdetails?.State,
                Role = checkExistinguser.Rolename
            };

            return customerResponse;
        }

        public async Task<IList<UserResponse>> GetAllUserDetails(int userType)
        {
            return _applicationDBContext.Users.Where(u => u.Accounttype == userType).Join(_applicationDBContext.Userdetails, i => i.Userid, o => o.UseridFk,
                     (i, o) => new UserResponse()
                     {
                         UserId = i.Userid,
                         AccountStatus = i.Accountstatus == 1 ? "Active" : "Hold",
                         ContactNumber = i.Contactnumber,
                         Email = i.Email,
                         FirstName = i.Firstname,
                         LastName = i.Lastname,
                         AccountType = i.Accounttype,
                         Address = o.Address,
                         City = o.City,
                         Country = o.Coutry,
                         canEditCustomer = i.Rightsforcustomeraccount,
                         canEditOrders = i.RightsForOrder,
                         canEditProducts = i.RightsForProduct,
                         CreditLimit = o.Creditlimit,
                         SoareceviedAmount = o.Soareceviedamount,
                         State = o.State
                     }
                 ).ToList();

        }

        public async Task<IList<UserResponse>> GetAllSubAdminUserDetails(int userType)
        {
            return _applicationDBContext.Users.Where(u => u.Accounttype == userType).Select(i => new UserResponse()
            {
                UserId = i.Userid,
                AccountStatus = i.Accountstatus == 1 ? "Active" : "Hold",
                ContactNumber = i.Contactnumber,
                Email = i.Email,
                FirstName = i.Firstname,
                LastName = i.Lastname,
                AccountType = i.Accounttype,
                canEditCustomer = i.Rightsforcustomeraccount,
                canEditOrders = i.RightsForOrder,
                canEditProducts = i.RightsForProduct

            }
                 ).ToList();

        }

        public async Task<IList<CustomerPriorityResponse>> GetAllUserPriority()
        {
            return _applicationDBContext.Users.Where(u => u.Accounttype == 1).Select(i => new CustomerPriorityResponse()
            {
                Userid = i.Userid,
                Firstname = i.Firstname,
                Lastname = i.Lastname,
                Priority = i.Priority ?? 0,
                Email = i.Email
            }).ToList();
        }

        public async Task<bool> UpdateUserPriority(List<CustomerPriorityResponse> customerPriorityResponse)
        {
            var checkExistinguser = (from cus in customerPriorityResponse
                        join user in _applicationDBContext.Users.AsNoTracking()
                        on cus.Userid equals user.Userid
                        select new Users
                        {
                            Userid = user.Userid,
                            Accountstatus = user.Accountstatus,
                            Accounttype = user.Accounttype,
                            Contactnumber = user.Contactnumber,
                            Createdby = user.Createdby,
                            Createddate = user.Createddate,
                            Customerproduct = user.Customerproduct,
                            Email = user.Email,
                            Firstname = user.Firstname,
                            Lastname = user.Lastname,
                            Modifiedby = user.Modifiedby,
                            Modifieddate = user.Modifieddate,
                            Password = user.Password,
                            Priority = cus.Priority,
                            Rightsforcustomeraccount = user.Rightsforcustomeraccount,
                            RightsForOrder = user.RightsForOrder,
                            RightsForProduct = user.RightsForProduct,
                            Role = user.Role,
                            Rolename = user.Rolename,
                            Salt = user.Salt,
                            Userdetails = user.Userdetails
                        }).ToList();
                       

            //var checkExistinguser = customerPriorityResponse.Join(
            //    _applicationDBContext.Users,
            //    u => u.Userid,
            //    c => c.Userid,
            //    (c, u) => new Users()
            //    {
            //        Userid = u.Userid,
            //        Email = u.Email,
            //        Accountstatus = u.Accountstatus,
            //        Accounttype = u.Accounttype,
            //        Contactnumber = u.Contactnumber,
            //        Createdby = u.Createdby,
            //        Createddate = u.Createddate,
            //        Customerproduct = u.Customerproduct,
            //        Firstname = u.Firstname,
            //        Lastname = u.Lastname,
            //        Modifiedby = u.Modifiedby,
            //        Modifieddate = u.Modifieddate,
            //        Password = u.Password,
            //        Priority = c.Priority,
            //        Rightsforcustomeraccount = u.Rightsforcustomeraccount,
            //        RightsForOrder = u.RightsForOrder,
            //        RightsForProduct = u.RightsForProduct,
            //        Role = u.Role,
            //        Rolename = u.Rolename,
            //        Salt = u.Salt
            //    }
            //    ).ToList();

            // _applicationDBContext.ChangeTracker.Entries<Users>();
            foreach(var u in checkExistinguser)
            {
                _applicationDBContext.Entry(u).State = EntityState.Detached;
                _applicationDBContext.Users.Update(u);
            }
            _applicationDBContext.SaveChanges();
            return true;
        }

        public async Task<bool> DeleteUserDetails(string email)
        {
            var checkExistinguser = _userRepository.FindByCondition(e => e.Email == email).FirstOrDefault();
            if (checkExistinguser != null)
            {
                var checkExistinguserdetails = _userDetailsRepository.FindByCondition(e => e.UseridFk == checkExistinguser.Userid).FirstOrDefault();

                if (checkExistinguserdetails != null)
                {
                    _userDetailsRepository.Delete(checkExistinguserdetails);
                }

                _applicationDBContext.Customerproduct.RemoveRange(_customerProductRepository.FindByCondition(e => e.Useridfk == checkExistinguser.Userid));
                _applicationDBContext.SaveChanges();
                _userRepository.Delete(checkExistinguser);
            }

            return true;

        }

        public Task<List<SOADetails>> GetSOADetails(string customerId)
        {

            var result = (from order in _applicationDBContext.Orders
                          join od in _applicationDBContext.OrdersProducts on order.Ordersid equals od.OrdersID
                          where order.Status == "Completed"
                          group od by new { order.Ordersid, order.Date, od.Productid } into users
                          select new SOADetails()
                          {
                              OrderDate = users.Key.Date,
                              OrderId = users.Key.Ordersid,
                              TotalAmount = users.Sum(e => Convert.ToDouble(e.Totalpricecustomerpays)),
                              Quantity = users.Sum(e => e.Quantity),
                              ProductId = users.Key.Productid
                          }).ToList();

            return Task.FromResult(result);
        }
        public Task<DashBoardDetails> GetDashBoardDetails(string customerId)
        {

            var userRole = _applicationDBContext.Users.Where(u => u.Email == customerId).FirstOrDefault()?.Rolename ?? "";


            var orders = (from order in _applicationDBContext.Orders
                          where order.Date <= DateTime.Now && order.Date > (DateTime.Now.AddDays(-7))
                          select new
                          {
                              Orderno = order.Ordersid,
                              TrackingNo= order.TrackingNo,
                              Status = order.Status                              
                          }
                          ).ToList();

            var result = new DashBoardDetails()
            {
                OrderCompleted = orders.Where(e => !string.IsNullOrEmpty(e.TrackingNo) ).Count(),
                OrdersInProgress = orders.Where(e => string.IsNullOrEmpty(e.TrackingNo) && e.Status != "OnHold").Count(),
                OrderHolds = orders.Where(e => e.Status == "OnHold").Count(),
                RecentOrders = orders.Count()
            };

            return Task.FromResult(result);
        }
    }
}
