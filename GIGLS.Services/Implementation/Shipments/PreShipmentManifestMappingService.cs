using System;
using System.Threading.Tasks;
using GIGLS.Core;
using System.Collections.Generic;
using GIGLS.Infrastructure;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.Domain;
using GIGLS.Core.IServices.User;
using System.Linq;
using AutoMapper;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.Enums;
using GIGLS.Core.DTO.Zone;
using GIGLS.Core.DTO.ServiceCentres;

namespace GIGLS.Services.Implementation.Shipments
{
    public class PreShipmentManifestMappingService : IPreShipmentManifestMappingService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly IPreShipmentService _preShipmentService;

        public PreShipmentManifestMappingService(IUnitOfWork uow,
            IUserService userService, IPreShipmentService preShipmentService)
        {
            _uow = uow;
            _userService = userService;
            _preShipmentService = preShipmentService;
            MapperConfig.Initialize();
        }

        public async Task<List<PreShipmentManifestMappingDTO>> GetAllManifestWaybillMappings()
        {
            var manifestWaybillMapings = await _uow.PreShipmentManifestMapping.GetManifestWaybillMappings();
            return manifestWaybillMapings.OrderByDescending(x => x.DateCreated).ToList();
        }

        //map waybills to Manifest
        public async Task MappingManifestToWaybills(string manifest, List<string> waybills)
        {
            try
            {
                var manifestObj = await _uow.PreShipmentManifestMapping.GetAsync(x => x.ManifestCode.Equals(manifest));


            }
            catch (Exception)
            {
                throw;
            }
        }


