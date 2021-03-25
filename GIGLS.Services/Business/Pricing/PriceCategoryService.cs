using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.PaymentTransactions;
using GIGLS.Core.DTO.Stores;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Stores;
using GIGLS.Infrastructure;

namespace GIGLS.Services.Business.Pricing
{
    public class PriceCategoryService : IPriceCategoryService
    {
        private readonly IUnitOfWork _uow;
        public PriceCategoryService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }
        
        public async Task<object> AddPriceCategory(PriceCategoryDTO priceCategory)
        {
            if (await _uow.PriceCategory.ExistAsync(c => c.PriceCategoryName.ToLower() == priceCategory.PriceCategoryName.Trim().ToLower() && c.CountryId == priceCategory.CountryId))
            {
                throw new GenericException("Price category already exist");
            }   
            var newcategory = Mapper.Map<PriceCategory>(priceCategory);
            _uow.PriceCategory.Add(newcategory);
            await _uow.CompleteAsync();
            return new { Id = newcategory.PriceCategoryId };
        }

        public async Task DeletePriceCategory(int priceCategoryId)
        {
            try
            {
                var category = await _uow.PriceCategory.GetAsync(priceCategoryId);
                if (category == null)
                {
                    throw new GenericException("Price category does not exist");
                }
                _uow.PriceCategory.Remove(category);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PriceCategoryDTO> GetPriceCategoryById(int priceCategoryId)
        {
            try
            {
                var category = await _uow.PriceCategory.GetPriceCategoryById(priceCategoryId);
                if (category == null)
                {
                    throw new GenericException("Price category does Not Exist");
                }

                var categoryDto = Mapper.Map<PriceCategoryDTO>(category);
                return categoryDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<PriceCategoryDTO>> GetPriceCategorys()
        {
            var categories = await _uow.PriceCategory.GetPriceCategories();
            return Mapper.Map<List<PriceCategoryDTO>>(categories);
        }

        public async Task UpdatePriceCategory(int priceCategoryId, PriceCategoryDTO priceCategory)
        {
            try
            {
                var cattoModify = await _uow.PriceCategory.GetAsync(priceCategoryId);
                if (cattoModify == null || cattoModify.PriceCategoryId != priceCategoryId)
                {
                    throw new GenericException("Price category information does not exist");
                }
                cattoModify.PriceCategoryName = priceCategory.PriceCategoryName;
                cattoModify.CategoryMinimumPrice = priceCategory.CategoryMinimumPrice;
                cattoModify.CategoryMinimumWeight = priceCategory.CategoryMinimumWeight;
                cattoModify.PricePerWeight = priceCategory.PricePerWeight;
                cattoModify.CountryId = priceCategory.CountryId;
                cattoModify.IsActive = priceCategory.IsActive;
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<PriceCategoryDTO>> GetPriceCategoriesByCountry(int countryId)
        {
            var categories = await _uow.PriceCategory.GetPriceCategoriesByCountryId(countryId);
            return Mapper.Map<List<PriceCategoryDTO>>(categories);
        }
    }
}
