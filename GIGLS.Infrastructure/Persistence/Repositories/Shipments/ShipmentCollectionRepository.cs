using GIGLS.Core.DTO.Report;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.Enums;
using GIGLS.CORE.Domain;
using GIGLS.CORE.DTO.Shipments;
using GIGLS.CORE.IRepositories.Shipments;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

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
                                                     DateCreated = shipmentCollection.DateCreated,
                                                     DestinationServiceCentreId = shipmentCollection.DestinationServiceCentreId,
                                                     OriginalDepartureServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == shipment.DepartureServiceCentreId).Select(x => new ServiceCentreDTO
                                                     {
                                                         Code = x.Code,
                                                         Name = x.Name
                                                     }).FirstOrDefault(),

                                                     OriginalDestinationServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == shipment.DestinationServiceCentreId).Select(x => new ServiceCentreDTO
                                                     {
                                                         Code = x.Code,
                                                         Name = x.Name
                                                     }).FirstOrDefault()
                                                 });


            return shipmentCollectionAsQueryable;
        }


        public async Task<List<ShipmentCollectionForContactDTO>> GetShipmentCollectionForContact(ShipmentContactFilterCriteria baseFilterCriteria)
        {
            try
            {
                var queryDate = baseFilterCriteria.getStartDateAndEndDate();
                var startDate1 = queryDate.Item1;
                var endDate1 = queryDate.Item2;

                //declare parameters for the stored procedure
                SqlParameter startDate = new SqlParameter("@StartDate", startDate1);
                SqlParameter endDate = new SqlParameter("@EndDate", endDate1);
                SqlParameter serviceCentreId = new SqlParameter("@ServiceCentreId", baseFilterCriteria.ServiceCentreId);
                SqlParameter scanStatus = new SqlParameter("@ScanStatus",ShipmentScanStatus.ARF);

                SqlParameter[] param = new SqlParameter[]
                {
                    serviceCentreId,
                    startDate,
                    endDate,
                    scanStatus
                };

                var result =  _context.Database.SqlQuery<ShipmentCollectionForContactDTO>("ShipmentCollectionForContacts " +
                   "@ServiceCentreId,@StartDate, @EndDate, @ScanStatus",
                   param).ToList();


                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
