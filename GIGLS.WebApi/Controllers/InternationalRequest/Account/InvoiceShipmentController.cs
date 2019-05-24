//using GIGLS.Core.IServices;
//using GIGLS.CORE.DTO.Account;
//using GIGLS.CORE.IServices.Account;
//using GIGLS.Services.Implementation;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using System.Web.Http;

//namespace GIGLS.WebApi.Controllers.Account
//{
//    //[Authorize]
//    [RoutePrefix("api/invoiceShipment")]
//    public class InvoiceShipmentController : BaseWebApiController
//    {
//        private readonly IInvoiceShipmentService _invoiceShipmentService;
//        public InvoiceShipmentController(IInvoiceShipmentService invoiceShipmentService) : base(nameof(InvoiceShipmentController))
//        {
//            _invoiceShipmentService = invoiceShipmentService;
//        }

//        [HttpGet]
//        [Route("")]
//        public async Task<IServiceResponse<IEnumerable<InvoiceShipmentDTO>>> GetInvoiceShipments()
//        {
//            return await HandleApiOperationAsync(async () =>
//            {

//                var invoiceShipment = await _invoiceShipmentService.GetInvoiceShipments();

//                return new ServiceResponse<IEnumerable<InvoiceShipmentDTO>>
//                {
//                    Object = invoiceShipment
//                };
//            });
//        }

//        [HttpPost]
//        [Route("")]
//        public async Task<IServiceResponse<object>> AddInvoiceShipment(InvoiceShipmentDTO invoiceShipmentDto)
//        {
//            return await HandleApiOperationAsync(async () =>
//            {
//                var invoiceShipment = await _invoiceShipmentService.AddInvoiceShipment(invoiceShipmentDto);

//                return new ServiceResponse<object>
//                {
//                    Object = invoiceShipment
//                };
//            });
//        }

//        [HttpGet]
//        [Route("{invoiceShipmentId:int}")]
//        public async Task<IServiceResponse<InvoiceShipmentDTO>> GetInvoiceShipment(int invoiceShipmentId)
//        {
//            return await HandleApiOperationAsync(async () =>
//            {
//                var invoiceShipment = await _invoiceShipmentService.GetInvoiceShipmentById(invoiceShipmentId);

//                return new ServiceResponse<InvoiceShipmentDTO>
//                {
//                    Object = invoiceShipment
//                };
//            });
//        }

//        [HttpDelete]
//        [Route("{invoiceShipmentId:int}")]
//        public async Task<IServiceResponse<bool>> DeleteInvoiceShipment(int invoiceShipmentId)
//        {
//            return await HandleApiOperationAsync(async () =>
//            {
//                await _invoiceShipmentService.RemoveInvoiceShipment(invoiceShipmentId);

//                return new ServiceResponse<bool>
//                {
//                    Object = true
//                };
//            });
//        }

//        [HttpPut]
//        [Route("{invoiceShipmentId:int}")]
//        public async Task<IServiceResponse<bool>> UpdateInvoiceShipment(int invoiceShipmentId, InvoiceShipmentDTO invoiceShipmentDto)
//        {
//            return await HandleApiOperationAsync(async () =>
//            {
//                await _invoiceShipmentService.UpdateInvoiceShipment(invoiceShipmentId, invoiceShipmentDto);

//                return new ServiceResponse<bool>
//                {
//                    Object = true
//                };
//            });
//        }

//    }
//}
