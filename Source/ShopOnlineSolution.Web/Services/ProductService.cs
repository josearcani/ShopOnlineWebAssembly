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

    public async Task<ProductDto> GetItem(int id)
    {
        try
        {
            var response = await this.httpClient.GetAsync($"api/Product/{id}");

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return default(ProductDto)!;
                }

                var resp = await response.Content.ReadFromJsonAsync<ProductDto>();
                return resp!;
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception(message);
            }
        }
        catch (Exception)
        {
            // Log exception
            throw;
        }
    }

    public async Task<IEnumerable<ProductDto>> GetItems()
    {
        try
        {
            // var products = await this.httpClient.GetFromJsonAsync<IEnumerable<ProductDto>>("api/Product");
            var response = await this.httpClient.GetAsync("api/Product");

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return Enumerable.Empty<ProductDto>();
                }

                var resp = await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();
                return resp!;
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception(message);
            }
        }
        catch (Exception)
        {
            // Log exception
            throw;
        }
    }
}