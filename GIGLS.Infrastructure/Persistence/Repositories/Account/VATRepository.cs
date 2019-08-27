using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.IRepositories.Account;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GIGLS.Core.DTO;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Account
{
    public class VATRepository : Repository<VAT, GIGLSContext>, IVATRepository
    {
        private GIGLSContext _context;

        public VATRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<VATDTO>> GetVATsAsync()
        {
            var vats = _context.VAT;
            var vatDto = from v in vats
                         join c in _context.Country on v.CountryId equals c.CountryId
                         select new VATDTO
                         {
                             VATId = v.VATId,
                             Value = v.Value,
                             Type = v.Type,
                             Name = v.Name,
                             CountryId = v.CountryId,
                             Country = new CountryDTO
                             {
                                 CountryId = c.CountryId,
                                 CountryName = c.CountryName,
                                 CurrencySymbol = c.CurrencySymbol
                             }
                         };

            return Task.FromResult(vatDto.ToList());
        }
        public Task<VATDTO> GetVATById(int vatId)
        {
            var vats = _context.VAT.Where(x => x.VATId == vatId);
            var vatDto = from v in vats
                         join c in _context.Country on v.CountryId equals c.CountryId
                         select new VATDTO
                         {
                             VATId = v.VATId,
                             Value = v.Value,
                             Type = v.Type,
                             Name = v.Name,
                             CountryId = v.CountryId,
                             Country = new CountryDTO
                             {
                                 CountryId = c.CountryId,
                                 CountryName = c.CountryName,
                                 CurrencySymbol = c.CurrencySymbol
                             }
                         };

            return Task.FromResult(vatDto.FirstOrDefault());
        }

        public Task<VATDTO> GetVATByCountry(int countryId)
        {
            var vats = _context.VAT.Where(x => x.CountryId == countryId);
            var vatDto = from v in vats
                         join c in _context.Country on v.CountryId equals c.CountryId
                         select new VATDTO
                         {
                             VATId = v.VATId,
                             Value = v.Value,
                             Type = v.Type,
                             Name = v.Name,
                             CountryId = v.CountryId,
                             Country = new CountryDTO
                             {
                                 CountryId = c.CountryId,
                                 CountryName = c.CountryName,
                                 CurrencySymbol = c.CurrencySymbol
                             }
                         };

            return Task.FromResult(vatDto.FirstOrDefault());
        }
    }
}