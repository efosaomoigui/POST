using POST.Core.IServices.Utility;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using POST.Core.DTO.Utility;
using POST.Core;
using POST.Infrastructure;
using POST.Core.Domain.Utility;
using AutoMapper;
using POST.Core.Enums;
using POST.Core.DTO;
using System.Linq;
using System.Net;
using System.Text;
using System.Security.Cryptography;

namespace POST.Services.Implementation.Utility
{
    public class GlobalPropertyService : IGlobalPropertyService
    {
        private readonly IUnitOfWork _uow;

        public GlobalPropertyService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<object> AddGlobalProperty(GlobalPropertyDTO globalProperty)
        {
            try
            {
                if (await _uow.GlobalProperty.ExistAsync(c => c.Key == globalProperty.Key && c.CountryId == globalProperty.CountryId))
                {
                    throw new GenericException("Global Property Information Already Exist", $"{(int)HttpStatusCode.Forbidden}");
                }

                var newGlobal = new GlobalProperty
                {
                    Key = globalProperty.Key,
                    Value = globalProperty.Value,
                    Description = globalProperty.Description,
                    CountryId = globalProperty.CountryId,
                    IsActive = true
                };

                _uow.GlobalProperty.Add(newGlobal);
                await _uow.CompleteAsync();
                return new { id = newGlobal.GlobalPropertyId };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<IEnumerable<GlobalPropertyDTO>> GetGlobalProperties()
        {
            var globalProperties = Mapper.Map<IEnumerable<GlobalPropertyDTO>>(_uow.GlobalProperty.GetAll());

            var countries = _uow.Country.GetAll();
            var countriesDTO = Mapper.Map<IEnumerable<CountryDTO>>(countries);

            foreach (var globalProp in globalProperties)
            {
                var country = countriesDTO.FirstOrDefault(s => s.CountryId == globalProp.CountryId);
                globalProp.Country = country;
            }

            return Task.FromResult(globalProperties);
        }

        public async Task<GlobalPropertyDTO> GetGlobalPropertyById(int globalPropertyId)
        {
            try
            {
                var global = await _uow.GlobalProperty.GetAsync(globalPropertyId);
                if (global == null)
                {
                    throw new GenericException("Global Property Not Found", $"{(int)HttpStatusCode.NotFound}");
                }

                var globalDTo = Mapper.Map<GlobalPropertyDTO>(global);

                var country = await _uow.Country.GetAsync(globalDTo.CountryId);
                var countryDTO = Mapper.Map<CountryDTO>(country);

                globalDTo.Country = countryDTO;

                return globalDTo;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task RemoveGlobalProperty(int globalPropertyId)
        {
            try
            {
                var global = await _uow.GlobalProperty.GetAsync(globalPropertyId);
                if (global == null)
                {
                    throw new GenericException("Global Property Not Found", $"{(int)HttpStatusCode.NotFound}");
                }
                _uow.GlobalProperty.Remove(global);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateGlobalProperty(int globalPropertyId, GlobalPropertyDTO globalProperty)
        {
            try
            {
                var global = await _uow.GlobalProperty.GetAsync(globalPropertyId);
                if (global == null || globalProperty.GlobalPropertyId != globalPropertyId)
                {
                    throw new GenericException("Global Property Not Found", $"{(int)HttpStatusCode.NotFound}");
                }

                global.Key = globalProperty.Key;
                global.Value = globalProperty.Value;
                global.Description = globalProperty.Description;
                global.IsActive = global.IsActive;
                global.CountryId = globalProperty.CountryId;
                _uow.Complete();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateGlobalProperty(int globalPropertyId, bool status)
        {
            try
            {
                var global = await _uow.GlobalProperty.GetAsync(globalPropertyId);
                if (global == null)
                {
                    throw new GenericException("Global Property Not Found", $"{(int)HttpStatusCode.NotFound}");
                }

                global.IsActive = status;
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GlobalPropertyDTO> GetGlobalProperty(GlobalPropertyType globalPropertyType, int countryId)
        {
            try
            {
                var global = _uow.GlobalProperty.SingleOrDefault(s => s.Key == globalPropertyType.ToString() && s.CountryId == countryId);
                if (global == null)
                {
                    throw new GenericException($"Global Property '{globalPropertyType}' does not exist", $"{(int)HttpStatusCode.NotFound}");
                }
                var globalDTo = Mapper.Map<GlobalPropertyDTO>(global);
                return await Task.FromResult(globalDTo);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<decimal> GetDropOffDiscountInGlobalProperty(int countryId)
        {
            try
            {
                var discountPercent = await GetGlobalProperty(GlobalPropertyType.GIGGODropOffDiscount, countryId);
                decimal discount = Convert.ToDecimal(discountPercent.Value);
                return discount;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> GenerateDeliveryCode()
        {
            try
            {
                int maxSize = 6;
                char[] chars = new char[54];
                string a;
                a = "abcdefghjkmnpqrstuvwxyzABCDEFGHJKMNPQRSTUVWXYZ23456789";
                chars = a.ToCharArray();
                int size = maxSize;
                byte[] data = new byte[1];
                RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
                crypto.GetNonZeroBytes(data);
                size = maxSize;
                data = new byte[size];
                crypto.GetNonZeroBytes(data);
                StringBuilder result = new StringBuilder(size);
                foreach (byte b in data)
                { result.Append(chars[b % (chars.Length - 1)]); }
                var strippedText = result.ToString();
                var number = "DN" + strippedText.ToUpper();
                return number;
            }
            catch
            {
                throw;
            }
        }

        public async Task<decimal> GetGoFasterPercentageInGlobalProperty(int countryId)
        {
            try
            {
                var discountPercent = await GetGlobalProperty(GlobalPropertyType.GoFaster, countryId);
                decimal discount = Convert.ToDecimal(discountPercent.Value);
                return discount;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
