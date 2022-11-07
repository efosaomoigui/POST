using AutoMapper;
using POST.Core.Domain;
using POST.Core.DTO;
using POST.Core.IRepositories;
using POST.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Infrastructure.Persistence.Repositories
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
