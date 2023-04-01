using Microsoft.AspNetCore.Components;

namespace ShopOnlineSolution.Web.Pages;

public class DisplayErrorBase : ComponentBase
{
    [Parameter]
    public string? ErrorMessage { get; set; }
}