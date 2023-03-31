using Microsoft.AspNetCore.Components;
using ShopOnlineSolution.Models.Dtos;
using ShopOnlineSolution.Web.Services.Contracts;

namespace ShopOnlineSolution.Web.Pages;

public class ProductsBase : ComponentBase
{
    [Inject]
    public IProductService ProductService { get; set; } = null!;

    public IEnumerable<ProductDto> Products { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        Products = await ProductService.GetItems();
    }

    protected IOrderedEnumerable<IGrouping<int, ProductDto>> GetGroupedProductsByCategory()
    {
        return  from product in Products
                group product by product.CategoryId into prodByCatGroup
                orderby prodByCatGroup.Key
                select prodByCatGroup;
    }

    protected string GetCategoryName(IGrouping<int, ProductDto> groupedProductsDto)
    {
        return groupedProductsDto.FirstOrDefault(pg => pg.CategoryId == groupedProductsDto.Key)!.CategoryName;
    }

}