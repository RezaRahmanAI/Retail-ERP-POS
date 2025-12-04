using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RetailERP.Application.DTOs;
using RetailERP.Application.Interfaces;
using RetailERP.Domain.Entities;
using RetailERP.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace RetailERP.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ErpDbContext _context;
    private readonly IMapper _mapper;

    public CategoryService( ErpDbContext context , IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> CreateCategoryAsync(CategoryDto categoryDto)
    {
        var category = _mapper.Map<Category>(categoryDto);

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return category.Id;
    }

    public async Task<bool> DeleteCategoryAsync(int categoryId)
    {
        var category = await _context.Categories.FindAsync(categoryId);
        if (category == null) return false;

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
    {
        var categories = await _context.Categories.ToListAsync();
        return _mapper.Map<List<CategoryDto>>(categories);
    }

    public async Task<CategoryDto> GetCategoryByIdAsync(int categoryId)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);

        return _mapper.Map<CategoryDto>(category);

    }

    public async Task<int> UpdateCategoryAsync(int id, CategoryDto categoryDto)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return 0;

        _mapper.Map(categoryDto, category);
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return category.Id;
    }
}

