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
        Product GetFirstOrDefault(Func<Product, bool> predicate);
        void Update(Product obj);

        Task AddProductSubmissionAsync(ProductSubmission model);
       
    }
}
