using GIGLS.Core.Domain.Partnership;
using GIGLS.Core.IRepositories.Partnership;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Partnership;
using System.Data.Entity;

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
                                };

            return Task.FromResult(partnerDto.ToList());
        }
    }
}
