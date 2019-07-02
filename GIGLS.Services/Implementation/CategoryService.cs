using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using GIGLS.Core.IServices;
using GIGLS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _uow;

        public CategoryService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<object> AddCategory(CategoryDTO categoryDto)
        {
            try
            {
                if (await _uow.Category.ExistAsync(c => c.CategoryName.ToLower() == categoryDto.CategoryName.Trim().ToLower()))
                {
                    throw new GenericException("Category already Exist");
                }
                var newCategory = Mapper.Map<Category>(categoryDto);
                _uow.Category.Add(newCategory);
                await _uow.CompleteAsync();
                return new { Id = newCategory.CategoryId };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteCategory(int categoryId)
        {
            try
            {
                var category = await _uow.Category.GetAsync(categoryId);
                if (category == null)
                {
                    throw new GenericException("Country does not exist");
                }
                _uow.Category.Remove(category);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<CategoryDTO>> GetCategories()
        {
            var categories = _uow.Category.GetAll();
            return Task.FromResult(Mapper.Map<List<CategoryDTO>>(categories));

        }

        public async Task<CategoryDTO> GetCategoryById(int categoryId)
        {
            try
            {
                var category = await _uow.Category.GetAsync(categoryId);
                if (category == null)
                {
                    throw new GenericException("Category Not Exist");
                }

                var countryDto = Mapper.Map<CategoryDTO>(category);
                return countryDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateCategory(int categoryId, CategoryDTO categoryDto)
        {
            try
            {
                var category = await _uow.Category.GetAsync(categoryId);
                if (category == null || categoryDto.CategoryId != categoryId)
                {
                    throw new GenericException("Category information does not exist");
                }
                category.CategoryName = categoryDto.CategoryName;
                
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
