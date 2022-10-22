﻿using POST.Core.Domain;
using POST.Core.IRepositories;
using POST.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Infrastructure.Persistence.Repositories
{
    public class DeliveryLocationRepository : Repository<DeliveryLocation, GIGLSContext>, IDeliveryLocationRepository
    {
        private GIGLSContext _context;
        public DeliveryLocationRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }
    }
}
