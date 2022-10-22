using AutoMapper;
using POST.Core;
using POST.Core.Domain;
using POST.Core.DTO.Account;
using POST.Core.IServices.Account;
using POST.Core.IServices.User;
using POST.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Services.Implementation.Account
{
    public class VATService : IVATService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;

        public VATService(IUnitOfWork uow, IUserService userService)
        {
            _uow = uow;
            _userService = userService;
            MapperConfig.Initialize();
        }

        public async Task<IEnumerable<VATDTO>> GetVATs()
        {
            var invoices = await _uow.VAT.GetVATsAsync();
            return invoices;
        }

        public async Task<VATDTO> GetVATById(int vatId)
        {
            var vat = await _uow.VAT.GetVATById(vatId);

            if (vat == null)
            {
                throw new GenericException("VAT does not exists");
            }
            
            return vat;
        }

        public async Task<object> AddVAT(VATDTO vatDto)
        {
            var newVAT = Mapper.Map<VAT>(vatDto);
            _uow.VAT.Add(newVAT);
            await _uow.CompleteAsync();
            return new { id = newVAT.VATId };
        }

        public async Task UpdateVAT(int vatId, VATDTO vatDto)
        {
            var vat = await _uow.VAT.GetAsync(vatId);

            if (vat == null)
            {
                throw new GenericException("VAT does not exists");
            }

            vat.VATId = vatId;
            vat.Name = vatDto.Name;
            vat.Value = vatDto.Value;
            vat.Type = vatDto.Type;
            vat.CountryId = vatDto.CountryId;

            await _uow.CompleteAsync();
        }

        public async Task RemoveVAT(int vatId)
        {
            var vat = await _uow.VAT.GetAsync(vatId);

            if (vat == null)
            {
                throw new GenericException("VAT does not exists");
            }
            _uow.VAT.Remove(vat);
            await _uow.CompleteAsync();
        }

        public async Task<VATDTO> GetVATByCountry()
        {
            var countryIds = await _userService.GetUserActiveCountryId();
            var vat = await _uow.VAT.GetVATByCountry(countryIds);
            return vat;
        }
    }
}