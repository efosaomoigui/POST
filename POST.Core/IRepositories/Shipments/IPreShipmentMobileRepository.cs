using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using POST.Core.DTO.Customers;
using POST.Core.DTO.Report;
using POST.Core.DTO.Shipments;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Shipments
{
    public interface IPreShipmentMobileRepository : IRepository<PreShipmentMobile>
    {
        Task<List<PreShipmentMobileDTO>> GetPreShipmentsForMobile();
        Task<PreShipmentMobileDTO> GetBasicPreShipmentMobileDetail(string waybill);
        IQueryable<PreShipmentMobileDTO> GetPreShipmentForUser(string userChannelCode);
        IQueryable<PreShipmentMobileDTO> GetShipments(string userChannelCode);
        Task<List<OutstandingPaymentsDTO>> GetAllOutstandingShipmentsForUser(string userChannelCode);
        Task<List<PreShipmentMobileReportDTO>> GetPreShipments(MobileShipmentFilterCriteria accountFilterCriteria);
        IQueryable<PreShipmentMobileDTO> GetBatchedPreShipmentForUser(string userChannelCode);
        IQueryable<PreShipmentMobileDTO> GetAllBatchedPreShipment();
        Task<List<AddressDTO>> GetTopFiveUserAddresses(string userID, bool isIntl);
        Task<PreShipmentMobileDTO> GetPreshipmentMobileByWaybill(string waybill);
        Task<CustomerDTO> GetBotUserWithPhoneNo(string phonenumber);
        Task<OutstandingPaymentsDTO> GetEquivalentAmountOfActiveCurrency(CurrencyEquivalentDTO currencyEquivalent);

    }
}