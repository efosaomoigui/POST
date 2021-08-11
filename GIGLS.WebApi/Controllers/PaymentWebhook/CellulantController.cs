using GIGLS.Core.DTO;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Wallet;
using GIGLS.CORE.DTO.Report;
using GIGLS.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.PaymentWebhook
{
    [Authorize]
    [RoutePrefix("api/cellulant")]
    public class CellulantController : BaseWebApiController
    {
        private readonly ICellulantPaymentService _cellulantService;

        public CellulantController(ICellulantPaymentService cellulantService) : base(nameof(CellulantController))
        {
            _cellulantService = cellulantService;
        }

        [HttpPost]
        [Route("transferDetails")]
        public async Task<IServiceResponse<List<TransferDetailsDTO>>> GetTransferDetailsLogFilter(BaseFilterCriteria filterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _cellulantService.GetTransferDetails(filterCriteria);

                return new ServiceResponse<List<TransferDetailsDTO>>
                {
                    Object = result
                };
            });
        }

        [HttpPost]
        [Route("searchtransferdetails")]
        public async Task<IServiceResponse<List<TransferDetailsDTO>>> GetTransferDetailsLogFilterByAccountNumber(BaseFilterCriteria filterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _cellulantService.GetTransferDetailsByAccountNumber(filterCriteria.SenderAccountNumber);

                return new ServiceResponse<List<TransferDetailsDTO>>
                {
                    Object = result
                };
            });
        }
    }
}