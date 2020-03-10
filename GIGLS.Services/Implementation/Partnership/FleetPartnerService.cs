using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain.Partnership;
using GIGLS.Core.DTO.Partnership;
using GIGLS.Core.DTO.Report;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.Partnership;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IServices.Utility;
using GIGLS.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Partnership
{
    public class FleetPartnerService : IFleetPartnerService
    {
        private readonly IUnitOfWork _uow;
        private readonly INumberGeneratorMonitorService _numberGeneratorMonitorService;
        private readonly IUserService _userService;

        public FleetPartnerService(IUnitOfWork uow, INumberGeneratorMonitorService numberGeneratorMonitorService)
        {
            _uow = uow;
            _numberGeneratorMonitorService = numberGeneratorMonitorService;
            MapperConfig.Initialize();
        }

        public async Task<object> AddFleetPartner(FleetPartnerDTO fleetPartnerDTO)
        {
            fleetPartnerDTO.PartnerName = fleetPartnerDTO.PartnerName.Trim();

            if (await _uow.FleetPartner.ExistAsync(v => v.PartnerName.ToLower() == fleetPartnerDTO.PartnerName.ToLower()))
            {
                throw new GenericException("Fleet Partner Already Exists");
            }

            var fleetPartnerCode = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.FleetPartner);

            var fleetPartner = Mapper.Map<FleetPartner>(fleetPartnerDTO);
            fleetPartner.FleetPartnerCode = fleetPartnerCode;
            _uow.FleetPartner.Add(fleetPartner);
            await _uow.CompleteAsync();
            return new { id = fleetPartner.FleetPartnerId };
        }

        public async Task UpdateFleetPartner(int partnerId, FleetPartnerDTO fleetPartnerDTO)
        {
            var existingParntner = await _uow.FleetPartner.GetAsync(partnerId);

            if (existingParntner == null)
            {
                throw new GenericException("Fleet Partner Does not Exist");
            }

            existingParntner.FirstName = fleetPartnerDTO.FirstName.Trim();
            existingParntner.LastName = fleetPartnerDTO.LastName.Trim();
            existingParntner.Email = fleetPartnerDTO.Email.Trim();
            existingParntner.PartnerName = fleetPartnerDTO.PartnerName.Trim();
            existingParntner.Address = fleetPartnerDTO.Address;
            existingParntner.OptionalPhoneNumber = fleetPartnerDTO.OptionalPhoneNumber;
            existingParntner.PhoneNumber = fleetPartnerDTO.PhoneNumber;
            await _uow.CompleteAsync();
        }

        public async Task RemoveFleetPartner(int partnerId)
        {
            var existingPartner = await _uow.FleetPartner.GetAsync(partnerId);

            if (existingPartner == null)
            {
                throw new GenericException("Fleet Partner does not exist");
            }
            _uow.FleetPartner.Remove(existingPartner);
            await _uow.CompleteAsync();
        }

        public async Task<IEnumerable<FleetPartnerDTO>> GetFleetPartners()
        {
            var partners = await _uow.FleetPartner.GetFleetPartnersAsync();
            return partners;
        }

        public async Task<FleetPartnerDTO> GetFleetPartnerById(int partnerId)
        {
            var partnerDto = new FleetPartnerDTO();
            var partner = await _uow.FleetPartner.GetAsync(partnerId);

            if (partner == null)
            {
                throw new GenericException("Fleet Partner does not exist");
            }
            else
            {
                partnerDto = Mapper.Map<FleetPartnerDTO>(partner);
                //var Wallet = await _uow.Wallet.GetAsync(s => s.CustomerCode == partner.PartnerCode);
                var Country = await _uow.Country.GetAsync(s => s.CountryId == partner.UserActiveCountryId);
                //if (Wallet != null)
                //{
                //    partnerDto.WalletBalance = Wallet.Balance;
                //    partnerDto.WalletId = Wallet.WalletId;
                //}
                //if (Country != null)
                //{
                //    partnerDto.CurrencySymbol = Country.CurrencySymbol;

                //}
            }
            return partnerDto;
        }

        public async Task<int> CountOfPartnersUnderFleet(string fleetCode)
        {
            var partners = await _uow.FleetPartner.FleetCount(fleetCode);
            return partners;
        }
        public async Task<List<VehicleTypeDTO>> GetVehiclesAttachedToFleetPartner(string fleetCode)
        {
            var vehicles = await _uow.FleetPartner.GetVehiclesAttachedToFleetPartner(fleetCode);
            return vehicles;
        }

        public async Task<List<PartnerTransactionsDTO>> GetFleetTransaction(ShipmentCollectionFilterCriteria filterCriteria)
        {
            //get the current login user 
            var currentUserId = await _userService.GetCurrentUserId();
            var currentUser = await _userService.GetUserById(currentUserId);
            var partners = new List<PartnerTransactionsDTO>();

            if (filterCriteria.IsDashboard)
            {
                partners = await _uow.PartnerTransactions.GetRecentFivePartnerTransactionsForFleet(currentUser.UserChannelCode);
            }
            else
            {
                partners = await _uow.PartnerTransactions.GetPartnerTransactionsForFleet(filterCriteria, currentUser.UserChannelCode);
            }

            return await Task.FromResult(partners);
        }

    }
}
