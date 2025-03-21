﻿using catch_up_backend.Dtos;

namespace catch_up_backend.Interfaces
{
    public interface ICategoryService
    {
        public Task<CategoryDto> AddCategory(CategoryDto newCategory);
        public Task<CategoryDto> EditCategory(int categoryId, CategoryDto newCategory);
        public Task<bool> DeleteCategory(int categoryId);
        public Task<List<CategoryDto>> GetAll();
        public Task<CategoryDto> GetById(int categoryId);
        public Task<List<CategoryDto>> SearchCategories(string searchingString);
        public Task<bool> IsUnique(string categoryName);
        public Task<int> GetCount();
        public Task<bool> IsExists(int categoryId);
        public Task<CategoryDto> GetActiveCategory(int categoryId);
        public Task<bool> IsActive(int categoryId);
    }
}
