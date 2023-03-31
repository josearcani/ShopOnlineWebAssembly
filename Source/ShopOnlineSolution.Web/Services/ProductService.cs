using System.Net.Http.Json;
using ShopOnlineSolution.Models.Dtos;
using ShopOnlineSolution.Web.Services.Contracts;

namespace ShopOnlineSolution.Web.Services;

public class ProductService : IProductService
{
    private readonly HttpClient httpClient;

    public ProductService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<IEnumerable<ProductDto>> GetItems()
    {
        try
        {
            var products = await this.httpClient.GetFromJsonAsync<IEnumerable<ProductDto>>("api/Product");
            return products;
        }
        catch (System.Exception)
        {
            throw;
        }
    }
}