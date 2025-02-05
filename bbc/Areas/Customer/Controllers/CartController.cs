using System.Security.Claims;
using bb.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bbc.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = _unitOfWork.Cart.GetCartItems(userId);
            return View(cartItems);
        }

        public IActionResult Add(int productId)
        {
            // Retrieve the actual user id from the claims instead of User.Identity.Name
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _unitOfWork.Cart.AddToCart(userId, productId, 1);
            return RedirectToAction("Index");
        }

        public IActionResult Remove(int productId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _unitOfWork.Cart.RemoveFromCart(userId, productId);
            return RedirectToAction("Index");
        }
    }
}