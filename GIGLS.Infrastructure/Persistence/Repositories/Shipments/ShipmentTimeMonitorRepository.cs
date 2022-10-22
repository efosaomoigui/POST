using AutoMapper;
using POST.Core.Domain;
using POST.Core.DTO.Report;
using POST.Core.DTO.ServiceCentres;
using POST.Core.DTO.Shipments;
using POST.Core.Enums;
using POST.Core.IRepositories.Shipments;
using POST.CORE.Domain;
using POST.CORE.DTO.Shipments;
using POST.CORE.IRepositories.Shipments;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.Shipments
{
    public class ShipmentTimeMonitorRepository : Repository<ShipmentTimeMonitor, GIGLSContext>, IShipmentTimeMonitorRepository
    {
        private GIGLSContext _context;
        public ShipmentTimeMonitorRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }
    }
}
