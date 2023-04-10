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

    public string TotalPrice { get; set; } = string.Empty;
    public int TotalQuantity { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            ShoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);
            this.CalculateCartSummaryTotals();
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
        this.CalculateCartSummaryTotals();
    }

    protected async Task UpdateQtyCartItem_Click(int id, int qty)
    {
        try
        {
            if (qty > 0)
            {
                var updateItemDto = new CartItemQtyUpdateDto
                {
                    CartItemId = id,
                    Qty = qty
                };

                var response = await ShoppingCartService.UpdateQty(updateItemDto);
                
                this.UpdateItemsTotalPrice(response);
                this.CalculateCartSummaryTotals();

            }
            else
            {
                var item = ShoppingCartItems.FirstOrDefault(i => i.Id == id);

                if (item != null)
                {
                    item.Qty = 1;
                    item.TotalPrice = item.Price;
                }
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    private void UpdateItemsTotalPrice(CartItemDto cartItemDto)
    {
        var item = this.GetCartItem(cartItemDto.Id);

        if (item != null)
        {
            item.TotalPrice = cartItemDto.Price * cartItemDto.Qty;
        }
    }

    private void CalculateCartSummaryTotals()
    {
        this.SetTotalPrice();
        this.SetTotalQuantity();
    }

    private void SetTotalPrice()
    {
        // responsible to calculte
        TotalPrice = ShoppingCartItems.Sum(t => t.TotalPrice).ToString("C"); 
    }

    private void SetTotalQuantity()
    {
        TotalQuantity = ShoppingCartItems.Sum(t => t.Qty);
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