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

namespace GIGLS.Services.Implementation.Shipments
{
    public class GroupWaybillNumberMappingService : IGroupWaybillNumberMappingService
    {
        private readonly IUnitOfWork _uow;
        private readonly IGroupWaybillNumberService _groupWaybillNumberService;
        private readonly IShipmentService _shipmentService;
        private readonly IUserService _userService;

        public GroupWaybillNumberMappingService(IUnitOfWork uow,
            IGroupWaybillNumberService groupWaybillNumberService,
            IShipmentService shipmentService,
            IUserService userService)
        {
            _uow = uow;
            _groupWaybillNumberService = groupWaybillNumberService;
            _shipmentService = shipmentService;
            _userService = userService;
            MapperConfig.Initialize();
        }


        public async Task<IEnumerable<GroupWaybillNumberMappingDTO>> GetAllGroupWayBillNumberMappings()
        {
            try
            {
                var serviceCenters = await _userService.GetPriviledgeServiceCenters();
                return await _uow.GroupWaybillNumberMapping.GetGroupWaybillMappings(serviceCenters);
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
        public async Task<List<WaybillNumberDTO>> GetWaybillNumbersInGroup(int groupWaybillNumberId)
        {
            try
            {
                var groupWaybillNumberDTO = await _groupWaybillNumberService.GetGroupWayBillNumberById(groupWaybillNumberId);
                var groupWaybillNumberMappingList = await _uow.GroupWaybillNumberMapping.FindAsync(x => x.GroupWaybillNumber == groupWaybillNumberDTO.GroupWaybillCode);

                //add to list
                List<WaybillNumberDTO> resultList = new List<WaybillNumberDTO>();
                foreach (var groupWaybillNumberMapping in groupWaybillNumberMappingList)
                {
                    var shipmentDTO = await _shipmentService.GetShipment(groupWaybillNumberMapping.WaybillNumber);
                    resultList.Add(new WaybillNumberDTO { WaybillCode = shipmentDTO.Waybill });
                }

                return resultList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Get WaybillNumbers In Group
        public async Task<List<WaybillNumberDTO>> GetWaybillNumbersInGroup(string groupWaybillNumber)
        {
            try
            {
                var groupWaybillNumberDTO = await _groupWaybillNumberService.GetGroupWayBillNumberById(groupWaybillNumber);
                var groupWaybillNumberMappingList = await _uow.GroupWaybillNumberMapping.FindAsync(x => x.GroupWaybillNumber == groupWaybillNumberDTO.GroupWaybillCode);

                //add to list
                List<WaybillNumberDTO> resultList = new List<WaybillNumberDTO>();
                foreach (var groupWaybillNumberMapping in groupWaybillNumberMappingList)
                {
                    var shipmentDTO = await _shipmentService.GetShipment(groupWaybillNumberMapping.WaybillNumber);
                    resultList.Add(new WaybillNumberDTO { WaybillCode = shipmentDTO.Waybill });
                }

                return resultList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //map waybillNumber to groupWaybillNumber
        //public async Task MappingWaybillNumberToGroup(int groupWaybillNumberId, int waybillNumberId)
        //{
        //    try
        //    {
        //        var groupWaybillNumberDTO = await _groupWaybillNumberService.GetGroupWayBillNumberById(groupWaybillNumberId);
        //        var waybillNumberDTO = await _shipmentService.GetWayBillNumberById(waybillNumberId);

        //        //validate the ids are in the system
        //        if (groupWaybillNumberDTO == null)
        //        {
        //            throw new GenericException($"No GroupWaybill exists for this Id: {groupWaybillNumberId}");
        //        }
        //        if (waybillNumberDTO == null)
        //        {
        //            throw new GenericException($"No Waybill exists for this Id: {waybillNumberId}");
        //        }

        //        //Add new Mapping
        //        var newMapping = new GroupWaybillNumberMapping
        //        {
        //            GroupWaybillNumber = groupWaybillNumberDTO.GroupWaybillCode,
        //            WaybillNumber = waybillNumberDTO.WaybillCode,
        //            IsActive = true,
        //            DateMapped = DateTime.Now
        //        };

        //        _uow.GroupWaybillNumberMapping.Add(newMapping);
        //        _uow.Complete();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //map waybillNumber to groupWaybillNumber
        public async Task MappingWaybillNumberToGroup(string groupWaybillNumber, string waybillNumber)
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

                // get the service centres
                var serviceCenters = await _userService.GetPriviledgeServiceCenters();
                var departureServiceCenterId = serviceCenters[0];

                //Add new Mapping
                var newMapping = new GroupWaybillNumberMapping
                {
                    GroupWaybillNumber = groupWaybillNumberDTO.GroupWaybillCode,
                    WaybillNumber = shipmentDTO.Waybill,
                    IsActive = true,
                    DateMapped = DateTime.Now,
                    DepartureServiceCentreId = departureServiceCenterId,
                    DestinationServiceCentreId = groupWaybillNumberDTO.ServiceCentreId
                };

                _uow.GroupWaybillNumberMapping.Add(newMapping);
                _uow.Complete();
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
                var groupWaybillNumberDTO = await _groupWaybillNumberService.GetGroupWayBillNumberById(groupWaybillNumber);

                //validate the ids are in the system
                if (groupWaybillNumberDTO == null)
                {
                    throw new GenericException($"No GroupWaybill exists for this : {groupWaybillNumber}");
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

                    //Add new Mapping
                    var newMapping = new GroupWaybillNumberMapping
                    {
                        GroupWaybillNumber = groupWaybillNumberDTO.GroupWaybillCode,
                        WaybillNumber = shipmentDTO.Waybill,
                        IsActive = true,
                        DateMapped = DateTime.Now,
                        DepartureServiceCentreId = departureServiceCenterId,
                        DestinationServiceCentreId = groupWaybillNumberDTO.ServiceCentreId
                    };
                    _uow.GroupWaybillNumberMapping.Add(newMapping);
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

                var groupWaybillNumberMapping = _uow.GroupWaybillNumberMapping.SingleOrDefault(x => (x.GroupWaybillNumber == groupWaybillNumber) && (x.WaybillNumber == waybillNumber));
                if (groupWaybillNumberMapping == null)
                {
                    throw new GenericException("GroupWaybillNumberMapping Does Not Exist");
                }
                _uow.GroupWaybillNumberMapping.Remove(groupWaybillNumberMapping);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
