using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using POST.Core.DTO.Wallet;
using POST.Core.IRepositories.Wallet;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;
using System.Linq;
using AutoMapper;
using System.Data.SqlClient;
using POST.Core.Enums;
using POST.Core.DTO;
using POST.Core.Domain.Wallet;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.Wallet
{
    public class CODGeneratedAccountNoRepository : Repository<Core.Domain.Wallet.CODGeneratedAccountNo, GIGLSContext>, ICODGeneratedAccountNoRepository
    {
        private GIGLSContext _context;

        public CODGeneratedAccountNoRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

    }
}
