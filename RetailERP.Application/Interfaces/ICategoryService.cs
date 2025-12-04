using RetailERP.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace RetailERP.Application.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
    Task<CategoryDto> GetCategoryByIdAsync(int categoryId);
    Task<int> CreateCategoryAsync(CategoryDto categoryDto);
    Task<int> UpdateCategoryAsync(int id, CategoryDto categoryDto);
    Task<bool> DeleteCategoryAsync(int categoryId);
}

