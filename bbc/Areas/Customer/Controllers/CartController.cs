using System.Security.Claims;
using bb.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace bbc.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public CartController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = _unitOfWork.Cart.GetCartItems(userId);
            // Set the Stripe publishable key so the view can initialize Stripe correctly.
            ViewData["PublishableKey"] = _configuration["Stripe:PublishableKey"];
            return View(cartItems);
        }

        public IActionResult Add(int productId)
        {
            // Retrieve the actual user id from the claims.
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

        public IActionResult Checkout()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = _unitOfWork.Cart.GetCartItems(userId);

            if (cartItems == null || !cartItems.Any())
            {
                TempData["Error"] = "Your cart is empty!";
                return RedirectToAction("Index");
            }

            // Also pass the publishable key to the checkout view.
            ViewData["PublishableKey"] = _configuration["Stripe:PublishableKey"];
            return View(cartItems); // Pass cart items to checkout view
        }
    }
}
