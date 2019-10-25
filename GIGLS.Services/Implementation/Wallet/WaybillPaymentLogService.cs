using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO.OnlinePayment;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IServices.Wallet;
using GIGLS.CORE.DTO.Shipments;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Wallet
{
    public class WaybillPaymentLogService : IWaybillPaymentLogService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;

        public WaybillPaymentLogService(IUnitOfWork uow, IUserService userService)
        {
            _uow = uow;
            _userService = userService;
            MapperConfig.Initialize();
        }

        public async Task<PaymentInitiate> AddWaybillPaymentLog(WaybillPaymentLogDTO waybillPaymentLog)
        {
            var MerchantId = ConfigurationManager.AppSettings["MerchantId"];
            var MerchantUsername = ConfigurationManager.AppSettings["MerchantUsername"];
            var MerchantKey = ConfigurationManager.AppSettings["MerchantKey"];

            //check the current transaction on the waybill
            var paymentLog = _uow.WaybillPaymentLog.GetAllAsQueryable()
                .Where(x => x.Waybill == waybillPaymentLog.Waybill).OrderByDescending(x => x.DateCreated).FirstOrDefault();

            if (paymentLog != null)
            {
                //think on how to solve this later but for now, return this below
                return new PaymentInitiate
                {
                    IsPaymentSuccessful = false,
                    MerchantId = MerchantId,
                    MerchantUsername = MerchantUsername,
                    MerchantKey = MerchantKey,
                    TransactionId = waybillPaymentLog.Reference
                };
            }


            if (waybillPaymentLog.UserId == null)
            {
                waybillPaymentLog.UserId = await _userService.GetCurrentUserId();
            }

            var newPaymentLog = Mapper.Map<WaybillPaymentLog>(waybillPaymentLog);

            _uow.WaybillPaymentLog.Add(newPaymentLog);
            await _uow.CompleteAsync();
                       
            return new PaymentInitiate
            {
                IsPaymentSuccessful = false,
                MerchantId = MerchantId,
                MerchantUsername = MerchantUsername,
                MerchantKey = MerchantKey,
                TransactionId = waybillPaymentLog.Reference
            };
        }

        public async Task<WaybillPaymentLogDTO> GetWaybillPaymentLogByReference(string reference)
        {
            var paymentlog =  await _uow.WaybillPaymentLog.GetAsync(x => x.Reference == reference);
            return Mapper.Map<WaybillPaymentLogDTO>(paymentlog);
        }

        public async Task<WaybillPaymentLogDTO> GetWaybillPaymentLogByWaybill(string waybill)
        {
            var paymentLog = _uow.WaybillPaymentLog.GetAllAsQueryable().Where(x => x.Waybill == waybill).LastOrDefault();
            var paymentDto =  Mapper.Map<WaybillPaymentLogDTO>(paymentLog);
            return await Task.FromResult(paymentDto);
        }

        public async Task<List<WaybillPaymentLogDTO>> GetWaybillPaymentLogListByWaybill(string waybill)
        {
            var paymentLog = await _uow.WaybillPaymentLog.FindAsync(x => x.Waybill == waybill);
            return Mapper.Map<List<WaybillPaymentLogDTO>>(paymentLog);
        }

        public async Task<IEnumerable<WaybillPaymentLogDTO>> GetWaybillPaymentLogs()
        {
            var paymentlogs = _uow.WaybillPaymentLog.GetAll().ToList();
            var paymentlogDto = Mapper.Map<IEnumerable<WaybillPaymentLogDTO>>(paymentlogs);
            return await Task.FromResult(paymentlogDto);
        }

        public Tuple<Task<List<WaybillPaymentLogDTO>>> GetWaybillPaymentLogs(FilterOptionsDto filterOptionsDto)
        {
            throw new NotImplementedException();
        }

        public Task UpdateWaybillPaymentLog(WaybillPaymentLogDTO waybillPaymentLog)
        {
            throw new NotImplementedException();
        }
    }
}
