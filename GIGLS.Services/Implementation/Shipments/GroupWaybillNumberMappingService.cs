using System;
using System.Threading.Tasks;
using GIGLS.Core;
using GIGL.GIGLS.Core.Domain;
using System.Collections.Generic;
using GIGLS.Infrastructure;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core.DTO.Shipments;
using GIGLS.CORE.DTO.Shipments;
using GIGLS.Core.IServices.User;
using System.Linq;
using GIGLS.Core.DTO.ServiceCentres;
using AutoMapper;
using GIGLS.Core.Domain;
using GIGLS.Core.IServices.ServiceCentres;

namespace GIGLS.Services.Implementation.Shipments
{
    public class GroupWaybillNumberMappingService : IGroupWaybillNumberMappingService
    {
        private readonly IUnitOfWork _uow;
        private readonly IGroupWaybillNumberService _groupWaybillNumberService;
        private readonly IShipmentService _shipmentService;
        private readonly IUserService _userService;
        private readonly IServiceCentreService _centreService;

        public GroupWaybillNumberMappingService(IUnitOfWork uow,
            IGroupWaybillNumberService groupWaybillNumberService,
            IShipmentService shipmentService, IServiceCentreService centreService,
            IUserService userService)
        {
            _uow = uow;
            _groupWaybillNumberService = groupWaybillNumberService;
            _shipmentService = shipmentService;
            _centreService = centreService;
            _userService = userService;
            MapperConfig.Initialize();
        }


