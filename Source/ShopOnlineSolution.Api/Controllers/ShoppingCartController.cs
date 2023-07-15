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

            if (cartItems is null) return NoContent();

            var products = await _productRepository.GetItems();

            if (products is null) throw new Exception("No products exist in the system");

            var cartItemsDto = cartItems!.ConvertToDto(products!).ToList();
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

            if (cartItem is null) return NotFound();

            var product = await _productRepository.GetItem(cartItem.ProductId);

            if (product is null) return NotFound();

            var cartItemDto = cartItem.ConvertToDto(product);

            return Ok(cartItemDto);
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

            if (newCartItem is null) return BadRequest();

            var product = await _productRepository.GetItem(newCartItem.ProductId);

            if (product is null) throw new Exception($"Something went wrong when attempting to retrieve product (productId: ({cartItemToAddDto.ProductId}))");

            var newCartItemDto = newCartItem.ConvertToDto(product);

            // it is a common practice to add the location of the new resource in the header
            // of the response
            // relevant resource pertainig to get the item action method
            // include de id of the new resource
            // include de new added object
            return CreatedAtAction(nameof(GetItem), new { id = newCartItem.Id }, newCartItemDto);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<CartItemDto>> DeleteItem(int id)
    {
        try
        {
            var cartItem = await _shoppingCartRepository.DeleteItem(id);

            if (cartItem is null) return NotFound();

            var product = await _productRepository.GetItem(cartItem.ProductId);

            if (product is null) return NotFound();

            var deletedCartItemDto = cartItem.ConvertToDto(product);

            return Ok(deletedCartItemDto);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult<CartItemDto>> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
    {
        try
        {
            var cartItem = await _shoppingCartRepository.UpdateQty(id, cartItemQtyUpdateDto);

            if (cartItem is null) return NotFound();

            var product = await _productRepository.GetItem(cartItem.ProductId);

            if (product is null) return NotFound();

            var updatedQtyCartItemDto = cartItem.ConvertToDto(product);

            return Ok(updatedQtyCartItemDto);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}