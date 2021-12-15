using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.User;
using GIGLS.Core.IRepositories;
using GIGLS.Core.IRepositories.BankSettlement;
using GIGLS.CORE.DTO.Shipments;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Infrastructure.Persistence.Repositories.Shipments
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
