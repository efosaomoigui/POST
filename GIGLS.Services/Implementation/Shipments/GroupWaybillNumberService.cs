using GIGLS.Core.IServices.Shipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GIGLS.Core;
using GIGLS.Infrastructure;
using AutoMapper;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IServices.Utility;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.ServiceCentres;
using GIGLS.Core.IServices.User;

namespace GIGLS.Services.Implementation.Shipments
{
    public class GroupWaybillNumberService : IGroupWaybillNumberService
    {
        private readonly IUnitOfWork _uow;
        private readonly INumberGeneratorMonitorService _service;
        private readonly IServiceCentreService _serviceCentreService;
        private readonly IUserService _userService;

        public GroupWaybillNumberService(IUnitOfWork uow,
            INumberGeneratorMonitorService service,
            IServiceCentreService serviceCentreService, IUserService userService)
        {
            _uow = uow;
            _service = service;
            _serviceCentreService = serviceCentreService;
            _userService = userService;
            MapperConfig.Initialize();
        }

        //This method add GroupWaybill Number to GroupWaybill Number Table
        public async Task<string> AddGroupWaybillNumber(GroupWaybillNumberDTO groupWaybillNumberDTO)
        {
            try
            {
                var groupwaybill = await _service.GenerateNextNumber(NumberGeneratorType.GroupWaybillNumber, groupWaybillNumberDTO.ServiceCentreCode);
                var serviceCentre = await _serviceCentreService.GetServiceCentreByCode(groupWaybillNumberDTO.ServiceCentreCode);

                var currentUserId = await _userService.GetCurrentUserId();

                var newGroupWaybill = new GroupWaybillNumber
                {
                    GroupWaybillCode = groupwaybill,
                    UserId = currentUserId,
                    ServiceCentreId = serviceCentre.ServiceCentreId,
                    IsActive = true
                };

                _uow.GroupWaybillNumber.Add(newGroupWaybill);
                await _uow.CompleteAsync();
                return groupwaybill;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //This is the method to call to get a Groupwaybill Number. It return Groupwaybill number to who call it
        public async Task<string> GenerateGroupWaybillNumber(GroupWaybillNumberDTO groupWaybillNumberDTO)
        {
            var groupwaybill = await AddGroupWaybillNumber(groupWaybillNumberDTO);
            return groupwaybill;
        }

        public async Task<IEnumerable<GroupWaybillNumberDTO>> GetAllGroupWayBillNumbers()
        {
            try
            {
                return await _uow.GroupWaybillNumber.GetGroupWaybills();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GroupWaybillNumberDTO>> GetActiveGroupWayBillNumbers()
        {
            try
            {
                var groupwaybill = await _uow.GroupWaybillNumber.FindAsync(x => x.IsActive == true);

                return groupwaybill.Select(w => new GroupWaybillNumberDTO
                {
                    IsActive = w.IsActive,
                    GroupWaybillCode = w.GroupWaybillCode,
                    GroupWaybillNumberId = w.GroupWaybillNumberId,
                    ServiceCentreId = w.ServiceCentreId,
                    UserId = w.UserId,
                    ServiceCentreCode = w.ServiceCentre?.Name
                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GroupWaybillNumberDTO>> GetDeliverGroupWayBillNumbers()
        {
            try
            {
                var groupwaybill = await _uow.GroupWaybillNumber.FindAsync(x => x.IsActive == false);

                return groupwaybill.Select(w => new GroupWaybillNumberDTO
                {
                    IsActive = w.IsActive,
                    GroupWaybillCode = w.GroupWaybillCode,
                    GroupWaybillNumberId = w.GroupWaybillNumberId,
                    ServiceCentreId = w.ServiceCentreId,
                    UserId = w.UserId,
                    ServiceCentreCode = w.ServiceCentre?.Name
                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GroupWaybillNumberDTO> GetGroupWayBillNumberById(string groupwaybillNumber)
        {
            try
            {
                var groupwaybill = await _uow.GroupWaybillNumber.GetAsync(x => x.GroupWaybillCode.Equals(groupwaybillNumber));

                if (groupwaybill == null)
                {
                    throw new GenericException("GroupWaybill information does not exist");
                }

                var groupwaybillDto = Mapper.Map<GroupWaybillNumberDTO>(groupwaybill);
                groupwaybillDto.ServiceCentreCode = groupwaybill.ServiceCentre?.Name;

                //set departure and destination service centres
                var groupWaybillNumberMappingList = await _uow.GroupWaybillNumberMapping.FindAsync(s => s.GroupWaybillNumber == groupwaybill.GroupWaybillCode);
                var groupWaybillNumberMapping = groupWaybillNumberMappingList.FirstOrDefault();

                if (groupWaybillNumberMapping != null)
                {
                    groupwaybillDto.DepartureServiceCentre = await _uow.ServiceCentre.GetAsync(s => s.ServiceCentreId == groupWaybillNumberMapping.DepartureServiceCentreId);
                    groupwaybillDto.DestinationServiceCentre = await _uow.ServiceCentre.GetAsync(s => s.ServiceCentreId == groupWaybillNumberMapping.DestinationServiceCentreId);
                }

                return groupwaybillDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GroupWaybillNumberDTO> GetGroupWayBillNumberById(int groupwaybillId)
        {
            try
            {
                var groupwaybill = await _uow.GroupWaybillNumber.GetAsync(x => x.GroupWaybillNumberId == groupwaybillId);

                if (groupwaybill == null)
                {
                    throw new GenericException("GroupWaybill information does not exist");
                }

                var groupwaybillDto = Mapper.Map<GroupWaybillNumberDTO>(groupwaybill);
                groupwaybillDto.ServiceCentreCode = groupwaybill.ServiceCentre?.Name;

                //set departure and destination service centres
                var groupWaybillNumberMappingList = await _uow.GroupWaybillNumberMapping.FindAsync(s => s.GroupWaybillNumber == groupwaybill.GroupWaybillCode);
                var groupWaybillNumberMapping = groupWaybillNumberMappingList.FirstOrDefault();

                groupwaybillDto.DepartureServiceCentre = groupWaybillNumberMapping.DepartureServiceCentre;
                groupwaybillDto.DestinationServiceCentre = groupWaybillNumberMapping.DestinationServiceCentre;

                return groupwaybillDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task RemoveGroupWaybillNumber(int groupwaybillId)
        {
            try
            {
                var groupwaybill = await _uow.GroupWaybillNumber.GetAsync(groupwaybillId);

                if (groupwaybill == null)
                {
                    throw new GenericException("GroupWaybill Not Exist");
                }
                _uow.GroupWaybillNumber.Remove(groupwaybill);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task RemoveGroupWaybillNumber(string groupwaybillId)
        {
            try
            {
                var groupwaybill = _uow.GroupWaybillNumber.SingleOrDefault(x => x.GroupWaybillCode == groupwaybillId);

                if (groupwaybill == null)
                {
                    throw new GenericException("GroupWaybill Not Exist");
                }
                _uow.GroupWaybillNumber.Remove(groupwaybill);
                await _uow.CompleteAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateGroupWaybillNumber(string groupwaybillNumber)
        {
            try
            {
                var groupwaybill = await _uow.GroupWaybillNumber.GetAsync(x => x.GroupWaybillCode == groupwaybillNumber);

                if (groupwaybill == null)
                {
                    throw new GenericException("GroupWaybill Not Exist");
                }
                groupwaybill.IsActive = false;
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateGroupWaybillNumber(int groupwaybillId)
        {
            try
            {
                var groupwaybill = await _uow.GroupWaybillNumber.GetAsync(groupwaybillId);

                if (groupwaybill == null)
                {
                    throw new GenericException("GroupWaybill Not Exist");
                }
                groupwaybill.IsActive = false;
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //This is use because I dont want an Exception to be throw when calling it
        public async Task<GroupWaybillNumber> GetGroupWayBillNumberForScan(string groupwaybillNumber)
        {
            var groupwaybill = await _uow.GroupWaybillNumber.GetAsync(x => x.GroupWaybillCode.Equals(groupwaybillNumber));
            return groupwaybill;
        }
    }
}
