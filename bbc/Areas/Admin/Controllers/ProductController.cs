using bb.DataAccess.Data;
using bb.DataAccess.Repository.IRepository;
using bb.Models;
using bb.Models.ViewModels;
using bb.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace bbc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return View(objProductList);
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                }),
                Product = new Product()
            };

            if (id == null || id == 0)
            {
                // Create product if id is null or 0
                return View(productVM);
            }
            else
            {
                // Update product if id exists
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);
            }
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file, IFormFile? previewFile, IFormFile? ebookFile)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                // --- Begin Image File Upload ---
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        // Delete the old image file
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.ImageUrl = @"\images\product\" + fileName;
                }
                // --- End Image File Upload ---

                // --- Begin Preview File Upload ---
                if (previewFile != null)
                {
                    string previewFileName = Guid.NewGuid().ToString() + Path.GetExtension(previewFile.FileName);
                    string previewPath = Path.Combine(wwwRootPath, @"PreviewPdfs\product");

                    if (!Directory.Exists(previewPath))
                    {
                        Directory.CreateDirectory(previewPath);
                    }

                    if (!string.IsNullOrEmpty(productVM.Product.PreviewUrl))
                    {
                        // Delete the old preview file
                        var oldPreviewPath = Path.Combine(wwwRootPath, productVM.Product.PreviewUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldPreviewPath))
                        {
                            System.IO.File.Delete(oldPreviewPath);
                        }
                    }

                    using (var previewStream = new FileStream(Path.Combine(previewPath, previewFileName), FileMode.Create))
                    {
                        previewFile.CopyTo(previewStream);
                    }
                    productVM.Product.PreviewUrl = @"\PreviewPdfs\product\" + previewFileName;
                }
                // --- End Preview File Upload ---

                // --- Begin EBook File Upload ---
                if (ebookFile != null)
                {
                    string ebookFileName = Guid.NewGuid().ToString() + Path.GetExtension(ebookFile.FileName);
                    string ebookPath = Path.Combine(wwwRootPath, @"EBooks\product");

                    if (!Directory.Exists(ebookPath))
                    {
                        Directory.CreateDirectory(ebookPath);
                    }

                    if (!string.IsNullOrEmpty(productVM.Product.EBook))
                    {
                        // Delete the old eBook file
                        var oldEbookPath = Path.Combine(wwwRootPath, productVM.Product.EBook.TrimStart('\\'));
                        if (System.IO.File.Exists(oldEbookPath))
                        {
                            System.IO.File.Delete(oldEbookPath);
                        }
                    }

                    using (var ebookStream = new FileStream(Path.Combine(ebookPath, ebookFileName), FileMode.Create))
                    {
                        ebookFile.CopyTo(ebookStream);
                    }
                    productVM.Product.EBook = @"\EBooks\product\" + ebookFileName;
                }
                // --- End EBook File Upload ---

                // Insert or update the product in the database
                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                }

                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(productVM);
            }
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitOfWork.Product.Get(u => u.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, productToBeDeleted.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}
