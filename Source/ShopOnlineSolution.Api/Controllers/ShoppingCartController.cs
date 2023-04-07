using Microsoft.AspNetCore.Mvc;
using ShopOnlineSolution.Api.Repositories.Contracts;
using ShopOnlineSolution.Extensions;
using ShopOnlineSolution.Models.Dtos;

namespace ShopOnlineSolution.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShoppingCartController : ControllerBase
{
    private readonly IShoppingCartRepository _shoppingCartRepository;
    private readonly IProductRepository _productRepository;

    public ShoppingCartController(IShoppingCartRepository shoppingCartRepository, IProductRepository productRepository)
    {
        _shoppingCartRepository = shoppingCartRepository;
        _productRepository = productRepository;
    }

    [HttpGet]
    [Route("{userId}/GetItems")]
    public async Task<ActionResult<IEnumerable<CartItemDto>>> GetItems(int userId)
    {
        try
        {
            var cartItems = await _shoppingCartRepository.GetItems(userId);

            if (cartItems == null)
            {
                return this.NoContent();
            }

            var products = await _productRepository.GetItems();

            if (products == null)
            {
                throw new Exception("No products exist in the system");
            }

            var cartItemsDto = cartItems.ConvertToDto(products).ToList();
            return Ok(cartItemsDto);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<CartItemDto>> GetItem(int id)
    {
        try
        {
            var cartItem = await _shoppingCartRepository.GetItem(id);

            if (cartItem == null)
            {
                return this.NotFound();
            }

            var product = await _productRepository.GetItem(cartItem.ProductId);

            if (product == null)
            {
                return this.NotFound();
            }

            var cartItemDto = cartItem.ConvertToDto(product);
            return this.Ok(cartItemDto);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<CartItemDto>> PostItem([FromBody] CartItemToAddDto cartItemToAddDto)
    {
        try
        {
            var newCartItem = await _shoppingCartRepository.AddItem(cartItemToAddDto);

            if (newCartItem == null)
            {
                return this.BadRequest();
            }

            var product = await _productRepository.GetItem(newCartItem.ProductId);

            if (product == null)
            {
                throw new Exception ($"Something went wrong when attempting to retrieve product (productId: ({cartItemToAddDto.ProductId}))");
            }

            var newCartItemDto = newCartItem.ConvertToDto(product);

            // it is a common practice to add the location of the new resource in the header 
            // of the response
            // relevant resource pertainig to get the item action method
            // include de id of the new resource
            // include de new added object
            return this.CreatedAtAction(nameof(GetItem), new { id = newCartItem.Id }, newCartItemDto);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // [HttpDelete]
    // [Route("{id:int}")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<CartItemDto>> DeleteItem(int id)
    {
        try
        {
            var cartItem = await _shoppingCartRepository.DeleteItem(id);

            if (cartItem  == null)
            {
                return this.NotFound();
            }

            var product = await _productRepository.GetItem(cartItem.ProductId);

            if (product == null)
            {
                return NotFound();
            }

            var deletedCartItemDto = cartItem.ConvertToDto(product);

            return this.Ok(deletedCartItemDto);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}