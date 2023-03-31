using Microsoft.AspNetCore.Components;
using ShopOnlineSolution.Models.Dtos;
using ShopOnlineSolution.Web.Services.Contracts;

namespace ShopOnlineSolution.Web.Pages;

public class ProductsBase : ComponentBase
{
    [Inject]
    public IProductService ProductService { get; set; }

    public IEnumerable<ProductDto> Products { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Products = await ProductService.GetItems();
    }
}