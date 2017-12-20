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
using GIGLS.Core.Enums;
using GIGLS.Core.DTO.ServiceCentres;

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
        /// <param name="dispatchDTO"></param>
        /// <returns></returns>
        public async Task<object> AddDispatch(DispatchDTO dispatchDTO)
        {
            try
            {
                // get user login service centre
                var serviceCenterIds = await _userService.GetPriviledgeServiceCenters();
                var userServiceCentreId = serviceCenterIds[0];

                //get the login user
                var currentUserId = await _userService.GetCurrentUserId();
                var currentUserDetail = await _userService.GetUserById(currentUserId);

                // create dispatch
                var newDispatch = Mapper.Map<Dispatch>(dispatchDTO);
                newDispatch.DispatchedBy  = currentUserDetail.FirstName + " " + currentUserDetail.LastName;
                newDispatch.ServiceCentreId = userServiceCentreId;
                _uow.Dispatch.Add(newDispatch);

                // update manifest
                var manifestObj = _uow.Manifest.SingleOrDefault(s => s.ManifestCode == dispatchDTO.ManifestNumber);
                if(manifestObj != null)
                {
                    var manifestEntity = _uow.Manifest.Get(manifestObj.ManifestId);
                    manifestEntity.DispatchedById = currentUserId;
                    manifestEntity.IsDispatched = true;
                }

                // update system wallet, by creating a wallet transaction
                //var systemWallet = await _walletService.GetSystemWallet();
                //var walletTransaction = new WalletTransactionDTO
                //{
                //    Amount = dispatch.Amount,
                //    WalletId = systemWallet.WalletId,
                //    CreditDebitType = Core.Enums.CreditDebitType.Credit,
                //    Description = "Debit from Dispatch"
                //};
                //await _walletService.UpdateWallet(systemWallet.WalletId, walletTransaction);

                
                //update General Ledger
                var generalLedger = new GeneralLedger()
                {
                    DateOfEntry = DateTime.Now,

                    ServiceCentreId = userServiceCentreId,
                    UserId = currentUserId,
                    Amount = dispatchDTO.Amount,
                    CreditDebitType = CreditDebitType.Debit,
                    Description = "Debit from Dispatch",
                    IsDeferred = false,
                    PaymentServiceType = PaymentServiceType.Dispatch
                };
                _uow.GeneralLedger.Add(generalLedger);

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

        public async Task<DispatchDTO> GetDispatchManifestCode(string manifest)
        {
            try
            {
                var dispatch = await _uow.Dispatch.GetAsync(x => x.ManifestNumber.Equals(manifest));
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
            var serviceCenterIds = await _userService.GetPriviledgeServiceCenters();
            var dispatchs =  await _uow.Dispatch.GetDispatchAsync(serviceCenterIds);

            foreach (var item in dispatchs)
            {
                // get the service cenre
                var departureSC = await _uow.Station.GetAsync((int)item.DepartureId);
                var destinationSC = await _uow.Station.GetAsync((int)item.DestinationId);

                item.Departure = Mapper.Map<StationDTO>(departureSC);
                item.Destination = Mapper.Map<StationDTO>(destinationSC);
            }
            return dispatchs;
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
                dispatch.ServiceCentreId = dispatchDTO.ServiceCentreId;

                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
