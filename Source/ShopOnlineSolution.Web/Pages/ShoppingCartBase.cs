using Microsoft.AspNetCore.Components;
using ShopOnlineSolution.Models.Dtos;
using ShopOnlineSolution.Web.Services.Contracts;

namespace ShopOnlineSolution.Web.Pages;

public class ShoppingCartBase : ComponentBase
{
    [Inject]
    public IShoppingCartService ShoppingCartService { get; set; } = null!;

    public IEnumerable<CartItemDto> ShoppingCartItems { get; set; } = Enumerable.Empty<CartItemDto>();

    public string? ErrorMessage { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            ShoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    protected async Task DeleteCartItem_Click(int id)
    {
        var cartItemDto = await ShoppingCartService.DeleteItem(id);
        
        // we can either make a new call and re render the component - less performand
        // we can delete direcly the list in the client side without making a new call

        this.RemoveCartItem(id);

    }

    private CartItemDto? GetCartItem(int id)
    {
        return ShoppingCartItems.FirstOrDefault(c => c.Id == id);
    }

    private void RemoveCartItem(int id)
    {
        var item = this.GetCartItem(id);
        //out collection is IEnumerable is fine when traversing through
        // not easy to remove an item from it
        // change IEnumerable to List

        // ShoppingCartItems.Remove(item);

        // or remove item from IEnumerable
        ShoppingCartItems = ShoppingCartItems.Where(item => item.Id != id);
    }

}