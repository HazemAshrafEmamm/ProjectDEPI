using BLL.Dtos.Medication;
using BLL.Dtos.Order;
using BLL.Services.AbstractServices.MedicationModule;
using DAL.Exceptions.OrderModule;
using Microsoft.AspNetCore.Mvc;
using PL.Extention;
using PL.Models.MedicationModels;
using PresentationLayer.Controller;

namespace PL.Controllers
{
    public class PharmacistController(IOrderService _orderService , IMedicationService _medicationService) : ApiControllerBase
    {
        [HttpGet("Medications")]
        public async Task<IActionResult> GetAllMedications()
        {
            var medications = await _medicationService.GetAllMedicationsAsync();

            return Ok(medications);
        }


        [HttpPost("UpdateMedication/{Id}")]
        public async Task<IActionResult> UpdateMedication(int Id , UpdateMedicationModel model)
        {
            var userId = ClaimsPrincipalExtensions.GetUserId(User); 
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var medication = await _medicationService.GetMedicationByIdAsync(Id);
            medication.Id = Id;
            medication.Name = model.Name;
            medication.Price = model.Price;
            medication.Stock = model.Stock;
            medication.Is_available = model.Is_available;
            await _medicationService.UpdateMedicationAsync(userId, medication);

            return Ok(medication);
        }
        [HttpGet("Medication/{id}")]
        public async Task<IActionResult> GetMedicationById(int id)
        {
            var medication = await _medicationService.GetMedicationByIdAsync(id);

            return Ok(medication);
        }
        [HttpPost("CreateMedication")]
        public async Task<IActionResult> CreateMedication([FromBody] CreateMedicationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var userId = ClaimsPrincipalExtensions.GetUserId(User);


            var medication = await _medicationService.CreateMedicationAsync(userId, dto);

            return CreatedAtAction(nameof(GetMedicationById), new { id = medication.Id }, medication);
        }

        [HttpDelete("DeleteMedication/{id}")]
        public async Task<IActionResult> DeleteMedication(int id)
        {
            var userId = ClaimsPrincipalExtensions.GetUserId(User);
            var medication = await _medicationService.GetMedicationByIdAsync(id);

            await _medicationService.DeleteMedicationAsync(userId, id);
            return NoContent();
        }


        [HttpGet("Orders")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("Order/{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _orderService.GetOrderForMerchantAsync(id);

            return Ok(order);
        }
        [HttpPut("Orders/{orderId}/Status")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatus dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var order = await _orderService.UpdateOrderStatusAsync(orderId, dto);

            return Ok(order);
        }

    }
}
