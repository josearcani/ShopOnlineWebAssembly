using Microsoft.EntityFrameworkCore;
using ShopOnlineSolution.Api.Data;
using ShopOnlineSolution.Api.Entities;
using ShopOnlineSolution.Api.Repositories.Contracts;
using ShopOnlineSolution.Models.Dtos;

namespace ShopOnlineSolution.Api.Repositories;

public class ShoppingCartRepository : IShoppingCartRepository
{
    private readonly ShopOnlineDbContext _context;

    public ShoppingCartRepository(ShopOnlineDbContext context)
    {
        _context = context;
    }

    private async Task<bool> CartItemExists(int cartId, int productId)
    {
        return await _context.CartItems.AnyAsync(c => c.CartId == cartId && c.ProductId == productId);
    }

    public async Task<CartItem> AddItem(CartItemToAddDto cartItemToAddDto)
    {
        if (await this.CartItemExists(cartItemToAddDto.CartId, cartItemToAddDto.ProductId) == false)
        {
            var item = await (from product in _context.Products
            where product.Id == cartItemToAddDto.ProductId
            select new CartItem
            {
                CartId = cartItemToAddDto.CartId,
                ProductId = product.Id,
                Qty = cartItemToAddDto.Qty
            }).SingleOrDefaultAsync();
            
            if (item != null)
            {
                var result = await _context.CartItems.AddAsync(item);
                await _context.SaveChangesAsync();
                return result.Entity;
            }
        }

        return null;
    }

    public async Task<CartItem> DeleteItem(int id)
    {
        // todo
        // verifiy element exists\
        var cartItem = await _context.CartItems.FindAsync(id);
        // var cartItem1 = await _context.CartItems.AnyAsync(c => c.Id == id);

        if (cartItem != null)
        {
            // delete
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
            return cartItem;
        }
        else
        {
            throw new Exception("item does not exist, dude are you dumb");
        }
    }

    public async Task<CartItem> GetItem(int id)
    {
        var item =  await (from cart in _context.Carts
        join cartItem in _context.CartItems
        on cart.Id equals cartItem.CartId
        where cartItem.Id == id
        select new CartItem
        {
            Id = cartItem.Id,
            CartId = cartItem.CartId,
            ProductId = cartItem.ProductId,
            Qty = cartItem.Qty
        }).SingleOrDefaultAsync();

        if (item == null)
        {
            throw new Exception("get item is null, good chance to implement logger");
        }
        
        return item;
    }

    public async Task<IEnumerable<CartItem>> GetItems(int userId)
    {
        return await (from cart in _context.Carts
        join cartItem in _context.CartItems
        on cart.Id equals cartItem.CartId
        where cart.UserId == userId
        select new CartItem
        {
            Id = cartItem.Id,
            ProductId = cartItem.ProductId,
            Qty = cartItem.Qty,
            CartId = cartItem.CartId
        }).ToListAsync();
    }

    public async Task<CartItem> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
    {
        var item = await _context.CartItems.FindAsync(id);

        if (item != null)
        {
            item.Qty = cartItemQtyUpdateDto.Qty;
            await _context.SaveChangesAsync();
            return item;
        }

        return null;
    }
}