using bb.DataAccess.Repository.IRepository;
using bb.Models;
using bb.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace bbc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductReviewController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductReviewController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Admin/ProductReview
      public IActionResult Index()
{
    var pendingProducts = _unitOfWork.Product.GetAll()
                              .Where(p => p.Status == ApprovalStatus.Pending)
                              .ToList();
    return View(pendingProducts);
}


        // GET: Admin/ProductReview/Details/5
        public IActionResult Details(int id)
        {
            var product = _unitOfWork.Product.GetFirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Admin/ProductReview/Approve/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Approve(int id)
        {
            var product = _unitOfWork.Product.GetFirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            product.Status = ApprovalStatus.Approved;
            _unitOfWork.Product.Update(product);
            _unitOfWork.Save();
            TempData["SuccessMessage"] = "Product approved successfully.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/ProductReview/Reject/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Reject(int id)
        {
            var product = _unitOfWork.Product.GetFirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            product.Status = ApprovalStatus.Rejected;
            _unitOfWork.Product.Update(product);
            _unitOfWork.Save();
            TempData["ErrorMessage"] = "Product rejected.";
            return RedirectToAction(nameof(Index));
        }
    }
}
