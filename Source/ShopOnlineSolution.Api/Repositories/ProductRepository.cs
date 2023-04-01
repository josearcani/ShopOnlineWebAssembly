using Microsoft.EntityFrameworkCore;
using ShopOnlineSolution.Api.Data;
using ShopOnlineSolution.Api.Entities;
using ShopOnlineSolution.Api.Repositories.Contracts;
// using ShopOnlineSolution.Models.Dtos;

namespace ShopOnlineSolution.Api.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ShopOnlineDbContext _context;
    public ProductRepository(ShopOnlineDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<ProductCategory>> GetCategories()
    {
        var categories = await _context.ProductCategories.ToListAsync();

        return categories;
    }

    public async Task<ProductCategory> GetCategory(int id)
    {
        var category = await _context.ProductCategories.SingleOrDefaultAsync(c => c.Id == id);

        return category!;
    }

    public async Task<Product> GetItem(int id)
    {
        var product = await _context.Products.FindAsync(id);
        return product!;
    }

    public async Task<IEnumerable<Product>> GetItems()
    {
        var products = await _context.Products.ToListAsync();

        return products;
    }
    
    // public async Task<IEnumerable<ProductDto>> GetItems()
    // {
    //     var products = await _context.Products.Include(p => p.ProductCategory).ToListAsync();
    //     // var products = await _context.Products.ToListAsync();

    //     return products;
    // }
}