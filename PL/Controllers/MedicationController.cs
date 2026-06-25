using BLL.Services.AbstractServices.MedicationModule;
using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class MedicationController(IMedicationService _medicationService) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetAllMedications()
        {
            var medications = await _medicationService.GetAllMedicationsAsync();
            return View(medications);
        }
        [HttpGet("Id")]
        public async Task<IActionResult> GetMedicationById(int Id)
        {
            var medication = await _medicationService.GetMedicationByIdAsync(Id);
            return View(medication);
        }
    }
}
