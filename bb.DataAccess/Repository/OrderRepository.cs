using System.Collections.Generic;
using System.Linq;
using bb.DataAccess.Data;
using bb.DataAccess.Repository.IRepository;
using bb.Models;
using Microsoft.EntityFrameworkCore;

namespace bb.DataAccess.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        // Single implementation of Add
        public void Add(Order order)
        {
            _db.Orders.Add(order);
        }

        // Single implementation of GetOrdersByUser
        public IEnumerable<Order> GetOrdersByUser(string userId)
        {
            return _db.Orders
                      .Include(o => o.OrderDetails)
                      .ThenInclude(od => od.Product)
                      .Where(o => o.UserId == userId)
                      .OrderByDescending(o => o.OrderDate)
                      .ToList();
        }
    }
}
