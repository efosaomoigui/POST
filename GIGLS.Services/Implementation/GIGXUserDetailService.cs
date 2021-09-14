using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.BankSettlement;
using GIGLS.Core.DTO.User;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.BankSettlement;
using GIGLS.Core.IServices.User;
using GIGLS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.BankSettlement
{
    public class GIGXUserDetailService : IGIGXUserDetailService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;

        public GIGXUserDetailService(IUnitOfWork uow, IUserService userService)
        {
            _uow = uow;
            _userService = userService;
            MapperConfig.Initialize();
        }

        public async Task<bool> AddGIGXUserDetail(GIGXUserDetailDTO gIGXUserDetailDTO)
        {

            try
            {
                if (await _uow.GIGXUserDetail.ExistAsync(v => v.CustomerCode.ToLower() == gIGXUserDetailDTO.CustomerCode.ToLower()))
                {
                    throw new GenericException($"User already have a pin");
                }

                // get the current user info
                var currentUserId = await _userService.GetCurrentUserId();
                var user = await _userService.GetUserById(currentUserId);
                var gigxUser = new GIGXUserDetail
                {
                    CustomerCode = user.UserChannelCode,
                    CustomerPin = gIGXUserDetailDTO.CustomerPin.ToString().Trim(),
                    PrivateKey = gIGXUserDetailDTO.PrivateKey,
                    PublicKey = gIGXUserDetailDTO.PublicKey,
                    WalletAddress = gIGXUserDetailDTO.WalletAddress
                };

                _uow.GIGXUserDetail.Add(gigxUser);
                await _uow.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<bool> CheckIfUserHasPin()
        {
            // get the current user info
            GIGXUserPinDTO gigxusersDTO = new GIGXUserPinDTO();
            gigxusersDTO.HasPin = false;
            var currentUserId = await _userService.GetCurrentUserId();
            var user = await _userService.GetUserById(currentUserId);
            var userPin = await _uow.GIGXUserDetail.GetGIGXUserDetailByCode(user.UserChannelCode);
            if (userPin != null && userPin.CustomerPin > 0)
            {
                gigxusersDTO.HasPin = true;
            }
            return gigxusersDTO.HasPin;
        }

        public async Task<GIGXUserPinDTO> VerifyUserPin(GIGXUserDetailDTO gIGXUserDetailDTO)
        {
            // get the current user info
            GIGXUserPinDTO gigxusersDTO = new GIGXUserPinDTO();
            gigxusersDTO.HasPin = true;
            var currentUserId = await _userService.GetCurrentUserId();
            var user = await _userService.GetUserById(currentUserId);
            var pin = gIGXUserDetailDTO.CustomerPin.ToString();
            var userPin = await _uow.GIGXUserDetail.GetGIGXUserDetailByCode(user.UserChannelCode);
            if (userPin != null && !String.IsNullOrEmpty(pin) && userPin.CustomerPin == gIGXUserDetailDTO.CustomerPin)
            {
                gigxusersDTO.GIGXUserDetailDTO = userPin;
                gigxusersDTO.GIGXUserDetailDTO.CustomerPin = 0;
            }
            return gigxusersDTO;
        }

    }
}
