using GIGLS.Core.Domain;
using GIGLS.Core.DTO.User;
using GIGLS.Core.IRepositories;
using GIGLS.Core.IRepositories.BankSettlement;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Infrastructure.Persistence.Repositories.BankSettlement
{
    public class GIGXUserDetailRepository : Repository<GIGXUserDetail, GIGLSContext>, IGIGXUserDetailRepository
    {
        private GIGLSContext _context;
        public GIGXUserDetailRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<GIGXUserDetailDTO>> GetGIGXUserDetails()
        {
            try
            {
                var gigxusers = _context.GIGXUserDetail.AsQueryable();

                List<GIGXUserDetailDTO> gigxusersDTO = new List<GIGXUserDetailDTO>();

                gigxusersDTO = (from r in gigxusers
                                select new GIGXUserDetailDTO()
                                  {
                                    GIGXUserDetailId = r.GIGXUserDetailId,
                                    CustomerCode = r.CustomerCode,
                                    CustomerPin = r.CustomerPin,
                                    PrivateKey = r.PrivateKey,
                                    PublicKey = r.PublicKey,
                                    DateCreated = r.DateCreated,
                                    WalletAddress = r.WalletAddress,
                                    GIGXEmail = r.GIGXEmail
                                  }).OrderByDescending(x => x.DateCreated).ToList();

                return Task.FromResult(gigxusersDTO.OrderByDescending(x => x.DateCreated).ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<GIGXUserDetailDTO> GetGIGXUserDetailByPin(string pin)
        {
            try
            {
                var gigxusers = _context.GIGXUserDetail.AsQueryable().Where(x => x.CustomerPin == pin);

                List<GIGXUserDetailDTO> gigxusersDTO = new List<GIGXUserDetailDTO>();

                gigxusersDTO = (from r in gigxusers
                                select new GIGXUserDetailDTO()
                                {
                                    GIGXUserDetailId = r.GIGXUserDetailId,
                                    CustomerCode = r.CustomerCode,
                                    CustomerPin = r.CustomerPin,
                                    PrivateKey = r.PrivateKey,
                                    PublicKey = r.PublicKey,
                                    DateCreated = r.DateCreated,
                                    WalletAddress = r.WalletAddress,
                                    GIGXEmail = r.GIGXEmail
                                }).OrderByDescending(x => x.DateCreated).ToList();

                return Task.FromResult(gigxusersDTO.FirstOrDefault());
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Task<GIGXUserDetailDTO> GetGIGXUserDetailByCode(string customerCode)
        {
            try
            {
                var gigxusers = _context.GIGXUserDetail.AsQueryable().Where(x => x.CustomerCode == customerCode);

                List<GIGXUserDetailDTO> gigxusersDTO = new List<GIGXUserDetailDTO>();

                gigxusersDTO = (from r in gigxusers
                                select new GIGXUserDetailDTO()
                                {
                                    GIGXUserDetailId = r.GIGXUserDetailId,
                                    CustomerCode = r.CustomerCode,
                                    CustomerPin = r.CustomerPin,
                                    PrivateKey = r.PrivateKey,
                                    PublicKey = r.PublicKey,
                                    DateCreated = r.DateCreated,
                                    WalletAddress = r.WalletAddress,
                                    GIGXEmail = r.GIGXEmail
                                }).OrderByDescending(x => x.DateCreated).ToList();

                return Task.FromResult(gigxusersDTO.FirstOrDefault());
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
