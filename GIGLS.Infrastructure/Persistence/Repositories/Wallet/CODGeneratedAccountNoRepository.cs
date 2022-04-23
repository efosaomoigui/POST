using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.IRepositories.Wallet;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Linq;
using AutoMapper;
using System.Data.SqlClient;
using GIGLS.Core.Enums;
using GIGLS.Core.DTO;
using GIGLS.Core.Domain.Wallet;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Wallet
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
