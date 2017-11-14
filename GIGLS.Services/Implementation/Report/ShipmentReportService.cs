using GIGLS.Core;
using GIGLS.CORE.IServices.Report;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Shipments;
using GIGLS.CORE.DTO.Report;

namespace GIGLS.Services.Implementation.Report
{
    public class ShipmentReportService : IShipmentReportService
    {
        private readonly IUnitOfWork _uow;

        public ShipmentReportService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<List<ShipmentDTO>> GetShipments(ShipmentFilterCriteria filterCriteria)
        {
            return await _uow.Shipment.GetShipments(filterCriteria);
        }

        public async Task<List<ShipmentDTO>> GetTodayShipments()
        {
            ShipmentFilterCriteria filterCriteria = new ShipmentFilterCriteria();
            filterCriteria.StartDate = System.DateTime.Today;
            return await _uow.Shipment.GetShipments(filterCriteria);
        }
    }
}
