using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using bb.DataAccess.Data;
using bb.DataAccess.Repository.IRepository;
using bb.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace bb.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductRepository(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
            : base(db)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public Product GetFirstOrDefault(Func<Product, bool> predicate)
        {
            return _db.Product.FirstOrDefault(predicate);
        }

        public void Update(Product obj)
        {
            var objFromDb = _db.Product.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Title = obj.Title;
                objFromDb.ISBN = obj.ISBN;
                objFromDb.Price = obj.Price;
                objFromDb.Price50 = obj.Price50;
                objFromDb.ListPrice = obj.ListPrice;
                objFromDb.Price100 = obj.Price100;
                objFromDb.Description = obj.Description;
                objFromDb.CategoryId = obj.CategoryId;
                objFromDb.Author = obj.Author;
                if (!string.IsNullOrEmpty(obj.ImageUrl))
                {
                    objFromDb.ImageUrl = obj.ImageUrl;
                }
                if (!string.IsNullOrEmpty(obj.PreviewUrl))
                {
                    objFromDb.PreviewUrl = obj.PreviewUrl;
                }
                if (!string.IsNullOrEmpty(obj.EBook))
                {
                    objFromDb.EBook = obj.EBook;
                }
            }
        }

        public async Task AddProductSubmissionAsync(ProductSubmission model)
        {
            // Map the view model to a new Product entity
            Product product = new Product
            {
                Title = model.Title,
                Description = model.Description,
                ISBN = model.ISBN,
                Author = model.Author, // Author comes from form input now
                ListPrice = model.ListPrice,
                Price = model.Price,
                Price50 = model.Price50,
                Price100 = model.Price100,
                CategoryId = model.CategoryId,

                // Mark as pending for admin review
                Status = ApprovalStatus.Pending,
                SubmittedBy = model.Author,
                SubmittedOn = DateTime.Now
            };

            // Use the same folder structure as in your admin Upsert code:
            if (model.Image != null)
            {
                // Save image to "\images\product"
                product.ImageUrl = await SaveFileAsync(model.Image, @"images\product");
            }
            if (model.PreviewFile != null)
            {
                // Save preview PDF to "\PreviewPdfs\product"
                product.PreviewUrl = await SaveFileAsync(model.PreviewFile, @"PreviewPdfs\product");
            }
            if (model.EBookFile != null)
            {
                // Save e-book PDF to "\EBooks\product"
                product.EBook = await SaveFileAsync(model.EBookFile, @"EBooks\product");
            }

            _db.Product.Add(product);
            await _db.SaveChangesAsync();
        }

        // Helper method to save a file into the specified folder (relative to wwwroot)
        private async Task<string> SaveFileAsync(IFormFile file, string folderPath)
        {
            // Build the absolute path using the provided folderPath.
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            // Return the relative path (with a leading backslash, as in your Upsert code)
            return @"\" + folderPath + @"\" + uniqueFileName;
        }

    }
}
