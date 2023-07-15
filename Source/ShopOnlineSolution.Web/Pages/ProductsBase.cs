using Microsoft.AspNetCore.Components;
using ShopOnlineSolution.Models.Dtos;
using ShopOnlineSolution.Web.Services.Contracts;

namespace ShopOnlineSolution.Web.Pages;

public class ProductsBase : ComponentBase
{
    [Inject]
    public IProductService ProductService { get; set; } = null!;
    [Inject]
    public IShoppingCartService ShoppingCartService { get; set; } = null!;

    public IEnumerable<ProductDto> Products { get; set; } = null!;

    public NavigationManager NavigationManager { get; set; } = null!;

    public string? ErrorMessage { get; set; }
    protected override async Task OnInitializedAsync()
    {
        try
        {
            Products = await ProductService.GetItems();

            // not the best performant way, this is not optimized
            var shoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);
            var totalQty = shoppingCartItems.Sum(item => item.Qty);

            ShoppingCartService.RaiseEventOnShoppingCartChanged(totalQty);
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    protected IOrderedEnumerable<IGrouping<int, ProductDto>> GetGroupedProductsByCategory()
    {
        return from product in Products
               group product by product.CategoryId into prodByCatGroup
               orderby prodByCatGroup.Key
               select prodByCatGroup;
    }

    protected static string GetCategoryName(IGrouping<int, ProductDto> groupedProductsDto)
    {
        return groupedProductsDto.FirstOrDefault(pg => pg.CategoryId == groupedProductsDto.Key)!.CategoryName;
    }
}