using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bb.Models;

namespace bb.DataAccess.Repository.IRepository
{
    public interface IOrderRepository
    {
        void Add(Order order);
        IEnumerable<Order> GetOrdersByUser(string userId);
    }
}