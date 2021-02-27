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
using GIGLS.Core.DTO.Report;
using GIGLS.Core.DTO.Customers;
using GIGLS.Core.IServices.Customers;
using System.Net;

namespace GIGLS.Services.Implementation.Partnership
{
    public class PartnerService : IPartnerService
    {
        private readonly IUnitOfWork _uow;
        private readonly IWalletService _walletService;
        private readonly INumberGeneratorMonitorService _numberGeneratorMonitorService;
        private readonly ICompanyService _companyService;

        public PartnerService(IUnitOfWork uow, IWalletService walletService, 
            INumberGeneratorMonitorService numberGeneratorMonitorService, ICompanyService companyService)
        {
            _uow = uow;
            _walletService = walletService;
            _numberGeneratorMonitorService = numberGeneratorMonitorService;
            _companyService = companyService;
            MapperConfig.Initialize();
        }

        public async Task<IEnumerable<PartnerDTO>> GetPartners()
        {
            var partners = await _uow.Partner.GetPartnersAsync();
            return partners;
        }


        public async Task<IEnumerable<VehicleTypeDTO>> GetVerifiedPartners(string fleetCode)
        {
            var partners = await _uow.Partner.GetPartnersAsync(fleetCode, true);
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
            var partner = await _uow.Partner.GetAsync(partnerId);

            if (partner == null)
            {
                throw new GenericException("PARTNER_NOT_EXIST");
            }
            else
            {
                PartnerDTO partnerDto = Mapper.Map<PartnerDTO>(partner);
                var wallet = await _uow.Wallet.GetAsync(s => s.CustomerCode == partner.PartnerCode);
                if (wallet != null)
                {
                    partnerDto.WalletBalance = wallet.Balance;
                    partnerDto.WalletId = wallet.WalletId;
                }

                var country = await _uow.Country.GetAsync(s => s.CountryId == partner.UserActiveCountryId);
                if (country != null)
                {
                    partnerDto.CurrencySymbol = country.CurrencySymbol;

                }
                return partnerDto;
            }
        }

        public async Task<PartnerDTO> GetPartnerByCode(string partnerCode)
        {
            var partnerDto = new PartnerDTO();
            var partner = await _uow.Partner.GetAsync(x => x.PartnerCode == partnerCode);

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

        public async Task<List<ExternalPartnerTransactionsPaymentDTO>> GetExternalPartnerTransactionsForPayment(ShipmentCollectionFilterCriteria shipmentCollectionFilterCriteria)
        {
            var partnersTransaction = await _uow.PartnerTransactions.GetExternalPartnerTransactionsForPayment(shipmentCollectionFilterCriteria);

            return partnersTransaction;
        }

        public async Task<IEnumerable<VehicleTypeDTO>> GetUnVerifiedPartners(ShipmentCollectionFilterCriteria filterCriteria)
        {
            string fleetCode = null;

            var partners = await _uow.Partner.GetPartnersAsync(fleetCode, false, filterCriteria);
            return partners;
        }

        //Send a mail to a partner that has not yet been verified
        public async Task ContactUnverifiedPartner(string email)
        {
            var partner = await _uow.Partner.GetAsync(x => x.Email == email);

            if(partner == null)
            {
                throw new GenericException($"Partner with email {email} does not exist", $"{(int)HttpStatusCode.NotFound}");
            }

            //Send Mail
            var messageDTO = new CompanyMessagingDTO
            {
                Email = partner.Email,
                Name = partner.PartnerName,
                UserChannelType = UserChannelType.Partner,
                PhoneNumber = partner.PhoneNumber
            };

            await _companyService.SendMessageToNewSignUps(messageDTO);
            partner.Contacted = true;

            await _uow.CompleteAsync();

        }
        public async Task<IEnumerable<VehicleTypeDTO>> GetVerifiedByRangePartners(ShipmentCollectionFilterCriteria filterCriteria)
        {
            string fleetCode = null;
            List<VehicleTypeDTO> partners = new List<VehicleTypeDTO>();

           if (filterCriteria.StartDate != null)
            {
                partners = await _uow.Partner.GetPartnersAsync(fleetCode, true, filterCriteria); 
            }
            else
            {
                partners = await _uow.Partner.GetPartnersAsync(fleetCode, true);
            }
            partners = partners.Where(x => x.IsVerified == true).ToList();
            if (partners.Any())
            {
                var today = DateTime.Now;
                foreach (var item in partners)
                {
                   var months = 12 * (today.Year - item.ActivityDate.Year) + today.Month - item.ActivityDate.Month;
                  var monthsApart = Math.Abs(months);
                    if (monthsApart >= 2)
                    {
                        item.Active = false;
                    }
                }

            }
            return partners.OrderByDescending(x => x.DateModified);
        }

        public async Task DeactivatePartner(int partnerId)
        {
            var existingPartner = await _uow.Partner.GetAsync(partnerId);

            if (existingPartner == null)
            {
                throw new GenericException("PARTNER_NOT_EXIST");
            }
            existingPartner.IsActivated = false;
            await _uow.CompleteAsync();
        }
    }
}
