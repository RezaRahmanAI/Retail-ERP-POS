using RetailERP.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace RetailERP.Application.Interfaces;
public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllProductsAsync();
    Task<ProductDto> GetProductByIdAsync(int productId);
    Task<int> CreateProductAsync(ProductDto productDto);
    Task<int> UpdateProductAsync(int id, ProductDto productDto);
    Task<bool> DeleteProductAsync(int productId);
}

