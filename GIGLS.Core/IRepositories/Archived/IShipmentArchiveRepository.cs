using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain.Archived;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.Shipments;
using GIGLS.CORE.DTO.Report;
using GIGLS.CORE.DTO.Shipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Archived
{
    public interface IShipmentArchiveRepository : IRepository<Shipment_Archive>
    {
        Task<Tuple<List<ShipmentDTO>, int>> GetShipments(FilterOptionsDto filterOptionsDto, int[] serviceCentreIds);
        Task<Tuple<List<ShipmentDTO>, int>> GetDestinationShipments(FilterOptionsDto filterOptionsDto, int[] serviceCentreIds);
        Task<Tuple<List<ShipmentDTO>, int>> GetShipmentDetailByWaybills(FilterOptionsDto filterOptionsDto, int[] serviceCentreIds, List<string> waybills);
        Task<List<ShipmentDTO>> GetShipments(ShipmentFilterCriteria queryDto, int[] serviceCentreIds);
        Task<List<ShipmentDTO>> GetShipments(int[] serviceCentreIds);
        Task<List<ShipmentDTO>> GetCustomerShipments(ShipmentFilterCriteria f_Criteria);
        IQueryable<Shipment_Archive> ShipmentsAsQueryable();
        Task<ShipmentDTO> GetBasicShipmentDetail(string waybill);
        Task<List<InvoiceViewDTO>> GetSalesForServiceCentre(AccountFilterCriteria accountFilterCriteria, int[] serviceCentreIds);
        Tuple<Task<List<IntlShipmentRequestDTO>>, int> GetIntlTransactionShipmentRequest(FilterOptionsDto filterOptionsDto, int[] serviceCentreIds);
        Task<List<CODShipmentDTO>> GetCODShipments(BaseFilterCriteria baseFilterCriteria);
        Task<List<CargoMagayaShipmentDTO>> GetCargoMagayaShipments(BaseFilterCriteria baseFilterCriteria);
        Task<List<InvoiceViewDTO>> GetWaybillForServiceCentre(string waybill, int[] serviceCentreIds);
        Task<ShipmentDTO> GetShipment(string waybill);
    }
}
