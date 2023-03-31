using Microsoft.AspNetCore.Components;
using ShopOnlineSolution.Models.Dtos;

namespace ShopOnlineSolution.Web.Pages;

public class DisplayProductsBase : ComponentBase
{
    [Parameter]
    public IEnumerable<ProductDto> Products { get; set; }
}