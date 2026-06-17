using BLL.Dtos.Order;
using BLL.Services.AbstractServices.MedicationModule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PL.Extention;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PL.Controllers
{
    [Authorize]
    [ValidateAntiForgeryToken]
    public class OrderController(IOrderService _orderService) : Controller
    {

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var Orders = await _orderService.GetMyOrdersAsync(User.GetUserId());
            return View(Orders);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int orderId)
        {
            var order = await _orderService.GetOrderAsync(orderId, User.GetUserId());
            return View(order);
        }
        [HttpPost]
        public async Task<IActionResult> Cancel(int orderId)
        {
            var order = await _orderService.CancelOrderAsync(orderId, User.GetUserId());
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderDto dto)
        {
            if(!ModelState.IsValid)
                return View(dto);
            var order = await _orderService.CreateOrderAsync(User.GetUserId(), dto);
            return RedirectToAction("Index");
        }
        

    }
    
}
