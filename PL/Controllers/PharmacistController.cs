using BLL.Dtos.Medication;
using BLL.Dtos.Order;
using BLL.Services.AbstractServices.MedicationModule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PL.Models.MedicationModels;
using System.Threading.Tasks;

namespace PL.Controllers
{
    [Authorize(Roles = "Pharmacist")]
    [ValidateAntiForgeryToken]
    public class PharmacistController(IOrderService _orderService , IMedicationService _medicationService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var medications = await _medicationService.GetAllMedicationsAsync();
            return View(medications);
        }
        [HttpGet("Medication/{Id}")]
        public async Task<IActionResult> GetMedicationById(int Id)
        {
            var medication = await _medicationService.GetMedicationByIdAsync(Id);
            return View(medication);
        }
        [HttpGet]
        public async Task<IActionResult> UpdateMedication(int Id)
        {
            var medication = await _medicationService.GetMedicationByIdAsync(Id);
            return View(medication);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateMedication(int Id , UpdateMedicationModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var medication = await _medicationService.GetMedicationByIdAsync(Id);
            if (medication == null)
                return NotFound();
            medication.Id = Id;
            medication.Name = model.Name;
            medication.Price = model.Price;
            medication.Stock = model.Stock;
            medication.Is_available = model.Is_available;
            await _medicationService.UpdateMedicationAsync(medication);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult CreateMedication()
        {            
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateMedication(CreateMedicationDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _medicationService.CreateMedicationAsync(dto);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMedication(int Id)
        {

            await _medicationService.DeleteMedicationAsync(Id);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return View(orders);
        }

        [HttpGet("Order/{Id}")]
        public async Task<IActionResult> GetOrderForMerchantAsync(int Id)
        {
            var order = await _orderService.GetOrderForMerchantAsync(Id);
            return View(order);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, UpdateOrderStatus dto)
        {
            if (!ModelState.IsValid)
                return View(dto);
            var order = await _orderService.UpdateOrderStatusAsync(orderId, dto);
            return View(order);
        }

    }
}
