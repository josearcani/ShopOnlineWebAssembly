using Microsoft.AspNetCore.Components;
using ShopOnlineSolution.Models.Dtos;
using ShopOnlineSolution.Web.Services.Contracts;

namespace ShopOnlineSolution.Web.Pages;

public class ProductDetailsBase : ComponentBase
{
    [Parameter]
    public int Id { get; set; }

    [Inject]
    public IProductService ProductService { get; set; } = null!;

    public ProductDto Product { get; set; } = null!;

    public string? ErrorMessage { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Product = await ProductService.GetItem(Id);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }
}