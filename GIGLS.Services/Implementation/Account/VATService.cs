using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.IServices.Account;
using GIGLS.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Account
{
    public class VATService : IVATService
    {
        private readonly IUnitOfWork _uow;

        public VATService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<IEnumerable<VATDTO>> GetVATs()
        {
            var invoices = await _uow.VAT.GetVATsAsync();
            return invoices;
        }

        public async Task<VATDTO> GetVATById(int vatId)
        {
            var vat = await _uow.VAT.GetAsync(vatId);

            if (vat == null)
            {
                throw new GenericException("VAT does not exists");
            }

            var vatDTO = Mapper.Map<VATDTO>(vat);

            return vatDTO;
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
    }
}
