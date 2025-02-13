using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bb.Models;

namespace bb.DataAccess.Repository.IRepository
{
    public interface ICartRepository
    {
        void AddToCart(string userId, int productId, int quantity);
        void RemoveFromCart(string userId, int productId);
        void ClearCart(string userId);
        IEnumerable<Cart> GetCartItems(string userId);
    }
}