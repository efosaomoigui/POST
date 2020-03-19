using GIGLS.Core.IServices.Partnership;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Partnership;
using GIGLS.Core;
using GIGLS.Core.IServices.Wallet;
using System.Collections.Generic;
using AutoMapper;
using GIGLS.Core.Domain.Partnership;
using GIGLS.Infrastructure;
using GIGLS.Core.IServices.Utility;
using GIGLS.Core.Enums;
using GIGLS.CORE.DTO.Report;
using System;
using System.Linq;

namespace GIGLS.Services.Implementation.Partnership
{
    public class PartnerService : IPartnerService
    {
        private readonly IUnitOfWork _uow;
        private readonly IWalletService _walletService;
        private readonly INumberGeneratorMonitorService _numberGeneratorMonitorService;

        public PartnerService(IUnitOfWork uow, IWalletService walletService, 
            INumberGeneratorMonitorService numberGeneratorMonitorService)
        {
            _uow = uow;
            _walletService = walletService;
            _numberGeneratorMonitorService = numberGeneratorMonitorService;
            MapperConfig.Initialize();
        }

        public async Task<IEnumerable<PartnerDTO>> GetPartners()
        {
            var partners = await _uow.Partner.GetPartnersAsync();
            return partners;
        }

        public async Task<IEnumerable<VehicleTypeDTO>> GetVerifiedPartners()
        {
            var partners = await _uow.Partner.GetVerifiedPartnersAsync();
            return partners;
        }

        public async Task<List<PartnerDTO>> GetPartnersByDate(BaseFilterCriteria filterCriteria)
        {
            var queryDate = filterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;
            var allpartners = _uow.Partner.GetAllAsQueryable();

            if (filterCriteria.StartDate == null & filterCriteria.EndDate == null)
            {
                startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1);
            }
            var allPartnersResult = allpartners.Where(s => s.DateCreated >= startDate && s.DateCreated < endDate);

            List<PartnerDTO> partnerDTO = (from r in allPartnersResult
                                           select new PartnerDTO()
                                           {
                                               PartnerId = r.PartnerId,
                                               PartnerName = r.PartnerName,
                                               PartnerCode = r.PartnerCode,
                                               PartnerType = r.PartnerType
                                           }).ToList();
            return await Task.FromResult(partnerDTO.OrderByDescending(x => x.DateCreated).ToList());
        }



        public async Task<PartnerDTO> GetPartnerById(int partnerId)
        {
            var partnerDto = new PartnerDTO();
            var partner = await _uow.Partner.GetAsync(partnerId);

            if (partner == null)
            {
                throw new GenericException("PARTNER_NOT_EXIST");
            }
            else
            {
                partnerDto = Mapper.Map<PartnerDTO>(partner);
                var Wallet = await _uow.Wallet.GetAsync(s => s.CustomerCode == partner.PartnerCode);
                var Country = await _uow.Country.GetAsync(s => s.CountryId == partner.UserActiveCountryId);
                if (Wallet != null)
                {
                    partnerDto.WalletBalance = Wallet.Balance;
                    partnerDto.WalletId = Wallet.WalletId;
                }
                if (Country != null)
                {
                    partnerDto.CurrencySymbol = Country.CurrencySymbol;
                   
                }
            }
            return partnerDto;
        }

        public async Task<object> AddPartner(PartnerDTO partnerDto)
        {            
            partnerDto.PartnerName = partnerDto.PartnerName.Trim();

            if (await _uow.Partner.ExistAsync(v => v.PartnerName.ToLower() == partnerDto.PartnerName.ToLower()))
            {
                throw new GenericException("PARTNER_EXISTS");
            }

            var partnerCode = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.Partner);

            var partner = Mapper.Map<Partner>(partnerDto);
            partner.PartnerCode = partnerCode;            
            _uow.Partner.Add(partner);
            await _uow.CompleteAsync();
            return new { id = partner.PartnerId };
        }
        
        public async Task UpdatePartner(int partnerId, PartnerDTO partner)
        {
            var existingParntner = await _uow.Partner.GetAsync(partnerId);

            if (existingParntner == null)
            {
                throw new GenericException("PARTNER_NOT_EXIST");
            }

            existingParntner.FirstName = partner.FirstName.Trim();
            existingParntner.LastName = partner.LastName.Trim();
            existingParntner.Email = partner.Email.Trim();
            existingParntner.PartnerName = partner.PartnerName.Trim();
            existingParntner.Address = partner.Address;
            existingParntner.OptionalPhoneNumber = partner.OptionalPhoneNumber;
            existingParntner.PartnerType = partner.PartnerType;
            existingParntner.PhoneNumber = partner.PhoneNumber;
            await _uow.CompleteAsync();
        }

        public async Task RemovePartner(int partnerId)
        {
            var existingPartner = await _uow.Partner.GetAsync(partnerId);

            if (existingPartner == null)
            {
                throw new GenericException("PARTNER_NOT_EXIST");
            }
            _uow.Partner.Remove(existingPartner);
            await _uow.CompleteAsync();
        }
        public async Task<IEnumerable<PartnerDTO>> GetExternalDeliveryPartners()
        {
            var partners = await _uow.Partner.GetExternalPartnersAsync();
            return partners;
        }

    }
}
