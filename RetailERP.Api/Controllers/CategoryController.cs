using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RetailERP.Application.DTOs;
using RetailERP.Application.Interfaces;
using RetailERP.Domain.Constants;

namespace RetailERP.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = RoleConstants.AllStaff)]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryDto>>> GetAll()
    {
        var category = await _categoryService.GetAllCategoriesAsync();
        return Ok(category);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetById(int id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);
        return Ok(category);
    }

    [HttpPost]
    [Authorize(Roles = RoleConstants.AdminAndManager)]
    public async Task<ActionResult<int>> Create(CategoryDto categoryDto)
    {
        var categoryId = await _categoryService.CreateCategoryAsync(categoryDto);
        return Ok(categoryId);
    }


    [HttpPut("{id}")]
    [Authorize(Roles = RoleConstants.AdminAndManager)]
    public async Task<ActionResult<int>> Update(int id, CategoryDto categoryDto)
    {
        var categoryId = await _categoryService.UpdateCategoryAsync(id, categoryDto);

        if (categoryId == 0) return NotFound(id);

        return Ok(categoryId);
    }


    [HttpDelete("{id}")]
    [Authorize(Roles = RoleConstants.AdminAndManager)]
    public async Task<ActionResult<bool>> Delete(int id)
    {
        var succes = await _categoryService.DeleteCategoryAsync(id);
        if (succes == false) return NotFound(id);

        return Ok(succes);
    }



}

