using ShopOnlineSolution.Api.Entities;
using ShopOnlineSolution.Models.Dtos;

namespace ShopOnlineSolution.Api.Repositories.Contracts;

/// <summary>
/// Wrapper for adding and deleting items from shopping cart.
/// </summary>
public interface IShoppingCartRepository
{
    /// <summary>
    /// Adds item to shopping cart.
    /// </summary>
    /// <param name="cartItemToAddDto">Data of the cart item.</param>
    /// <returns>Cart Item.</returns>
    Task<CartItem?> AddItem(CartItemToAddDto cartItemToAddDto);

    /// <summary>
    /// Updates the quantity of an item in a shopping cart.
    /// (<paramref name="id"/>, <paramref name="cartItemQtyUpdateDto"/>).
    /// </summary>
    /// <param name="id">Id of cart.</param>
    /// <param name="cartItemQtyUpdateDto">Data of the cart item.</param>
    /// <returns>Cart Item.</returns>
    Task<CartItem?> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto);

    /// <summary>
    /// Removes a particular item from shopping cart.
    /// </summary>
    /// <param name="id">Id of the item.</param>
    /// <returns>Cart Item.</returns>
    Task<CartItem?> DeleteItem(int id);

    /// <summary>
    /// Retrieve data from item that it is already in the shopping cart.
    /// </summary>
    /// <param name="id">Id of the item.</param>
    /// <returns>Cart Item.</returns>
    Task<CartItem?> GetItem(int id);

    /// <summary>
    /// Retrieve all data from a particular shopping cart.
    /// </summary>
    /// <param name="userId">Id of the user.</param>
    /// <returns>Cart Item.</returns>
    Task<IEnumerable<CartItem?>> GetItems(int userId);
}