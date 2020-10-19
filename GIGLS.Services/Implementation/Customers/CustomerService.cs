using System;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Customers;
using GIGLS.Core.IServices.Customers;
using GIGLS.Core;
using AutoMapper;
using GIGLS.Core.Enums;
using System.Collections.Generic;
using GIGLS.CORE.Enums;
using System.Configuration;
using GIGLS.Core.DTO.Shipments;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Linq;
using GIGLS.Core.Domain;

namespace GIGLS.Services.Implementation.Customers
{
    public class CustomerService : ICustomerService
    {
        private readonly IIndividualCustomerService _individualCustomerService;
        private readonly ICompanyService _companyService;
        private readonly IUnitOfWork _uow;

        public CustomerService(IUnitOfWork uow,IIndividualCustomerService individualCustomerService, ICompanyService companyService)
        {
            _uow = uow;
            _individualCustomerService = individualCustomerService;
            _companyService = companyService;
            MapperConfig.Initialize();
        }
        
        private async Task<string> AddCountryCodeToPhoneNumber(string phoneNumber, int countryId)
        {
            var country = await _uow.Country.GetAsync(x => x.CountryId == countryId);
            if (country != null)
            {
                phoneNumber = phoneNumber.Substring(1, phoneNumber.Length - 1);
                string phone = $"{country.PhoneNumberCode}{phoneNumber}";
                phoneNumber = phone;

            }
            return phoneNumber;
        }

