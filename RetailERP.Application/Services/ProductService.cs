using RetailERP.Application.DTOs;
using RetailERP.Application.Interfaces;
using RetailERP.Infrastructure.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RetailERP.Domain.Entities;

namespace RetailERP.Application.Services;

public class ProductService : IProductService
{
    private readonly ErpDbContext _context;
    private readonly IMapper _mapper;

    public ProductService( ErpDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> CreateProductAsync(ProductDto productDto)
    {
        var product = _mapper.Map<Product>(productDto);
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product.Id;
    }

    public async Task<bool> DeleteProductAsync(int productId)
    {
        var prodcut = await _context.Products.FindAsync(productId);
        if (prodcut == null) return false;

        _context.Products.Remove(prodcut);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        var products = await _context.Products.Include(p => p.Category).ToListAsync();
        return _mapper.Map<List<ProductDto>>(products);
    }

    public async Task<ProductDto> GetProductByIdAsync(int productId)
    {
        var product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == productId);
        return _mapper.Map<ProductDto>(product);
    }

    public async Task<int> UpdateProductAsync(int id, ProductDto productDto)
    {
        var product = await _context.Products.FindAsync(id);
        if ( product == null) return 0;

        _mapper.Map(productDto, product);
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
        return product.Id;
    }
}

