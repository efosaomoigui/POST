﻿using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Wallet;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.Wallet
{
    [Authorize(Roles = "SuperAdmin,SubAdmin,Shipment,Account,Report")]
    [RoutePrefix("api/wallet")]
    public class WalletController : BaseWebApiController
    {
        private readonly IWalletService _walletService;
        public WalletController(IWalletService walletService) :base(nameof(WalletController))
        {
            _walletService = walletService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<WalletDTO>>> GetWallets()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var wallets = await _walletService.GetWallets();
                return new ServiceResponse<IEnumerable<WalletDTO>>
                {
                    Object = wallets
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{walletId:int}")]
        public async Task<IServiceResponse<WalletDTO>> GetWalletById(int walletId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _walletService.GetWalletById(walletId);

                return new ServiceResponse<WalletDTO>
                {
                    Object = result
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("getsystemwallet")]
        public async Task<IServiceResponse<WalletDTO>> GetSystemWalletById()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _walletService.GetSystemWallet();

                return new ServiceResponse<WalletDTO>
                {
                    Object = result
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddWallet(WalletDTO newWallet)
        {
            return await HandleApiOperationAsync(async () =>
            {
                //var Wallet = await _walletService.AddWallet(newWallet);
                await _walletService.AddWallet(newWallet);
                return new ServiceResponse<object>
                {
                    //Object = Wallet
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{walletId:int}")]
        public async Task<IServiceResponse<object>> UpdateWallet(int walletId, WalletTransactionDTO walletTransactionDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _walletService.UpdateWallet(walletId, walletTransactionDTO);
                return new ServiceResponse<object>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{walletId:int}")]
        public async Task<IServiceResponse<bool>> DeleteWallet(int walletId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _walletService.RemoveWallet(walletId);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }
    }
}