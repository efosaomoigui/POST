using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.BankSettlement;
using GIGLS.Core.DTO.Report;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.BankSettlement;
using GIGLS.Services.Implementation;
using GIGLS.Services.Scheduler;
using GIGLS.WebApi.Filters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.BankSettlement
{
    [Authorize]
    [RoutePrefix("api/bankprocessingorders")]
    public class BankProcessingOrderForAllController : BaseWebApiController
    {
        private readonly IBankShipmentSettlementService _bankprocessingorder;

        public BankProcessingOrderForAllController(IBankShipmentSettlementService bankprocessingorder) : base(nameof(BankProcessingOrderForAllController))
        {
            _bankprocessingorder = bankprocessingorder;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("SearchBankOrder")]
        public async Task<IServiceResponse<object>> SearchBankOrder(string refCode, DepositType type)
        {
            return await HandleApiOperationAsync(async () =>
            {
                //All cash shipments from sales
                //var bankprocessingorders = await _bankprocessingorder.SearchBankProcessingOrder(refCode, type);
                var bankprocessingorders = await _bankprocessingorder.SearchBankProcessingOrderV2(refCode, type);
                return new ServiceResponse<object>
                {
                    Object = bankprocessingorders.Item2,
                    Total = bankprocessingorders.Item3,
                    RefCode = bankprocessingorders.Item1,
                    Shipmentcodref = bankprocessingorders.Item4
                };
            });
        }


        [HttpGet]
        [Route("SearchBankOrder2")]
        public async Task<IServiceResponse<object>> SearchBankOrder2(string refCode, DepositType type)
        {
            return await HandleApiOperationAsync(async () =>
            {
                //All cash shipments from sales
                var bankprocessingorders = await _bankprocessingorder.SearchBankProcessingOrder2(refCode, type);
                return new ServiceResponse<object>
                {
                    Object = bankprocessingorders.Item4,
                    Total = bankprocessingorders.Item3,
                    RefCode = bankprocessingorders.Item1,
                };
            });
        }

        [HttpGet]
        [Route("SearchBankOrder3")]
        public async Task<IServiceResponse<object>> SearchBankOrder3(string refCode, DepositType type)
        {
            return await HandleApiOperationAsync(async () =>
            {
                //All cash shipments from sales
                //var bankprocessingorders = await _bankprocessingorder.SearchBankProcessingOrder3(refCode, type);
                var bankprocessingorders = await _bankprocessingorder.SearchBankProcessingOrderV2(refCode, type);
                return new ServiceResponse<object>
                {
                    Object = bankprocessingorders.Item2,
                    Total = bankprocessingorders.Item3,
                    RefCode = bankprocessingorders.Item1,
                    Shipmentcodref = bankprocessingorders.Item4
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("getbankprocessingorderForshipmentandcod")]
        public async Task<IServiceResponse<List<BankProcessingOrderForShipmentAndCODDTO>>> GetBankProcessingOrderForShipmentAndCOD(DepositType type)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var resValue = await _bankprocessingorder.GetBankProcessingOrderForShipmentAndCOD(type);
                return new ServiceResponse<List<BankProcessingOrderForShipmentAndCODDTO>>
                {
                    Object = resValue
                };
            });
        }

        //Helps get all processing order by the type: COD or Shipment from 

        [HttpPost]
        [Route("getbankOrderprocessingcodebyDate")]
        public async Task<IServiceResponse<List<BankProcessingOrderCodesDTO>>> GetBankOrderProcessingCodeByDate(DepositType type, BankDepositFilterCriteria dateFilterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var resValue = await _bankprocessingorder.GetBankOrderProcessingCodeByDate(type, dateFilterCriteria);
                return new ServiceResponse<List<BankProcessingOrderCodesDTO>>
                {
                    Object = resValue
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("getbankOrderprocessingcodebyByServiceCenter")]
        public async Task<IServiceResponse<List<BankProcessingOrderCodesDTO>>> getbankOrderprocessingcodebyByServiceCenter(DepositType type, BankDepositFilterCriteria dateFilterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var resValue = await _bankprocessingorder.GetBankOrderProcessingCodeByServiceCenter(type, dateFilterCriteria);
                return new ServiceResponse<List<BankProcessingOrderCodesDTO>>
                {
                    Object = resValue
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("getregionalbankOrderprocessingcodebyDate")]
        public async Task<IServiceResponse<List<BankProcessingOrderCodesDTO>>> GetRegionalBankOrderProcessingCodeByDate(DepositType type, BankDepositFilterCriteria dateFilterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var resValue = await _bankprocessingorder.GetRegionalBankOrderProcessingCodeByDate(type, dateFilterCriteria);
                return new ServiceResponse<List<BankProcessingOrderCodesDTO>>
                {
                    Object = resValue
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("getBanks")]
        public async Task<IServiceResponse<IEnumerable<BankDTO>>> GetBanks()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var banks = await _bankprocessingorder.GetBanks();
                return new ServiceResponse<IEnumerable<BankDTO>>
                {
                    Object = banks
                };
            });
        }

        [HttpPost]
        [Route("getbankOrderprocessingcodeforECObyDate")]
        public async Task<IServiceResponse<List<BankProcessingOrderCodesDTO>>> GetRegionalAndECOBankOrderProcessingCodeByDate(DepositType type, BankDepositFilterCriteria dateFilterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var resValue = await _bankprocessingorder.GetRegionalAndECOBankOrderProcessingCodeByDate(type, dateFilterCriteria);
                return new ServiceResponse<List<BankProcessingOrderCodesDTO>>
                {
                    Object = resValue
                };
            });
        }


    }
}
