using GIGLS.Core.IServices;
using GIGLS.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core.DTO.Shipments;
using GIGLS.CORE.DTO.Shipments;
using GIGLS.WebApi.Filters;
using GIGLS.CORE.DTO.Report;

namespace GIGLS.WebApi.Controllers.Shipments
{
    [Authorize(Roles = "Shipment")]
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

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("groupbydate")]
        public async Task<IServiceResponse<IEnumerable<GroupWaybillNumberMappingDTO>>> GetGroupWaybillNumberMappings(DateFilterCriteria dateFilterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var mappings = await _service.GetAllGroupWayBillNumberMappings(dateFilterCriteria);
                return new ServiceResponse<IEnumerable<GroupWaybillNumberMappingDTO>>
                {
                    Object = mappings
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("mapmultiple")]
        public async Task<IServiceResponse<bool>> MappingWaybillNumberToGroup(GroupWaybillNumberMappingDTO data)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.MappingWaybillNumberToGroup(data.GroupWaybillNumber, data.WaybillNumbers);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("mapmultipleForOverdue")]
        public async Task<IServiceResponse<bool>> MappingWaybillNumberToGroupForOverdue(GroupWaybillNumberMappingDTO data)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.MappingWaybillNumberToGroupForOverdue(data.GroupWaybillNumber, data.WaybillNumbers);
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

                //var groupWaybillNumberDTO = await _service.GetGroupForWaybillNumber(waybillNumber);
                var groupWaybillNumberDTO = await _service.GetGroupForWaybillNumberByServiceCentre(waybillNumber);

                return new ServiceResponse<GroupWaybillNumberDTO>
                {
                    Object = groupWaybillNumberDTO
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("waybillnumbersingroup/id/{groupWaybillNumberId}")]
        public async Task<IServiceResponse<GroupWaybillNumberMappingDTO>> GetWaybillNumbersInGroup(int groupWaybillNumberId)
        {
            return await HandleApiOperationAsync(async () =>
            {

                var waybillNumbersInGroup = await _service.GetWaybillNumbersInGroup(groupWaybillNumberId);

                return new ServiceResponse<GroupWaybillNumberMappingDTO>
                {
                    Object = waybillNumbersInGroup
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("waybillnumbersingroup/code/{groupWaybillNumber}")]
        public async Task<IServiceResponse<GroupWaybillNumberMappingDTO>> GetWaybillNumbersInGroup(string groupWaybillNumber)
        {
            return await HandleApiOperationAsync(async () =>
            {

                var waybillNumbersInGroup = await _service.GetWaybillNumbersInGroup(groupWaybillNumber);

                return new ServiceResponse<GroupWaybillNumberMappingDTO>
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
