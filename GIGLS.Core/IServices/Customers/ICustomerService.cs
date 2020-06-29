﻿using GIGLS.Core.DTO.Customers;
using System.Threading.Tasks;
using GIGLS.Core.Enums;
using System.Collections.Generic;

namespace GIGLS.Core.IServices.Customers
{
    public interface ICustomerService : IServiceDependencyMarker
    {
        Task<CustomerDTO> CreateCustomer(CustomerDTO customer);
        Task<CustomerDTO> CreateStoreKeeperCustomer(CustomerDTO customerDTO);
        Task<CustomerDTO> GetCustomer(int customerId, CustomerType customerType);
        Task<IndividualCustomerDTO> GetCustomerByPhoneNumber(string phoneNumber);
        Task<List<CustomerDTO>> GetCustomers(CustomerType customerType);
        Task<List<CustomerDTO>> SearchForCustomers(CustomerSearchOption searchOption);
        Task<CustomerDTO> GetCustomer(string userChannelCode, UserChannelType userChannelType);
    }
}
