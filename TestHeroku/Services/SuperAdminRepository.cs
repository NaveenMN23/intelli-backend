using IntelliCRMAPIService.Attribute;
using IntelliCRMAPIService.BL;
using IntelliCRMAPIService.DBContext;
using IntelliCRMAPIService.Model;
using IntelliCRMAPIService.Repository;
using System.Data;
using System.Linq;

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

            if(result && userResponse.UploadFile !=null)
            {
                var productTable = await _excelConverter.ConvertToDataTable(userResponse.UploadFile.OpenReadStream());

                foreach (DataRow row in productTable.Rows)
                {
                    var values = row.ItemArray;

                    if (Convert.ToString(values[1]) == "")
                        continue;

                    var customerproduct = new Customerproduct()
                    {
                        Productid = Convert.ToInt32(values[0]),
                        Productname = Convert.ToString(values[1]),
                        Productprice = Convert.ToString(values[2]),
                        Qtyassign = Convert.ToString(values[3]),
                        Useridfk = userResponse.UserId
                    };

                    var existingCust = _customerProductRepository.FindByCondition(e => e.Useridfk == userResponse.UserId && e.Productid == customerproduct.Productid).FirstOrDefault();
                    if (existingCust != null && existingCust != default)
                    {
                        existingCust.Productprice = customerproduct.Productprice;
                        existingCust.Qtyassign = customerproduct.Qtyassign;
                        existingCust.Modifiedby = userResponse.RequestedBy;
                        existingCust.Modifieddate = DateTime.Now;
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

            return result;
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

        private async Task<bool> SaveUserDetails(UserResponse userResponse)
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
                    Rightsforcustomeraccount = userResponse.RightsForCustomerAccount,
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
                checkExistinguser.Rightsforcustomeraccount = userResponse.RightsForCustomerAccount;
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

            return true;
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
                    Rightsforcustomeraccount = userResponse.RightsForCustomerAccount,
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
                checkExistinguser.Rightsforcustomeraccount = userResponse.RightsForCustomerAccount;
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
                AccountStatus = checkExistinguser.Accountstatus == 1 ? "Active" :"Hold",
                ContactNumber = checkExistinguser.Contactnumber,
                Email = checkExistinguser.Email,
                FirstName = checkExistinguser.Firstname,
                LastName = checkExistinguser.Lastname,
                AccountType = checkExistinguser.Accounttype,
                Address = checkExistinguserdetails?.Address,
                City = checkExistinguserdetails?.City,
                Country = checkExistinguserdetails?.Coutry,
                RightsForCustomerAccount = checkExistinguser.Rightsforcustomeraccount,
                CreditLimit = checkExistinguserdetails?.Creditlimit,
                SoareceviedAmount = checkExistinguserdetails?.Soareceviedamount,
                State = checkExistinguserdetails?.State
            };

            return customerResponse;
        }

        public async Task<IList<UserResponse>> GetAllUserDetails(int userType)
        {
           return _applicationDBContext.Users.Where(u => u.Accounttype == userType ).Join(_applicationDBContext.Userdetails, i => i.Userid, o => o.UseridFk,
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
                        RightsForCustomerAccount = i.Rightsforcustomeraccount,
                        CreditLimit = o.Creditlimit,
                        SoareceviedAmount = o.Soareceviedamount,
                        State = o.State
                    }
                ).ToList();

        }

        public async Task<IList<UserResponse>> GetAllSubAdminUserDetails(int userType)
        {
            return _applicationDBContext.Users.Where(u => u.Accounttype == userType).Select( i => new UserResponse()
                     {
                         UserId = i.Userid,
                         AccountStatus = i.Accountstatus == 1 ? "Active" : "Hold",
                         ContactNumber = i.Contactnumber,
                         Email = i.Email,
                         FirstName = i.Firstname,
                         LastName = i.Lastname,
                         AccountType = i.Accounttype,
                         RightsForCustomerAccount = i.Rightsforcustomeraccount

                     }
                 ).ToList();

        }
    }
}
