using GIGLS.Core.IServices.Fleets;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Fleets;
using GIGLS.Core;
using AutoMapper;
using GIGLS.Core.Domain;
using GIGLS.Infrastructure;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IServices.Wallet;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO.Wallet;

namespace GIGLS.Services.Implementation.Fleets
{
    public class DispatchService : IDispatchService
    {
        private readonly IUserService _userService;

        private readonly IWalletService _walletService;

        private readonly IUnitOfWork _uow;

        public DispatchService(IUserService userService, IWalletService walletService
            , IUnitOfWork uow)
        {
            _walletService = walletService;
            _userService = userService;
            _uow = uow;
            MapperConfig.Initialize();
        }

        /// <summary>
        /// This method creates a new dispatch, updates the manifest and system wallet information
        /// </summary>
        /// <param name="dispatch"></param>
        /// <returns></returns>
        public async Task<object> AddDispatch(DispatchDTO dispatch)
        {
            try
            {
                //get the login user
                var userId = await _userService.GetCurrentUserId();

                // create dispatch
                var newDispatch = Mapper.Map<Dispatch>(dispatch);
                _uow.Dispatch.Add(newDispatch);

                // update manifest
                var manifestObj = _uow.Manifest.SingleOrDefault(s => s.ManifestCode == dispatch.ManifestNumber);
                if(manifestObj != null)
                {
                    var manifestEntity = _uow.Manifest.Get(manifestObj.ManifestId);
                    manifestEntity.DispatchedById = userId;
                    manifestEntity.IsDispatched = true;
                }

                // update system wallet, by creating a wallet transaction
                var systemWallet = await _walletService.GetSystemWallet();
                var walletTransaction = new WalletTransactionDTO
                {
                    Amount = dispatch.Amount,
                    WalletId = systemWallet.WalletId,
                    CreditDebitType = Core.Enums.CreditDebitType.Credit,
                    Description = "Credit from Dispatch"
                };
                await _walletService.UpdateWallet(systemWallet.WalletId, walletTransaction);

                // commit transaction
                await _uow.CompleteAsync();
                return new { Id = newDispatch.DispatchId };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteDispatch(int dispatchId)
        {
            try
            {
                var dispatch = await _uow.Dispatch.GetAsync(dispatchId);
                if (dispatch == null)
                {
                    throw new GenericException("Information does not Exist");
                }
                _uow.Dispatch.Remove(dispatch);
                await _uow.CompleteAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DispatchDTO> GetDispatchById(int dispatchId)
        {
            try
            {
                var dispatch = await _uow.Dispatch.GetAsync(dispatchId);
                if (dispatch == null)
                {
                    throw new GenericException("Information does not Exist");
                }
                return Mapper.Map<DispatchDTO>(dispatch);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DispatchDTO>> GetDispatchs()
        {
            return await _uow.Dispatch.GetDispatchAsync();
        }

        public async Task UpdateDispatch(int dispatchId, DispatchDTO dispatchDTO)
        {
            try
            {
                var dispatch = await _uow.Dispatch.GetAsync(dispatchId);
                if (dispatch == null || dispatchDTO.DispatchId != dispatchId)
                {
                    throw new GenericException("Information does not Exist");
                }

                dispatch.DispatchId = dispatchDTO.DispatchId;
                dispatch.RegistrationNumber = dispatchDTO.RegistrationNumber;
                dispatch.ManifestNumber = dispatchDTO.ManifestNumber;
                dispatch.Amount = dispatchDTO.Amount;
                dispatch.RescuedDispatchId = dispatchDTO.RescuedDispatchId;
                dispatch.DriverDetail = dispatchDTO.DriverDetail;
                dispatch.DispatchedBy = dispatchDTO.DispatchedBy;
                dispatch.ReceivedBy = dispatchDTO.ReceivedBy;
                dispatch.DispatchCategory = dispatchDTO.DispatchCategory;
                dispatch.DepartureId = dispatchDTO.DepartureId;
                dispatch.DestinationId = dispatchDTO.DestinationId;

                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
