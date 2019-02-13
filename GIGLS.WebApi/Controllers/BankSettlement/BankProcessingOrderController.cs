using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.BankSettlement;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.BankSettlement;
using GIGLS.Core.IServices.CashOnDeliveryBalance;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.BankSettlement
{
    [Authorize(Roles = "Account")]
    [RoutePrefix("api/BankProcessingOrderWaybillsandCode")]
    public class BankProcessingOrderController : BaseWebApiController
    {
        private readonly IBankShipmentSettlementService _bankprocessingorder; 

        public BankProcessingOrderController(IBankShipmentSettlementService bankprocessingorder) :base(nameof(BankProcessingOrderController))
        {
            _bankprocessingorder = bankprocessingorder;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("RequestBankProcessingOrderForShipment")]
        public async Task<IServiceResponse<object>> RequestBankProcessingOrderForShipment(DateTime requestdate)
        {
            return await HandleApiOperationAsync(async () =>
            {
                //All cash shipments from sales
                var bankshipmentprocessingorders = await _bankprocessingorder.GetBankProcessingOrderForShipment(requestdate);
                return new ServiceResponse<object>
                {
                    Object = bankshipmentprocessingorders.Item2,
                    Total = bankshipmentprocessingorders.Item3,
                    RefCode = bankshipmentprocessingorders.Item1
                };
            });
        }


        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("RequestBankProcessingOrderForCOD")]
        public async Task<IServiceResponse<object>> RequestBankProcessingOrderForCOD(DateTime requestdate)
        {
            return await HandleApiOperationAsync(async () =>
            {

                //All cash CODs from sales
                var bankshipmentprocessingorders = await _bankprocessingorder.GetBankProcessingOrderForCOD(requestdate);
                return new ServiceResponse<object>
                {
                    Object = bankshipmentprocessingorders.Item2,
                    Total = bankshipmentprocessingorders.Item3,
                    RefCode = bankshipmentprocessingorders.Item1
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("searchbankorder")]
        public async Task<IServiceResponse<object>> SearchBankOrder() 
        {
            return await HandleApiOperationAsync(async () =>
            {
                //All cash shipments from sales
                //var bankshipmentprocessingorders = await _bankprocessingorder.GetBankProcessingOrderForShipment(requestdate);
                return new ServiceResponse<object>
                {
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("addbankprocessingorderCode")]
        public async Task<IServiceResponse<object>> AddBankProcessingOrderCode(BankProcessingOrderCodesDTO bkoc)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var bankshipmentprocessingorders = await _bankprocessingorder.AddBankProcessingOrderCode(bkoc);
                return new ServiceResponse<object>
                {
                };
            });
        }


        //[GIGLSActivityAuthorize(Activity = "Create")]
        //[HttpPost]
        //[Route("addbankprocessingOrderforshipmentandcod")]
        //public async Task<IServiceResponse<object>> AddBankProcessingOrderForShipmentAndCOD(BankProcessingOrderForShipmentAndCODDTO bkoc) 
        //{
        //    return await HandleApiOperationAsync(async () =>
        //    {
        //        //var bankshipmentprocessingorders = await _bankprocessingorder.AddBankProcessingOrderForShipmentAndCOD(bkoc);
        //        return new ServiceResponse<object>
        //        {
        //        };
        //    });
        //}

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("getbankprocessingorderForshipmentandcod")]
        public async Task<IServiceResponse<List<BankProcessingOrderForShipmentAndCODDTO>>> GetBankProcessingOrderForShipmentAndCOD()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var resValue = await _bankprocessingorder.GetBankProcessingOrderForShipmentAndCOD();
                return new ServiceResponse<List<BankProcessingOrderForShipmentAndCODDTO>>
                {
                    Object = resValue
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("getbankOrderprocessingcode")]
        public async Task<IServiceResponse<List<BankProcessingOrderCodesDTO>>> GetBankOrderProcessingCode() 
        {
            return await HandleApiOperationAsync(async () =>
            {
                var resValue = await _bankprocessingorder.GetBankOrderProcessingCode();
                return new ServiceResponse<List<BankProcessingOrderCodesDTO>>
                {
                    Object = resValue
                };
            });
        }


    }
}
