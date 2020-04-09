using AutoMapper;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.Zone;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.Customers;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core.IServices.User;
using GIGLS.CORE.DTO.Report;
using GIGLS.CORE.DTO.Shipments;
using GIGLS.CORE.IServices.Shipments;
using GIGLS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Shipments
{
    public class HUBManifestWaybillMappingService : IHUBManifestWaybillMappingService
    {
        private readonly IUnitOfWork _uow;
        private readonly IManifestService _manifestService;
        private readonly IUserService _userService;
        private readonly ICustomerService _customerService;
        private readonly IShipmentService _shipmentService;
        private readonly IShipmentCollectionService _collectionService;

        public HUBManifestWaybillMappingService(IUnitOfWork uow, IManifestService manifestService,
            IUserService userService, ICustomerService customerService, IShipmentService shipmentService, IShipmentCollectionService collectionService)
        {
            _uow = uow;
            _manifestService = manifestService;
            _userService = userService;
            _customerService = customerService;
            _shipmentService = shipmentService;
            _collectionService = collectionService;
            MapperConfig.Initialize();
        }
        public async Task<List<HUBManifestWaybillMappingDTO>> GetAllHUBManifestWaybillMappings()
        {
            //var serviceIds = await _userService.GetPriviledgeServiceCenters();
            //return await _uow.HUBManifestWaybillMapping.GetHUBManifestWaybillMappings(serviceIds);

            var resultSet = new HashSet<string>();
            var result = new List<HUBManifestWaybillMappingDTO>();

            var serviceIds = await _userService.GetPriviledgeServiceCenters();
            var manifestWaybillMapings = await _uow.HUBManifestWaybillMapping.GetHUBManifestWaybillMappings(serviceIds);

            foreach (var item in manifestWaybillMapings)
            {
                if (resultSet.Add(item.ManifestCode))
                {
                    result.Add(item);
                }
            }

            return result.OrderByDescending(x => x.DateCreated).ToList();
        }

        public async Task<List<HUBManifestWaybillMappingDTO>> GetAllHUBManifestWaybillMappings(DateFilterCriteria dateFilterCriteria)
        {
            var resultSet = new HashSet<string>();
            var result = new List<HUBManifestWaybillMappingDTO>();

            var serviceIds = await _userService.GetPriviledgeServiceCenters();
            var manifestWaybillMapings = await _uow.HUBManifestWaybillMapping.GetHUBManifestWaybillMappings(serviceIds, dateFilterCriteria);

            foreach (var item in manifestWaybillMapings)
            {
                if (resultSet.Add(item.ManifestCode))
                {
                    result.Add(item);
                }
            }

            //set the departure and destination hub
            var allServiceCentres = _uow.ServiceCentre.GetServiceCentres().Result;
            foreach (var manifestItem in result)
            {
                manifestItem.DepartureServiceCentre = allServiceCentres.Where(s => s.ServiceCentreId == manifestItem.ManifestDetails.DepartureServiceCentreId).FirstOrDefault();
                manifestItem.DestinationServiceCentre = allServiceCentres.Where(s => s.ServiceCentreId == manifestItem.ManifestDetails.DestinationServiceCentreId).FirstOrDefault();
            }

            return result.OrderByDescending(x => x.DateCreated).ToList();
        }

        //map waybills to Manifest        
        public async Task MappingManifestToWaybills(string manifest, List<string> waybills)
        {
            try
            {
                var serviceIds = await _userService.GetPriviledgeServiceCenters();

                //Extract hub manifest for all input waybills into the memory
                var hubManifestMapping= _uow.HUBManifestWaybillMapping.GetAllAsQueryable().Where(x => waybills.Contains(x.Waybill));
                var hubManifestMappingResult = hubManifestMapping.ToList();
                
                //1. check if any of the waybills has not been mapped to a manifest 
                // and has not been process for return in case it was not delivered (i.e still active) that day
                var isWaybillMappedActive = hubManifestMappingResult.Where(x => x.IsActive == true).Select(x => x.Waybill).Distinct().ToList();

                if (isWaybillMappedActive.Count() > 0)
                {
                    throw new GenericException($"Error: Delivery Manifest cannot be created. " +
                               $"The following waybills [{string.Join(", ", isWaybillMappedActive.ToList())}] already been manifested");
                }
                
                //check if any Waybill has been added to input manifest, silently remove the waybill from the waybills list 
                var isWaybillMappedAlready = hubManifestMappingResult.Where(x => x.ManifestCode == manifest).Select(x => x.Waybill).Distinct().ToList();
                
                if (isWaybillMappedAlready.Count() > 0)
                {
                    foreach (string item in isWaybillMappedAlready)
                    {
                        waybills.Remove(item);
                    }
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

                List<HUBManifestWaybillMapping> hubMapping = new List<HUBManifestWaybillMapping>();

                foreach (var waybill in waybills)
                {
                    //check if the waybill available for collection
                    var shipmentCollection = await _uow.ShipmentCollection.GetAsync(x => x.Waybill == waybill);
                    if (shipmentCollection == null)
                    {
                        throw new GenericException($"Waybill {waybill} is not available for collection");
                    }

                    //check if the user is at the final destination centre of the shipment
                    if (serviceIds.Length == 1 && serviceIds[0] == shipmentCollection.DestinationServiceCentreId)
                    {
                    }
                    else
                    {
                        throw new GenericException("Error processing request. The login user is not at the final Destination nor has the right privilege");
                    }

                    //check if the shipment is at the final destination with a scan of ARF (WHEN SHIPMENT ARRIVED FINAL DESTINATION)                    
                    if (shipmentCollection.ShipmentScanStatus != ShipmentScanStatus.ARF)
                    {
                        throw new GenericException($"Shipment with waybill: {waybill} is not available for Processing");
                    }
                    else
                    {
                        //WC -- SCAN BEFORE SHIPMENT IS TAKEN OUT FOR DELIVERY TO RECEIVER
                        shipmentCollection.ShipmentScanStatus = ShipmentScanStatus.WC;
                    }

                    //Add new Mapping
                    var newMapping = new HUBManifestWaybillMapping
                    {
                        ManifestCode = manifest,
                        Waybill = waybill,
                        IsActive = true,
                        ServiceCentreId = shipmentCollection.DestinationServiceCentreId
                    };

                    hubMapping.Add(newMapping);
                }

                _uow.HUBManifestWaybillMapping.AddRange(hubMapping);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //map waybills to HUBManifest
        public async Task MappingHUBManifestToWaybills(string manifest, List<string> waybills, int DepartureServiceCentreId, int DestinationServiceCentreId)
        {
            try
            {
                //Extract hub manifest for all input waybills into the memory
                var hubManifestMapping = _uow.HUBManifestWaybillMapping.GetAllAsQueryable().Where(x => waybills.Contains(x.Waybill));
                var hubManifestMappingResult = hubManifestMapping.ToList();
                
                //check if any Waybill has been added to input manifest, silently remove the waybill from the waybills list 
                var isWaybillMappedAlready = hubManifestMappingResult.Where(x => x.ManifestCode == manifest).Select(x => x.Waybill).Distinct().ToList();

                if (isWaybillMappedAlready.Count() > 0)
                {
                    foreach (string item in isWaybillMappedAlready)
                    {
                        waybills.Remove(item);
                    }
                }

                var serviceCenter = await _uow.ServiceCentre.GetAsync(DepartureServiceCentreId);
                string user = await _userService.GetCurrentUserId();
                var manifestObj = await _uow.Manifest.GetAsync(x => x.ManifestCode.Equals(manifest));

                //1. create the manifest if manifest does not exist
                if (manifestObj == null)
                {
                    var newManifest = new Manifest
                    {
                        DateTime = DateTime.Now,
                        ManifestCode = manifest,
                        ManifestType = ManifestType.HUB,
                        DepartureServiceCentreId = DepartureServiceCentreId,
                        DestinationServiceCentreId = DestinationServiceCentreId
                    };
                    _uow.Manifest.Add(newManifest);
                }
                
                List<HUBManifestWaybillMapping> hubMapping = new List<HUBManifestWaybillMapping>();
                List<ShipmentTracking> shipmentTrackingList = new List<ShipmentTracking>();

                //Get all shipment collection into the memory
                var shipmentCollectionList = _uow.ShipmentCollection.GetAllAsQueryable().Where(x => waybills.Contains(x.Waybill)).ToList();

                foreach (var waybill in waybills)
                {
                    //check if the waybill available for collection
                    var shipmentCollection = shipmentCollectionList.Where(x => x.Waybill == waybill).FirstOrDefault();

                    if (shipmentCollection == null)
                    {
                        throw new GenericException($"Waybill {waybill} is not available for collection");
                    }

                    //check if the shipment is at the final destination with a scan of ARF (WHEN SHIPMENT ARRIVED FINAL DESTINATION)                    
                    if (shipmentCollection.ShipmentScanStatus != ShipmentScanStatus.ARF)
                    {
                        throw new GenericException($"Shipment with waybill: {waybill} is not available for Processing");
                    }
                    else
                    {
                        //Add new Mapping
                        var newMapping = new HUBManifestWaybillMapping
                        {
                            ManifestCode = manifest,
                            Waybill = waybill,
                            IsActive = true,
                            ServiceCentreId = shipmentCollection.DestinationServiceCentreId
                        };
                        
                        //DPC -- //SCAN BEFORE SHIPMENT IS TAKEN OUT FOR DELIVERY TO HUB
                        //shipmentCollection.ShipmentScanStatus = ShipmentScanStatus.DPC;

                        //Add scan status to  the tracking page
                        var newShipmentTracking = new ShipmentTracking
                        {
                            Waybill = waybill,
                            Location = serviceCenter.Name,
                            Status = ShipmentScanStatus.DPC.ToString(),
                            DateTime = DateTime.Now,
                            UserId = user,
                            ServiceCentreId = serviceCenter.ServiceCentreId
                        };
                        
                        hubMapping.Add(newMapping);
                        shipmentTrackingList.Add(newShipmentTracking);
                    }
                }

                //update all shipment as grouped
                shipmentCollectionList.ForEach(x => x.ShipmentScanStatus = ShipmentScanStatus.DPC);

                _uow.HUBManifestWaybillMapping.AddRange(hubMapping);
                _uow.ShipmentTracking.AddRange(shipmentTrackingList);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task MappingHUBManifestToWaybillsForScanner(string manifest, List<string> waybills, int DestinationServiceCentreId)
        {
            try
            {
                FilterOptionsDto filterOptionsDto = new FilterOptionsDto
                {
                    count = 1000000,
                    page = 1,
                    sortorder = "0",
                    PageIndex = 1,
                    PageSize = 1000000
                };

                //Get all shipment available for hub manifest in the service centre
                var _shipmentWaitingForCollectionForHub = await _collectionService.GetShipmentWaitingForCollectionForHub(filterOptionsDto);
                var _shipmentWaitingForCollectionForHubResult = _shipmentWaitingForCollectionForHub.Item1.Select(x => x.Waybill).ToList();
                
                var isWaybillAvailableForMapping = waybills.Where(x => !_shipmentWaitingForCollectionForHubResult.Contains(x));

                if (isWaybillAvailableForMapping.Count() > 0)
                {
                    throw new GenericException($"Error: The following waybills [{string.Join(", ", isWaybillAvailableForMapping.ToList())}]" +
                           $" can not be added to the manifest because they are not available for processing. Remove them from the list to proceed");
                }

                var serviceCenters = await _userService.GetPriviledgeServiceCenters();
                int departureServiceCentreId = serviceCenters[0];

                var serviceCenter = await _uow.ServiceCentre.GetAsync(departureServiceCentreId);
                string user = await _userService.GetCurrentUserId();
                var manifestObj = await _uow.Manifest.GetAsync(x => x.ManifestCode.Equals(manifest));

                //1. create the manifest if manifest does not exist
                if (manifestObj == null)
                {
                    var newManifest = new Manifest
                    {
                        DateTime = DateTime.Now,
                        ManifestCode = manifest,
                        ManifestType = ManifestType.HUB,
                        DepartureServiceCentreId = departureServiceCentreId,
                        DestinationServiceCentreId = DestinationServiceCentreId
                    };
                    _uow.Manifest.Add(newManifest);
                }

                List<HUBManifestWaybillMapping> hubMapping = new List<HUBManifestWaybillMapping>();
                List<ShipmentTracking> shipmentTrackingList = new List<ShipmentTracking>();

                //Get all shipment collection into the memory
                var shipmentCollectionList = _uow.ShipmentCollection.GetAllAsQueryable().Where(x => waybills.Contains(x.Waybill)).ToList();

                foreach (var waybill in waybills)
                {
                    //check if the waybill available for collection
                    var shipmentCollection = shipmentCollectionList.Where(x => x.Waybill == waybill && x.ShipmentScanStatus == ShipmentScanStatus.ARF).FirstOrDefault();

                    if (shipmentCollection == null)
                    {
                        throw new GenericException($"Shipment with waybill: {waybill} is not available for Processing");
                    }
                    else
                    {
                        //Add new Mapping
                        var newMapping = new HUBManifestWaybillMapping
                        {
                            ManifestCode = manifest,
                            Waybill = waybill,
                            IsActive = true,
                            ServiceCentreId = shipmentCollection.DestinationServiceCentreId
                        };
                        
                        //Add scan status to  the tracking page
                        var newShipmentTracking = new ShipmentTracking
                        {
                            Waybill = waybill,
                            Location = serviceCenter.Name,
                            Status = ShipmentScanStatus.DPC.ToString(),
                            DateTime = DateTime.Now,
                            UserId = user,
                            ServiceCentreId = serviceCenter.ServiceCentreId
                        };

                        hubMapping.Add(newMapping);
                        shipmentTrackingList.Add(newShipmentTracking);
                    }
                }

                //update all shipment as grouped
                shipmentCollectionList.ForEach(x => x.ShipmentScanStatus = ShipmentScanStatus.DPC);

                _uow.HUBManifestWaybillMapping.AddRange(hubMapping);
                _uow.ShipmentTracking.AddRange(shipmentTrackingList);
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
                var isWaybillMappedActive = _uow.HUBManifestWaybillMapping.GetAllAsQueryable();
                isWaybillMappedActive = isWaybillMappedActive.Where(x => x.IsActive == true && waybills.Contains(x.Waybill));

                List<string> isWaybillsMappedActiveResult = isWaybillMappedActive.Select(x => x.Waybill).Distinct().ToList();

                if (isWaybillsMappedActiveResult.Count() > 0)
                {
                    throw new GenericException($"Error: Delivery Manifest cannot be created. " +
                               $"The following waybills [{string.Join(", ", isWaybillsMappedActiveResult.ToList())}] already been manifested");
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
                    var isWaybillMapped = await _uow.HUBManifestWaybillMapping.ExistAsync(x => x.ManifestCode == manifest && x.Waybill == waybill);

                    //if the waybill has not been added to this manifest, add it
                    if (!isWaybillMapped)
                    {
                        //Add new Mapping
                        var newMapping = new HUBManifestWaybillMapping
                        {
                            ManifestCode = manifest,
                            Waybill = waybill,
                            IsActive = true,
                            ServiceCentreId = serviceIds[0]
                        };
                        _uow.HUBManifestWaybillMapping.Add(newMapping);
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
                var shipmentCollectionList = shipmentCollection.Select(x => x.Waybill).Distinct().ToList();

                //1a. Get the shipment status of the waybills we want to manifest && extrack waybills into a list from shipment collection

                //1b. check if all the waybills has the same status (ARF)
                if (shipmentCollectionList.Count() == 0)
                {
                    throw new GenericException($"No waybill available for Processing");
                }

                if (shipmentCollectionList.Count() != waybills.Count())
                {
                    var result = waybills.Where(x => !shipmentCollectionList.Contains(x));

                    if (result.Count() > 0)
                    {
                        throw new GenericException($"Error: Delivery Manifest cannot be created. " +
                            $"The following waybills [{string.Join(", ", result.ToList())}] are not available for Processing");
                    }
                }

                //2. Get the shipment details of all the waybills that we want to manifest again by 
                // check if the waybill is not cancelled, Home Delivery and the user that want to manifest it is at the final service centre of the waybills  
                //var InvoicesBySC = _uow.Invoice.GetAllFromInvoiceView();
                //InvoicesBySC = InvoicesBySC.Where(x => x.IsCancelled == false && x.PickupOptions == PickupOptions.HOMEDELIVERY && waybills.Contains(x.Waybill));
                //InvoicesBySC = InvoicesBySC.Where(x => x.IsCancelled == false && waybills.Contains(x.Waybill));
                var InvoicesBySC = _uow.Shipment.GetAllAsQueryable().Where(x => waybills.Contains(x.Waybill) && x.IsCancelled == false && x.IsDeleted == false);

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
                            $"The following waybills [{string.Join(", ", result.ToList())}] are not available for Processing. " +
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
        public async Task<List<HUBManifestWaybillMappingDTO>> GetWaybillsInManifest(string manifestcode)
        {
            try
            {
                var manifestDTO = await _manifestService.GetManifestByCode(manifestcode);

                //needed to set the departure and destination hub
                var allServiceCentres = _uow.ServiceCentre.GetServiceCentres().Result;
                //set the departure and destination hub
                manifestDTO.DepartureServiceCentre = allServiceCentres.Where(s => s.ServiceCentreId == manifestDTO.DepartureServiceCentreId).FirstOrDefault();
                manifestDTO.DestinationServiceCentre = allServiceCentres.Where(s => s.ServiceCentreId == manifestDTO.DestinationServiceCentreId).FirstOrDefault();

                var manifestWaybillMappingList = await _uow.HUBManifestWaybillMapping.FindAsync(x => x.ManifestCode == manifestDTO.ManifestCode);
                var manifestWaybillNumberMappingDto = Mapper.Map<List<HUBManifestWaybillMappingDTO>>(manifestWaybillMappingList.ToList());
                foreach (var manifestwaybill in manifestWaybillNumberMappingDto)
                {
                    manifestwaybill.ManifestDetails = manifestDTO;

                    //get shipment detail 
                    manifestwaybill.Shipment = await _shipmentService.GetBasicShipmentDetail(manifestwaybill.Waybill);

                    CustomerType customerType;
                    if (manifestwaybill.Shipment.CustomerType == CustomerType.Company.ToString())
                    {
                        customerType = CustomerType.Company;
                    }
                    else
                    {
                        customerType = CustomerType.IndividualCustomer;
                    }

                    //Get  customer detail
                    var currentCustomerObject = await _customerService.GetCustomer(manifestwaybill.Shipment.CustomerId, customerType);
                    manifestwaybill.Shipment.CustomerDetails = currentCustomerObject;

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
        public async Task<List<HUBManifestWaybillMappingDTO>> GetWaybillsInManifestForDispatchOld()
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
                    return new List<HUBManifestWaybillMappingDTO>();
                }

                var manifestDTO = await _manifestService.GetManifestByCode(currentUserDispatch.ManifestNumber);
                var manifestWaybillMappingList = await _uow.HUBManifestWaybillMapping.FindAsync(x => x.ManifestCode == manifestDTO.ManifestCode);

                var manifestWaybillNumberMappingDto = Mapper.Map<List<HUBManifestWaybillMappingDTO>>(manifestWaybillMappingList.ToList());

                foreach (var manifestwaybill in manifestWaybillNumberMappingDto)
                {
                    manifestwaybill.ManifestDetails = manifestDTO;

                    //get shipment detail 
                    manifestwaybill.Shipment = await _shipmentService.GetShipment(manifestwaybill.Waybill);

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

        public async Task<List<HUBManifestWaybillMappingDTO>> GetWaybillsInManifestForDispatch()
        {
            try
            {
                //get the current user
                string userId = await _userService.GetCurrentUserId();

                //get the dispatch for the user
                var userDispatchs = _uow.Dispatch.GetAll().Where(s => s.DriverDetail == userId && s.ReceivedBy == null).ToList();
                int userDispatchsCount = userDispatchs.Count;

                if (userDispatchsCount == 0)
                {
                    //return an empty list
                    return new List<HUBManifestWaybillMappingDTO>();
                }
                else
                {
                    //get the active manifest for the dispatch user
                    //error, the dispatch user can have an undelivered dispatch
                    var manifestCodeArray = userDispatchs.Select(s => s.ManifestNumber).ToList();
                    var manifestObjects = _uow.Manifest.GetAll().Where(s =>
                    manifestCodeArray.Contains(s.ManifestCode) && s.ManifestType == ManifestType.Delivery).ToList();

                    //update userDispatchs
                    var deliveryManifestCodeArray = manifestObjects.Select(s => s.ManifestCode).ToList();
                    userDispatchs = userDispatchs.Where(s =>
                    deliveryManifestCodeArray.Contains(s.ManifestNumber)).ToList();
                }

                List<HUBManifestWaybillMappingDTO> manifestWaybillNumberMappingDto = new List<HUBManifestWaybillMappingDTO>();

                foreach (var manifestcode in userDispatchs)
                {
                    //Get all waybills mapped to a manifest
                    var manifestWaybillMappingList = await _uow.HUBManifestWaybillMapping.FindAsync(x => x.ManifestCode == manifestcode.ManifestNumber);

                    //Get manifest detail
                    var manifestDTO = await _manifestService.GetManifestByCode(manifestcode.ManifestNumber);

                    //map the data to the DTO
                    var manifestMappingDto = Mapper.Map<List<HUBManifestWaybillMappingDTO>>(manifestWaybillMappingList.ToList());

                    //add manifest details to the dto
                    foreach (var waybill in manifestMappingDto)
                    {
                        waybill.ManifestDetails = manifestDTO;
                    }

                    //add it to range of array
                    manifestWaybillNumberMappingDto.AddRange(manifestMappingDto);
                }

                //get shipment detail for the manifest
                foreach (var manifestwaybill in manifestWaybillNumberMappingDto)
                {
                    //get shipment detail 
                    manifestwaybill.Shipment = await _shipmentService.GetShipment(manifestwaybill.Waybill);

                    //get from ShipmentCollection
                    var shipmentCollectionObj = await _uow.ShipmentCollection.GetAsync(x => x.Waybill == manifestwaybill.Waybill);
                    if (shipmentCollectionObj != null)
                    {
                        manifestwaybill.ShipmentScanStatus = shipmentCollectionObj.ShipmentScanStatus;
                    }
                }

                //[{string.Join(", ", deliveryManifestCodeArray)}]");
                // var deliveryManifestCodeArray = manifestObjects.Select(s => s.ManifestCode).ToList();

                //map all the manifest code to the first in the list 
                var userDispatchsArray = userDispatchs.Select(u => u.ManifestNumber).ToList();
                manifestWaybillNumberMappingDto[0].ManifestCode = string.Join(", ", userDispatchsArray);

                return manifestWaybillNumberMappingDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Get All Manifests that a Waybill has been mapped to
        public async Task<List<HUBManifestWaybillMappingDTO>> GetManifestForWaybill(string waybill)
        {
            try
            {
                //check if the user is at the service centre
                var serviceCentreIds = await _userService.GetPriviledgeServiceCenters();

                var waybillMappingList = await _uow.HUBManifestWaybillMapping.FindAsync(x => x.Waybill == waybill && serviceCentreIds.Contains(x.ServiceCentreId));

                if (waybillMappingList == null)
                {
                    throw new GenericException($"Waybill {waybill} has not been mapped to any manifest");
                }

                //add to list
                List<HUBManifestWaybillMappingDTO> resultList = new List<HUBManifestWaybillMappingDTO>();

                foreach (var waybillmapped in waybillMappingList)
                {
                    //get the manifest detail for the waybill
                    var manifestDTO = await _manifestService.GetManifestByCode(waybillmapped.ManifestCode);
                    var dispatch = await _uow.Dispatch.GetAsync(d => d.ManifestNumber == waybillmapped.ManifestCode);

                    var waybillMapping = Mapper.Map<HUBManifestWaybillMappingDTO>(waybillmapped);
                    waybillMapping.ManifestDetails = manifestDTO;

                    if (dispatch != null)
                    {
                        waybillMapping.ManifestDetails.DispatchedBy = dispatch.DispatchedBy;
                        waybillMapping.ManifestDetails.ReceiverBy = dispatch.ReceivedBy;
                    }

                    //set the departure and destination hub
                    var allServiceCentres = _uow.ServiceCentre.GetServiceCentres().Result;
                    waybillMapping.DepartureServiceCentre = allServiceCentres.Where(s => s.ServiceCentreId == waybillMapping.ManifestDetails.DepartureServiceCentreId).FirstOrDefault();
                    waybillMapping.DestinationServiceCentre = allServiceCentres.Where(s => s.ServiceCentreId == waybillMapping.ManifestDetails.DestinationServiceCentreId).FirstOrDefault();

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
        public async Task<HUBManifestWaybillMappingDTO> GetActiveManifestForWaybill(string waybill)
        {
            try
            {
                var activeManifest = await _uow.HUBManifestWaybillMapping.GetAsync(x => x.Waybill == waybill && x.IsActive == true);
                if (activeManifest == null)
                {
                    throw new GenericException($"There is no active Manifest for this Waybill {waybill}");
                }

                //get the manifest and dispatch detail for the waybill
                var manifestDTO = await _manifestService.GetManifestByCode(activeManifest.ManifestCode);
                var activeManifestDto = Mapper.Map<HUBManifestWaybillMappingDTO>(activeManifest);
                activeManifestDto.ManifestDetails = manifestDTO;

                var dispatchList = await _uow.Dispatch.FindAsync(d => d.ManifestNumber == activeManifest.ManifestCode);
                var dispatch = dispatchList.FirstOrDefault();
                if (dispatch != null)
                {
                    activeManifestDto.ManifestDetails.DispatchedBy = dispatch.DispatchedBy;
                    activeManifestDto.ManifestDetails.ReceiverBy = dispatch.ReceivedBy;
                }

                //set the departure and destination hub
                var allServiceCentres = _uow.ServiceCentre.GetServiceCentres().Result;
                activeManifestDto.DepartureServiceCentre = allServiceCentres.Where(s => s.ServiceCentreId == activeManifestDto.ManifestDetails.DepartureServiceCentreId).FirstOrDefault();
                activeManifestDto.DestinationServiceCentre = allServiceCentres.Where(s => s.ServiceCentreId == activeManifestDto.ManifestDetails.DestinationServiceCentreId).FirstOrDefault();

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

                var manifestWaybillMapping = await _uow.HUBManifestWaybillMapping.GetAsync(x => x.ManifestCode == manifest && x.Waybill == waybill);

                if (manifestWaybillMapping == null)
                {
                    throw new GenericException($"Waybill {waybill} is not mapped to the manifest {manifest}");
                }

                //update shipment collection centre
                var shipmentCollection = await _uow.ShipmentCollection.GetAsync(x => x.Waybill == waybill && x.ShipmentScanStatus == ShipmentScanStatus.DPC);

                if (shipmentCollection != null)
                {
                    shipmentCollection.ShipmentScanStatus = ShipmentScanStatus.ARF;
                }

                _uow.HUBManifestWaybillMapping.Remove(manifestWaybillMapping);
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
                var getServiceCenterCode = await _userService.GetCurrentServiceCenter();

                foreach (var waybill in waybills)
                {
                    //1. check if the waybill is in the manifest 
                    var manifestWaybillMapping = await _uow.HUBManifestWaybillMapping.GetAsync(x => x.ManifestCode == manifest && x.Waybill == waybill);

                    if (manifestWaybillMapping == null)
                    {
                        throw new GenericException($"Waybill {waybill} is not mapped to the manifest {manifest}");
                    }

                    //update manifestWaybillMapping status for the waybill
                    manifestWaybillMapping.IsActive = false;

                    var shipmentCollection = await _uow.ShipmentCollection.GetAsync(x => x.Waybill == waybill);
                    if (shipmentCollection == null)
                    {
                        throw new GenericException($"Shipment with waybill: {waybill} is not available for Processing");
                    }
                    else
                    {
                        //Update shipment collection to make it available at collection centre
                        shipmentCollection.ShipmentScanStatus = ShipmentScanStatus.ARF;
                        shipmentCollection.DestinationServiceCentreId = serviceCenter.ServiceCentreId;

                        //Update the Destination Service Centre on the Shipment to the Current HUB
                        var shipment = await _uow.Shipment.GetAsync(x => x.Waybill == waybill);
                        shipment.DestinationServiceCentreId = serviceCenter.ServiceCentreId;

                        //Add scan status to  the tracking page
                        var newShipmentTracking = new ShipmentTracking
                        {
                            Waybill = waybill,
                            Location = serviceCenter.Name,
                            Status = ShipmentScanStatus.ARP.ToString(),
                            DateTime = DateTime.Now,
                            UserId = user,
                            ServiceCentreId = serviceCenter.ServiceCentreId
                        };
                        _uow.ShipmentTracking.Add(newShipmentTracking);
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
                List<ShipmentDTO> shipmentsBySC = new List<ShipmentDTO>();

                var serviceCenters = await _userService.GetPriviledgeServiceCenters();

                //1. get all shipments at colletion centre for the service centre which status is ARF
                //var shipmentCollection = await _uow.ShipmentCollection.FindAsync(x => x.ShipmentScanStatus == ShipmentScanStatus.ARF);
                var shipmentCollection = _uow.ShipmentCollection.GetAll()
                    .Where(x => x.ShipmentScanStatus == ShipmentScanStatus.ARF).Select(x => x.Waybill);

                //2. Get shipment details for the service centre that are at the collection centre using the waybill and service centre
                var InvoicesBySC = _uow.Invoice.GetAllInvoiceShipments();

                //filter by destination service center that is not cancelled and it is home delivery
                InvoicesBySC = InvoicesBySC.Where(x => x.PickupOptions == PickupOptions.HOMEDELIVERY);

                if (serviceCenters.Length > 0)
                {
                    InvoicesBySC = InvoicesBySC.Where(s => serviceCenters.Contains(s.DestinationServiceCentreId));
                }

                ////final list
                InvoicesBySC = InvoicesBySC.Where(s => shipmentCollection.Contains(s.Waybill));
                var InvoicesBySCList = InvoicesBySC.ToList();

                shipmentsBySC = (from r in InvoicesBySCList
                                 select new ShipmentDTO()
                                 {
                                     ShipmentId = r.ShipmentId,
                                     Waybill = r.Waybill,
                                     CustomerId = r.CustomerId,
                                     CustomerType = r.CustomerType,
                                     DateCreated = r.DateCreated,
                                     DateModified = r.DateModified,
                                     DeliveryOptionId = r.DeliveryOptionId,
                                     DeliveryOption = new DeliveryOptionDTO
                                     {
                                         Code = r.DeliveryOptionCode,
                                         Description = r.DeliveryOptionDescription
                                     },
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
                                     DiscountValue = r.DiscountValue,
                                     //ShipmentPackagePrice = r.ShipmentPackagePrice,
                                     CompanyType = r.CompanyType,
                                     CustomerCode = r.CustomerCode,
                                     Description = r.Description
                                 }).ToList();
                return shipmentsBySC;
            }
            catch (Exception)
            {
                throw;
            }
        }


        //get manifest waiting to signoff
        public async Task<List<HUBManifestWaybillMappingDTO>> GetManifestWaitingForSignOff()
        {
            var resultSet = new HashSet<string>();
            var result = new List<HUBManifestWaybillMappingDTO>();

            var serviceIds = await _userService.GetPriviledgeServiceCenters();

            //get delivery manifest that have been dispatched but not received
            var manifests = _uow.Manifest.GetAll().Where(x => x.ManifestType == ManifestType.Delivery && x.IsDispatched == true && x.IsReceived == false).Select(m => m.ManifestCode).Distinct().ToList();

            var manifestWaybillMapings = await _uow.HUBManifestWaybillMapping.GetHUBManifestWaybillWaitingForSignOff(serviceIds, manifests);

            foreach (var item in manifestWaybillMapings)
            {
                if (resultSet.Add(item.ManifestCode))
                {
                    result.Add(item);
                }
            }

            return result.OrderByDescending(x => x.DateCreated).ToList();
        }

        //get all manifests that the waybill pass through 
        public async Task<List<HUBManifestWaybillMappingDTO>> GetManifestHistoryForWaybill(string waybill)
        {
            try
            {
                List<HUBManifestWaybillMappingDTO> resultList = new List<HUBManifestWaybillMappingDTO>();

                //This part hanlde internal and external manifest
                // 1.Get waybill in a Group Waybill
                var groupWaybillNumberMapping = await _uow.GroupWaybillNumberMapping.FindAsync(x => x.WaybillNumber == waybill);
                if (groupWaybillNumberMapping != null)
                {
                    foreach (var s in groupWaybillNumberMapping.ToList())
                    {
                        //2. Use the Groupwaybill to get manifest
                        var manifestGroupWaybillMapings = await _uow.ManifestGroupWaybillNumberMapping.GetAsync(x => x.GroupWaybillNumber == s.GroupWaybillNumber);

                        //get the manifest detail for the waybill
                        var manifestDTO = await _manifestService.GetManifestByCode(manifestGroupWaybillMapings.ManifestCode);
                        var dispatch = await _uow.Dispatch.GetAsync(d => d.ManifestNumber == manifestGroupWaybillMapings.ManifestCode);

                        HUBManifestWaybillMappingDTO manifest = new HUBManifestWaybillMappingDTO();
                        manifest.DateCreated = manifestGroupWaybillMapings.DateCreated;
                        manifest.DateModified = manifestGroupWaybillMapings.DateModified;
                        manifest.ManifestCode = manifestGroupWaybillMapings.ManifestCode;

                        manifest.ManifestDetails = manifestDTO;

                        if (dispatch != null)
                        {
                            manifest.ManifestDetails.DispatchedBy = dispatch.DispatchedBy;
                            manifest.ManifestDetails.ReceiverBy = dispatch.ReceivedBy;
                        }

                        resultList.Add(manifest);
                    }
                }

                //This part hanlde delivery manifest
                var waybillMappingList = await _uow.HUBManifestWaybillMapping.FindAsync(x => x.Waybill == waybill);

                if (waybillMappingList != null)
                {
                    foreach (var waybillmapped in waybillMappingList)
                    {
                        //get the manifest detail for the waybill
                        var manifestDTO = await _manifestService.GetManifestByCode(waybillmapped.ManifestCode);
                        var dispatch = await _uow.Dispatch.GetAsync(d => d.ManifestNumber == waybillmapped.ManifestCode);

                        var waybillMapping = Mapper.Map<HUBManifestWaybillMappingDTO>(waybillmapped);
                        waybillMapping.ManifestDetails = manifestDTO;

                        if (dispatch != null)
                        {
                            waybillMapping.ManifestDetails.DispatchedBy = dispatch.DispatchedBy;
                            waybillMapping.ManifestDetails.ReceiverBy = dispatch.ReceivedBy;
                        }

                        resultList.Add(waybillMapping);
                    }
                }

                return resultList.OrderBy(x => x.ManifestDetails.DateTime).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}