        //Get Waybills In Manifest
        public async Task<List<PreShipmentManifestMappingDTO>> GetWaybillsInManifest(string manifestcode)
        {
            try
            {
                var manifestWaybillMappingList = await _uow.PreShipmentManifestMapping.FindAsync(x => x.ManifestCode == manifestcode);
                var manifestWaybillNumberMappingDto = Mapper.Map<List<PreShipmentManifestMappingDTO>>(manifestWaybillMappingList.ToList());

                return manifestWaybillNumberMappingDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Get Waybills In Manifest for Dispatch
        public async Task<List<PreShipmentManifestMappingDTO>> GetWaybillsInManifestForPickup()
        {
            try
            {
                var manifestWaybillMappingList = await _uow.PreShipmentManifestMapping.
                    FindAsync(x => x.PreShipment.RequestStatus == PreShipmentRequestStatus.Valid
                    && x.PreShipment.ProcessingStatus == PreShipmentProcessingStatus.Valid);
                var manifestWaybillNumberMappingDto = Mapper.Map<List<PreShipmentManifestMappingDTO>>(manifestWaybillMappingList.ToList());

                return manifestWaybillNumberMappingDto;
            }
            catch (Exception)
            {
                throw;
            }
        }


        //Get All Manifests that a Waybill has been mapped to
        public async Task<PreShipmentManifestMappingDTO> GetManifestForWaybill(string waybill)
        {
            try
            {
                var waybillMapping = _uow.PreShipmentManifestMapping.SingleOrDefault(x => x.Waybill == waybill);

                if (waybillMapping == null)
                {
                    throw new GenericException($"Waybill {waybill} has not been mapped to any manifest");
                }

                var preShipment = waybillMapping.PreShipment;
                var preShipmentDto = Mapper.Map<PreShipmentManifestMappingDTO>(preShipment);
                return await Task.FromResult(preShipmentDto);
            }
            catch (Exception)
            {
                throw;
            }
        }


        //remove Waybill from manifest
        public async Task RemoveWaybillFromManifest(string manifest, string waybill)
        {
            try
            {
                //var manifestDTO = await _manifestService.GetManifestByCode(manifest);

                //var manifestWaybillMapping = await _uow.ManifestWaybillMapping.GetAsync(x => x.ManifestCode == manifest && x.Waybill == waybill);

                //if (manifestWaybillMapping == null)
                //{
                //    throw new GenericException($"Waybill {waybill} does not mapped to the manifest {manifest}");
                //}

                ////update shipment collection centre
                //var shipmentCollection = await _uow.ShipmentCollection.GetAsync(x => x.Waybill == waybill && x.ShipmentScanStatus == ShipmentScanStatus.WC);

                //if (shipmentCollection != null)
                //{
                //    shipmentCollection.ShipmentScanStatus = ShipmentScanStatus.ARF;
                //}

                //_uow.ManifestWaybillMapping.Remove(manifestWaybillMapping);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //update Waybill in manifest that is not delivered
        public async Task ReturnWaybillsInManifest(string manifest, List<string> waybills)
        {
            try
            {
                //var serviceIds = await _userService.GetPriviledgeServiceCenters();
                //var serviceCenter = await _uow.ServiceCentre.GetAsync(serviceIds[0]);
                //string user = await _userService.GetCurrentUserId();

                //var manifestDTO = await _manifestService.GetManifestByCode(manifest);

                //foreach (var waybill in waybills)
                //{
                //    //1. check if the waybill is in the manifest 
                //    var manifestWaybillMapping = await _uow.ManifestWaybillMapping.GetAsync(x => x.ManifestCode == manifest && x.Waybill == waybill);

                //    if (manifestWaybillMapping == null)
                //    {
                //        throw new GenericException($"Waybill {waybill} does not mapped to the manifest {manifest}");
                //    }

                //    //2. check if the user is at the final destination centre of the shipment
                //    if (serviceIds.Length == 1 && serviceIds[0] == manifestWaybillMapping.ServiceCentreId)
                //    {
                //        //update manifestWaybillMapping status for the waybill
                //        manifestWaybillMapping.IsActive = false;

                //        //3. check if the waybill has not been delivered 
                //        var shipmentCollection = await _uow.ShipmentCollection.GetAsync(x => x.Waybill == waybill && x.ShipmentScanStatus == ShipmentScanStatus.WC);
                //        if (shipmentCollection == null)
                //        {
                //            throw new GenericException($"Shipment with waybill: {waybill} is not available for Processing");
                //        }
                //        else
                //        {
                //            //Update shipment collection to make it available at collection centre
                //            shipmentCollection.ShipmentScanStatus = ShipmentScanStatus.ARF;

                //            //Add scan status to  the tracking page
                //            var newShipmentTracking = new ShipmentTracking
                //            {
                //                Waybill = waybill,
                //                Location = serviceCenter.Name,
                //                Status = ShipmentScanStatus.SRC.ToString(),
                //                DateTime = DateTime.Now,
                //                UserId = user
                //            };
                //            _uow.ShipmentTracking.Add(newShipmentTracking);
                //        }
                //    }
                //    else
                //    {
                //        throw new GenericException("Error processing request. The login user is not at the final Destination nor has the right privilege");
                //    }
                //}

                //await _uow.CompleteAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<PreShipmentDTO>> GetUnMappedWaybillsForPickupManifest()
        {
            try
            {
                //    var serviceCenters = await _userService.GetPriviledgeServiceCenters();

                //    //var filterOptionsDto = new FilterOptionsDto
                //    //{
                //    //    count = 500,
                //    //    page = 1,
                //    //    sortorder = "0"
                //    //};

                //    //1. get all shipments at colletion centre for the service centre which status is ARF
                //    var shipmentCollection = await _uow.ShipmentCollection.FindAsync(x => x.ShipmentScanStatus == ShipmentScanStatus.ARF);

                //    if (shipmentCollection.Count() == 0)
                //    {
                //        return new List<ShipmentDTO>();
                //    }

                //    var waybills = new List<string>();
                //    foreach (var waybillCollection in shipmentCollection)
                //    {
                //        waybills.Add(waybillCollection.Waybill);
                //    }

                //    //2. Get shipment details for the service centre that are at the collection centre using the waybill and service centre
                //    //var shipmentsBySC = await _uow.Shipment.GetShipmentDetailByWaybills(filterOptionsDto, serviceCenters, result).Item1;
                //    var InvoicesBySC = _uow.Invoice.GetAllFromInvoiceView();

                //    //filter by destination service center that is not cancelled and it is home delivery
                //    InvoicesBySC = InvoicesBySC.Where(x => x.IsCancelled == false && x.PickupOptions == PickupOptions.HOMEDELIVERY);

                //    if (serviceCenters.Length > 0)
                //    {
                //        InvoicesBySC = InvoicesBySC.Where(s => serviceCenters.Contains(s.DestinationServiceCentreId));
                //    }

                //    //filter by Local or International Shipment
                //    //if (filterOptionsDto.IsInternational != null)
                //    //{
                //    //    InvoicesBySC = InvoicesBySC.Where(s => s.IsInternational == filterOptionsDto.IsInternational);
                //    //}


                //    ////final list
                //    var InvoicesBySCList = InvoicesBySC.ToList();
                //    if (waybills.Count > 0)
                //    {
                //        InvoicesBySCList = InvoicesBySCList.Where(s => waybills.Contains(s.Waybill)).ToList();
                //    }

                //    List<ShipmentDTO> shipmentsBySC = (from r in InvoicesBySCList
                //                                       select new ShipmentDTO()
                //                                       {
                //                                           ShipmentId = r.ShipmentId,
                //                                           Waybill = r.Waybill,
                //                                           CustomerId = r.CustomerId,
                //                                           CustomerType = r.CustomerType,
                //                                           //ActualDateOfArrival = r.ActualDateOfArrival,
                //                                           DateCreated = r.DateCreated,
                //                                           DateModified = r.DateModified,
                //                                           DeliveryOptionId = r.DeliveryOptionId,
                //                                           DeliveryOption = new DeliveryOptionDTO
                //                                           {
                //                                               Code = r.DeliveryOptionCode,
                //                                               Description = r.DeliveryOptionDescription
                //                                           },
                //                                           //DeliveryTime = r.DeliveryTime,
                //                                           DepartureServiceCentreId = r.DepartureServiceCentreId,
                //                                           DepartureServiceCentre = new ServiceCentreDTO()
                //                                           {
                //                                               Code = r.DepartureServiceCentreCode,
                //                                               Name = r.DepartureServiceCentreName
                //                                           },
                //                                           DestinationServiceCentreId = r.DestinationServiceCentreId,
                //                                           DestinationServiceCentre = new ServiceCentreDTO()
                //                                           {
                //                                               Code = r.DestinationServiceCentreCode,
                //                                               Name = r.DestinationServiceCentreName
                //                                           },

                //                                           //ExpectedDateOfArrival = r.ExpectedDateOfArrival,
                //                                           PaymentStatus = r.PaymentStatus,
                //                                           ReceiverAddress = r.ReceiverAddress,
                //                                           ReceiverCity = r.ReceiverCity,
                //                                           ReceiverCountry = r.ReceiverCountry,
                //                                           ReceiverEmail = r.ReceiverEmail,
                //                                           ReceiverName = r.ReceiverName,
                //                                           ReceiverPhoneNumber = r.ReceiverPhoneNumber,
                //                                           ReceiverState = r.ReceiverState,
                //                                           SealNumber = r.SealNumber,
                //                                           UserId = r.UserId,
                //                                           Value = r.Value,
                //                                           GrandTotal = r.GrandTotal,
                //                                           //AppliedDiscount = r.AppliedDiscount,
                //                                           DiscountValue = r.DiscountValue,
                //                                           ShipmentPackagePrice = r.ShipmentPackagePrice,
                //                                           CompanyType = r.CompanyType,
                //                                           CustomerCode = r.CustomerCode,
                //                                           Description = r.Description
                //                                       }).ToList();


                //    //var shipmentsBySC = await _uow.Shipment.GetShipmentDetailByWaybills(filterOptionsDto, serviceCenters, result).Item1;

                //    return shipmentsBySC;
                var list = new List<PreShipmentDTO>();
                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
