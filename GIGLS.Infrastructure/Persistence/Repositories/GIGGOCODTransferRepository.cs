using AutoMapper;
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
    public class GIGGOCODTransferRepository : Repository<GIGGOCODTransfer, GIGLSContext>, IGIGGOCODTransferRepository
    {
        private readonly GIGLSContext _context;
        public GIGGOCODTransferRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public Task<GIGGOCODTransferResponseDTO> GetCODTransfer(string waybill)
        {
            var codTransfer =  _context.GIGGOCODTransfer.Where(x => x.Waybill == waybill).FirstOrDefault();

            var result = Mapper.Map<GIGGOCODTransferResponseDTO>(codTransfer);

            return Task.FromResult(result);
        }
    }
}
