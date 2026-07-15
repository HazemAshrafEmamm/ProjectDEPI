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
        #region Patient - Functionality
        [Authorize(Roles = "PATIENT")]
        [HttpGet("MyOrders")]
        public async Task<IActionResult> GetMyOrders()
        {
            var orders = await _orderService.GetMyOrdersAsync(User.GetUserId());

            return Ok(orders);
        }
        [Authorize(Roles = "PATIENT")]
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteOrder(int OrderId)
        {
            await  _orderService.DeleteOrderAsync(OrderId,User.GetUserId());

            return NoContent();
        }

        [Authorize(Roles = "PATIENT")]
        [HttpGet("GetMyOrder/{orderId}")]
        public async Task<IActionResult> GetMyOrder(int orderId)
        {
            var order = await _orderService.GetOrderAsync(orderId, User.GetUserId());

            return Ok(order);
        }
        [Authorize(Roles = "PATIENT")]
        [HttpPost("Cancel/{orderId}")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var order = await _orderService.CancelOrderAsync(orderId, User.GetUserId());

            return Ok(order);
        }
        [Authorize(Roles = "PATIENT")]
        [HttpPost("Create")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var order = await _orderService.CreateOrderAsync(User.GetUserId(), dto);
            return Ok(order);
        }

        #endregion


        #region Pharmcist - Functionality 


        [Authorize(Roles = "PHARMACIST")]
        [HttpGet("Orders")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }
        [Authorize(Roles = "PHARMACIST")]
        [HttpGet("GetOrder/{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _orderService.GetOrderForMerchantAsync(id);

            return Ok(order);
        }
        [Authorize(Roles = "PHARMACIST")]
        [HttpPut("Orders/{orderId}/Status")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatus dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var order = await _orderService.UpdateOrderStatusAsync(orderId, dto);

            return Ok(order);
        } 
        #endregion

    }
}
    

