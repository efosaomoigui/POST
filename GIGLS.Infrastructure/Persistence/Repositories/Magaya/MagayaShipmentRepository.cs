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
using GIGLS.Core.IRepositories.Magaya;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Magaya
{
    public class MagayaShipmentRepository : Repository<MagayaShipmentAgility, GIGLSContext>, IMagayaShipmentRepository
    {
        private GIGLSContext _context;
        public MagayaShipmentRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        //public Task<List<MagayaShipmentAgilityDTO>> GetShipments(int[] serviceCentreIds)
        //{
        //    var shipment = _context.Shipment.AsQueryable();
        //    if (serviceCentreIds.Length > 0)
        //    {
        //        shipment = _context.Shipment.Where(s => serviceCentreIds.Contains(s.DepartureServiceCentreId));
        //    }

        //    //filter by cancelled shipments
        //    shipment = shipment.Where(s => s.IsCancelled == false);


        //    List<ShipmentDTO> shipmentDto = (from r in shipment
        //                                     select new ShipmentDTO()
        //                                     {
        //                                         ShipmentId = r.ShipmentId,
        //                                         Waybill = r.Waybill,
        //                                         CustomerId = r.CustomerId,
        //                                         CustomerType = r.CustomerType,
        //                                         ActualDateOfArrival = r.ActualDateOfArrival,
        //                                         //ActualReceiverName = r.ActualReceiverName,
        //                                         //ActualreceiverPhone = r.ActualreceiverPhone,
        //                                         //Comments = r.Comments,
        //                                         DateCreated = r.DateCreated,
        //                                         DateModified = r.DateModified,
        //                                         DeliveryOptionId = r.DeliveryOptionId,
        //                                         DeliveryOption = new DeliveryOptionDTO
        //                                         {
        //                                             Code = r.DeliveryOption.Code,
        //                                             Description = r.DeliveryOption.Description
        //                                         },
        //                                         DeliveryTime = r.DeliveryTime,
        //                                         DepartureServiceCentreId = r.DepartureServiceCentreId,
        //                                         DepartureServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == r.DepartureServiceCentreId).Select(x => new ServiceCentreDTO
        //                                         {
        //                                             Code = x.Code,
        //                                             Name = x.Name
        //                                         }).FirstOrDefault(),
        //                                         DestinationServiceCentreId = r.DestinationServiceCentreId,
        //                                         DestinationServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == r.DestinationServiceCentreId).Select(x => new ServiceCentreDTO
        //                                         {
        //                                             Code = x.Code,
        //                                             Name = x.Name
        //                                         }).FirstOrDefault(),
        //                                         ExpectedDateOfArrival = r.ExpectedDateOfArrival,
        //                                         //GroupWaybill = r.GroupWaybill,
        //                                         //IdentificationType = r.IdentificationType,
        //                                         //IndentificationUrl = r.IndentificationUrl,
        //                                         //IsDomestic = r.IsDomestic,
        //                                         PaymentStatus = r.PaymentStatus,
        //                                         ReceiverAddress = r.ReceiverAddress,
        //                                         ReceiverCity = r.ReceiverCity,
        //                                         ReceiverCountry = r.ReceiverCountry,
        //                                         ReceiverEmail = r.ReceiverEmail,
        //                                         ReceiverName = r.ReceiverName,
        //                                         ReceiverPhoneNumber = r.ReceiverPhoneNumber,
        //                                         ReceiverState = r.ReceiverState,
        //                                         SealNumber = r.SealNumber,
        //                                         UserId = r.UserId,
        //                                         Value = r.Value,
        //                                         GrandTotal = r.GrandTotal,
        //                                         AppliedDiscount = r.AppliedDiscount,
        //                                         DiscountValue = r.DiscountValue,
        //                                         ShipmentPackagePrice = r.ShipmentPackagePrice,
        //                                         ShipmentPickupPrice = r.ShipmentPickupPrice,
        //                                         CompanyType = r.CompanyType,
        //                                         CustomerCode = r.CustomerCode,
        //                                         Description = r.Description,
        //                                         SenderAddress = r.SenderAddress,
        //                                         SenderState = r.SenderState,
        //                                         ApproximateItemsWeight = r.ApproximateItemsWeight,
        //                                         DepartureCountryId = r.DepartureCountryId,
        //                                         DestinationCountryId = r.DestinationCountryId,
        //                                         CurrencyRatio = r.CurrencyRatio,

        //                                         ShipmentCancel = Context.ShipmentCancel.Where(c => c.Waybill == r.Waybill).Select(x => new ShipmentCancelDTO
        //                                         {
        //                                             CancelReason = x.CancelReason
        //                                         }).FirstOrDefault(),
        //                                         //DepartureTerminalName = r.DepartureTerminal.Name,
        //                                         //DestinationTerminalName = r.DestinationTerminal.Name       
        //                                         //ShipmentItems = Context.ShipmentItem.Where(s => s.ShipmentId == r.ShipmentId).ToList()z
        //                                     }).ToList();


        //    return Task.FromResult(shipmentDto.ToList());
        //}
         
    }
}