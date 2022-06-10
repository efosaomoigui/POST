using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using GIGLS.Core.IRepositories;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Infrastructure.Persistence.Repositories
{
    public class ShipmentCategory : Repository<InboundShipmentCategory, GIGLSContext>, IShipmentCategory
    {
        private GIGLSContext _context;
        public ShipmentCategory(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<InboundShipmentCategoryDTO>> GetInboundCategory(int countryId)
        {
            try
            {
                var category = _context.InboundShipmentCategory.Where(x => x.CountryId == countryId);

                List<InboundShipmentCategoryDTO> inboundShipmentCategoryDTO = (from c in category
                                                                               select
                                                                               new InboundShipmentCategoryDTO()
                                                                               {
                                                                                   InboundShipmentCategoryId = c.InboundShipmentCategoryId,
                                                                                   ShipmentCategoryName = _context.ShipmentCategory.Where(x => x.ShipmentCategoryId == c.ShipmentCategoryId).FirstOrDefault().ShipmentCategoryName,
                                                                                   CountryName = _context.Country.Where(x => x.CountryId == c.CountryId).FirstOrDefault().CountryName,
                                                                                   IsGoFaster = c.IsGoFaster,
                                                                                   IsGoStandard = c.IsGoStandard
                                                                               }).ToList();
                return Task.FromResult(inboundShipmentCategoryDTO.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
