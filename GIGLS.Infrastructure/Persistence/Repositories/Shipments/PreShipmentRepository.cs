using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IRepositories.Shipments;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Linq;
using GIGLS.CORE.DTO.Report;
using GIGLS.Core.Enums;
using GIGLS.CORE.Enums;
using GIGLS.Core.DTO.Zone;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.CORE.DTO.Shipments;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.Report;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Shipments
{
    public class PreShipmentRepository : Repository<PreShipment, GIGLSContext>, IPreShipmentRepository
    {
        private GIGLSContext _context;
        public PreShipmentRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<PreShipment> PreShipmentsAsQueryable()
        {
            var preShipments = _context.PreShipment.AsQueryable();
            return preShipments;
        }

        public Task<List<PreShipmentDTO>> GetDropOffsForUser(ShipmentCollectionFilterCriteria filterCriteria, string currentUserId)
        {
            //get startDate and endDate
            var queryDate = filterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;

            var dropOffs = _context.PreShipment.AsQueryable().Where(x => x.SenderUserId == currentUserId);

            if (filterCriteria.StartDate == null && filterCriteria.EndDate == null)
            {
                //Last 20 days
                startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(-20);
                endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1);
            }

            dropOffs = dropOffs.Where(x => x.DateCreated >= startDate && x.DateCreated < endDate).OrderByDescending(s => s.DateCreated);

            List<PreShipmentDTO> shipmentDto = (from r in dropOffs
                                                select new PreShipmentDTO()
                                                {
                                                    PreShipmentId = r.PreShipmentId,
                                                    TempCode = r.TempCode,
                                                    Waybill = r.Waybill,
                                                    CompanyType = r.CompanyType,
                                                    DateCreated = r.DateCreated,
                                                    DateModified = r.DateModified,
                                                    ReceiverAddress = r.ReceiverAddress,
                                                    ReceiverCity = r.ReceiverCity,
                                                    ReceiverName = r.ReceiverName,
                                                    ReceiverPhoneNumber = r.ReceiverPhoneNumber,
                                                    SenderUserId = r.SenderUserId,
                                                    Value = r.Value,
                                                    IsProcessed = r.IsProcessed,
                                                    CustomerCode = r.CustomerCode,
                                                    PickupOptions = r.PickupOptions,
                                                    SenderCity = r.SenderCity,
                                                    DepartureStationId = r.DepartureStationId,
                                                    DestinationStationId = r.DestinationStationId,
                                                    PreShipmentItems = _context.PreShipmentItem.Where(s => s.PreShipmentId == r.PreShipmentId)
                                                                        .Select(x => new PreShipmentItemDTO
                                                                        {
                                                                            Description = x.Description,
                                                                            ShipmentType = x.ShipmentType,
                                                                            Weight = x.Weight,
                                                                            Quantity = x.Quantity,
                                                                            SpecialPackageId = x.SpecialPackageId,
                                                                            PreShipmentId = x.PreShipmentId,
                                                                            PreShipmentItemId = x.PreShipmentItemId
                                                                        }).ToList()
                                                }).ToList();

            return Task.FromResult(shipmentDto.OrderByDescending(x => x.DateCreated).ToList());
        }


    }
}
