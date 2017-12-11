using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.WebApi.Filters;

namespace GIGLS.WebApi.Controllers.Shipments
{
    [Authorize(Roles = "SuperAdmin,SubAdmin,Shipment,Account,Report")]
    [RoutePrefix("api/groupwaybill")]
    public class GroupWaybillNumberController : BaseWebApiController
    {
        private readonly IGroupWaybillNumberService _service;

        public GroupWaybillNumberController(IGroupWaybillNumberService service) : base(nameof(GroupWaybillNumberController))
        {
            _service = service;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<GroupWaybillNumberDTO>>> GetAllGroupWaybills()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var groupwaybills = await _service.GetAllGroupWayBillNumbers();

                return new ServiceResponse<IEnumerable<GroupWaybillNumberDTO>>
                {
                    Object = groupwaybills
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("active")]
        public async Task<IServiceResponse<List<GroupWaybillNumberDTO>>> GetActiveGroupWaybills()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var groupwaybills = await _service.GetActiveGroupWayBillNumbers();

                return new ServiceResponse<List<GroupWaybillNumberDTO>>
                {
                    Object = groupwaybills
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("delivery")]
        public async Task<IServiceResponse<List<GroupWaybillNumberDTO>>> GetDeliveryGroupWaybills()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var groupwaybills = await _service.GetDeliverGroupWayBillNumbers();

                return new ServiceResponse<List<GroupWaybillNumberDTO>>
                {
                    Object = groupwaybills
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{groupwaybillId:int}")]
        public async Task<IServiceResponse<GroupWaybillNumberDTO>> GetGroupWaybillById(int groupwaybillId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var groupwaybills = await _service.GetGroupWayBillNumberById(groupwaybillId);

                return new ServiceResponse<GroupWaybillNumberDTO>
                {
                    Object = groupwaybills
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{groupwaybill}")]
        public async Task<IServiceResponse<GroupWaybillNumberDTO>> GetGroupWaybillByCode(string groupwaybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var groupwaybills = await _service.GetGroupWayBillNumberById(groupwaybill);

                return new ServiceResponse<GroupWaybillNumberDTO>
                {
                    Object = groupwaybills
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{groupwaybillId:int}")]
        public async Task<IServiceResponse<bool>> DeleteGroupWaybillById(int groupwaybillId)
        {
            return await HandleApiOperationAsync(async () =>
            {

                await _service.RemoveGroupWaybillNumber(groupwaybillId);
                return new ServiceResponse<bool>
                {
                    Object = true
                };

            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{groupwaybill}")]
        public async Task<IServiceResponse<bool>> DeleteGroupWaybillByCode(string groupwaybill)
        {
            return await HandleApiOperationAsync(async () =>
            {

                await _service.RemoveGroupWaybillNumber(groupwaybill);
                return new ServiceResponse<bool>
                {
                    Object = true
                };

            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{groupwaybillId:int}")]
        public async Task<IServiceResponse<bool>> UpdateGroupWaybillById(int groupwaybillId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.UpdateGroupWaybillNumber(groupwaybillId);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{groupwaybill}")]
        public async Task<IServiceResponse<bool>> UpdateGroupWaybillByCode(string groupwaybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.UpdateGroupWaybillNumber(groupwaybill);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("generategroupwaybillnumber")]
        public async Task<IServiceResponse<string>> GenerateGroupWaybillNumber(GroupWaybillNumberDTO groupWaybillNumberDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var groupwaybills = await _service.GenerateGroupWaybillNumber(groupWaybillNumberDTO);

                return new ServiceResponse<string>
                {
                    Object = groupwaybills
                };
            });
        }
    }
}
