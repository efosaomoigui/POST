using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGL.POST.Core.Domain;
using POST.Core.DTO.Shipments;
using POST.Core.IRepositories.Shipments;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;
using System.Linq;
using POST.CORE.DTO.Report;
using POST.Core.Enums;
using POST.CORE.Enums;
using POST.Core.DTO.Zone;
using POST.Core.DTO.ServiceCentres;
using POST.CORE.DTO.Shipments;
using POST.Core.DTO.Account;
using POST.Core.DTO.Report;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.Shipments
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

            var dropOffs = _context.PreShipment.AsQueryable().Where(x => x.SenderUserId == currentUserId && x.IsActive == true);

            if (filterCriteria.StartDate == null && filterCriteria.EndDate == null)
            {
                //Last 20 days
                startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(-60);
                endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1);
            }

            dropOffs = dropOffs.Where(x => x.DateCreated >= startDate && x.DateCreated < endDate).OrderByDescending(s => s.DateCreated);

            if (filterCriteria.StartDate == null && filterCriteria.EndDate == null)
            {
                //Take current 200 records
                dropOffs = dropOffs.Take(200);
            }

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
                                                    //SenderName = i.FirstName + ' ' + i.LastName,
                                                    SenderPhoneNumber = r.SenderPhoneNumber,
                                                    IsAgent = r.IsAgent,
                                                    DepartureStationId = r.DepartureStationId,
                                                    DestinationStationId = r.DestinationStationId,
                                                    DestinationServiceCenterId = r.DestinationServiceCenterId,
                                                    PreShipmentItems = _context.PreShipmentItem.Where(s => s.PreShipmentId == r.PreShipmentId)
                                                                        .Select(x => new PreShipmentItemDTO
                                                                        {
                                                                            Description = x.Description,
                                                                            ShipmentType = x.ShipmentType,
                                                                            Weight = x.Weight,
                                                                            Quantity = x.Quantity,
                                                                            SpecialPackageId = x.SpecialPackageId,
                                                                            PreShipmentId = x.PreShipmentId,
                                                                            PreShipmentItemId = x.PreShipmentItemId,
                                                                            ItemValue = x.ItemValue
                                                                        }).ToList()
                                                }).ToList();
            if (shipmentDto.Any())
            {
                for (int i = 0; i < shipmentDto.Count; i++)
                {
                    var item = shipmentDto[i];
                    var sender = _context.Users.FirstOrDefault(x => x.Id == item.SenderUserId);
                    if (sender != null)
                    {
                        item.SenderName = sender.FirstName + ' ' + sender.LastName;
                    }

                    //also get weight
                    foreach (var d in item.PreShipmentItems)
                    {
                        var itemWeight = _context.SpecialDomesticPackage.FirstOrDefault(y => y.SpecialDomesticPackageId == d.SpecialPackageId);
                        if (itemWeight != null && itemWeight.Weight != null)
                        {
                            d.Weight = (double)itemWeight.Weight;
                        }

                    }
                }
            }

            return Task.FromResult(shipmentDto.OrderByDescending(x => x.DateCreated).ToList());
        }



        public Task<List<PreShipmentDTO>> GetDropOffsForUserByUserCodeOrPhoneNo(SearchOption searchOption)
        {
            var dropOffs = _context.PreShipment.Where(x => (x.CustomerCode.ToLower() == searchOption.Option.ToLower() || x.SenderPhoneNumber.ToLower() == searchOption.Option.ToLower()) && x.IsProcessed == false).ToList();
            List<PreShipmentDTO> shipmentDto = (from r in dropOffs
                                                join i in Context.Users on r.SenderUserId equals i.Id
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
                                                    SenderName = i.FirstName + ' ' + i.LastName,
                                                    SenderPhoneNumber = r.SenderPhoneNumber,
                                                    IsAgent = r.IsAgent,
                                                    DepartureStationId = r.DepartureStationId,
                                                    DestinationStationId = r.DestinationStationId,
                                                }).ToList();

            return Task.FromResult(shipmentDto.OrderByDescending(x => x.DateCreated).ToList());
        }

    }
}
