using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using GIGLS.Core.IRepositories;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.User;
using GIGLS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly IUnitOfWork _uow;
        private IUserService _userService;

        public PaymentMethodService(IUnitOfWork uow,  IUserService userService)
        {
            _uow = uow;
            _userService = userService;
            MapperConfig.Initialize();
        }

        public async Task<bool> AddPaymentMethod(PaymentMethodDTO paymentMethodDTO)
        {
            try
            {
                if (paymentMethodDTO is null)
                {
                    throw new GenericException("invalid payload", $"{(int)HttpStatusCode.BadRequest}");
                }

                var entity = await _uow.PaymentMethod.ExistAsync(x => x.PaymentMethodId == paymentMethodDTO.PaymentMethodId);
                if (entity)
                {
                    throw new GenericException($"This payment method with PaymentId {paymentMethodDTO.PaymentMethodId} already exist.", $"{(int)HttpStatusCode.Forbidden}");
                }

                var paymentMethod = Mapper.Map<PaymentMethod>(paymentMethodDTO);
                _uow.PaymentMethod.Add(paymentMethod);
                await _uow.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<PaymentMethodDTO>> GetPaymentMethodByUserActiveCountry()
        {
            try
            {
                var userId = await _userService.GetCurrentUserId();
                var user = await _userService.GetUserById(userId);
                if (user.UserActiveCountryId < 0)
                {
                    throw new GenericException("invalid country Id", $"{(int)HttpStatusCode.BadRequest}");
                }

                var result = await _uow.PaymentMethod.GetPaymentMethodByUserActiveCountry(user.UserActiveCountryId);
                
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
