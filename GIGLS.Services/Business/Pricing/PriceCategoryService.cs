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
using GIGLS.Core.IServices.User;
using GIGLS.Infrastructure;

namespace GIGLS.Services.Business.Pricing
{
    public class PriceCategoryService : IPriceCategoryService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        public PriceCategoryService(IUnitOfWork uow, IUserService userService)
        {
            _uow = uow;
            _userService = userService;
            MapperConfig.Initialize();
        }
        
        public async Task<object> AddPriceCategory(PriceCategoryDTO priceCategory)
        {
            if (await _uow.PriceCategory.ExistAsync(c => c.PriceCategoryName.ToLower() == priceCategory.PriceCategoryName.Trim().ToLower() && c.CountryId == priceCategory.CountryId && c.DepartureCountryId == priceCategory.DepartureCountryId))
            {
                throw new GenericException("Price category already exist");
            }
            if (priceCategory.CountryId == priceCategory.DepartureCountryId)
            {
                throw new GenericException("Price category can't be set for the same country");
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
                cattoModify.DepartureCountryId = priceCategory.DepartureCountryId;
                cattoModify.IsActive = priceCategory.IsActive;
                cattoModify.SubminimumPrice = priceCategory.SubminimumPrice;
                cattoModify.SubminimumWeight = priceCategory.SubminimumWeight;
                cattoModify.IsHazardous = priceCategory.IsHazardous;
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

        public async Task<IEnumerable<PriceCategoryDTO>> GetPriceCategoriesBothCountries(int countryId)
        {
            //Use default country
            var currentUserId = await _userService.GetCurrentUserId();
            var currentUser = await _userService.GetUserById(currentUserId);
            var userCountryId = currentUser.UserActiveCountryId;
            var categories = await _uow.PriceCategory.GetPriceCategoriesByCountryId(countryId,userCountryId);
            return Mapper.Map<List<PriceCategoryDTO>>(categories);
        }
    }
}