        public async Task<IEnumerable<GroupWaybillNumberMappingDTO>> GetAllGroupWayBillNumberMappings()
        {
            try
            {
                var resultSet = new HashSet<string>();
                var result = new List<GroupWaybillNumberMappingDTO>();

                var serviceCenters = await _userService.GetPriviledgeServiceCenters();
                var groupWaybillMapings = await _uow.GroupWaybillNumberMapping.GetGroupWaybillMappings(serviceCenters);
                foreach (var item in groupWaybillMapings)
                {
                    if (resultSet.Add(item.GroupWaybillNumber))
                    {
                        result.Add(item);
                    }
                }

                return result.ToList().OrderByDescending(x => x.DateCreated).Take(100);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Get GroupWaybill Number For WaybillNumber
        public async Task<GroupWaybillNumberDTO> GetGroupForWaybillNumber(string waybillNumber)
        {
            try
            {
                var shipmentDTO = await _shipmentService.GetShipment(waybillNumber);
                var groupWaybillNumberMapping = await _uow.GroupWaybillNumberMapping.GetAsync(x => x.WaybillNumber == shipmentDTO.Waybill && x.IsDeleted == false);

                if (groupWaybillNumberMapping == null)
                {
                    throw new GenericException($"No GroupWaybill exists for this Waybill: {waybillNumber}");
                }

                var groupWaybillNumberDTO = await _groupWaybillNumberService.GetGroupWayBillNumberById(groupWaybillNumberMapping.GroupWaybillNumber);
                return groupWaybillNumberDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        //Get GroupWaybill Number For WaybillNumber by ServiceCentre
        public async Task<GroupWaybillNumberDTO> GetGroupForWaybillNumberByServiceCentre(string waybillNumber)
        {
            try
            {
                var shipmentDTO = await _shipmentService.GetShipment(waybillNumber);

                //filter by SC
                var serviceCentreIds = await _userService.GetPriviledgeServiceCenters();

                //1. check if waybill is a TransitWaybill
                var transitWaybillCheck = false;
                var transitWaybillNumber = await _uow.TransitWaybillNumber.GetAsync(s => s.WaybillNumber == waybillNumber);
                if (transitWaybillNumber != null)
                {
                    transitWaybillCheck = true;
                    if (!serviceCentreIds.Contains(transitWaybillNumber.ServiceCentreId))
                    {
                        throw new GenericException($"This Waybill: {waybillNumber} (Transit) is not located in this service centre");
                    }
                }

                //2. 
                if (serviceCentreIds.Length > 0)
                {
                    if (!serviceCentreIds.Contains(shipmentDTO.DepartureServiceCentreId))
                    {
                        if(transitWaybillCheck == false)
                        {
                            throw new GenericException($"This Waybill: {waybillNumber} was not created in this service centre");
                        }
                    }
                }

                var groupWaybillNumberMapping = await _uow.GroupWaybillNumberMapping.GetAsync(x => x.WaybillNumber == shipmentDTO.Waybill);
                if (groupWaybillNumberMapping == null)
                {
                    throw new GenericException($"No GroupWaybill exists for this Waybill: {waybillNumber}");
                }

                var groupWaybillNumberDTO = await _groupWaybillNumberService.GetGroupWayBillNumberById(groupWaybillNumberMapping.GroupWaybillNumber);
                return groupWaybillNumberDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }


        //Get WaybillNumbers In Group
        public async Task<GroupWaybillNumberMappingDTO> GetWaybillNumbersInGroup(int groupWaybillNumberId)
        {
            try
            {
                var groupWaybillNumberDTO = await _groupWaybillNumberService.GetGroupWayBillNumberById(groupWaybillNumberId);
                var groupWaybillNumberMappingList = await _uow.GroupWaybillNumberMapping.FindAsync(x => x.GroupWaybillNumber == groupWaybillNumberDTO.GroupWaybillCode);

                //add to list
                List<WaybillNumberDTO> resultList = new List<WaybillNumberDTO>();
                List<ShipmentDTO> shipmentList = new List<ShipmentDTO>();
                var departureServiceCentre = new ServiceCentreDTO();
                var destinationServiceCentre = new ServiceCentreDTO();
                foreach (var groupWaybillNumberMapping in groupWaybillNumberMappingList)
                {
                    var shipmentDTO = await _shipmentService.GetShipment(groupWaybillNumberMapping.WaybillNumber);
                    resultList.Add(new WaybillNumberDTO { WaybillCode = shipmentDTO.Waybill });
                    shipmentList.Add(new ShipmentDTO
                    {
                        Waybill = shipmentDTO.Waybill,
                        DepartureServiceCentre = shipmentDTO.DepartureServiceCentre,
                        DestinationServiceCentre = shipmentDTO.DestinationServiceCentre
                    });

                    departureServiceCentre = shipmentDTO.DepartureServiceCentre;
                    destinationServiceCentre = shipmentDTO.DestinationServiceCentre;
                }

                var groupWaybillNumberMappingDTO = new GroupWaybillNumberMappingDTO
                {
                    GroupWaybillNumber = groupWaybillNumberDTO.GroupWaybillCode,
                    DepartureServiceCentre = departureServiceCentre,
                    DestinationServiceCentre = destinationServiceCentre,
                    Shipments = shipmentList
                };

                return groupWaybillNumberMappingDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Get WaybillNumbers In Group
        public async Task<GroupWaybillNumberMappingDTO> GetWaybillNumbersInGroup(string groupWaybillNumber)
        {
            try
            {
                var groupWaybillNumberDTO = await _groupWaybillNumberService.GetGroupWayBillNumberById(groupWaybillNumber);
                var groupWaybillNumberMappingList = await _uow.GroupWaybillNumberMapping.FindAsync(x => x.GroupWaybillNumber == groupWaybillNumberDTO.GroupWaybillCode);

                //add to list
                List<WaybillNumberDTO> resultList = new List<WaybillNumberDTO>();
                List<ShipmentDTO> shipmentList = new List<ShipmentDTO>();
                List<string> waybills = new List<string>();
                var departureServiceCentre = new ServiceCentreDTO();
                var destinationServiceCentre = new ServiceCentreDTO();
                var allInvoices = _uow.Invoice.GetAllFromInvoiceView().ToList();
                foreach (var groupWaybillNumberMapping in groupWaybillNumberMappingList)
                {
                    var shipmentDTO = allInvoices.FirstOrDefault(s => s.Waybill == groupWaybillNumberMapping.WaybillNumber);
                    if(shipmentDTO == null)
                    {
                        continue;
                    }

                    resultList.Add(new WaybillNumberDTO { WaybillCode = shipmentDTO.Waybill });
                    shipmentList.Add(new ShipmentDTO
                    {
                        Waybill = shipmentDTO.Waybill,
                        DepartureServiceCentreId = shipmentDTO.DepartureServiceCentreId,
                        DepartureServiceCentre = new ServiceCentreDTO()
                        {
                            Name = shipmentDTO.DepartureServiceCentreName,
                            Code = shipmentDTO.DepartureServiceCentreCode,
                            ServiceCentreId = shipmentDTO.DepartureServiceCentreId
                        },

                        DestinationServiceCentreId = shipmentDTO.DestinationServiceCentreId,
                        DestinationServiceCentre = new ServiceCentreDTO()
                        {
                            Name = shipmentDTO.DestinationServiceCentreName,
                            Code = shipmentDTO.DestinationServiceCentreCode,
                            ServiceCentreId = shipmentDTO.DestinationServiceCentreId
                        }
                    });

                    waybills.Add(shipmentDTO.Waybill);

                    departureServiceCentre = new ServiceCentreDTO()
                    {
                        Name = shipmentDTO.DepartureServiceCentreName,
                        Code = shipmentDTO.DepartureServiceCentreCode,
                        ServiceCentreId = shipmentDTO.DepartureServiceCentreId
                    };
                    destinationServiceCentre = new ServiceCentreDTO()
                    {
                        Name = shipmentDTO.DestinationServiceCentreName,
                        Code = shipmentDTO.DestinationServiceCentreCode,
                        ServiceCentreId = shipmentDTO.DestinationServiceCentreId
                    };
                }

                var groupWaybillNumberMappingDTO = new GroupWaybillNumberMappingDTO
                {
                    GroupWaybillNumber = groupWaybillNumberDTO.GroupWaybillCode,
                    DepartureServiceCentre = departureServiceCentre,
                    DestinationServiceCentre = destinationServiceCentre,
                    Shipments = shipmentList,
                    WaybillNumbers = waybills
                };

                return groupWaybillNumberMappingDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //map waybillNumber to groupWaybillNumber
        public async Task MappingWaybillNumberToGroup(string groupWaybillNumber, List<string> waybillNumberList)
        {
            try
            {
                var groupwaybillObj = await _uow.GroupWaybillNumber.GetAsync(x => x.GroupWaybillCode.Equals(groupWaybillNumber));

                //validate the ids are in the system
                var serviceCenterId = int.Parse(groupWaybillNumber.Substring(1, 3));
                var serviceCentre = await _centreService.GetServiceCentreById(serviceCenterId);
                if (groupwaybillObj == null)
                {                    
                    var currentUserId = await _userService.GetCurrentUserId();
                    var newGroupWaybill = new GroupWaybillNumber
                    {
                        GroupWaybillCode = groupWaybillNumber,
                        UserId = currentUserId,
                        ServiceCentreId = serviceCentre.ServiceCentreId,
                        IsActive = true
                    };

                    _uow.GroupWaybillNumber.Add(newGroupWaybill);
                    await _uow.CompleteAsync();
                }

                //ensure that the Manifest containing the Groupwaybill has not been dispatched
                var manifestGroupWaybillNumberMapping = _uow.ManifestGroupWaybillNumberMapping.SingleOrDefault(x => x.GroupWaybillNumber == groupWaybillNumber);
                if (manifestGroupWaybillNumberMapping != null)
                {
                    var manifestObject = _uow.Manifest.SingleOrDefault(x => x.ManifestCode == manifestGroupWaybillNumberMapping.ManifestCode);
                    if (manifestObject != null && manifestObject.IsDispatched)
                    {
                        throw new GenericException($"Error: The Manifest: {manifestObject.ManifestCode} assigned to this waybill has already been dispatched.");
                    }
                }

                foreach (var waybillNumber in waybillNumberList)
                {
                    var shipmentDTO = await _shipmentService.GetShipment(waybillNumber);
                    if (shipmentDTO == null)
                    {
                        throw new GenericException($"No Shipment exists for this : {waybillNumber}");
                    }

                    // get the service centres
                    var serviceCenters = await _userService.GetPriviledgeServiceCenters();
                    var departureServiceCenterId = serviceCenters[0];


                    //check if waybill has not been grouped 
                    var isWaybillGroup = await _uow.GroupWaybillNumberMapping.ExistAsync(x => x.GroupWaybillNumber == groupWaybillNumber && x.WaybillNumber == shipmentDTO.Waybill);

                    //if the waybill has not been grouped, group it
                    if (!isWaybillGroup)
                    {
                        //Add new Mapping
                        var newMapping = new GroupWaybillNumberMapping
                        {
                            GroupWaybillNumber = groupWaybillNumber,
                            WaybillNumber = shipmentDTO.Waybill,
                            IsActive = true,
                            DateMapped = DateTime.Now,
                            DepartureServiceCentreId = departureServiceCenterId,
                            DestinationServiceCentreId = serviceCenterId,
                            OriginalDepartureServiceCentreId = departureServiceCenterId,
                        };
                        _uow.GroupWaybillNumberMapping.Add(newMapping);

                        //check if waybill is a transitWaybill then update entry
                        var transitWaybill = _uow.TransitWaybillNumber.SingleOrDefault(s => s.WaybillNumber == shipmentDTO.Waybill);
                        if(transitWaybill != null)
                        {
                            transitWaybill.IsGrouped = true;
                        }
                    }
                }

                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //remove waybillNumber from groupWaybillNumber
        public async Task RemoveWaybillNumberFromGroup(string groupWaybillNumber, string waybillNumber)
        {
            try
            {
                var groupWaybillNumberDTO = await _groupWaybillNumberService.GetGroupWayBillNumberById(groupWaybillNumber);
                var shipmentDTO = await _shipmentService.GetShipment(waybillNumber);

                //validate the ids are in the system
                if (groupWaybillNumberDTO == null)
                {
                    throw new GenericException($"No GroupWaybill exists for this : {groupWaybillNumber}");
                }
                if (shipmentDTO == null)
                {
                    throw new GenericException($"No Shipment exists for this : {waybillNumber}");
                }

                //ensure that the Manifest containing the Groupwaybill has not been dispatched
                var manifestGroupWaybillNumberMapping = _uow.ManifestGroupWaybillNumberMapping.SingleOrDefault(x => x.GroupWaybillNumber == groupWaybillNumber);
                if(manifestGroupWaybillNumberMapping !=  null)
                {
                    var manifestObject = _uow.Manifest.SingleOrDefault(x => x.ManifestCode == manifestGroupWaybillNumberMapping.ManifestCode);
                    if(manifestObject != null && manifestObject.IsDispatched)
                    {
                        throw new GenericException($"Error: The Manifest: {manifestObject.ManifestCode} assigned to this waybill has already been dispatched.");
                    }
                }

                var groupWaybillNumberMapping = _uow.GroupWaybillNumberMapping.SingleOrDefault(x => (x.GroupWaybillNumber == groupWaybillNumber) && (x.WaybillNumber == waybillNumber));
                if (groupWaybillNumberMapping == null)
                {
                    throw new GenericException("GroupWaybillNumberMapping Does Not Exist");
                }
                _uow.GroupWaybillNumberMapping.Remove(groupWaybillNumberMapping);

                //check if waybill is a transitWaybill then update entry
                var transitWaybill = _uow.TransitWaybillNumber.SingleOrDefault(s => s.WaybillNumber == shipmentDTO.Waybill);
                if (transitWaybill != null)
                {
                    transitWaybill.IsGrouped = false;
                }

                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
