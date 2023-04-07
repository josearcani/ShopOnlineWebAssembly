using ShopOnlineSolution.Models.Dtos;

namespace ShopOnlineSolution.Web.Services.Contracts;

public interface IShoppingCartService
{
    Task<IEnumerable<CartItemDto>> GetItems(int userId);
    // Task<CartItemDto> GetItem(int id);
    Task<CartItemDto> AddItem(CartItemToAddDto cartItemToAddDto);
}