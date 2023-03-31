using Microsoft.AspNetCore.Mvc;
using ShopOnlineSolution.Api.Repositories.Contracts;
using ShopOnlineSolution.Extensions;
using ShopOnlineSolution.Models.Dtos;

namespace ShopOnlineSolution.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _repository;
    public ProductController(IProductRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetItems()
    {
        try
        {
            var products = await _repository.GetItems();
            var productCategories = await _repository.GetCategories();

            if (products == null || productCategories == null )
            {
                return NotFound();
            }
            else
            {
                var productDtos = products.ConvertToDto(productCategories);

                return Ok(productDtos);
            }
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
        }
    } 

}