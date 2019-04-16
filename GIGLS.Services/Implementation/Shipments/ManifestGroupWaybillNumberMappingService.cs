﻿using System;
using System.Threading.Tasks;
using GIGLS.Core;
using System.Collections.Generic;
using GIGLS.Infrastructure;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.Domain;
using GIGLS.Core.IServices.User;
using System.Linq;
using GIGLS.CORE.DTO.Report;

namespace GIGLS.Services.Implementation.Shipments
{
    public class ManifestGroupWaybillNumberMappingService : IManifestGroupWaybillNumberMappingService
    {
        private readonly IUnitOfWork _uow;
        private readonly IManifestService _manifestService;
        private readonly IGroupWaybillNumberService _groupWaybillNumberService;
        private readonly IUserService _userService;

        public ManifestGroupWaybillNumberMappingService(IUnitOfWork uow,
            IManifestService manifestService, IGroupWaybillNumberService groupWaybillNumberService,
            IUserService userService)
        {
            _uow = uow;
            _manifestService = manifestService;
            _groupWaybillNumberService = groupWaybillNumberService;
            _userService = userService;
            MapperConfig.Initialize();
        }

        //Get Manifest For GroupWaybillNumber
        public async Task<ManifestDTO> GetManifestForGroupWaybillNumber(int groupWaybillNumberId)
        {
            try
            {
                var groupWaybillNumberDTO = await _groupWaybillNumberService.GetGroupWayBillNumberById(groupWaybillNumberId);
                var manifestGroupWaybillNumberMapping = await _uow.ManifestGroupWaybillNumberMapping.GetAsync(x => x.GroupWaybillNumber == groupWaybillNumberDTO.GroupWaybillCode);

                if (manifestGroupWaybillNumberMapping == null)
                {
                    throw new GenericException($"No Manifest exists for this GroupWaybill Id: {groupWaybillNumberId}");
                }

                var manifestDTO = await _manifestService.GetManifestByCode(manifestGroupWaybillNumberMapping.ManifestCode);
                return manifestDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Get Manifest For GroupWaybillNumber
        public async Task<ManifestDTO> GetManifestForGroupWaybillNumber(string groupWaybillNumber)
        {
            try
            {
                var groupWaybillNumberDTO = await _groupWaybillNumberService.GetGroupWayBillNumberById(groupWaybillNumber);
                var manifestGroupWaybillNumberMapping = await _uow.ManifestGroupWaybillNumberMapping.GetAsync(x => x.GroupWaybillNumber == groupWaybillNumberDTO.GroupWaybillCode);

                if (manifestGroupWaybillNumberMapping == null)
                {
                    throw new GenericException($"No Manifest exists for this GroupWaybill : {groupWaybillNumber}");
                }

                var manifestDTO = await _manifestService.GetManifestByCode(manifestGroupWaybillNumberMapping.ManifestCode);
                return manifestDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }


        //Get GroupWaybillNumbers In Manifest
        public async Task<List<GroupWaybillNumberDTO>> GetGroupWaybillNumbersInManifest(int manifestId)
        {
            try
            {
                var manifestDTO = await _manifestService.GetManifestById(manifestId);
                var manifestGroupWaybillNumberMappingList = await _uow.ManifestGroupWaybillNumberMapping.FindAsync(x => x.ManifestCode == manifestDTO.ManifestCode);

                //add to list
                var resultSet = new HashSet<string>();
                List<GroupWaybillNumberDTO> resultList = new List<GroupWaybillNumberDTO>();
                foreach (var manifestGroupWaybillNumberMapping in manifestGroupWaybillNumberMappingList)
                {
                    var groupWaybillNumberDTO = await _groupWaybillNumberService.GetGroupWayBillNumberById(manifestGroupWaybillNumberMapping.GroupWaybillNumber);
                    if (resultSet.Add(groupWaybillNumberDTO.GroupWaybillCode))
                    {
                        resultList.Add(groupWaybillNumberDTO);
                    }
                }

                return resultList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Get WaybillNumbers In Group
        public async Task<List<GroupWaybillNumberDTO>> GetGroupWaybillNumbersInManifest(string manifest)
        {
            try
            {
                var manifestDTO = await _manifestService.GetManifestByCode(manifest);
                var manifestGroupWaybillNumberMappingList = await _uow.ManifestGroupWaybillNumberMapping.FindAsync(x => x.ManifestCode == manifestDTO.ManifestCode);

                //add to list
                var resultSet = new HashSet<string>();
                List<GroupWaybillNumberDTO> resultList = new List<GroupWaybillNumberDTO>();
                foreach (var manifestGroupWaybillNumberMapping in manifestGroupWaybillNumberMappingList)
                {
                    var groupWaybillNumberDTO = await _groupWaybillNumberService.GetGroupWayBillNumberById(manifestGroupWaybillNumberMapping.GroupWaybillNumber);
                    if (resultSet.Add(groupWaybillNumberDTO.GroupWaybillCode))
                    {
                        resultList.Add(groupWaybillNumberDTO);
                    }
                }

                return resultList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //map groupWaybillNumber to Manifest
        public async Task MappingManifestToGroupWaybillNumber(string manifest, List<string> groupWaybillNumberList)
        {
            try
            {
                var userId = await _userService.GetCurrentUserId();

                //var manifestDTO = await _manifestService.GetManifestByCode(manifest);
                var manifestObj = await _uow.Manifest.GetAsync(x => x.ManifestCode.Equals(manifest));

                //validate the ids are in the system
                if (manifestObj == null)
                {
                    await _manifestService.AddManifest(
                        new ManifestDTO
                        {
                            ManifestCode = manifest,
                            DateTime = DateTime.Now,
                            //DispatchedBy = userId
                        });
                    //throw new GenericException($"No Manifest exists for this code: {manifest}");
                }

                foreach (var groupWaybillNumber in groupWaybillNumberList)
                {
                    var groupWaybillNumberDTO = await _groupWaybillNumberService.GetGroupWayBillNumberById(groupWaybillNumber);

                    if (groupWaybillNumberDTO == null)
                    {
                        throw new GenericException($"No GroupWaybill exists for this number: {groupWaybillNumber}");
                    }

                    //check if GroupWaybill has not been added to manifest 
                    var isGroupWaybillMapped = await _uow.ManifestGroupWaybillNumberMapping.ExistAsync(x => x.ManifestCode == manifest && x.GroupWaybillNumber == groupWaybillNumber);

                    //if the waybill has not been added to manifest, add it
                    if (!isGroupWaybillMapped)
                    {
                        //Add new Mapping
                        var newMapping = new ManifestGroupWaybillNumberMapping
                        {
                            ManifestCode = manifest,
                            GroupWaybillNumber = groupWaybillNumberDTO.GroupWaybillCode,
                            IsActive = true,
                            DateMapped = DateTime.Now
                        };

                        _uow.ManifestGroupWaybillNumberMapping.Add(newMapping);

                        //Update The Group Waybill HasManifest to True
                        var groupWaybill = await _uow.GroupWaybillNumber.GetAsync(groupWaybillNumberDTO.GroupWaybillNumberId);
                        groupWaybill.HasManifest = true;
                    }
                }

                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //remove groupWaybillNumber from manifest
        public async Task RemoveGroupWaybillNumberFromManifest(string manifest, string groupWaybillNumber)
        {
            try
            {
                var manifestDTO = await _manifestService.GetManifestByCode(manifest);
                var groupWaybillNumberDTO = await _groupWaybillNumberService.GetGroupWayBillNumberById(groupWaybillNumber);

                //validate the ids are in the system
                if (manifestDTO == null)
                {
                    throw new GenericException($"No Manifest exists for this code: {manifest}");
                }
                if (groupWaybillNumberDTO == null)
                {
                    throw new GenericException($"No GroupWaybill exists for this number: {groupWaybillNumber}");
                }

                var manifestGroupWaybillNumberMapping = _uow.ManifestGroupWaybillNumberMapping.SingleOrDefault(x => (x.ManifestCode == manifest) && (x.GroupWaybillNumber == groupWaybillNumber));
                if (manifestGroupWaybillNumberMapping == null)
                {
                    throw new GenericException("ManifestGroupWaybillNumberMapping Does Not Exist");
                }
                _uow.ManifestGroupWaybillNumberMapping.Remove(manifestGroupWaybillNumberMapping);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ManifestGroupWaybillNumberMappingDTO>> GetAllManifestGroupWayBillNumberMappings()
        {
            try
            {
                var serviceCenters = _userService.GetPriviledgeServiceCenters().Result;
                var manifestGroupWaybillMapings = await _uow.ManifestGroupWaybillNumberMapping.GetManifestGroupWaybillNumberMappings(serviceCenters);
                return manifestGroupWaybillMapings;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ManifestGroupWaybillNumberMappingDTO>> GetAllManifestGroupWayBillNumberMappings(DateFilterCriteria dateFilterCriteria)
        {
            try
            {
                var serviceCenters = _userService.GetPriviledgeServiceCenters().Result;
                var manifestGroupWaybillMapings = await _uow.ManifestGroupWaybillNumberMapping.GetManifestGroupWaybillNumberMappings(serviceCenters, dateFilterCriteria);
                return manifestGroupWaybillMapings;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Get Manifest For GroupWaybillNumber
        public async Task<ManifestGroupWaybillNumberMappingDTO> GetManifestForWaybill(string waybill)
        {
            //1. Get waybill in a Group Waybill
            var groupWaybillNumberMapping = await _uow.GroupWaybillNumberMapping.FindAsync(x => x.WaybillNumber == waybill);
            if (groupWaybillNumberMapping == null)
            {
                throw new GenericException($"No Manifest exists for this Waybill: {waybill}");
            }
            
            //check if the user is at the service centre
            var serviceCentreIds = await _userService.GetPriviledgeServiceCenters();
            string groupwaybill = null;

            if(serviceCentreIds.Length > 0)
            {
                foreach(var s in groupWaybillNumberMapping.ToList())
                {
                    if (serviceCentreIds.Contains(s.DepartureServiceCentreId))
                    {
                        groupwaybill = s.GroupWaybillNumber.ToString();
                    }
                }
            }
            
            //2. Use the Groupwaybill to get manifest
            var manifestGroupWaybillMapings = await _uow.ManifestGroupWaybillNumberMapping.GetManifestGroupWaybillNumberMappingsUsingGroupWaybill(groupwaybill);

            if (manifestGroupWaybillMapings == null)
            {
                throw new GenericException($"No Manifest exists for this Waybill in your service centre: {waybill}");
            }

            return manifestGroupWaybillMapings;
        }
    }
}
