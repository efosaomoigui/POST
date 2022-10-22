using POST.Core.Domain;
using POST.Core.DTO;
using POST.Core.DTO.User;
using POST.Core.IRepositories;
using POST.Core.IRepositories.BankSettlement;
using POST.CORE.DTO.Shipments;
using POST.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POST.Infrastructure.Persistence.Repositories.Shipments
{
    public class UnidentifiedItemsForInternationalShippingRepository : Repository<UnidentifiedItemsForInternationalShipping, GIGLSContext>, IUnidentifiedItemsForInternationalShippingRepository
    {
        private GIGLSContext _context;
        public UnidentifiedItemsForInternationalShippingRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }
    }
}
