using System.Net.Http.Json;
using System.Text;
using Newtonsoft.Json;
using ShopOnlineSolution.Models.Dtos;
using ShopOnlineSolution.Web.Services.Contracts;

namespace ShopOnlineSolution.Web.Services;

public class ShoppingCartService : IShoppingCartService
{
    private readonly HttpClient _httpClient;
    public event Action<int> OnShoppingCartChanged;

    public ShoppingCartService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<CartItemDto> AddItem(CartItemToAddDto cartItemToAddDto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync<CartItemToAddDto>("api/ShoppingCart", cartItemToAddDto);

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return default(CartItemDto)!;
                }

                var resp = await response.Content.ReadFromJsonAsync<CartItemDto>();
                return resp!;
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http status: {response.StatusCode} - {message}");
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<CartItemDto> DeleteItem(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/ShoppingCart/{id}");

            if (response.IsSuccessStatusCode)
            {
                var cartItemDto = await response.Content.ReadFromJsonAsync<CartItemDto>();
                
                if (cartItemDto == null)
                {
                    throw new Exception("this is strange");
                }
                return cartItemDto;
            }
            else
            {
                return default(CartItemDto)!;
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<IEnumerable<CartItemDto>> GetItems(int userId)
    {
        try
        {
            // var response = await _httpClient.GetAsync($"api/{userId}/GetItems");
            var response = await _httpClient.GetAsync($"api/ShoppingCart/{userId}/GetItems");

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return Enumerable.Empty<CartItemDto>();
                }

                var cartItemsList = await response.Content.ReadFromJsonAsync<IEnumerable<CartItemDto>>();

                if (cartItemsList == null)
                {
                    throw new Exception("this is strange");
                }
                return cartItemsList;
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http status: {response.StatusCode} - {message}");
            }
        }
        catch (Exception)
        {
            // log exception, friendly message
            throw;
        }
    }

    public void RaiseEventOnShoppingCartChanged(int totalQty)
    {
        //check the ecent has subs
        if (OnShoppingCartChanged != null)
        {
            OnShoppingCartChanged.Invoke(totalQty);
        }
    }

    public async Task<CartItemDto> UpdateQty(CartItemQtyUpdateDto cartItemQtyUpdateDto)
    {
        try
        {
            //serialize when pass to the server
            // sent a stringContent so we can pass in the appropiate format to the server
            var jsonRequest = JsonConvert.SerializeObject(cartItemQtyUpdateDto);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");

            var response = await _httpClient.PatchAsync($"api/ShoppingCart/{cartItemQtyUpdateDto.CartItemId}", content);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            
            var resp = await response.Content.ReadFromJsonAsync<CartItemDto>();
            return resp!;
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }
}