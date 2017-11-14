using System;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Customers;
using GIGLS.Core.IServices.Customers;
using GIGLS.Core;
using AutoMapper;
using GIGLS.Core.Enums;

namespace GIGLS.Services.Implementation.Customers
{
    public class CustomerService : ICustomerService
    {
        public IIndividualCustomerService _individualCustomerService { get; set; }
        public ICompanyService _companyService { get; set; }
        public IUnitOfWork _uow { get; set; }

        public CustomerService(IUnitOfWork uow,
            IIndividualCustomerService individualCustomerService, ICompanyService companyService)
        {
            _uow = uow;
            _individualCustomerService = individualCustomerService;
            _companyService = companyService;
            MapperConfig.Initialize();
        }

        public async Task<CustomerDTO> CreateCustomer(CustomerDTO customerDTO)
        {
            try
            {

                // handle Company customers
                if (CustomerType.Company.Equals(customerDTO.CustomerType))
                {
                    var companyId = 0;
                    var CompanyByName = await _uow.Company.FindAsync(c => c.Name.ToLower() == customerDTO.Name.Trim().ToLower());
                    foreach (var item in CompanyByName)
                    {
                        companyId = item.CompanyId;
                    }

                    if (companyId > 0)
                    {
                        // update
                        customerDTO.CompanyId = companyId;
                        var companyDTO = Mapper.Map<CompanyDTO>(customerDTO);
                        await _companyService.UpdateCompany(companyId, companyDTO);
                    }
                    else
                    {
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
                    var individualCustomerId = 0;
                    var individualCustomerByPhone = await _uow.IndividualCustomer.FindAsync(c => c.PhoneNumber == customerDTO.PhoneNumber.Trim());
                    var individualCustomerByEmail = await _uow.IndividualCustomer.FindAsync(c => c.Email == customerDTO.Email.Trim());
                    foreach(var item in individualCustomerByPhone)
                    {
                         individualCustomerId = item.IndividualCustomerId;
                    }
                    foreach (var item in individualCustomerByEmail)
                    {
                         individualCustomerId = item.IndividualCustomerId;
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
                        // create new
                        var individualCustomerDTO = Mapper.Map<IndividualCustomerDTO>(customerDTO);
                        var createdCustomer = await _individualCustomerService.AddCustomer(individualCustomerDTO);
                        customerDTO.IndividualCustomerId = createdCustomer.IndividualCustomerId;
                    }
                }

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

    }
}
