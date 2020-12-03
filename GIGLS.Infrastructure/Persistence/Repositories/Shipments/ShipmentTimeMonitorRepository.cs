using AutoMapper;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Report;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.Enums;
using GIGLS.Core.IRepositories.Shipments;
using GIGLS.CORE.Domain;
using GIGLS.CORE.DTO.Shipments;
using GIGLS.CORE.IRepositories.Shipments;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Shipments
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
