using bb.DataAccess.Data;
using bb.DataAccess.Repository.IRepository;
using bb.DataAccess.Repository;
using Microsoft.AspNetCore.Hosting;
using System;

namespace BulkyBook.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }
        public ICartRepository Cart { get; private set; }
        public IOrderRepository Order { get; private set; }

        public UnitOfWork(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Product = new ProductRepository(_db, webHostEnvironment);
            Cart = new CartRepository(_db);
            Order = new OrderRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
