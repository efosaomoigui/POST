using GIGLS.Core.DTO.Customers;
using System.Threading.Tasks;
using GIGLS.Core.Enums;
using System.Collections.Generic;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.Domain;
using System;
using GIGLS.Core.DTO.User;

namespace GIGLS.Core.IServices.Customers
{
    public interface ICustomerService : IServiceDependencyMarker
    {
        Task<CustomerDTO> CreateCustomer(CustomerDTO customer);
        Task<CustomerDTO> GetGIGLCorporateAccount();
        Task<CustomerDTO> GetCustomer(int customerId, CustomerType customerType);
        Task<IndividualCustomerDTO> GetCustomerByPhoneNumber(string phoneNumber);
        Task<List<CustomerDTO>> GetCustomers(CustomerType customerType);
        Task<List<CustomerDTO>> SearchForCustomers(CustomerSearchOption searchOption);
        Task<CustomerDTO> GetCustomer(string userChannelCode, UserChannelType userChannelType);
        Task<CustomerDTO> CreateCustomerIntl(CustomerDTO customerDTO);
        Task<IndividualCustomerDTO> GetCustomerByCode(string customerCode);
        Task<List<ShipmentActivityDTO>> GetShipmentActivities(string waybill);
        Task<DeliveryNumberDTO> GetDeliveryNoByWaybill(string waybill);
        Task<Object> GetCustomerBySearchParam(string customerType, SearchOption option);
        Task<UserDTO> GetInternationalUser(string email);
        Task<bool> DeactivateInternationalUser(string email);
    }
}
