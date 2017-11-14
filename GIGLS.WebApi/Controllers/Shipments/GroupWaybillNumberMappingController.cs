﻿using GIGLS.Core.IServices;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.IServices.ServiceCentres;
using GIGLS.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.Core.DTO.User;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.Shipments;
using GIGLS.CORE.DTO.Shipments;
using GIGLS.WebApi.Filters;

namespace GIGLS.WebApi.Controllers.Shipments
{
    //[Authorize]
    [RoutePrefix("api/groupwaybillnumbermapping")]
    public class GroupWaybillNumberMappingController : BaseWebApiController
    {
        private readonly IGroupWaybillNumberMappingService _service;
        public GroupWaybillNumberMappingController(IGroupWaybillNumberMappingService service) : base(nameof(GroupWaybillNumberMappingController))
        {
            _service = service;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<GroupWaybillNumberMappingDTO>>> GetGroupWaybillNumberMappings()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var mappings = await _service.GetAllGroupWayBillNumberMappings();
                return new ServiceResponse<IEnumerable<GroupWaybillNumberMappingDTO>>
                {
                    Object = mappings
                };
            });
        }

        //[HttpPost]
        //[Route("id")]
        //public async Task<IServiceResponse<bool>> MappingWaybillNumberToGroup(int groupWaybillNumberId, int waybillNumberId)
        //{
        //    return await HandleApiOperationAsync(async () =>
        //    {
        //        await _service.MappingWaybillNumberToGroup(groupWaybillNumberId, waybillNumberId);
        //        return new ServiceResponse<bool>
        //        {
        //            Object = true
        //        };
        //    });
        //}

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("code")]
        public async Task<IServiceResponse<bool>> MappingWaybillNumberToGroup(string groupWaybillNumber, string waybillNumber)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.MappingWaybillNumberToGroup(groupWaybillNumber, waybillNumber);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("mapmultiple")]
        public async Task<IServiceResponse<bool>> MappingWaybillNumberToGroup(string groupWaybillNumber, List<string> waybillNumbers)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.MappingWaybillNumberToGroup(groupWaybillNumber, waybillNumbers);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }


        //[HttpGet]
        //[Route("groupforwaybillnumber/id/{waybillNumberId}")]
        //public async Task<IServiceResponse<GroupWaybillNumberDTO>> GetGroupForWaybillNumber(int waybillNumberId)
        //{
        //    return await HandleApiOperationAsync(async () =>
        //    {

        //        var groupWaybillNumberDTO = await _service.GetGroupForWaybillNumber(waybillNumberId);

        //        return new ServiceResponse<GroupWaybillNumberDTO>
        //        {
        //            Object = groupWaybillNumberDTO
        //        };
        //    });
        //}

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("groupforwaybillnumber/code/{waybillNumber}")]
        public async Task<IServiceResponse<GroupWaybillNumberDTO>> GetGroupForWaybillNumber(string waybillNumber)
        {
            return await HandleApiOperationAsync(async () =>
            {

                var groupWaybillNumberDTO = await _service.GetGroupForWaybillNumber(waybillNumber);

                return new ServiceResponse<GroupWaybillNumberDTO>
                {
                    Object = groupWaybillNumberDTO
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("waybillnumbersingroup/id/{groupWaybillNumberId}")]
        public async Task<IServiceResponse<List<WaybillNumberDTO>>> GetWaybillNumbersInGroup(int groupWaybillNumberId)
        {
            return await HandleApiOperationAsync(async () =>
            {

                var waybillNumbersInGroup = await _service.GetWaybillNumbersInGroup(groupWaybillNumberId);

                return new ServiceResponse<List<WaybillNumberDTO>>
                {
                    Object = waybillNumbersInGroup
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("waybillnumbersingroup/code/{groupWaybillNumber}")]
        public async Task<IServiceResponse<List<WaybillNumberDTO>>> GetWaybillNumbersInGroup(string groupWaybillNumber)
        {
            return await HandleApiOperationAsync(async () =>
            {

                var waybillNumbersInGroup = await _service.GetWaybillNumbersInGroup(groupWaybillNumber);

                return new ServiceResponse<List<WaybillNumberDTO>>
                {
                    Object = waybillNumbersInGroup
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("RemoveWaybillNumberFromGroup/{groupWaybillNumber}/{waybillNumber}")]
        public async Task<IServiceResponse<bool>> RemoveWaybillNumberFromGroup(string groupWaybillNumber, string waybillNumber)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.RemoveWaybillNumberFromGroup(groupWaybillNumber, waybillNumber);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }
    }
}
