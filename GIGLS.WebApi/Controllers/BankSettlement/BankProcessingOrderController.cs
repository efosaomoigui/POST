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
    [Authorize(Roles = "Account, Shipment")]
    [RoutePrefix("api/BankProcessingOrderWaybillsandCode")]
    public class BankProcessingOrderController : BaseWebApiController
    {
        private readonly IBankShipmentSettlementService _bankprocessingorder; 

        public BankProcessingOrderController(IBankShipmentSettlementService bankprocessingorder) :base(nameof(BankProcessingOrderController))
        {
            _bankprocessingorder = bankprocessingorder;
        }

        //This one searches for all Shipments recorded: InvoiceView
        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("RequestBankProcessingOrderForShipment")]
        public async Task<IServiceResponse<object>> RequestBankProcessingOrderForShipment(DepositType type)
        {
            return await HandleApiOperationAsync(async () =>
            {
                //All cash shipments from sales
                var bankshipmentprocessingorders = await _bankprocessingorder.GetBankProcessingOrderForShipment(type);
                return new ServiceResponse<object>
                {
                    Object = bankshipmentprocessingorders.Item2,
                    Total = bankshipmentprocessingorders.Item3,
                    RefCode = bankshipmentprocessingorders.Item1
                };
            });
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("RequestBankProcessingOrderForShipment/ScheduledTask")]
        public async Task<IServiceResponse<object>> RequestBankProcessingOrderForShipment_ScheduledTask(DepositType type, int ServiceCenter) 
        {
            return await HandleApiOperationAsync(async () =>
            {
                //All cash shipments from sales
                var o = await _bankprocessingorder.GetBankProcessingOrderForShipment_ScheduleTask(ServiceCenter, type);
                return new ServiceResponse<object>
                {
                    Object = o
                };
            });
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("RequestBankProcessingOrderForCOD/ScheduledTask")]
        public async Task<IServiceResponse<object>> RequestBankProcessingOrderForCOD_ScheduledTask(DepositType type, int ServiceCenter)
        {
            return await HandleApiOperationAsync(async () =>
            {
                //All cash shipments from sales
                var o = await _bankprocessingorder.GetBankProcessingOrderForCOD_ScheduledTask(type, ServiceCenter); 
                return new ServiceResponse<object>
                {
                    Object = o
                };
            });
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("RequestBankProcessingOrderForDemurrage/ScheduledTask")]
        public async Task<IServiceResponse<object>> RequestBankProcessingOrderForDemurrage_ScheduledTask(DepositType type, int ServiceCenter)
        {
            return await HandleApiOperationAsync(async () =>
            {
                //All cash shipments from sales
                var o = await _bankprocessingorder.GetBankProcessingOrderForDemurrage_ScheduleTask(type, ServiceCenter);  
                return new ServiceResponse<object>
                {
                    Object = o
                };
            });
        }

        //This one searches for all COD recorded: CashOnDeliveryRegisterAccount
        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("RequestBankProcessingOrderForCOD")]
        public async Task<IServiceResponse<object>> RequestBankProcessingOrderForCOD(DepositType type)
        {
            return await HandleApiOperationAsync(async () =>
            {
                //All cash CODs from sales
                var bankshipmentprocessingorders = await _bankprocessingorder.GetBankProcessingOrderForCOD(type);
                return new ServiceResponse<object>
                {
                    Object = bankshipmentprocessingorders.Item2,
                    Total = bankshipmentprocessingorders.Item3,
                    RefCode = bankshipmentprocessingorders.Item1
                };
            });
        }

        //This one searches for all Denurrage recorded: CashOnDeliveryRegisterAccount
        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("RequestBankProcessingOrderForDemurrage")]
        public async Task<IServiceResponse<object>> RequestBankProcessingOrderForDemurrage(DepositType type)  
        {
            return await HandleApiOperationAsync(async () =>
            {
                //All cash CODs from sales
                var bankshipmentprocessingorders = await _bankprocessingorder.GetBankProcessingOrderForDemurrage(type);
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


        [GIGLSActivityAuthorize(Activity = "View")]
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

        [GIGLSActivityAuthorize(Activity = "View")]
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

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("MarkAsDeposited")]
        public async Task<IServiceResponse<object>> MarkAsDeposited(BankProcessingOrderCodesDTO bkoc)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _bankprocessingorder.UpdateBankOrderProcessingCode(bkoc);
                return new ServiceResponse<object>
                {
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("MarkAsDeposited_cod")]
        public async Task<IServiceResponse<object>> MarkAsDeposited_cod(BankProcessingOrderCodesDTO bkoc)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _bankprocessingorder.UpdateBankOrderProcessingCode_cod(bkoc); 
                return new ServiceResponse<object>
                {
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("markasdeposited_demurrage")]
        public async Task<IServiceResponse<object>> MarkAsDeposited_demurrage(BankProcessingOrderCodesDTO bkoc)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _bankprocessingorder.UpdateBankOrderProcessingCode_demurrage(bkoc);
                return new ServiceResponse<object>
                {
                };
            });
        }


        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("MarkAsVerified_cod")]
        public async Task<IServiceResponse<object>> MarkAsVerified_cod(BankProcessingOrderCodesDTO bkoc)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _bankprocessingorder.MarkAsVerified_cod(bkoc);
                return new ServiceResponse<object>
                {
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("markasVerified_demurrage")]
        public async Task<IServiceResponse<object>> MarkAsVerified_demurrage(BankProcessingOrderCodesDTO bkoc)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _bankprocessingorder.MarkAsVerified_demurrage(bkoc); 
                return new ServiceResponse<object>
                {
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("MarkAsVerified")]
        public async Task<IServiceResponse<object>> MarkAsVerified(BankProcessingOrderCodesDTO bkoc)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _bankprocessingorder.MarkAsVerified(bkoc);
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
                await _bankprocessingorder.AddBankProcessingOrderCode(bkoc);
                return new ServiceResponse<object>
                {
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")] 
        [HttpPost]
        [Route("addbankprocessingorderCodedemurrageonly")]
        public async Task<IServiceResponse<object>> AddBankProcessingOrderCodeDemurrageOnly(BankProcessingOrderCodesDTO bkoc)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _bankprocessingorder.AddBankProcessingOrderCodeDemurrageOnly(bkoc);
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
        
        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("getbankOrderprocessingcodebyDate")]
        public async Task<IServiceResponse<List<BankProcessingOrderCodesDTO>>> GetBankOrderProcessingCodeByDate(DepositType type, BankDepositFilterCriteria dateFilterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var resValue = await _bankprocessingorder.GetBankOrderProcessingCodeByDate(type,dateFilterCriteria);
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


        //This one searches for all new Paid Out CODs
        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("RequestCODCustomerWhoNeedPayOut")]
        public async Task<IServiceResponse<object>> RequestCODCustomerWhoNeedPayOut()
        {
            return await HandleApiOperationAsync(async () =>
            {
                //All cash CODs from sales
                var bankshipmentprocessingorders = await _bankprocessingorder.GetCODCustomersWhoNeedPayOut();
                return new ServiceResponse<object>
                {
                    Object = bankshipmentprocessingorders
                };
            });
        }

        //This one searches for all new Paid Out CODs
        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("PaidOutCODLists")]
        public async Task<IServiceResponse<object>> PaidOutCODLists() 
        {
            return await HandleApiOperationAsync(async () =>
            {
                //All cash CODs from sales
                var bankshipmentprocessingorders = await _bankprocessingorder.GetPaidOutCODLists();
                return new ServiceResponse<object>
                {
                    Object = bankshipmentprocessingorders
                };
            });
        }

        //This one searches for all new Paid Out CODs
        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("UpdateCODCustomerWhoNeedPayOut")]
        public async Task<IServiceResponse<object>> UpdateCODCustomerWhoNeedPayOut(NewInvoiceViewDTO invoiceinfo)
        {
            return await HandleApiOperationAsync(async () =>
            {
                //All cash CODs from sales
                await _bankprocessingorder.UpdateCODCustomersWhoNeedPayOut(invoiceinfo);

                return new ServiceResponse<object>
                {
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

        //This one searches for all new Paid Out CODs
        //[GIGLSActivityAuthorize(Activity = "View")]
        //[HttpGet]
        //[Route("ViewCustomersPaidOutCODs")]
        //public async Task<IServiceResponse<object>> ViewCustomersPaidOutCODs()
        //{

        //    return null;
        //    //return await HandleApiOperationAsync(async () =>
        //    //{
        //    //    //All cash CODs from sales
        //    //    var bankshipmentprocessingorders = await _bankprocessingorder.GetBankProcessingOrderForDemurrage(type);
        //    //    return new ServiceResponse<object>
        //    //    {
        //    //        Object = bankshipmentprocessingorders.Item2,
        //    //        Total = bankshipmentprocessingorders.Item3,
        //    //        RefCode = bankshipmentprocessingorders.Item1
        //    //    };
        //    //});
        //}


    }
}
