using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Shipments;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIGL.GIGLS.Core.Domain;
using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.DTO.Shipments;
using GIGLS.CORE.DTO.Shipments;
using System.Linq.Expressions;

namespace GIGLS.Infrastructure.Persistence.Repositories.Shipments
{
    public class PreShipmentMobileRepository : Repository<PreShipmentMobile, GIGLSContext>, IPreShipmentMobileRepository
    {
        private GIGLSContext _context;
        public PreShipmentMobileRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }
    }
}
