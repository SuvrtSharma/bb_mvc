using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System.Collections.Generic;

namespace bb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class PaymentController : Controller
    {
        // Optional: If you want a separate checkout page view.
        public IActionResult Checkout()
        {
            return View();
        }

        // POST: Customer/Payment/CreateCheckoutSession
        [HttpPost]
        public IActionResult CreateCheckoutSession()
        {
            // In a real app, dynamically build line items from the cart.
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
            return View();
        }

        // Called if the payment is canceled.
        public IActionResult Cancel()
        {
            return View();
        }
    }
}
