using GIGLS.Core.IServices.Utility;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Utility;
using GIGLS.Core;
using GIGLS.Infrastructure;
using GIGLS.Core.Domain.Utility;
using AutoMapper;
using GIGLS.Core.Enums;
using GIGLS.Core.DTO;
using System.Linq;

namespace GIGLS.Services.Implementation.Utility
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
                if (await _uow.GlobalProperty.ExistAsync(c => c.Key == globalProperty.Key))
                {
                    throw new GenericException("Information already exist");
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
                    throw new GenericException("Information does not exist");
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
                    throw new GenericException("Information does not exist");
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
                    throw new GenericException("Information does not exist");
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
                    throw new GenericException("Information does not exist");
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
                var global = _uow.GlobalProperty.SingleOrDefault(s => s.Key == globalPropertyType.ToString()
                    && s.CountryId == countryId);
                var globalDTo = Mapper.Map<GlobalPropertyDTO>(global);
                return await Task.FromResult(globalDTo);
            }
            catch (Exception)
            {
                throw new GenericException($"Global Property '{globalPropertyType.ToString()}' does not exist");
            }
        }
    }
}
