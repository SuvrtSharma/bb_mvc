using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bb.Models;

namespace bb.DataAccess.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>

    {
        object GetFirstOrDefault(Func<object, bool> value);
        void Update(Product obj);


    }
}
