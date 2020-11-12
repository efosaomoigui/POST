using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Report;
using GIGLS.Core.DTO.Shipments;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Shipments
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

    }
}