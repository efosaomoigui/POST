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
using GIGLS.CORE.DTO.Shipments;

namespace GIGLS.Services.Implementation.Shipments
{
    public class ManifestWaybillMappingService : IManifestWaybillMappingService
    {
        private readonly IUnitOfWork _uow;
        private readonly IManifestService _manifestService;
        private readonly IUserService _userService;
        private readonly IShipmentService _shipmentService;

        public ManifestWaybillMappingService(IUnitOfWork uow, IManifestService manifestService, IUserService userService, IShipmentService shipmentService)
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
                    if(shipment == null)
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
                    var isWaybillMappedActive = await _uow.ManifestWaybillMapping.ExistAsync(x => x.Waybill == waybill && x.IsActive == true);
                    if (isWaybillMappedActive)
                    {
                        throw new GenericException($"Waybill {waybill} has already been manifested");
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
                    manifestwaybill.Shipment = await _shipmentService.GetShipment(manifestwaybill.Waybill);
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

                var filterOptionsDto = new FilterOptionsDto
                {
                    count = 500,
                    page = 1,
                    sortorder = "0"
                };

                //1. get all shipments at colletion centre for the service centre which status is ARF
                var shipmentCollection = await _uow.ShipmentCollection.FindAsync(x => x.ShipmentScanStatus == ShipmentScanStatus.ARF);

                if(shipmentCollection.Count() == 0)
                {
                    return new List<ShipmentDTO>();
                }

                var result = new List<string>();
                foreach (var waybillCollection in shipmentCollection)
                {
                    result.Add(waybillCollection.Waybill);
                }

                //2. Get shipment details for the service centre that are at the collection centre using the waybill and service centre
                var shipmentsBySC = await _uow.Shipment.GetShipmentDetailByWaybills(filterOptionsDto, serviceCenters, result).Item1;

                return shipmentsBySC;
            }
            catch (Exception e)
            {
                throw;
            }
        }
                

    }
}
