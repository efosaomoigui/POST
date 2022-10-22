using POST.Core.DTO.Customers;
using System.Threading.Tasks;
using POST.Core.Enums;
using System.Collections.Generic;
using POST.Core.DTO.Shipments;
using POST.Core.Domain;
using System;
using POST.Core.DTO.User;
using POST.CORE.DTO.Report;
using POST.Core.DTO.Account;

namespace POST.Core.IServices.Customers
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
        Task<object> GetByCode(string customerCode);
        Task<List<InvoiceViewDTO>> GetCoporateMonthlyTransaction(DateFilterForDropOff filter);
    }
}