        public async Task<CustomerDTO> CreateCustomer(CustomerDTO customerDTO)
        {
            try
            {
                // handle Company customers
                if (CustomerType.Company.Equals(customerDTO.CustomerType))
                {
                    int companyId = 0;

                    var companyByCode = await _uow.Company.GetAsync(x => x.CustomerCode == customerDTO.CustomerCode);

                    if(companyByCode == null)
                    {
                        var CompanyByName = await _uow.Company.FindAsync(c => c.Name.ToLower() == customerDTO.Name.ToLower()
                        || c.PhoneNumber == customerDTO.PhoneNumber || c.Email == customerDTO.Email || c.CustomerCode == customerDTO.CustomerCode);

                        foreach (var item in CompanyByName)
                        {
                            companyId = item.CompanyId;
                        }
                    }
                    else
                    {
                        companyId = companyByCode.CompanyId;
                    }

                    if (companyId > 0)
                    {
                        customerDTO.CompanyId = companyId;
                        var companyDTO = Mapper.Map<CompanyDTO>(customerDTO);
                        //await _companyService.UpdateCompany(companyId, companyDTO);
                    }
                    else
                    {
                        if (customerDTO.PhoneNumber.StartsWith("0"))
                        {
                            customerDTO.PhoneNumber = await AddCountryCodeToPhoneNumber(customerDTO.PhoneNumber, customerDTO.UserActiveCountryId);
                        }

                        // create new
                        var companyDTO = Mapper.Map<CompanyDTO>(customerDTO);
                        var createdCustomer = await _companyService.AddCompany(companyDTO);
                        customerDTO = Mapper.Map<CustomerDTO>(companyDTO);
                        customerDTO.CompanyId = createdCustomer.CompanyId;
                    }
                }

                // handle IndividualCustomers
                if (CustomerType.IndividualCustomer.Equals(customerDTO.CustomerType))
                {
                    int individualCustomerId = 0;
                    var individualCustomerByPhone = await _uow.IndividualCustomer.
                        GetAsync(c => c.PhoneNumber == customerDTO.PhoneNumber || c.CustomerCode == customerDTO.CustomerCode);

                    if(individualCustomerByPhone != null)
                    {
                        individualCustomerId = individualCustomerByPhone.IndividualCustomerId;
                    }

                    if (individualCustomerId > 0)
                    {
                        // update
                        customerDTO.IndividualCustomerId = individualCustomerId;
                        var individualCustomerDTO = Mapper.Map<IndividualCustomerDTO>(customerDTO);
                        await _individualCustomerService.UpdateCustomer(individualCustomerId, individualCustomerDTO);
                    }
                    else
                    {
                        if (customerDTO.PhoneNumber.StartsWith("0"))
                        {
                            customerDTO.PhoneNumber = await AddCountryCodeToPhoneNumber(customerDTO.PhoneNumber, customerDTO.UserActiveCountryId);
                        }

                        // create new
                        var individualCustomerDTO = Mapper.Map<IndividualCustomerDTO>(customerDTO);
                        var createdCustomer = await _individualCustomerService.AddCustomer(individualCustomerDTO);
                        customerDTO.IndividualCustomerId = createdCustomer.IndividualCustomerId;
                        customerDTO.CustomerCode = createdCustomer.CustomerCode;
                    }
                }

                return customerDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CustomerDTO> CreateCustomerIntl(CustomerDTO customerDTO) 
        {
            try
            {
                // handle Company customers
                if (CustomerType.Company.Equals(customerDTO.CustomerType))
                {
                    int companyId = 0;

                    var companyByCode = await _uow.Company.GetAsync(x => x.CustomerCode == customerDTO.CustomerCode);

                    if (companyByCode == null)
                    {
                        var CompanyByName = await _uow.Company.FindAsync(c => c.Name.ToLower() == customerDTO.Name.ToLower()
                        || c.PhoneNumber == customerDTO.PhoneNumber || c.Email == customerDTO.Email || c.CustomerCode == customerDTO.CustomerCode);

                        foreach (var item in CompanyByName)
                        {
                            companyId = item.CompanyId;
                        }
                    }
                    else
                    {
                        companyId = companyByCode.CompanyId;
                    }

                    if (companyId > 0)
                    {
                        customerDTO.CompanyId = companyId;
                        var companyDTO = Mapper.Map<CompanyDTO>(customerDTO);
                        //await _companyService.UpdateCompany(companyId, companyDTO);
                    }
                    else
                    {
                        if (customerDTO.PhoneNumber.StartsWith("0"))
                        {
                            customerDTO.PhoneNumber = await AddCountryCodeToPhoneNumber(customerDTO.PhoneNumber, customerDTO.UserActiveCountryId);
                        }

                        // create new
                        var companyDTO = Mapper.Map<CompanyDTO>(customerDTO);
                        var createdCustomer = await _companyService.AddCompany(companyDTO);
                        customerDTO = Mapper.Map<CustomerDTO>(companyDTO);
                        customerDTO.CompanyId = createdCustomer.CompanyId;
                    }
                }

                // handle IndividualCustomers
                if (CustomerType.IndividualCustomer.Equals(customerDTO.CustomerType))
                {
                    int individualCustomerId = 0;
                    var individualCustomerByPhone = await _uow.IndividualCustomer.
                        GetAsync(c => c.PhoneNumber == customerDTO.PhoneNumber || c.CustomerCode == customerDTO.CustomerCode);

                    if (individualCustomerByPhone != null)
                    {
                        individualCustomerId = individualCustomerByPhone.IndividualCustomerId;
                    }

                    if (individualCustomerId > 0)
                    {
                        // update
                        customerDTO.IndividualCustomerId = individualCustomerId;
                        var individualCustomerDTO = Mapper.Map<IndividualCustomerDTO>(customerDTO);
                        await _individualCustomerService.UpdateCustomer(individualCustomerId, individualCustomerDTO);
                    }
                    else
                    {
                        if (customerDTO.PhoneNumber.StartsWith("0"))
                        {
                            customerDTO.PhoneNumber = await AddCountryCodeToPhoneNumber(customerDTO.PhoneNumber, customerDTO.UserActiveCountryId);
                        }

                        // create new
                        var individualCustomerDTO = Mapper.Map<IndividualCustomerDTO>(customerDTO);
                        var createdCustomer = await _individualCustomerService.AddCustomer(individualCustomerDTO);
                        customerDTO.IndividualCustomerId = createdCustomer.IndividualCustomerId;
                        customerDTO.CustomerCode = createdCustomer.CustomerCode;
                    }
                }

                return customerDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get Store Keeper as Corporate Customer
        public async Task<CustomerDTO> GetGIGLCorporateAccount()
        {
            try
            {
                string accountCode = ConfigurationManager.AppSettings["GIGLCorporateAccount"];
                var customerDTO = await GetCustomer(accountCode, UserChannelType.Corporate);
                return customerDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CustomerDTO> GetCustomer(int customerId, CustomerType customerType)
        {
            try
            {
                // handle Company customers
                if (CustomerType.Company.Equals(customerType))
                {
                    var companyDTO = await _companyService.GetCompanyById(customerId);
                    var customerDTO = Mapper.Map<CustomerDTO>(companyDTO);
                    customerDTO.CustomerType = CustomerType.Company;

                    return customerDTO;
                }                
                else
                {
                    // handle IndividualCustomers
                    var individualCustomerDTO = await _individualCustomerService.GetCustomerById(customerId);
                    var customerDTO = Mapper.Map<CustomerDTO>(individualCustomerDTO);
                    customerDTO.CustomerType = CustomerType.IndividualCustomer;

                    return customerDTO;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CustomerDTO> GetCustomer(string userChannelCode, UserChannelType userChannelType)
        {
            try
            {
                if (UserChannelType.Ecommerce.Equals(userChannelType) || UserChannelType.Corporate.Equals(userChannelType))
                {
                    var companyDTO = await _companyService.GetCompanyByCode(userChannelCode);
                    var customerDTO = Mapper.Map<CustomerDTO>(companyDTO);
                    customerDTO.CustomerType = CustomerType.Company;
                    return customerDTO;
                }
                else if (UserChannelType.IndividualCustomer.Equals(userChannelType))
                {
                    var individualCustomerDTO = await _individualCustomerService.GetCustomerByCode(userChannelCode);
                    var customerDTO = Mapper.Map<CustomerDTO>(individualCustomerDTO);
                    customerDTO.CustomerType = CustomerType.IndividualCustomer;
                    return customerDTO;
                }
                return new CustomerDTO { };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<CustomerDTO>> GetCustomers(CustomerType customerType)
        {
            try
            {
                // handle Company customers
                if (CustomerType.IndividualCustomer.Equals(customerType))
                {
                    var individualCustomerDTO = await _individualCustomerService.GetIndividualCustomers();
                    var customerDTO = Mapper.Map<List<CustomerDTO>>(individualCustomerDTO);

                    foreach (var item in customerDTO)
                    {
                        item.CustomerType = CustomerType.IndividualCustomer;
                    }
                    return customerDTO;
                }
                else
                {
                    var companyDTO = await _companyService.GetCompanies();
                    var customerDTO = Mapper.Map<List<CustomerDTO>>(companyDTO);

                    foreach (var item in customerDTO)
                    {
                        item.CustomerType = CustomerType.Company;
                    }

                    return customerDTO;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IndividualCustomerDTO> GetCustomerByPhoneNumber(string phoneNumber)
        {
            try
            {
                if (phoneNumber.StartsWith("0"))
                {
                    phoneNumber = phoneNumber.Remove(0, 1);
                }
                // handle IndividualCustomers
                var individualCustomerDTO = await _individualCustomerService.GetCustomerByPhoneNumber(phoneNumber);
                return individualCustomerDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<CustomerDTO>> SearchForCustomers(CustomerSearchOption searchOption)
        {
            try
            {
                if (searchOption.SearchData.StartsWith("0"))
                {
                    searchOption.SearchData = searchOption.SearchData.Substring(1, searchOption.SearchData.Length - 1);
                }

                // handle Company customers
                if (FilterCustomerType.IndividualCustomer.Equals(searchOption.CustomerType))
                {
                    var individualCustomerDTO = await _individualCustomerService.GetIndividualCustomers(searchOption.SearchData);
                    var customerDTO = Mapper.Map<List<CustomerDTO>>(individualCustomerDTO);

                    foreach (var item in customerDTO)
                    {
                        item.CustomerType = CustomerType.IndividualCustomer;
                        item.CompanyId = item.IndividualCustomerId;
                    }
                    return customerDTO;
                }
                else
                {
                    CompanyType companyType;

                    if (FilterCustomerType.Corporate.Equals(searchOption.CustomerType))
                    {
                        companyType = CompanyType.Corporate;
                    }
                    else
                    {
                        companyType = CompanyType.Ecommerce;
                    }
                    var companyDTO = await _companyService.GetCompanies(companyType, searchOption);
                    var customerDTO = Mapper.Map<List<CustomerDTO>>(companyDTO);

                    foreach (var item in customerDTO)
                    {
                        item.CustomerType = CustomerType.Company;
                        item.IndividualCustomerId = item.CompanyId;
                    }

                    return customerDTO;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IndividualCustomerDTO> GetCustomerByCode(string customerCode)
        {
            try
            {
                var individualCustomerDTO = await _individualCustomerService.GetCustomerByCode(customerCode);
                return individualCustomerDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DeliveryNumberDTO> GetDeliveryNoByWaybill(string waybill)
        {
            try
            {
                var dto = new DeliveryNumberDTO();
                var item = await _uow.DeliveryNumber.GetAsync(x => x.Waybill == waybill);
                dto = Mapper.Map<DeliveryNumberDTO>(item);
                return dto;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<ShipmentActivityDTO>> GetShipmentActivities(string waybill)
        {
            var shipmentActivity = new List<ShipmentActivityDTO>();
            HttpClient client = new HttpClient();
            var nodeURL = ConfigurationManager.AppSettings["NodeBaseUrl"];
            var url = ConfigurationManager.AppSettings["NodeGetShipmentByWaybill"];
            nodeURL = $"{nodeURL}{url}?waybill={waybill}&exPickUpList=yes&exUpdate=yes&exActivejobs=yes";
         
            HttpResponseMessage response = await client.GetAsync(nodeURL);
            //response.EnsureSuccessStatusCode();
            string jObject = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<DataResponse>(jObject);

            if (result.Data.ApiList.Any())
            {
                foreach (var item in result.Data.ApiList)
                {
                    //get actions performed on shipment
                   int actionIndx = item.Url.LastIndexOf('/');
                    actionIndx++;
                    var action = item.Url.Substring(actionIndx);
                    var partnerName = String.Empty;
                    var partnerPhoneNo = String.Empty;
                    if (item.Body.PartnerId != null)
                    {
                        var partnerInfo = await _uow.Partner.GetPartnerByUserId(item.Body.PartnerId);
                        if (partnerInfo != null)
                        {
                            partnerName = partnerInfo.PartnerName;
                            partnerPhoneNo = partnerInfo.PhoneNumber; 
                        }
                    }
                    var obj = new ShipmentActivityDTO
                    {
                        Action = action.ToUpper(),
                        ActionBy = partnerName,
                        ActionTime = item.CreationDate,
                        ActionReason = item.ReasonText,
                        CreatedOn = item.CreationDate
                    };
                    if (item.StatusCode == "200")
                    {
                        obj.ActionResult = "SUCCESSFUL";
                    }
                    else
                    {
                        obj.ActionResult = "FAILED";
                    }
                    obj.PhoneNo = partnerPhoneNo;
                    obj.Waybill = waybill;
                    shipmentActivity.Add(obj);
                }
            }
            return shipmentActivity;
        }

    }
}
