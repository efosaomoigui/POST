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
    public class ManifestWaybillMappingService : IManifestWaybillMappingService
    {
        private readonly IUnitOfWork _uow;
        private readonly IManifestService _manifestService;
        private readonly IUserService _userService;
        private readonly IShipmentService _shipmentService;

        public ManifestWaybillMappingService(IUnitOfWork uow, IManifestService manifestService,
            IUserService userService, IShipmentService shipmentService)
        {
            _uow = uow;
            _manifestService = manifestService;
            _userService = userService;
            _shipmentService = shipmentService;
            MapperConfig.Initialize();
        }

        public async Task<List<ManifestWaybillMappingDTO>> GetAllManifestWaybillMappings()
        {
            //var serviceIds = await _userService.GetPriviledgeServiceCenters();
            //return await _uow.ManifestWaybillMapping.GetManifestWaybillMappings(serviceIds);

            var resultSet = new HashSet<string>();
            var result = new List<ManifestWaybillMappingDTO>();

            var serviceIds = _userService.GetPriviledgeServiceCenters().Result;
            var manifestWaybillMapings = await _uow.ManifestWaybillMapping.GetManifestWaybillMappings(serviceIds);
            foreach (var item in manifestWaybillMapings)
            {
                if (resultSet.Add(item.ManifestCode))
                {
                    result.Add(item);
                }
            }

            return result.OrderByDescending(x => x.DateCreated).ToList();
        }

        //map waybills to Manifest
        public async Task MappingManifestToWaybills(string manifest, List<string> waybills)
        {
            try
            {
                var serviceIds = await _userService.GetPriviledgeServiceCenters();

                //1. check if any of the waybills has not been mapped to a manifest 
                // and has not been process for return in case it was not delivered (i.e still active) that day
                var isWaybillMappedActive = _uow.ManifestWaybillMapping.GetAllAsQueryable();
                isWaybillMappedActive = isWaybillMappedActive.Where(x => x.IsActive == true && waybills.Contains(x.Waybill));

                var isWaybillsMappedActiveResult = isWaybillMappedActive.Select(x => x.Waybill).Distinct().ToList();

                if (isWaybillsMappedActiveResult.Count() > 0)
                {
                    throw new GenericException($"Error: Delivery Manifest cannot be created. " +
                               $"The follwoing waybills [{string.Join(", ", isWaybillsMappedActiveResult.ToList())}] already been manifested");
                }


                var manifestObj = await _uow.Manifest.GetAsync(x => x.ManifestCode.Equals(manifest));

                //create the manifest if manifest does not exist
                if (manifestObj == null)
                {
                    var newManifest = new Manifest
                    {
                        DateTime = DateTime.Now,
                        ManifestCode = manifest,
                        ManifestType = ManifestType.Delivery
                    };
                    _uow.Manifest.Add(newManifest);
                }

                foreach (var waybill in waybills)
                {
                    //check if the waybill exist
                    var shipment = await _uow.Shipment.GetAsync(x => x.Waybill == waybill);
                    if (shipment == null)
                    {
                        throw new GenericException($"No Waybill exists for this number: {waybill}");
                    }

                    //check if the user is at the final destination centre of the shipment
                    if (serviceIds.Length == 1 && serviceIds[0] == shipment.DestinationServiceCentreId)
                    {
                    }
                    else
                    {
                        throw new GenericException("Error processing request. The login user is not at the final Destination nor has the right privilege");
                    }

                    //check if the shipment is at the final destination with a scan of ARF (WHEN SHIPMENT ARRIVED FINAL DESTINATION)
                    var shipmentCollection = await _uow.ShipmentCollection.GetAsync(x => x.ShipmentScanStatus == ShipmentScanStatus.ARF && x.Waybill == waybill);
                    if (shipmentCollection == null)
                    {
                        throw new GenericException($"Shipment with waybill: {waybill} is not available for Processing");
                    }
                    else
                    {
                        //WC -- SCAN BEFORE SHIPMENT IS TAKEN OUT FOR DELIVERY TO RECEIVER
                        shipmentCollection.ShipmentScanStatus = ShipmentScanStatus.WC;
                    }

                    //check if the waybill has been mapped to a manifest 
                    //and it has not been process for return in case it was not delivered (i.e still active) that day
                    //var isWaybillMappedActive = await _uow.ManifestWaybillMapping.ExistAsync(x => x.Waybill == waybill && x.IsActive == true);
                    //if (isWaybillMappedActive)
                    //{
                    //    throw new GenericException($"Waybill {waybill} has already been manifested");
                    //}

                    //check if Waybill has not been added to this manifest 
                    var isWaybillMapped = await _uow.ManifestWaybillMapping.ExistAsync(x => x.ManifestCode == manifest && x.Waybill == waybill);

                    //if the waybill has not been added to this manifest, add it
                    if (!isWaybillMapped)
                    {
                        //Add new Mapping
                        var newMapping = new ManifestWaybillMapping
                        {
                            ManifestCode = manifest,
                            Waybill = waybill,
                            IsActive = true,
                            ServiceCentreId = shipment.DestinationServiceCentreId
                        };
                        _uow.ManifestWaybillMapping.Add(newMapping);
                    }

                    //automatic scan all the way also
                }

                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }


        //map waybills to Manifest for Mobile
        private async Task MappingManifestToWaybillsMobile(string manifest, List<string> waybills, int[] serviceIds)
        {
            try
            {
                //1. check if the waybill has been mapped to a manifest 
                // and it has not been process for return in case it was not delivered (i.e still active) that day
                var isWaybillMappedActive = _uow.ManifestWaybillMapping.GetAllAsQueryable();
                isWaybillMappedActive = isWaybillMappedActive.Where(x => x.IsActive == true && waybills.Contains(x.Waybill));

                var isWaybillsMappedActiveResult = isWaybillMappedActive.Select(x => x.Waybill).Distinct().ToList();

                if (isWaybillsMappedActiveResult.Count() > 0)
                {
                    throw new GenericException($"Error: Delivery Manifest cannot be created. " +
                               $"The follwoing waybills [{string.Join(", ", isWaybillsMappedActiveResult.ToList())}] already been manifested");
                }
                
                var manifestObj = await _uow.Manifest.GetAsync(x => x.ManifestCode.Equals(manifest));
                //create the manifest if manifest does not exist
                if (manifestObj == null)
                {
                    var newManifest = new Manifest
                    {
                        DateTime = DateTime.Now,
                        ManifestCode = manifest,
                        ManifestType = ManifestType.Delivery
                    };
                    _uow.Manifest.Add(newManifest);
                }
                                
                foreach (var waybill in waybills)
                {
                    //check if the shipment is at the final destination with a scan of ARF (WHEN SHIPMENT ARRIVED FINAL DESTINATION)
                    var shipmentCollection = await _uow.ShipmentCollection.GetAsync(x => x.ShipmentScanStatus == ShipmentScanStatus.ARF && x.Waybill == waybill);
                    if (shipmentCollection == null)
                    {
                        throw new GenericException($"Shipment with waybill: {waybill} is not available for Processing");
                    }
                    else
                    {
                        //WC -- SCAN BEFORE SHIPMENT IS TAKEN OUT FOR DELIVERY TO RECEIVER
                        shipmentCollection.ShipmentScanStatus = ShipmentScanStatus.WC;
                    }
                    
                    //check if Waybill has not been added to this manifest 
                    var isWaybillMapped = await _uow.ManifestWaybillMapping.ExistAsync(x => x.ManifestCode == manifest && x.Waybill == waybill);

                    //if the waybill has not been added to this manifest, add it
                    if (!isWaybillMapped)
                    {
                        //Add new Mapping
                        var newMapping = new ManifestWaybillMapping
                        {
                            ManifestCode = manifest,
                            Waybill = waybill,
                            IsActive = true,
                            ServiceCentreId = serviceIds[0]
                        };
                        _uow.ManifestWaybillMapping.Add(newMapping);
                    }                    
                }

                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }


        //map waybills to Manifest from Mobile
        public async Task MappingManifestToWaybillsMobile(string manifest, List<string> waybills)
        {
            try
            {
                var serviceCenters = await _userService.GetPriviledgeServiceCenters();

                //1a. Get the shipment status of the waybills we want to manifest
                var shipmentCollection = _uow.ShipmentCollection.GetAllAsQueryable();
                shipmentCollection = shipmentCollection.Where(x => x.ShipmentScanStatus == ShipmentScanStatus.ARF && waybills.Contains(x.Waybill));

                //extrack waybills into a list from shipment collection
                var shipmentCollectionList = shipmentCollection.Select(x => x.Waybill).Distinct().ToList();

                //1b. check if all the waybills has the same status (ARF)
                if (shipmentCollectionList.Count() == 0)
                {
                    throw new GenericException($"None of the waybills is not available for Processing");
                }

                if(shipmentCollectionList.Count() != waybills.Count())
                {
                    var result = waybills.Where(x => !shipmentCollectionList.Contains(x));

                    if (result.Count() > 0)
                    {
                        throw new GenericException($"Error: Delivery Manifest cannot be created. " +
                            $"The follwoing waybills [{string.Join(", ", result.ToList())}] are not available for Processing");
                    }
                }
                
                //2. Get the shipment details of all the waybills that we want to manifest again by 
                // check if the waybill is not cancelled, Home Delivery and the user that want to manifest it is at the final service centre of the waybills  
                var InvoicesBySC = _uow.Invoice.GetAllFromInvoiceView();
                InvoicesBySC = InvoicesBySC.Where(x => x.IsCancelled == false && x.PickupOptions == PickupOptions.HOMEDELIVERY && waybills.Contains(x.Waybill));
                
                //filter if the shipment is at the final service centre
                if (serviceCenters.Length > 0)
                {
                    InvoicesBySC = InvoicesBySC.Where(s => serviceCenters.Contains(s.DestinationServiceCentreId));
                }

                ////final list of home delivery waybills
                var InvoicesBySCList = InvoicesBySC.Select(x => x.Waybill).Distinct().ToList();

                //1b. check if all the waybills are equal to our home delivery 
                if (InvoicesBySCList.Count() != waybills.Count())
                {
                    var result = waybills.Where(x => !InvoicesBySCList.Contains(x));

                    if (result.Count() > 0)
                    {
                        throw new GenericException($"Error: Delivery Manifest cannot be created. " +
                            $"The follwoing waybills [{string.Join(", ", result.ToList())}] are not available for Processing. " +
                            "The login user is not at the final Destination or waybills not Home Delivery");
                    }
                }
                else
                {
                    await MappingManifestToWaybillsMobile(manifest, waybills, serviceCenters);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Get Waybills In Manifest
        public async Task<List<ManifestWaybillMappingDTO>> GetWaybillsInManifest(string manifestcode)
        {
            try
            {
                var manifestDTO = await _manifestService.GetManifestByCode(manifestcode);
                var manifestWaybillMappingList = await _uow.ManifestWaybillMapping.FindAsync(x => x.ManifestCode == manifestDTO.ManifestCode);

                var manifestWaybillNumberMappingDto = Mapper.Map<List<ManifestWaybillMappingDTO>>(manifestWaybillMappingList.ToList());

                foreach (var manifestwaybill in manifestWaybillNumberMappingDto)
                {
                    manifestwaybill.ManifestDetails = manifestDTO;

                    //get shipment detail 
                    manifestwaybill.Shipment = await _shipmentService.GetBasicShipmentDetail(manifestwaybill.Waybill);

                    //get from ShipmentCollection
                    var shipmentCollectionObj = await _uow.ShipmentCollection.GetAsync(x => x.Waybill == manifestwaybill.Waybill);
                    if (shipmentCollectionObj != null)
                    {
                        manifestwaybill.ShipmentScanStatus = shipmentCollectionObj.ShipmentScanStatus;
                    }
                }

                return manifestWaybillNumberMappingDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Get Waybills In Manifest for Dispatch
        public async Task<List<ManifestWaybillMappingDTO>> GetWaybillsInManifestForDispatch()
        {
            try
            {
                //get the current user
                string userId = await _userService.GetCurrentUserId();

                //get the dispatch for the user
                var userDispatchs = _uow.Dispatch.GetAll().Where(s => s.DriverDetail == userId && s.ReceivedBy == null).ToList();

                //get the active manifest for the dispatch user
                if (userDispatchs.Count > 0)
                {
                    //error, the dispatch user cannot have an undelivered dispatch
                    var manifestCodeArray = userDispatchs.Select(s => s.ManifestNumber).ToList();
                    var manifestObjects = _uow.Manifest.GetAll().Where(s =>
                    manifestCodeArray.Contains(s.ManifestCode) && s.ManifestType == ManifestType.Delivery).ToList();

                    if (manifestObjects.Count > 1)
                    {
                        //error, the dispatch user cannot have more than one undelivered dispatch
                        throw new GenericException("Error: Dispatch User cannot have more than one undelivered Manifest Dispatch.");
                    }

                    //update userDispatchs
                    var deliveryManifestCodeArray = manifestObjects.Select(s => s.ManifestCode).ToList();
                    userDispatchs = userDispatchs.Where(s => 
                    deliveryManifestCodeArray.Contains(s.ManifestNumber)).ToList();
                }


                var currentUserDispatch = userDispatchs.FirstOrDefault();
                if (currentUserDispatch == null)
                {
                    //return an empty list
                    return new List<ManifestWaybillMappingDTO>();
                }

                var manifestDTO = await _manifestService.GetManifestByCode(currentUserDispatch.ManifestNumber);
                var manifestWaybillMappingList = await _uow.ManifestWaybillMapping.FindAsync(x => x.ManifestCode == manifestDTO.ManifestCode);

                var manifestWaybillNumberMappingDto = Mapper.Map<List<ManifestWaybillMappingDTO>>(manifestWaybillMappingList.ToList());

                foreach (var manifestwaybill in manifestWaybillNumberMappingDto)
                {
                    manifestwaybill.ManifestDetails = manifestDTO;

                    //get shipment detail 
                    manifestwaybill.Shipment = await _shipmentService.GetShipment(manifestwaybill.Waybill);

                    //get from ShipmentCollection
                    var shipmentCollectionObj = await _uow.ShipmentCollection.GetAsync(x => x.Waybill == manifestwaybill.Waybill);
                    if(shipmentCollectionObj != null)
                    {
                        manifestwaybill.ShipmentScanStatus = shipmentCollectionObj.ShipmentScanStatus;
                    }
                }

                return manifestWaybillNumberMappingDto;
            }
            catch (Exception)
            {
                throw;
            }
        }


        //Get All Manifests that a Waybill has been mapped to
        public async Task<List<ManifestWaybillMappingDTO>> GetManifestForWaybill(string waybill)
        {
            try
            {
                var waybillMappingList = await _uow.ManifestWaybillMapping.FindAsync(x => x.Waybill == waybill);

                if (waybillMappingList == null)
                {
                    throw new GenericException($"Waybill {waybill} has not been mapped to any manifest");
                }

                //add to list
                List<ManifestWaybillMappingDTO> resultList = new List<ManifestWaybillMappingDTO>();

                foreach (var waybillmapped in waybillMappingList)
                {
                    //get the manifest detail for the waybill
                    var manifestDTO = await _manifestService.GetManifestByCode(waybillmapped.ManifestCode);
                    var waybillMapping = Mapper.Map<ManifestWaybillMappingDTO>(waybillmapped);
                    waybillMapping.ManifestDetails = manifestDTO;

                    resultList.Add(waybillMapping);
                }

                return resultList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Get active Manifest that a Waybill is mapped to
        public async Task<ManifestWaybillMappingDTO> GetActiveManifestForWaybill(string waybill)
        {
            try
            {
                var activeManifest = await _uow.ManifestWaybillMapping.GetAsync(x => x.Waybill == waybill && x.IsActive == true);

                if (activeManifest == null)
                {
                    throw new GenericException($"There is no active Manifest for this Waybill {waybill}");
                }

                //get the manifest detail for the waybill
                var manifestDTO = await _manifestService.GetManifestByCode(activeManifest.ManifestCode);

                var activeManifestDto = Mapper.Map<ManifestWaybillMappingDTO>(activeManifest);
                activeManifestDto.ManifestDetails = manifestDTO;

                return activeManifestDto;
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
                var manifestDTO = await _manifestService.GetManifestByCode(manifest);

                var manifestWaybillMapping = await _uow.ManifestWaybillMapping.GetAsync(x => x.ManifestCode == manifest && x.Waybill == waybill);

                if (manifestWaybillMapping == null)
                {
                    throw new GenericException($"Waybill {waybill} does not mapped to the manifest {manifest}");
                }

                //update shipment collection centre
                var shipmentCollection = await _uow.ShipmentCollection.GetAsync(x => x.Waybill == waybill && x.ShipmentScanStatus == ShipmentScanStatus.WC);

                if (shipmentCollection != null)
                {
                    shipmentCollection.ShipmentScanStatus = ShipmentScanStatus.ARF;
                }

                _uow.ManifestWaybillMapping.Remove(manifestWaybillMapping);
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
                var serviceIds = await _userService.GetPriviledgeServiceCenters();
                var serviceCenter = await _uow.ServiceCentre.GetAsync(serviceIds[0]);
                string user = await _userService.GetCurrentUserId();

                var manifestDTO = await _manifestService.GetManifestByCode(manifest);

                foreach (var waybill in waybills)
                {
                    //1. check if the waybill is in the manifest 
                    var manifestWaybillMapping = await _uow.ManifestWaybillMapping.GetAsync(x => x.ManifestCode == manifest && x.Waybill == waybill);

                    if (manifestWaybillMapping == null)
                    {
                        throw new GenericException($"Waybill {waybill} does not mapped to the manifest {manifest}");
                    }

                    //2. check if the user is at the final destination centre of the shipment
                    if (serviceIds.Length == 1 && serviceIds[0] == manifestWaybillMapping.ServiceCentreId)
                    {
                        //update manifestWaybillMapping status for the waybill
                        manifestWaybillMapping.IsActive = false;

                        //3. check if the waybill has not been delivered 
                        var shipmentCollection = await _uow.ShipmentCollection.GetAsync(x => x.Waybill == waybill && x.ShipmentScanStatus == ShipmentScanStatus.WC);
                        if (shipmentCollection == null)
                        {
                            throw new GenericException($"Shipment with waybill: {waybill} is not available for Processing");
                        }
                        else
                        {
                            //Update shipment collection to make it available at collection centre
                            shipmentCollection.ShipmentScanStatus = ShipmentScanStatus.ARF;

                            //Add scan status to  the tracking page
                            var newShipmentTracking = new ShipmentTracking
                            {
                                Waybill = waybill,
                                Location = serviceCenter.Name,
                                Status = ShipmentScanStatus.SRC.ToString(),
                                DateTime = DateTime.Now,
                                UserId = user
                            };
                            _uow.ShipmentTracking.Add(newShipmentTracking);
                        }
                    }
                    else
                    {
                        throw new GenericException("Error processing request. The login user is not at the final Destination nor has the right privilege");
                    }
                }

                await _uow.CompleteAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ShipmentDTO>> GetUnMappedWaybillsForDeliveryManifestByServiceCentre()
        {
            try
            {
                var serviceCenters = await _userService.GetPriviledgeServiceCenters();

                //var filterOptionsDto = new FilterOptionsDto
                //{
                //    count = 500,
                //    page = 1,
                //    sortorder = "0"
                //};

                //1. get all shipments at colletion centre for the service centre which status is ARF
                var shipmentCollection = await _uow.ShipmentCollection.FindAsync(x => x.ShipmentScanStatus == ShipmentScanStatus.ARF);

                if (shipmentCollection.Count() == 0)
                {
                    return new List<ShipmentDTO>();
                }

                var waybills = new List<string>();
                foreach (var waybillCollection in shipmentCollection)
                {
                    waybills.Add(waybillCollection.Waybill);
                }

                //2. Get shipment details for the service centre that are at the collection centre using the waybill and service centre
                //var shipmentsBySC = await _uow.Shipment.GetShipmentDetailByWaybills(filterOptionsDto, serviceCenters, result).Item1;
                var InvoicesBySC = _uow.Invoice.GetAllFromInvoiceView();

                //filter by destination service center that is not cancelled and it is home delivery
                InvoicesBySC = InvoicesBySC.Where(x => x.IsCancelled == false && x.PickupOptions == PickupOptions.HOMEDELIVERY);

                if (serviceCenters.Length > 0)
                {
                    InvoicesBySC = InvoicesBySC.Where(s => serviceCenters.Contains(s.DestinationServiceCentreId));
                }

                //filter by Local or International Shipment
                //if (filterOptionsDto.IsInternational != null)
                //{
                //    InvoicesBySC = InvoicesBySC.Where(s => s.IsInternational == filterOptionsDto.IsInternational);
                //}


                ////final list
                var InvoicesBySCList = InvoicesBySC.ToList();
                if (waybills.Count > 0)
                {
                    InvoicesBySCList = InvoicesBySCList.Where(s => waybills.Contains(s.Waybill)).ToList();
                }

                List<ShipmentDTO> shipmentsBySC = (from r in InvoicesBySCList
                                                   select new ShipmentDTO()
                                                 {
                                                     ShipmentId = r.ShipmentId,
                                                     Waybill = r.Waybill,
                                                     CustomerId = r.CustomerId,
                                                     CustomerType = r.CustomerType,
                                                     //ActualDateOfArrival = r.ActualDateOfArrival,
                                                     DateCreated = r.DateCreated,
                                                     DateModified = r.DateModified,
                                                     DeliveryOptionId = r.DeliveryOptionId,
                                                     DeliveryOption = new DeliveryOptionDTO
                                                     {
                                                         Code = r.DeliveryOptionCode,
                                                         Description = r.DeliveryOptionDescription
                                                     },
                                                     //DeliveryTime = r.DeliveryTime,
                                                     DepartureServiceCentreId = r.DepartureServiceCentreId,
                                                     DepartureServiceCentre = new ServiceCentreDTO()
                                                     {
                                                         Code = r.DepartureServiceCentreCode,
                                                         Name = r.DepartureServiceCentreName
                                                     },
                                                     DestinationServiceCentreId = r.DestinationServiceCentreId,
                                                     DestinationServiceCentre = new ServiceCentreDTO()
                                                     {
                                                         Code = r.DestinationServiceCentreCode,
                                                         Name = r.DestinationServiceCentreName
                                                     },

                                                       //ExpectedDateOfArrival = r.ExpectedDateOfArrival,
                                                     PaymentStatus = r.PaymentStatus,
                                                     ReceiverAddress = r.ReceiverAddress,
                                                     ReceiverCity = r.ReceiverCity,
                                                     ReceiverCountry = r.ReceiverCountry,
                                                     ReceiverEmail = r.ReceiverEmail,
                                                     ReceiverName = r.ReceiverName,
                                                     ReceiverPhoneNumber = r.ReceiverPhoneNumber,
                                                     ReceiverState = r.ReceiverState,
                                                     SealNumber = r.SealNumber,
                                                     UserId = r.UserId,
                                                     Value = r.Value,
                                                     GrandTotal = r.GrandTotal,
                                                     //AppliedDiscount = r.AppliedDiscount,
                                                     DiscountValue = r.DiscountValue,
                                                     ShipmentPackagePrice = r.ShipmentPackagePrice,
                                                     CompanyType = r.CompanyType,
                                                     CustomerCode = r.CustomerCode,
                                                     Description = r.Description
                                                 }).ToList();


                //var shipmentsBySC = await _uow.Shipment.GetShipmentDetailByWaybills(filterOptionsDto, serviceCenters, result).Item1;

                return shipmentsBySC;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
