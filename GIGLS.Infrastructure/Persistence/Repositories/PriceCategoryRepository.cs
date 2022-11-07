using GIGL.POST.Core.Domain;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using POST.Core.DTO;
using POST.Core.Domain;
using POST.Core.IRepositories;
using POST.Core.DTO.PaymentTransactions;
using POST.Core.Enums;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.PriceCategorys
{
    public class PriceCategoryRepository : Repository<PriceCategory, GIGLSContext>, IPriceCategoryRepository
    {
        private GIGLSContext _context;

        public PriceCategoryRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<PriceCategoryDTO>> GetPriceCategoriesByCountryId(int countryId)
        {
            try
            {
                var PriceCategorys = _context.PriceCategory.Where(s => s.CountryId == countryId);

                var PriceCategorysDTO = from s in PriceCategorys
                                        select new PriceCategoryDTO
                                        {
                                            PriceCategoryName = s.PriceCategoryName,
                                            CategoryMinimumPrice = s.CategoryMinimumPrice,
                                            PricePerWeight = s.PricePerWeight,
                                            PriceCategoryId = s.PriceCategoryId,
                                            CountryId = s.CountryId,
                                            IsActive = s.IsActive,
                                            CategoryMinimumWeight = s.CategoryMinimumWeight,
                                            CountryName = _context.Country.Where(x => x.CountryId == countryId).FirstOrDefault().CountryName,
                                            SubminimumPrice = s.SubminimumPrice,
                                            SubminimumWeight = s.SubminimumWeight,
                                            IsHazardous = s.IsHazardous,
                                            DeliveryType = s.DeliveryType
                                        };
                return Task.FromResult(PriceCategorysDTO.OrderBy(x => x.PriceCategoryName).ToList());

            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<PriceCategoryDTO> GetPriceCategoryById(int priceCategoryId)
        {
            try
            {
                var PriceCategorys = _context.PriceCategory.Where(s => s.PriceCategoryId == priceCategoryId);

                var PriceCategorysDTO = from s in PriceCategorys
                                        select new PriceCategoryDTO
                                        {
                                            PriceCategoryName = s.PriceCategoryName,
                                            CategoryMinimumPrice = s.CategoryMinimumPrice,
                                            PricePerWeight = s.PricePerWeight,
                                            PriceCategoryId = s.PriceCategoryId,
                                            CountryId = s.CountryId,
                                            DepartureCountryId = s.DepartureCountryId,
                                            CategoryMinimumWeight = s.CategoryMinimumWeight,
                                            IsActive = s.IsActive,
                                            CountryName = _context.Country.Where(x => x.CountryId == s.CountryId).FirstOrDefault().CountryName,
                                            DepartureCountryName = _context.Country.Where(x => x.CountryId == s.DepartureCountryId).FirstOrDefault().CountryName,
                                            SubminimumPrice = s.SubminimumPrice,
                                            SubminimumWeight = s.SubminimumWeight,
                                            IsHazardous = s.IsHazardous,
                                            DeliveryType = s.DeliveryType
                                        };
                return Task.FromResult(PriceCategorysDTO.FirstOrDefault());

            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<PriceCategoryDTO>> GetPriceCategories()
        {
            try
            {
                var categories = _context.PriceCategory;

                var categoriesDTO = from s in categories
                                join c in Context.Country on s.CountryId equals c.CountryId
                                select new PriceCategoryDTO
                                {
                                    PriceCategoryName = s.PriceCategoryName,
                                    CategoryMinimumPrice = s.CategoryMinimumPrice,
                                    PricePerWeight = s.PricePerWeight,
                                    PriceCategoryId = s.PriceCategoryId,
                                    CategoryMinimumWeight = s.CategoryMinimumWeight,
                                    CountryName = c.CountryName,
                                    IsActive = s.IsActive,
                                    CountryId = c.CountryId,
                                    DepartureCountryId = s.DepartureCountryId,
                                    DepartureCountryName = _context.Country.Where(x => x.CountryId == s.DepartureCountryId).FirstOrDefault().CountryName,
                                    SubminimumPrice = s.SubminimumPrice,
                                    SubminimumWeight = s.SubminimumWeight,
                                    IsHazardous = s.IsHazardous,
                                    DeliveryType = s.DeliveryType
                                };
                return Task.FromResult(categoriesDTO.OrderBy(x => x.PriceCategoryName).ToList());

            }
            catch (Exception)
            {
                throw;
            }
        }


        public Task<List<PriceCategoryDTO>> GetPriceCategoriesByCountryId(int destCountryId, int deptCountryID)
        {
            try
            {
                var PriceCategorys = _context.PriceCategory.Where(s => s.CountryId == destCountryId && s.DepartureCountryId == deptCountryID && s.DeliveryType == DeliveryType.GOSTANDARDED);

                var PriceCategorysDTO = from s in PriceCategorys
                                        select new PriceCategoryDTO
                                        {
                                            PriceCategoryName = s.PriceCategoryName,
                                            CategoryMinimumPrice = s.CategoryMinimumPrice,
                                            PricePerWeight = s.PricePerWeight,
                                            PriceCategoryId = s.PriceCategoryId,
                                            CountryId = s.CountryId,
                                            IsActive = s.IsActive,
                                            CategoryMinimumWeight = s.CategoryMinimumWeight,
                                            CountryName = _context.Country.Where(x => x.CountryId == destCountryId).FirstOrDefault().CountryName,
                                            SubminimumPrice = s.SubminimumPrice,
                                            SubminimumWeight = s.SubminimumWeight,
                                            IsHazardous = s.IsHazardous,
                                            DeliveryType = s.DeliveryType
                                        };
                return Task.FromResult(PriceCategorysDTO.OrderBy(x => x.PriceCategoryName).ToList());

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
