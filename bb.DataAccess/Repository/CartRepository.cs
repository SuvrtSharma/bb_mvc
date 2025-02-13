using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bb.DataAccess.Data;
using bb.DataAccess.Repository.IRepository;
using bb.Models;
using Microsoft.EntityFrameworkCore;

namespace bb.DataAccess.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _db;

        public CartRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public void AddToCart(string userId, int productId, int quantity)
        {
            var cartItem = _db.Carts.FirstOrDefault(c => c.UserId == userId && c.ProductId == productId);
            if (cartItem == null)
            {
                // Add a new cart item
                _db.Carts.Add(new Cart
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = quantity
                });
            }
            else
            {
                // Update the quantity if the item already exists
                cartItem.Quantity += quantity;
            }
            _db.SaveChanges();
        }
        public void RemoveFromCart(string userId, int productId)
        {
            var cartItem = _db.Carts.FirstOrDefault(c => c.UserId == userId && c.ProductId == productId);
            if (cartItem != null)
            {
                _db.Carts.Remove(cartItem);
                _db.SaveChanges();
            }
        }


        public IEnumerable<Cart> GetCartItems(string userId)
        {
            return _db.Carts
                      .Include(c => c.Product)  // Include the Product navigation property
                      .Where(c => c.UserId == userId)
                      .ToList();
        }

        public IEnumerable<Cart> GetCarts(string userId)
        {
            return _db.Carts.Where(c => c.UserId == userId).ToList();
        }
        public void ClearCart(string userId)
        {
            // Get all cart items for the user
            var cartItems = _db.Carts.Where(c => c.UserId == userId);
            // Remove all of them at once
            _db.Carts.RemoveRange(cartItems);
        }
    }
}