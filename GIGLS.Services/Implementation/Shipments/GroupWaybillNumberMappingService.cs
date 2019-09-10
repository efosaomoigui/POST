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
using GIGLS.Core.Domain;
using GIGLS.Core.IServices.ServiceCentres;
using GIGLS.CORE.IServices.Shipments;
using GIGLS.CORE.Domain;
using GIGLS.Core.Enums;
using GIGLS.CORE.DTO.Report;

namespace GIGLS.Services.Implementation.Shipments
{
    public class GroupWaybillNumberMappingService : IGroupWaybillNumberMappingService
    {
        private readonly IUnitOfWork _uow;
        private readonly IGroupWaybillNumberService _groupWaybillNumberService;
        private readonly IShipmentService _shipmentService;
        private readonly IOverdueShipmentService _overdueShipment;
        private readonly IUserService _userService;
        private readonly IServiceCentreService _centreService;

        public GroupWaybillNumberMappingService(IUnitOfWork uow,
            IGroupWaybillNumberService groupWaybillNumberService,
            IOverdueShipmentService overdueShipment,
            IShipmentService shipmentService, IServiceCentreService centreService,
            IUserService userService)
        {
            _uow = uow;
            _groupWaybillNumberService = groupWaybillNumberService;
            _shipmentService = shipmentService;
            _centreService = centreService;
            _overdueShipment = overdueShipment;
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

        public async Task<IEnumerable<GroupWaybillNumberMappingDTO>> GetAllGroupWayBillNumberMappings(DateFilterCriteria dateFilterCriteria)
        {
            try
            {
                var resultSet = new HashSet<string>();
                var result = new List<GroupWaybillNumberMappingDTO>();

                var serviceCenters = await _userService.GetPriviledgeServiceCenters();
                var groupWaybillMapings = await _uow.GroupWaybillNumberMapping.GetGroupWaybillMappings(serviceCenters, dateFilterCriteria);

                foreach (var item in groupWaybillMapings)
                {
                    if (resultSet.Add(item.GroupWaybillNumber))
                    {
                        result.Add(item);
                    }
                }

                return result.ToList();
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
                //var allInvoices = _uow.Invoice.GetAllFromInvoiceView();
                foreach (var groupWaybillNumberMapping in groupWaybillNumberMappingList)
                {
                    //var shipmentDTO = allInvoices.FirstOrDefault(s => s.Waybill == groupWaybillNumberMapping.WaybillNumber);
                    var shipmentDTO = await _uow.Shipment.GetAsync(s => s.Waybill == groupWaybillNumberMapping.WaybillNumber);
                    if(shipmentDTO == null)
                    {
                        continue;
                    }

                    //who mapped the group
                    string userName = groupWaybillNumberMapping.UserId;
                    var mappedUser = await _uow.User.GetUserById(groupWaybillNumberMapping.UserId);
                    if(mappedUser != null)
                    {
                        userName = mappedUser.FirstName + " " + mappedUser.LastName;
                    }

                    resultList.Add(new WaybillNumberDTO { WaybillCode = shipmentDTO.Waybill });

                    //Get service centre detail for departure and destination
                    departureServiceCentre = await _centreService.GetServiceCentreById(shipmentDTO.DepartureServiceCentreId);
                    destinationServiceCentre = await _centreService.GetServiceCentreById(shipmentDTO.DestinationServiceCentreId);

                    shipmentList.Add(new ShipmentDTO
                    {
                        Waybill = shipmentDTO.Waybill,
                        DepartureServiceCentreId = shipmentDTO.DepartureServiceCentreId,
                        DepartureServiceCentre = departureServiceCentre,
                        //DepartureServiceCentre = new ServiceCentreDTO()
                        //{
                        //    Name = shipmentDTO.DepartureServiceCentreName,
                        //    Code = shipmentDTO.DepartureServiceCentreCode,
                        //    ServiceCentreId = shipmentDTO.DepartureServiceCentreId
                        //},

                        DestinationServiceCentreId = shipmentDTO.DestinationServiceCentreId,
                        DestinationServiceCentre = destinationServiceCentre,
                        //DestinationServiceCentre = new ServiceCentreDTO()
                        //{
                        //    Name = shipmentDTO.DestinationServiceCentreName,
                        //    Code = shipmentDTO.DestinationServiceCentreCode,
                        //    ServiceCentreId = shipmentDTO.DestinationServiceCentreId
                        //},

                        //use shipment DateCreated to represent Date Mapped
                        DateCreated = groupWaybillNumberMapping.DateMapped,

                        //use userId to represent who mapped the waybill to group
                        UserId = userName
                    });

                    waybills.Add(shipmentDTO.Waybill);

                    //departureServiceCentre = new ServiceCentreDTO()
                    //{
                    //    Name = shipmentDTO.DepartureServiceCentreName,
                    //    Code = shipmentDTO.DepartureServiceCentreCode,
                    //    ServiceCentreId = shipmentDTO.DepartureServiceCentreId
                    //};
                    //destinationServiceCentre = new ServiceCentreDTO()
                    //{
                    //    Name = shipmentDTO.DestinationServiceCentreName,
                    //    Code = shipmentDTO.DestinationServiceCentreCode,
                    //    ServiceCentreId = shipmentDTO.DestinationServiceCentreId
                    //};
                    
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
                //validate the ids are in the system
                var serviceCenterId = int.Parse(groupWaybillNumber.Substring(1, 3));
                var serviceCentre = await _centreService.GetServiceCentreById(serviceCenterId);
                
                // get the service centres of login user
                var serviceCenters = await _userService.GetPriviledgeServiceCenters();           
                if (serviceCenters.Length == 0)
                {
                    throw new GenericException("Error processing request. The login user is not assign to any service centre nor has the right privilege");
                }

                int departureServiceCenterId = serviceCenters[0];
                var currentUserId = await _userService.GetCurrentUserId();

                //Get GroupWaybill Details
                var groupwaybillObj = await _uow.GroupWaybillNumber.GetAsync(x => x.GroupWaybillCode.Equals(groupWaybillNumber));

                if(groupwaybillObj == null)
                {
                    var newGroupWaybill = new GroupWaybillNumber
                    {
                        GroupWaybillCode = groupWaybillNumber,
                        UserId = currentUserId,
                        ServiceCentreId = serviceCentre.ServiceCentreId,
                        IsActive = true,
                        DepartureServiceCentreId = departureServiceCenterId
                    };
                    _uow.GroupWaybillNumber.Add(newGroupWaybill);
                }
                else
                {
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
                }                

                List<GroupWaybillNumberMapping> groupWaybillNumberMapping = new List<GroupWaybillNumberMapping>();

                foreach (var waybillNumber in waybillNumberList)
                {
                    var shipmentDTO = await _uow.Shipment.GetAsync(x => x.Waybill == waybillNumber);

                    if (shipmentDTO == null)
                    {
                        throw new GenericException($"No Shipment exists for this : {waybillNumber}");
                    }

                    //if the groupwaybill service centre is not the same as the waybill destination
                    if(shipmentDTO.DestinationServiceCentreId != serviceCentre.ServiceCentreId)
                    {
                        throw new GenericException($"Waybill {waybillNumber} cannot be added to the Groupwaybill {groupWaybillNumber}");
                    }

                    //check if waybill has not been grouped 
                    var isWaybillGroup = await _uow.GroupWaybillNumberMapping.ExistAsync(x => x.GroupWaybillNumber == groupWaybillNumber && x.WaybillNumber == waybillNumber);

                    //if the waybill has not been grouped, group it
                    if (!isWaybillGroup)
                    {
                        //Add new Mapping
                        var newMapping = new GroupWaybillNumberMapping
                        {
                            GroupWaybillNumber = groupWaybillNumber,
                            WaybillNumber = waybillNumber,
                            IsActive = true,
                            DateMapped = DateTime.Now,
                            DepartureServiceCentreId = departureServiceCenterId,
                            DestinationServiceCentreId = serviceCenterId,
                            OriginalDepartureServiceCentreId = departureServiceCenterId,
                            UserId = currentUserId
                        };
                        
                        groupWaybillNumberMapping.Add(newMapping);

                        //Mark the waybill that it has been Grouped
                        shipmentDTO.IsGrouped = true;

                        //check if waybill is a transitWaybill then update entry
                        var transitWaybill = _uow.TransitWaybillNumber.SingleOrDefault(s => s.WaybillNumber == waybillNumber);
                        if (transitWaybill != null)
                        {
                            transitWaybill.IsGrouped = true;
                        }
                    }
                }

                _uow.GroupWaybillNumberMapping.AddRange(groupWaybillNumberMapping);
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
                
                var shipmentDTO = await _uow.Shipment.GetAsync(x => x.Waybill == waybillNumber);

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
                    throw new GenericException("GroupWaybillNumberMapping does not exist");
                }
                _uow.GroupWaybillNumberMapping.Remove(groupWaybillNumberMapping);

                //check if waybill is a transitWaybill then update entry
                var transitWaybill = _uow.TransitWaybillNumber.SingleOrDefault(s => s.WaybillNumber == shipmentDTO.Waybill);
                if (transitWaybill != null)
                {
                    transitWaybill.IsGrouped = false;
                }

                //check for OverdueShipment 
                var overdueShipment = _uow.OverdueShipment.SingleOrDefault(x => (x.Waybill == waybillNumber));
                if (overdueShipment != null)
                {
                    overdueShipment.OverdueShipmentStatus = OverdueShipmentStatus.UnGrouped;
                }

                //Mark the waybill that it has not been Grouped
                if(groupWaybillNumberDTO.DepartureServiceCentreId == shipmentDTO.DepartureServiceCentreId)
                {
                    shipmentDTO.IsGrouped = false;
                }

                await _uow.CompleteAsync();

                //Delete the GroupWaybill If All the Waybills attached to it have been deleted.
                var checkIfWaybillExistForGroup = await _uow.GroupWaybillNumberMapping.FindAsync(x => x.GroupWaybillNumber == groupWaybillNumber);
                if(checkIfWaybillExistForGroup.Count() == 0)
                {
                    await _groupWaybillNumberService.RemoveGroupWaybillNumber(groupWaybillNumberDTO.GroupWaybillNumberId);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //map waybillNumber to groupWaybillNumber for Overdue
        public async Task MappingWaybillNumberToGroupForOverdue(string groupWaybillNumber, List<string> waybillNumberList)
        {
            try
            {
                //1. check if the waybill are over due shipment
                await ValidateOverDueShipments(waybillNumberList);

                //2. call existing api that handle grouping
                await MappingWaybillNumberToGroup(groupWaybillNumber, waybillNumberList);

                //3. Save into OverdueShipment as grouped status
                //update shipment collection as GroupedForWarehouse and remove overshipment 
                var currentUserId = await _userService.GetCurrentUserId();
                foreach (var waybill in waybillNumberList)
                {
                    var overdueShipment = _uow.OverdueShipment.FindAsync(s => s.Waybill == waybill).Result.FirstOrDefault();
                    if (overdueShipment == null)
                    {
                        //save
                        var overdueShipmentNew = new OverdueShipment()
                        {
                            Waybill = waybill,
                            DateCreated = DateTime.Now,
                            OverdueShipmentStatus = OverdueShipmentStatus.Grouped,
                            UserId = currentUserId
                        };
                        _uow.OverdueShipment.Add(overdueShipmentNew);
                    }
                    else
                    {
                        //update status
                        overdueShipment.OverdueShipmentStatus = OverdueShipmentStatus.Grouped;
                    }
                }
                await _uow.CompleteAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task ValidateOverDueShipments(List<string> waybillNumberList)
        {
            try
            {
                //get all shipments by servicecentre
                var serviceCenters = await _userService.GetPriviledgeServiceCenters();                
                List<string> shipmentsWaybills = _uow.Shipment.GetAllAsQueryable().Where(s => s.IsCancelled == false && s.CompanyType != CompanyType.Ecommerce.ToString() && serviceCenters.Contains(s.DestinationServiceCentreId)).Select(x => x.Waybill).Distinct().ToList();

                // filter by global property for OverDueShipments
                var overDueDaysCountObj = _uow.GlobalProperty.SingleOrDefault(s => s.Key == GlobalPropertyType.OverDueDaysCount.ToString());
                if (overDueDaysCountObj == null)
                {
                    throw new GenericException($"The Global property 'Over Due Days Count' has not been set. Kindly contact admin.");
                }
                var overDueDaysCount = overDueDaysCountObj.Value;
                int globalProp = int.Parse(overDueDaysCount);
                var overdueDate = DateTime.Now.Subtract(TimeSpan.FromDays(globalProp));

                var shipmentCollection = _uow.ShipmentCollection.GetAllAsQueryable().
                    Where(x => x.ShipmentScanStatus == ShipmentScanStatus.ARF && (x.DateCreated <= overdueDate)).ToList();

                shipmentCollection = shipmentCollection.Where(s => shipmentsWaybills.Contains(s.Waybill)).OrderByDescending(x => x.DateCreated).ToList();

                //ensure that already grouped waybills don't appear with this list
                var overdueShipment = _uow.OverdueShipment.GetAllAsQueryable().
                    Where(s => s.OverdueShipmentStatus == OverdueShipmentStatus.Grouped).ToList();

                //filter the two lists
                shipmentCollection =
                    shipmentCollection.Where(s => !overdueShipment.Select(d => d.Waybill).Contains(s.Waybill)).ToList();

                var overdueShipmentList = shipmentCollection.Select(x => x.Waybill).Distinct().ToList();
                
                //validate if the waybills are all overdue shipment
                var result = waybillNumberList.Where(x => !overdueShipmentList.Contains(x));

                if (result.Count() > 0)
                {
                    throw new GenericException($"Error: Group cannot be created. " +
                        $"The following waybills [{string.Join(", ", result.ToList())}] are not overdue shipments");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        
    }
}
