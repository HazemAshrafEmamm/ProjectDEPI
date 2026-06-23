using BLL.Dtos.Order;
using BLL.Services.AbstractServices.MedicationModule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PL.Extention;
using PresentationLayer.Controller;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PL.Controllers
{
    [Authorize]
    public class OrderController(IOrderService _orderService) : ApiControllerBase
    {
        [HttpGet("MyOrders")]
        public async Task<IActionResult> GetMyOrders()
        {
            var orders = await _orderService.GetMyOrdersAsync(User.GetUserId());

            return Ok(orders);
        }

        [HttpGet("Details/{orderId}")]
        public async Task<IActionResult> GetOrder(int orderId)
        {
            var order = await _orderService.GetOrderAsync(orderId, User.GetUserId());

            return Ok(order);
        }

        [HttpPost("Cancel/{orderId}")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var order = await _orderService.CancelOrderAsync(orderId, User.GetUserId());

            return Ok(order);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var order = await _orderService.CreateOrderAsync(User.GetUserId(), dto);


            return Ok(order);
        }
   
    }
}
    

