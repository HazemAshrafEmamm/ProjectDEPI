using BLL.Dtos.Order;
using BLL.Services.AbstractServices.MedicationModule;
using BLL.Services.ImplementationService.MedicationModule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PL.Extention;
using PresentationLayer.Controller;

namespace PL.Controllers
{
    [Authorize(Roles = "Patient")]
    public class BasketController(IBasketService _basketService) : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetBasket()
        {
            var basket = await _basketService.GetBasketAsync(User.GetUserId());
            return Ok(basket);
        }

        [HttpPost("AddItem")]
        public async Task<IActionResult> AddItem([FromBody] BasketItemInputDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var basket = await _basketService.AddItemAsync(User.GetUserId(), dto);
            return Ok(basket);
        }

        [HttpPut("UpdateItem/{medicationId}")]
        public async Task<IActionResult> UpdateItem(int medicationId, [FromBody] int quantity)
        {
            var basket = await _basketService.UpdateItemQuantityAsync(User.GetUserId(), medicationId, quantity);
            return Ok(basket);
        }

        [HttpDelete("RemoveItem/{medicationId}")]
        public async Task<IActionResult> RemoveItem(int medicationId)
        {
            await _basketService.RemoveItemAsync(User.GetUserId(), medicationId);
            return NoContent();
        }

        [HttpDelete("Clear")]
        public async Task<IActionResult> ClearBasket()
        {
            await _basketService.ClearBasketAsync(User.GetUserId());
            return NoContent();
        }
    }

}
