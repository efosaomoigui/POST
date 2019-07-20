﻿using GIGLS.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices
{
    public interface ICategoryService : IServiceDependencyMarker
    {
        Task<List<CategoryDTO>> GetCategories();
        Task<CategoryDTO> GetCategoryById(int categoryId);
        Task<object> AddCategory(CategoryDTO categoryDto);
        Task UpdateCategory(int categoryId, CategoryDTO categoryDto);
        Task DeleteCategory(int categoryId);
    }
}