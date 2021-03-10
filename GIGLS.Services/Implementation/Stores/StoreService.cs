using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core;
using GIGLS.Core.DTO.Stores;
using GIGLS.Core.IServices.Stores;
using GIGLS.Infrastructure;
using GIGLS.Services.Implementation.Shipments;

namespace GIGLS.Services.Implementation.Stores
{
    public class StoreService : IStoreService
    {   
        private readonly IUnitOfWork _uow;
        public StoreService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }
   
        public async Task<object> AddStore(StoreDTO store)
        {
            try
            {
                if (await _uow.Store.ExistAsync(c => c.StoreName.ToLower() == store.StoreName.Trim().ToLower() && c.CountryId == store.CountryId))
                {
                    throw new GenericException("Store already exist");
                }

                //save image to blob
                using (WebClient webClient = new WebClient())
                {
                    byte[] data = webClient.DownloadData(store.storeImage);
                    var filename = $"{store.StoreName}.png";
                    var imgurl = await AzureBlobServiceUtil.UploadAsync(data, filename);
                    store.storeImage = imgurl;

                }
                var newstore = Mapper.Map<Store>(store);
               _uow.Store.Add(newstore);
                await _uow.CompleteAsync();
                return new { Id = newstore.StoreId };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task DeleteStore(int storeId)
        {
            try
            {
                var store = await _uow.Store.GetAsync(storeId);
                if (store == null)
                {
                    throw new GenericException("Store does not exist");
                }
                _uow.Store.Remove(store);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<StoreDTO> GetStoreById(int storeId)
        {
            try
            {
                var store = await _uow.Store.GetStoreById(storeId);
                if (store == null)
                {
                    throw new GenericException("Store Not Exist");
                }

                var storeDto = Mapper.Map<StoreDTO>(store);
                return storeDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<StoreDTO>> GetStores()
        {
            var stores = await _uow.Store.GetStores();
            return Mapper.Map<List<StoreDTO>>(stores);
        }

        public async Task UpdateStore(int storeId, StoreDTO store)
        {
            try
            {
                var storetoModify = await _uow.Store.GetAsync(storeId);
                if (storetoModify == null || storetoModify.StoreId != storeId)
                {
                    throw new GenericException("Store information does not exist");
                }
               storetoModify.StoreName = store.StoreName;
               storetoModify.Address = store.Address;
               storetoModify.City = store.City;
               storetoModify.State = store.State;
               storetoModify.URL = store.URL;
               storetoModify.CountryId = store.CountryId;
               storetoModify.storeImage = store.storeImage;
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
