using bb.DataAccess.Repository.IRepository;
using bb.Models;
using bb.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace bbc.Areas.Author.Controllers
{
    [Area("Author")]
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // GET: Author/Products/Submit
        public IActionResult Submit()
        {
            return View();
        }

        // POST: Author/Products/Submit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(ProductSubmission model)
        {
            if (ModelState.IsValid)
            {
                // Removed setting the Author property from the logged-in user.
                // The Author value will come from the form submission.
                await _productRepository.AddProductSubmissionAsync(model);
                TempData["SuccessMessage"] = "Product submitted successfully and is awaiting admin approval.";
                return RedirectToAction("Submit");
            }
            return View(model);
        }
    }
}
