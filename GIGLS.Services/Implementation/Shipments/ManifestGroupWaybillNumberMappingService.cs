using AutoMapper;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Fleets;
using GIGLS.Core.DTO.Shipments;
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
    public class ManifestGroupWaybillNumberMappingService : IManifestGroupWaybillNumberMappingService
    {
        private readonly IUnitOfWork _uow;
        private readonly IManifestService _manifestService;
        private readonly IGroupWaybillNumberService _groupWaybillNumberService;
        private readonly IUserService _userService;
        private readonly IManifestWaybillMappingService _manifestWaybillMappingService;

        public ManifestGroupWaybillNumberMappingService(IUnitOfWork uow,
            IManifestService manifestService, IGroupWaybillNumberService groupWaybillNumberService,
            IUserService userService, IManifestWaybillMappingService manifestWaybillMappingService)
        {
            _uow = uow;
            _manifestService = manifestService;
            _groupWaybillNumberService = groupWaybillNumberService;
            _userService = userService;
            _manifestWaybillMappingService = manifestWaybillMappingService;
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
            catch (Exception ex)
            {
                throw;
            }
        }

        //Get  30 Waybills in a list of objects containing manifests for Riders Delivery Progress Page
        public async Task<List<ManifestWaybillMappingDTO>> GetWaybillsInListOfManifest(string captainId)
        {
            try
            {
                var baselineDate = DateTime.Today.AddDays(-30);

                
                //1. get all dispatch for that captain -- update this to get the data from the database instead of app memory
                var dispatchResult = _uow.Dispatch.GetAll().Where(x => x.DriverDetail.Equals(captainId) && x.DateCreated >= baselineDate)
                                                            .ToList();
                                                                    
                var dispatchResultDTO = Mapper.Map<List<DispatchDTO>>(dispatchResult);

                //2. get the manifest waybill mapping
                var finalResult = await GetWaybillsInManifestForDispatch(dispatchResultDTO);
                return finalResult;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Get waybills according to date range selected for Riders Delivery Progress Page
        public async Task<List<ManifestWaybillMappingDTO>> GetAllWaybillsinListOfManifest(string captainId, DateFilterCriteria dateFilterCriteria)
        {
            try
            {                                
                var dispatchresult = await _uow.ManifestWaybillMapping.GetWaybillsinManifestMappings(captainId, dateFilterCriteria);

                //2. get the manifest waybill mapping
                var finalResult = await GetWaybillsInManifestForDispatch(dispatchresult);
                return finalResult;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<List<ManifestWaybillMappingDTO>> GetWaybillsInManifestForDispatch(IEnumerable<DispatchDTO> dispatchList)
        {
            try
            {
                List<ManifestWaybillMappingDTO> finalResult = new List<ManifestWaybillMappingDTO>();
                
                foreach (var dispatch in dispatchList)
                {
                    //for only delivery manifest
                    var manifest = await _uow.Manifest.GetAsync(s => s.ManifestCode == dispatch.ManifestNumber && s.ManifestType == Core.Enums.ManifestType.Delivery);

                    if (manifest != null)
                    {
                        var mwp = await _manifestWaybillMappingService.GetWaybillsInManifest(dispatch.ManifestNumber);
                        finalResult.AddRange(mwp);
                    }

                    //Assuming the want both
                    //var manifest = await _uow.Manifest.GetAsync(s => s.ManifestCode == dispatch.ManifestNumber);

                    //if (manifest.ManifestType == Core.Enums.ManifestType.Delivery)
                    //{
                    //    var deliveryManifest = await _manifestWaybillMappingService.GetWaybillsInManifest(dispatch.ManifestNumber);
                    //    finalResult.AddRange(deliveryManifest);
                    //}
                    //else
                    //{
                    //    //get all waybills in other manifest 
                    //}
                }
                return finalResult;
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
                    var newManifest = new Manifest
                    {
                        DateTime = DateTime.Now,
                        ManifestCode = manifest
                    };
                    _uow.Manifest.Add(newManifest);
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
                await _uow.CompleteAsync();
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
                //if (manifestDTO == null)
                //{
                //    throw new GenericException($"No Manifest exists for this code: {manifest}");
                //}
                //if (groupWaybillNumberDTO == null)
                //{
                //    throw new GenericException($"No GroupWaybill exists for this number: {groupWaybillNumber}");
                //}

                var manifestGroupWaybillNumberMapping = _uow.ManifestGroupWaybillNumberMapping.SingleOrDefault(x => x.ManifestCode == manifest && x.GroupWaybillNumber == groupWaybillNumber);
                if (manifestGroupWaybillNumberMapping != null)
                {
                    _uow.ManifestGroupWaybillNumberMapping.Remove(manifestGroupWaybillNumberMapping);

                    //set GroupWaybill HasManifest to false
                    var groupwaybill = _uow.GroupWaybillNumber.SingleOrDefault(x => x.GroupWaybillCode == groupWaybillNumber);
                    groupwaybill.HasManifest = false;
                    _uow.Complete();
                }
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

                //group the result by manifest                
                var resultGroup = manifestGroupWaybillMapings.GroupBy(x => x.ManifestCode).ToList();
                var result = new List<ManifestGroupWaybillNumberMappingDTO>();

                foreach (var resultGrp in resultGroup)
                {
                    result.Add(resultGrp.FirstOrDefault());
                }

                return result;
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

            if (serviceCentreIds.Length > 0)
            {
                foreach (var s in groupWaybillNumberMapping.ToList())
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

        //Search For Manifest
        public async Task<ManifestDTO> GetManifestSearch(string manifestCode)
        {
            try
            {
                var manifestDTO = await _manifestService.GetManifestByCode(manifestCode);

                var DispatchName = _uow.User.GetUserById(manifestDTO.DispatchedBy).Result;
                if (DispatchName != null)
                {
                    manifestDTO.DispatchedBy = DispatchName.FirstName + " " + DispatchName.LastName;
                }

                var RecieverName = _uow.User.GetUserById(manifestDTO.ReceiverBy).Result;
                if (RecieverName != null)
                {
                    manifestDTO.ReceiverBy = RecieverName.FirstName + " " + RecieverName.LastName;
                }

                return manifestDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
