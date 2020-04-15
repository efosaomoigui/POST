using GIGLS.Core.Domain.Partnership;
using GIGLS.Core.IRepositories.Partnership;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Partnership;
using System.Data.Entity;
using GIGLS.Core.DTO;
using System;
using GIGLS.Core.Enums;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Partnership
{
    public class PartnerRepository : Repository<Partner, GIGLSContext>, IPartnerRepository
    {
        private GIGLSContext _context;
        public PartnerRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public Task<Partner> GetLastValidPartnerCode()
        {
            var partnercode = from Partner in Context.Partners
                                orderby Partner.PartnerCode descending
                                    select Partner;
            return partnercode.FirstOrDefaultAsync();
        }

        public Task<List<PartnerDTO>> GetPartnersAsync()
        {
            var partners = _context.Partners;
            
            var partnerDto = from partner in partners
                                select new PartnerDTO
                                {
                                    PartnerId = partner.PartnerId,
                                    PartnerName = partner.PartnerName,
                                    Email = partner.Email,
                                    Address = partner.Address,
                                    PartnerCode = partner.PartnerCode,
                                    PhoneNumber = partner.PhoneNumber,
                                    OptionalPhoneNumber = partner.OptionalPhoneNumber,
                                    PartnerType = partner.PartnerType,
                                    FirstName = partner.FirstName,
                                    LastName = partner.LastName,
                                    IdentificationNumber = "",
                                    WalletPan = "",
                                    IsActivated = partner.IsActivated,
                                    ActivityStatus = partner.ActivityStatus
                                };

            return Task.FromResult(partnerDto.ToList());
        }

        //Remove this old one after review
        public Task<List<VehicleTypeDTO>> GetVerifiedPartnersAsync()
        {
            var partners = _context.Partners.AsQueryable().Where(x => x.IsActivated == true);

            var partnerDto = from partner in partners
                             join vehicle in _context.VehicleType on partner.PartnerCode equals vehicle.Partnercode
                             select new VehicleTypeDTO
                             {
                                 PartnerName = partner.PartnerName,
                                 Vehicletype = vehicle.Vehicletype,
                                 Partnercode = vehicle.Partnercode,
                                 PartnerFirstName = partner.FirstName,
                                 PartnerLastName = partner.LastName,
                                 PartnerPhoneNumber = partner.PhoneNumber,
                                 PartnerType = partner.PartnerType,
                                 EnterprisePartner = _context.FleetPartner.Where(s => s.FleetPartnerCode == partner.FleetPartnerCode)
                                 .Select(x => new FleetPartnerDTO
                                 {
                                     FirstName = x.FirstName,
                                     LastName = x.LastName
                                 }).FirstOrDefault(),
                                 ActivityStatus = partner.ActivityStatus
                             };
            return Task.FromResult(partnerDto.ToList());
        }

        public Task<List<VehicleTypeDTO>> GetVerifiedPartnersAsync(string fleetCode)
        {
            var partners = _context.Partners.AsQueryable().Where(x => x.IsActivated == true);

            if (fleetCode != null)
            {
                partners = partners.Where(x => x.FleetPartnerCode == fleetCode);
            }            

            var partnerDto = from partner in partners
                             join vehicle in _context.VehicleType on partner.PartnerCode equals vehicle.Partnercode
                             select new VehicleTypeDTO
                             {
                                 PartnerName = partner.PartnerName,
                                 Vehicletype = vehicle.Vehicletype,
                                 Partnercode = vehicle.Partnercode,
                                 PartnerFirstName = partner.FirstName,
                                 PartnerLastName = partner.LastName,
                                 PartnerPhoneNumber = partner.PhoneNumber,
                                 PartnerType = partner.PartnerType,
                                 ActivityStatus = partner.ActivityStatus,
                                 ActivityDate = partner.ActivityDate,
                                 EnterprisePartner = _context.FleetPartner.Where(s => s.FleetPartnerCode == partner.FleetPartnerCode)
                                 .Select(x => new FleetPartnerDTO
                                 {
                                     FirstName = x.FirstName,
                                     LastName = x.LastName
                                 }).FirstOrDefault(),
                             };
            return Task.FromResult(partnerDto.ToList());
        }

        public Task<PartnerDTO> GetPartnerByIdWithCountry(int customerId)
        {
            try
            {
                var partners = _context.Partners.Where(x => x.PartnerId == customerId);
                var partnersDto = from partner in partners
                                    select new PartnerDTO
                                    {
                                        PartnerId = partner.PartnerId,
                                        PartnerName = partner.PartnerName,
                                        Email = partner.Email,
                                        Address = partner.Address,
                                        PartnerCode = partner.PartnerCode,
                                        PhoneNumber = partner.PhoneNumber,
                                        OptionalPhoneNumber = partner.OptionalPhoneNumber,
                                        PartnerType = partner.PartnerType,
                                        FirstName = partner.FirstName,
                                        LastName = partner.LastName,
                                        IdentificationNumber = "",
                                        WalletPan = "",
                                        UserActiveCountryId = partner.UserActiveCountryId,
                                        Country = _context.Country.Where(x => x.CountryId == partner.UserActiveCountryId).Select(x => new CountryDTO
                                        {
                                            CountryId = x.CountryId,
                                            CountryName = x.CountryName,
                                            CurrencySymbol = x.CurrencySymbol,
                                            CurrencyCode = x.CurrencyCode,
                                            PhoneNumberCode = x.PhoneNumberCode
                                        }).FirstOrDefault(),
                                        ActivityStatus = partner.ActivityStatus
                                    };
                return Task.FromResult(partnersDto.FirstOrDefault());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<PartnerDTO>> GetExternalPartnersAsync()
        {
            var partners = _context.Partners.Where(s=>s.PartnerType == Core.Enums.PartnerType.DeliveryPartner);

            var partnerDto = from partner in partners
                             join wallet in _context.Wallets on partner.PartnerCode equals wallet.CustomerCode
                             join country in _context.Country on partner.UserActiveCountryId equals country.CountryId
                             select new PartnerDTO
                             {
                                 PartnerId = partner.PartnerId,
                                 PartnerName = partner.PartnerName,
                                 Email = partner.Email,
                                 Address = partner.Address,
                                 PartnerCode = partner.PartnerCode,
                                 PhoneNumber = partner.PhoneNumber,
                                 OptionalPhoneNumber = partner.OptionalPhoneNumber,
                                 PartnerType = partner.PartnerType,
                                 FirstName = partner.FirstName,
                                 LastName = partner.LastName,
                                 IsActivated = partner.IsActivated,
                                 WalletBalance = wallet.Balance,
                                 CurrencySymbol = country.CurrencySymbol,
                                 ActivityStatus = partner.ActivityStatus
                             };

            return Task.FromResult(partnerDto.ToList());
        }

        public Task<List<PartnerDTO>> GetPartnerBySearchParameters(string parameter)
        {
            try
            {
                if (parameter != null)
                {
                    parameter = parameter.ToLower();
                }
                var partners = _context.Partners.Where(x => x.Email.ToLower() == parameter || x.PartnerCode.ToLower() == parameter 
                    || x.PartnerName.Contains(parameter) || x.PhoneNumber.Contains(parameter) || x.FirstName.Contains(parameter) || x.LastName.Contains(parameter));

                var partnersDto = from partner in partners
                                  select new PartnerDTO
                                  {
                                      PartnerId = partner.PartnerId,
                                      PartnerName = partner.PartnerName,
                                      Email = partner.Email,
                                      Address = partner.Address,
                                      PartnerCode = partner.PartnerCode,
                                      PhoneNumber = partner.PhoneNumber,
                                      OptionalPhoneNumber = partner.OptionalPhoneNumber,
                                      PartnerType = partner.PartnerType,
                                      FirstName = partner.FirstName,
                                      LastName = partner.LastName,
                                      IdentificationNumber = "",
                                      WalletPan = "",
                                      UserActiveCountryId = partner.UserActiveCountryId,
                                      Country = _context.Country.Where(x => x.CountryId == partner.UserActiveCountryId).Select(x => new CountryDTO
                                      {
                                          CountryId = x.CountryId,
                                          CountryName = x.CountryName,
                                          CurrencySymbol = x.CurrencySymbol,
                                          CurrencyCode = x.CurrencyCode,
                                          PhoneNumberCode = x.PhoneNumberCode
                                      }).FirstOrDefault()
                                  };
                return Task.FromResult(partnersDto.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
