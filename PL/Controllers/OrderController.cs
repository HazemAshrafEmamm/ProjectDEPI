using BLL.Dtos.Order;
using BLL.Services.AbstractServices.MedicationModule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PL.Extention;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PL.Controllers
{
    [Authorize("Patient")]
    public class OrderController(IOrderService _orderService) : Controller
    {

        public async Task<IActionResult> Index()
        {

            var Orders = await _orderService.GetMyOrdersAsync(User.GetUserId());
            return View();
        }
        public async Task<IActionResult> Details(int orderId)
        {
            var order = await _orderService.GetOrderAsync(orderId, User.GetUserId());
            return View(order);
        }
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
            var order = await _orderService.CreateOrderAsync(User.GetUserId(), dto);
            return RedirectToAction("Index");
        }








    }
    
}
