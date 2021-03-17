﻿using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.DTO.Stores;
using GIGLS.Core.IRepositories.Stores;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using GIGLS.Core.DTO;


namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Stores
{
    public class StoreRepository : Repository<Store, GIGLSContext>, IStoreRepository
    {
        private GIGLSContext _context;

        public StoreRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<StoreDTO>> GetStoresByCountryId(int countryId)
        {
            try
            {
                var stores = _context.Store.Where(s => s.CountryId == countryId);

                var storesDTO = from s in stores
                                select new StoreDTO
                                {
                                    StoreName = s.StoreName,
                                    URL = s.URL,
                                    Address = s.Address,
                                    City = s.City,
                                    storeImage = s.storeImage
                                };
                return Task.FromResult(storesDTO.OrderBy(x => x.StoreName).ToList());

            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<StoreDTO>> GetStores()
        {
            try
            {
                var stores = _context.Store;

                var storesDTO = from s in stores
                                join c in Context.Country on s.CountryId equals c.CountryId
                                select new StoreDTO
                                {
                                    StoreName = s.StoreName,
                                    StoreId = s.StoreId,
                                    URL = s.URL,
                                    Address = s.Address,
                                    City = s.City,
                                    storeImage = s.storeImage,
                                    CountryName = c.CountryName,
                                    CountryId = c.CountryId,
                                };
                return Task.FromResult(storesDTO.OrderBy(x => x.StoreName).ToList());

            }
            catch (Exception)
            {
                throw;
            }
        }

        public  Task <StoreDTO> GetStoreById(int storeId)
        {
            try
            {
                var stores = _context.Store;

                var storesDTO = from s in stores
                                join c in Context.Country on s.CountryId equals c.CountryId
                                where s.StoreId == storeId
                                select new StoreDTO
                                {
                                    StoreName = s.StoreName,
                                    StoreId = s.StoreId,
                                    URL = s.URL,
                                    Address = s.Address,
                                    City = s.City,
                                    storeImage = s.storeImage,
                                    CountryName = c.CountryName,
                                    CountryId = c.CountryId,
                                };
                return Task.FromResult(storesDTO.FirstOrDefault());

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
