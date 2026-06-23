using BLL.Dtos.Medication;
using BLL.Dtos.Order;
using BLL.Services.AbstractServices.MedicationModule;
using Microsoft.AspNetCore.Mvc;
using PL.Models.MedicationModels;

namespace PL.Controllers
{
    public class PharmacistController(IOrderService _orderService , IMedicationService _medicationService) : Controller
    {
        [HttpGet("Medications")]
        public async Task<IActionResult> GetAllMedications()
        {
            var medications = await _medicationService.GetAllMedicationsAsync();
            if (medications == null)
                return NotFound("No medications found.");

            return Ok(medications);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateMedication(int Id , UpdateMedicationModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var medication = await _medicationService.GetMedicationByIdAsync(Id);
            if (medication == null)
                return NotFoundException($"Medication with ID {Id} not found.");
            medication.Id = Id;
            medication.Name = model.Name;
            medication.Price = model.Price;
            medication.Stock = model.Stock;
            medication.Is_available = model.Is_available;
            await _medicationService.UpdateMedicationAsync(medication);

            return Ok(medication);
        }
        [HttpGet("Medications/{id}")]
        public async Task<IActionResult> GetMedicationById(int id)
        {
            var medication = await _medicationService.GetMedicationByIdAsync(id);
            if (medication == null)
                return NotFound($"Medication with ID {id} not found.");

            return Ok(medication);
        }
        [HttpPost("Medications")]
        public async Task<IActionResult> CreateMedication([FromBody] CreateMedicationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var medication = await _medicationService.CreateMedicationAsync(dto);
            if (medication == null)
                return BadRequest("Failed to create medication.");

            return CreatedAtAction(nameof(GetMedicationById), new { id = medication.Id }, medication);
        }

        [HttpDelete("Medications/{id}")]
        public async Task<IActionResult> DeleteMedication(int id)
        {
            var medication = await _medicationService.GetMedicationByIdAsync(id);
            if (medication == null)
                return NotFound($"Medication with ID {id} not found.");

            await _medicationService.DeleteMedicationAsync(id);
            return NoContent();
        }


        [HttpGet("Orders")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            if (orders == null)
                return NotFound("No orders found.");

            return Ok(orders);
        }

        [HttpGet("Orders/{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _orderService.GetOrderForMerchantAsync(id);
            if (order == null)
                return NotFoundException($"Order with ID {id} not found.");

            return Ok(order);
        }
        [HttpPut("Orders/{orderId}/Status")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatus dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var order = await _orderService.UpdateOrderStatusAsync(orderId, dto);
            if (order == null)
                return NotFoundException($"Order with ID {orderId} not found.");

            return Ok(order);
        }

    }
}
