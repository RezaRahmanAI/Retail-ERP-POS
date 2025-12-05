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
public class ProductController : ControllerBase
{

    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductDto>>> GetAll()
    {
        var product = await _productService.GetAllProductsAsync();
        return Ok(product);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetById(int id) {
        var product = await _productService.GetProductByIdAsync(id);
        return Ok(product);
    }

    [HttpPost]
    [Authorize(Roles = RoleConstants.AdminAndManager)]
    public async Task<ActionResult<int>> Create(ProductDto product)
    {
        var productId = await _productService.CreateProductAsync(product);
        return Ok(productId);

    }


    [HttpPut("{id}")]
    [Authorize(Roles = RoleConstants.AdminAndManager)]
    public async Task<ActionResult<int>> Update(int id, [FromBody] ProductDto productDto)
    {
        var productId = await _productService.UpdateProductAsync(id, productDto);

        if (productId == 0) return NotFound(id);

        return Ok(productId);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = RoleConstants.AdminAndManager)]
    public async Task<ActionResult> Delete(int id)
    {
        var success = await _productService.DeleteProductAsync(id);

        if(success == false) return NotFound(id);

        return Ok(success);
    }

}
