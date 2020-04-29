using AutoMapper;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.Zone;
using GIGLS.Core.DTO.ShipmentScan;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.Customers;
using GIGLS.Core.IServices.Fleets;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core.IServices.User;
using GIGLS.CORE.DTO.Report;
using GIGLS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Shipments
{
    public class ManifestWaybillMappingService : IManifestWaybillMappingService
    {
        private readonly IUnitOfWork _uow;
        private readonly IManifestService _manifestService;
        private readonly IUserService _userService;
        private readonly ICustomerService _customerService;
        private readonly IShipmentService _shipmentService;
        public readonly IDispatchService _dispatchService;
        private readonly IPreShipmentMobileService _preShipmentMobileService;

        public ManifestWaybillMappingService(IUnitOfWork uow, IManifestService manifestService,
            IUserService userService, ICustomerService customerService, IShipmentService shipmentService, IPreShipmentMobileService preShipmentMobileService)
        {
            _uow = uow;
            _manifestService = manifestService;
            _userService = userService;
            _customerService = customerService;
            _shipmentService = shipmentService;
            _preShipmentMobileService = preShipmentMobileService;

            MapperConfig.Initialize();
        }

        public async Task<List<ManifestWaybillMappingDTO>> GetAllManifestWaybillMappings()
        {
            //var serviceIds = await _userService.GetPriviledgeServiceCenters();
            //return await _uow.ManifestWaybillMapping.GetManifestWaybillMappings(serviceIds);

            var resultSet = new HashSet<string>();
            var result = new List<ManifestWaybillMappingDTO>();

            var serviceIds = await _userService.GetPriviledgeServiceCenters();
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

        public async Task<List<ManifestWaybillMappingDTO>> GetAllManifestWaybillMappings(DateFilterCriteria dateFilterCriteria)
        {
            var resultSet = new HashSet<string>();
            var result = new List<ManifestWaybillMappingDTO>();

            var serviceIds = await _userService.GetPriviledgeServiceCenters();
            var manifestWaybillMapings = await _uow.ManifestWaybillMapping.GetManifestWaybillMappings(serviceIds, dateFilterCriteria);

            foreach (var item in manifestWaybillMapings)
            {
                if (resultSet.Add(item.ManifestCode))
                {
                    result.Add(item);
                }
            }

            return result.OrderByDescending(x => x.DateCreated).ToList();
        }

        //get pickup request manifest by date 
        public async Task<List<PickupManifestWaybillMappingDTO>> GetAllPickupManifestWaybillMappings(DateFilterCriteria dateFilterCriteria)
        {
            var resultSet = new HashSet<string>();
            var result = new List<PickupManifestWaybillMappingDTO>();

            var pickupManifestWaybillMapping = await _uow.PickupManifestWaybillMapping.GetPickupManifestWaybillMapping(dateFilterCriteria);

            foreach (var item in pickupManifestWaybillMapping)
            {
                if (resultSet.Add(item.ManifestCode))
                {
                    result.Add(item);
                }
            }
            return result.OrderByDescending(x => x.DateModified).ToList();
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

                if (isWaybillsMappedActiveResult.Any())
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

                List<string> isWaybillsMappedActiveResult = isWaybillMappedActive.Select(x => x.Waybill).Distinct().ToList();

                if (isWaybillsMappedActiveResult.Any())
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
                var shipmentCollectionList = shipmentCollection.Select(x => x.Waybill).Distinct().ToList();

                //1a. Get the shipment status of the waybills we want to manifest && extrack waybills into a list from shipment collection

                //1b. check if all the waybills has the same status (ARF)
                int shipmentCollectionListCount = shipmentCollectionList.Count;
                if (shipmentCollectionListCount == 0)
                {
                    throw new GenericException($"No waybill available for Processing");
                }

                if (shipmentCollectionListCount != waybills.Count)
                {
                    var result = waybills.Where(x => !shipmentCollectionList.Contains(x));

                    if (result.Any())
                    {
                        throw new GenericException($"Error: Delivery Manifest cannot be created. " +
                            $"The following waybills [{string.Join(", ", result.ToList())}] are not available for Processing");
                    }
                }

                //2. Get the shipment details of all the waybills that we want to manifest again by 
                // check if the waybill is not cancelled, Home Delivery and the user that want to manifest it is at the final service centre of the waybills  
                //var InvoicesBySC = _uow.Invoice.GetAllFromInvoiceView();
                //var InvoicesBySC = _uow.Invoice.GetAllFromInvoiceAndShipments().Where(x => waybills.Contains(x.Waybill));
                var InvoicesBySC = _uow.Shipment.GetAllAsQueryable().Where(x => waybills.Contains(x.Waybill) && x.IsCancelled == false && x.IsDeleted == false);

                //InvoicesBySC = InvoicesBySC.Where(x => x.IsCancelled == false && x.PickupOptions == PickupOptions.HOMEDELIVERY && waybills.Contains(x.Waybill));
                //InvoicesBySC = InvoicesBySC.Where(x => x.IsCancelled == false && waybills.Contains(x.Waybill));

                //filter if the shipment is at the final service centre
                if (serviceCenters.Length > 0)
                {
                    InvoicesBySC = InvoicesBySC.Where(s => serviceCenters.Contains(s.DestinationServiceCentreId));
                }

                ////final list of home delivery waybills
                var InvoicesBySCList = InvoicesBySC.Select(x => x.Waybill).Distinct().ToList();

                //1b. check if all the waybills are equal to our home delivery 
                if (InvoicesBySCList.Count != waybills.Count)
                {
                    var result = waybills.Where(x => !InvoicesBySCList.Contains(x));

                    if (result.Any())
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

        //map waybill to Manifest (Pickup)
        public async Task MappingManifestToWaybillsPickup(string manifest, List<string> waybills)
        {
            try
            {
                //var serviceIds = await _userService.GetPriviledgeServiceCenters();
                var gigGOServiceCenter = await _userService.GetGIGGOServiceCentre();
                int userServiceCentreId = gigGOServiceCenter.ServiceCentreId;

                //1. check if any of the waybills has not been mapped to a manifest 
                // and has not been process for return in case it was not delivered (i.e still active) that day
                var isWaybillMappedActive = _uow.PickupManifestWaybillMapping.GetAllAsQueryable();
                isWaybillMappedActive = isWaybillMappedActive.Where(x => x.IsActive == true && waybills.Contains(x.Waybill));

                var isWaybillsMappedActiveResult = isWaybillMappedActive.Select(x => x.Waybill).Distinct().ToList();

                if (isWaybillsMappedActiveResult.Any())
                {
                    throw new GenericException($"Error: Manifest cannot be created. " +
                               $"The following waybills [{string.Join(", ", isWaybillsMappedActiveResult.ToList())}] already been manifested");
                }

                var manifestObj = await _uow.PickupManifest.GetAsync(x => x.ManifestCode.Equals(manifest));

                //create the manifest if manifest does not exist
                if (manifestObj == null)
                {
                    var newManifest = new PickupManifest
                    {
                        DateTime = DateTime.Now,
                        ManifestCode = manifest,
                        ManifestType = ManifestType.Pickup
                    };
                    _uow.PickupManifest.Add(newManifest);
                }

                var newWaybillList = new HashSet<string>(waybills);
                var newMappingList = new List<PickupManifestWaybillMapping>();
                List<string> newWaybillToMap = new List<string>();

                foreach (var waybill in newWaybillList)
                {
                    //check if Waybill has not been added to this manifest 
                    var isWaybillMapped = await _uow.PickupManifestWaybillMapping.ExistAsync(x => x.ManifestCode == manifest && x.Waybill == waybill);

                    //if the waybill has not been added to this manifest, add it
                    if (!isWaybillMapped)
                    {
                        //Add new Mapping
                        var newMapping = new PickupManifestWaybillMapping
                        {
                            ManifestCode = manifest,
                            Waybill = waybill,
                            IsActive = true,
                            ServiceCentreId = userServiceCentreId
                        };

                        newWaybillToMap.Add(waybill);
                        newMappingList.Add(newMapping);
                    }
                }

                //what if some of the waybill has been added before and some is in processing
                //update all as shipment Assigned for Pickup
                if (newMappingList.Any())
                {
                    var shipmentList = _uow.PreShipmentMobile.GetAllAsQueryable().Where(x => newWaybillToMap.Contains(x.Waybill)).ToList();
                    shipmentList.ForEach(x => x.shipmentstatus = "Assigned for Pickup");

                    _uow.PickupManifestWaybillMapping.AddRange(newMappingList);
                }

                foreach(string waybill in newWaybillToMap)
                {
                    //create a list to add waybill that has not been mapped
                    //then add then using addrange
                    await _preShipmentMobileService.ScanMobileShipment(new ScanDTO
                    {
                        WaybillNumber = waybill,
                        ShipmentScanStatus = ShipmentScanStatus.MAPT
                    });
                }

                _uow.Complete();
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
                        var scanList = await _uow.ScanStatus.GetAsync(x => x.Code == manifestwaybill.ShipmentScanStatus.ToString());

                        if (scanList != null)
                        {
                            manifestwaybill.ScanDescription = scanList.Incident;
                        }
                    }
                }

                return manifestWaybillNumberMappingDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Get Waybills in Pickup Manifest
        public async Task<List<PickupManifestWaybillMappingDTO>> GetWaybillsInPickupManifest(string manifestCode)
        {
            try
            {
                var pickupManifestDTO = await _manifestService.GetPickupManifestByCode(manifestCode);
                var pickupManifestWaybillMappingList = await _uow.PickupManifestWaybillMapping.FindAsync(x => x.ManifestCode == pickupManifestDTO.ManifestCode);

                var pickupManifestWaybillMappingDto = Mapper.Map<List<PickupManifestWaybillMappingDTO>>(pickupManifestWaybillMappingList.ToList());

                foreach (var pickupManifestWaybill in pickupManifestWaybillMappingDto)
                {
                    pickupManifestWaybill.PickupManifestDetails = pickupManifestDTO;

                    //get preshipment details
                    pickupManifestWaybill.PreShipment = await _preShipmentMobileService.GetPreShipmentDetail(pickupManifestWaybill.Waybill);
                }
                return pickupManifestWaybillMappingDto;

            }
            catch (Exception)
            {
                throw;
            }
        }


        //Get Waybills In Manifest for Dispatch
        public async Task<List<ManifestWaybillMappingDTO>> GetWaybillsInManifestForDispatchOld()
        {
            try
            {
                //get the current user
                string userId = await _userService.GetCurrentUserId();

                //get the dispatch for the user
                var userDispatchs = _uow.Dispatch.GetAll().Where(s => s.DriverDetail == userId && s.ReceivedBy == null).ToList();

                //get the active manifest for the dispatch user
                if (userDispatchs.Any())
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

        public async Task<List<ManifestWaybillMappingDTO>> GetWaybillsInManifestForDispatch()
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
                    return new List<ManifestWaybillMappingDTO>();
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

                List<ManifestWaybillMappingDTO> manifestWaybillNumberMappingDto = new List<ManifestWaybillMappingDTO>();

                foreach (var manifestcode in userDispatchs)
                {
                    //Get all waybills mapped to a manifest
                    var manifestWaybillMappingList = await _uow.ManifestWaybillMapping.FindAsync(x => x.ManifestCode == manifestcode.ManifestNumber);

                    //Get manifest detail
                    var manifestDTO = await _manifestService.GetManifestByCode(manifestcode.ManifestNumber);

                    //map the data to the DTO
                    var manifestMappingDto = Mapper.Map<List<ManifestWaybillMappingDTO>>(manifestWaybillMappingList.ToList());

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

                //map all the manifest code to the first in the list 
                if (userDispatchs.Any())
                {
                    var userDispatchsArray = userDispatchs.Select(u => u.ManifestNumber).ToList();
                    manifestWaybillNumberMappingDto[0].ManifestCode = string.Join(", ", userDispatchsArray);
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
                //check if the user is at the service centre
                var serviceCentreIds = await _userService.GetPriviledgeServiceCenters();

                var waybillMappingList = await _uow.ManifestWaybillMapping.FindAsync(x => x.Waybill == waybill && serviceCentreIds.Contains(x.ServiceCentreId));

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
                    var dispatch = await _uow.Dispatch.GetAsync(d => d.ManifestNumber == waybillmapped.ManifestCode);

                    var waybillMapping = Mapper.Map<ManifestWaybillMappingDTO>(waybillmapped);
                    waybillMapping.ManifestDetails = manifestDTO;

                    if (dispatch != null)
                    {
                        waybillMapping.ManifestDetails.DispatchedBy = dispatch.DispatchedBy;
                        waybillMapping.ManifestDetails.ReceiverBy = dispatch.ReceivedBy;
                    }

                    resultList.Add(waybillMapping);
                }

                return resultList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        ////Get All Waybills in Manifests that has been mapped to a rider
        //public async Task<List<ManifestWaybillMappingDTO>> GetWaybillsForDrivers(string riderId)
        //{
        //    try
        //    {
        //        var riderWaybillList = await _uow.ManifestWaybillMapping.FindAsync(x => x.)
        //    }
        //}
        //Get active Manifest that a Waybill is mapped to
        public async Task<ManifestWaybillMappingDTO> GetActiveManifestForWaybill(string waybill)
        {
            try
            {
                //check if the user is at the service centre
                var serviceCentreIds = await _userService.GetPriviledgeServiceCenters();

                var activeManifest = await _uow.ManifestWaybillMapping.GetAsync(x => x.Waybill == waybill && x.IsActive == true && serviceCentreIds.Contains(x.ServiceCentreId));

                if (activeManifest == null)
                {
                    throw new GenericException($"There is no active Manifest for this Waybill {waybill}");
                }

                //get the manifest and dispatch detail for the waybill
                var manifestDTO = await _manifestService.GetManifestByCode(activeManifest.ManifestCode);
                var activeManifestDto = Mapper.Map<ManifestWaybillMappingDTO>(activeManifest);
                activeManifestDto.ManifestDetails = manifestDTO;

                var dispatchList = await _uow.Dispatch.FindAsync(d => d.ManifestNumber == activeManifest.ManifestCode);
                var dispatch = dispatchList.FirstOrDefault();
                if (dispatch != null)
                {
                    activeManifestDto.ManifestDetails.DispatchedBy = dispatch.DispatchedBy;
                    activeManifestDto.ManifestDetails.ReceiverBy = dispatch.ReceivedBy;
                }

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
                    throw new GenericException($"Waybill {waybill} is not mapped to the manifest {manifest}");
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

        //remove Waybill from Pickup Manifest
        public async Task RemoveWaybillFromPickupManifest(string manifest, string waybill)
        {
            try
            {

                var pickupManifestDTO = await _manifestService.GetPickupManifestByCode(manifest);
                var pickuupManifestWaybillMapping = await _uow.PickupManifestWaybillMapping.GetAsync(x => x.ManifestCode == manifest && x.Waybill == waybill);

                if (pickuupManifestWaybillMapping == null)
                {
                    throw new GenericException($"Waybill {waybill} is not mapped to the manifest {manifest}");
                }

                //update shipmnt status
                var shipment = await _uow.PreShipmentMobile.GetAsync(x => x.Waybill == waybill);
                if (shipment != null)
                {
                    shipment.shipmentstatus = "Shipment created";
                }
                _uow.PickupManifestWaybillMapping.Remove(pickuupManifestWaybillMapping);


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
                //var getServiceCenterCode = await _userService.GetCurrentServiceCenter(); 

                List<CashOnDeliveryRegisterAccount> codRegisterCollectsForASingleWaybillList = new List<CashOnDeliveryRegisterAccount>();
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

                        if (shipmentCollection != null)
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
                                UserId = user,
                                ServiceCentreId = serviceCenter.ServiceCentreId
                            };
                            _uow.ShipmentTracking.Add(newShipmentTracking);
                        }
                    }
                    else
                    {
                        throw new GenericException("Error processing request. The login user is not at the final Destination nor has the right privilege");
                    }

                    //3. check and return only delivered shipments in order to update cod
                    var shipmentCollectionDelivered = await _uow.ShipmentCollection.GetAsync(x => x.Waybill == waybill && x.IsCashOnDelivery == true && (x.ShipmentScanStatus == ShipmentScanStatus.OKT || x.ShipmentScanStatus == ShipmentScanStatus.OKC));
                    if (shipmentCollectionDelivered != null)
                    {
                        //Update CashOnDevliveryRegisterAccount As  Cash Recieved at Service Center
                        var codRegisterCollectsForASingleWaybill = _uow.CashOnDeliveryRegisterAccount.Find(s => s.Waybill == waybill).FirstOrDefault();

                        if (codRegisterCollectsForASingleWaybill != null)
                        {
                            codRegisterCollectsForASingleWaybill.CODStatusHistory = CODStatushistory.RecievedAtServiceCenter;
                            codRegisterCollectsForASingleWaybill.ServiceCenterId = serviceCenter.ServiceCentreId; //getServiceCenterCode[0].ServiceCentreId;
                            codRegisterCollectsForASingleWaybillList.Add(codRegisterCollectsForASingleWaybill);
                        }
                        continue;
                    }
                }

                //update codRegisterCollectsForASingleWaybillList in the db
                codRegisterCollectsForASingleWaybillList.ForEach(s => s.CODStatusHistory = CODStatushistory.RecievedAtServiceCenter);
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
                var shipmentCollection = _uow.ShipmentCollection.GetAll()
                    .Where(x => x.ShipmentScanStatus == ShipmentScanStatus.ARF && serviceCenters.Contains(x.DestinationServiceCentreId)).Select(x => x.Waybill).ToList();

                //2. Get shipment details for the service centre that are at the collection centre using the waybill and service centre
                //var InvoicesBySC = _uow.Invoice.GetAllInvoiceShipments();
                var InvoicesBySC = _uow.Invoice.GetAllFromInvoiceAndShipments();

                //filter by destination service center that is not cancelled and it is home delivery
                InvoicesBySC = InvoicesBySC.Where(x => x.PickupOptions == PickupOptions.HOMEDELIVERY && x.IsShipmentCollected == false);
                InvoicesBySC = InvoicesBySC.Where(s => serviceCenters.Contains(s.DestinationServiceCentreId));

                ////final list
                //InvoicesBySC = InvoicesBySC.Where(s => shipmentCollection.Contains(s.Waybill));
                var InvoicesBySCResult = InvoicesBySC.ToList();
                var InvoicesBySCList = InvoicesBySC.Where(s => shipmentCollection.Contains(s.Waybill)).ToList();

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

        public async Task<List<PreShipmentMobileDTO>> GetUnMappedWaybillsForPickupManifest(int senderStationId)
        {
            try
            {
                var preshipment = _uow.PreShipmentMobile.GetAllAsQueryable().Where(x => x.SenderStationId == senderStationId && x.shipmentstatus == "Shipment created").ToList();

                var preshipmentdto = Mapper.Map<List<PreShipmentMobileDTO>>(preshipment);

                return preshipmentdto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //get manifest waiting to signoff
        public async Task<List<ManifestWaybillMappingDTO>> GetManifestWaitingForSignOff()
        {
            var resultSet = new HashSet<string>();
            var result = new List<ManifestWaybillMappingDTO>();

            var serviceIds = await _userService.GetPriviledgeServiceCenters();

            //get delivery manifest that have been dispatched but not received
            var manifests = _uow.Manifest.GetAll().Where(x => x.ManifestType == ManifestType.Delivery && x.IsDispatched == true && x.IsReceived == false).Select(m => m.ManifestCode).Distinct().ToList();

            var manifestWaybillMapings = await _uow.ManifestWaybillMapping.GetManifestWaybillWaitingForSignOff(serviceIds, manifests);

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
        public async Task<List<ManifestWaybillMappingDTO>> GetManifestHistoryForWaybill(string waybill)
        {
            try
            {
                List<ManifestWaybillMappingDTO> resultList = new List<ManifestWaybillMappingDTO>();

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

                        ManifestWaybillMappingDTO manifest = new ManifestWaybillMappingDTO();
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
                var waybillMappingList = await _uow.ManifestWaybillMapping.FindAsync(x => x.Waybill == waybill);

                if (waybillMappingList != null)
                {
                    foreach (var waybillmapped in waybillMappingList)
                    {
                        //get the manifest detail for the waybill
                        var manifestDTO = await _manifestService.GetManifestByCode(waybillmapped.ManifestCode);
                        var dispatch = await _uow.Dispatch.GetAsync(d => d.ManifestNumber == waybillmapped.ManifestCode);

                        var waybillMapping = Mapper.Map<ManifestWaybillMappingDTO>(waybillmapped);
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

        //Get Pickup Manifest
        public async Task<PickupManifestDTO> GetPickupManifest(string manifestCode)
        {
            try
            {
                var pickupManifestDTO = await _manifestService.GetPickupManifestByCode(manifestCode);
                
                return pickupManifestDTO;

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
