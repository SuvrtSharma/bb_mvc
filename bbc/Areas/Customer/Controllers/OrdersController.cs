using System.Security.Claims;
using bb.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using bb.Models;

namespace bb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = _unitOfWork.Order.GetOrdersByUser(userId);
            return View(orders);
        }
    }
}
