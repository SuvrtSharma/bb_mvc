using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using bb.DataAccess.Repository.IRepository;
using bb.Models;

namespace bb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize] // Ensure only authenticated users access these actions
    public class PaymentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Optional: Separate checkout view
        public IActionResult Checkout()
        {
            return View();
        }

        // POST: Customer/Payment/CreateCheckoutSession
        [HttpPost]
        public IActionResult CreateCheckoutSession()
        {
            // For a real app, dynamically build line items from the user's cart.
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Shopping Cart Purchase",
                            },
                            UnitAmount = 1000, // $10.00 in cents
                        },
                        Quantity = 1,
                    },
                },
                Mode = "payment",
                SuccessUrl = Url.Action("Success", "Payment", null, Request.Scheme),
                CancelUrl = Url.Action("Cancel", "Payment", null, Request.Scheme),
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return Json(new { sessionId = session.Id });
        }

        // Called on successful payment.
        public IActionResult Success()
        {
            // Retrieve the current user's ID. With [Authorize] this should not be null.
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                TempData["Error"] = "User not authenticated.";
                return RedirectToAction("Index", "Cart");
            }

            // Retrieve cart items for the user.
            var cartItems = _unitOfWork.Cart.GetCartItems(userId);
            if (cartItems == null || !cartItems.Any())
            {
                TempData["Error"] = "Your cart is empty!";
                return RedirectToAction("Index", "Cart");
            }

            // Create a new order using the items from the cart.
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                Total = cartItems.Sum(item => item.Quantity * (decimal)(item.Product?.Price ?? 0)),
                OrderDetails = cartItems.Select(item => new OrderDetail
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Quantity * (decimal)(item.Product?.Price ?? 0)
                }).ToList()
            };

            // Save the order to the database.
            _unitOfWork.Order.Add(order);
            _unitOfWork.Save();

            // Optionally, clear the user's cart after order creation.
            _unitOfWork.Cart.ClearCart(userId);
            _unitOfWork.Save();

            // Return the Success view, passing the order so you can display order details.
            return View(order);
        }

        // Called if the payment is canceled.
        public IActionResult Cancel()
        {
            return View();
        }
    }
}
