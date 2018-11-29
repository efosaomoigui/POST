using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.CORE.Domain;
using GIGLS.CORE.DTO.Shipments;
using GIGLS.CORE.IRepositories.Shipments;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Linq;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Shipments
{
    public class ShipmentCollectionRepository : Repository<ShipmentCollection, GIGLSContext>, IShipmentCollectionRepository
    {
        private GIGLSContext _context;
        public ShipmentCollectionRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }


        public IQueryable<ShipmentCollectionDTO> ShipmentCollectionsForEcommerceAsQueryable(bool isEcommerce)
        {
            var shipmentCollectionAsQueryable = (from shipmentCollection in _context.ShipmentCollection
                                                 join shipment in _context.Shipment on
                                                 new { shipmentCollection.Waybill, Key2 = isEcommerce } equals
                                                 new { shipment.Waybill, Key2 = (shipment.CompanyType == "Ecommerce") }
                                                 select new ShipmentCollectionDTO()
                                                 {
                                                     Waybill = shipmentCollection.Waybill,
                                                     Name = shipmentCollection.Name,
                                                     PhoneNumber = shipmentCollection.PhoneNumber,
                                                     Email = shipmentCollection.Email,
                                                     Address = shipmentCollection.Address,
                                                     City = shipmentCollection.City,
                                                     State = shipmentCollection.State,
                                                     IndentificationUrl = shipmentCollection.IndentificationUrl,
                                                     ShipmentScanStatus = shipmentCollection.ShipmentScanStatus,
                                                     UserId = shipmentCollection.UserId,

                                                     OriginalDepartureServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == shipment.DepartureServiceCentreId).Select(x => new ServiceCentreDTO
                                                     {
                                                         Code = x.Code,
                                                         Name = x.Name
                                                     }).FirstOrDefault(),

                                                     OriginalDestinationServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == shipment.DestinationServiceCentreId).Select(x => new ServiceCentreDTO
                                                     {
                                                         Code = x.Code,
                                                         Name = x.Name
                                                     }).FirstOrDefault(),
                                                 });


            return shipmentCollectionAsQueryable;
        }

    }
}
