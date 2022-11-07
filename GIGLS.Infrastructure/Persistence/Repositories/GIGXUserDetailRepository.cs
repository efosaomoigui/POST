using POST.Core.Domain;
using POST.Core.DTO.User;
using POST.Core.IRepositories;
using POST.Core.IRepositories.BankSettlement;
using POST.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POST.Infrastructure.Persistence.Repositories.BankSettlement
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
                if (gigxusersDTO.Count > 0 && !String.IsNullOrEmpty(gigxusersDTO.FirstOrDefault().CustomerPin))
                {
                    gigxusersDTO.FirstOrDefault().HasPin = true;
                }
                return Task.FromResult(gigxusersDTO.FirstOrDefault());
            }
            catch (Exception)
            {
                throw;
            }
        }


        public Task<GIGXUserDetail> GetMGIGXUserDetailByCode(string customerCode)
        {
            try
            {
                var gigxusers = _context.GIGXUserDetail.AsQueryable().Where(x => x.CustomerCode == customerCode);

                List<GIGXUserDetail> gigxusersDTO = new List<GIGXUserDetail>();

                gigxusersDTO = (from r in gigxusers
                                select new GIGXUserDetail()
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

        public Task<GIGXUserDetailDTO> GetGIGXUserDetailByCodeNew(string customerCode)
        {
            try
            {
                GIGXUserDetailDTO result = new GIGXUserDetailDTO();
                var gigxusers = _context.GIGXUserDetail.AsQueryable().Where(x => x.CustomerCode == customerCode);
                List<GIGXUserDetailDTO> gigxusersDTO = new List<GIGXUserDetailDTO>();

                gigxusersDTO = (from r in gigxusers
                                select new GIGXUserDetailDTO()
                                {
                                    WalletAddress = r.WalletAddress,
                                    GIGXEmail = r.GIGXEmail,
                                    DateCreated = r.DateCreated,
                                    CustomerPin = r.CustomerPin
                                }).OrderByDescending(x => x.DateCreated).ToList();
                if (gigxusersDTO.Count > 0 &&!String.IsNullOrEmpty(gigxusersDTO.FirstOrDefault().CustomerPin))
                {
                    gigxusersDTO.FirstOrDefault().HasPin = true;
                    gigxusersDTO.FirstOrDefault().CustomerPin = null;
                    result = gigxusersDTO.FirstOrDefault();
                }
                return Task.FromResult(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
