using BLL.Services.AbstractServices.MedicationModule;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Controller;

namespace PL.Controllers
{
    public class MedicationController(IMedicationService _medicationService) : ApiControllerBase
    {
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllMedications()
        {
            var medications = await _medicationService.GetAllMedicationsAsync();
            if (medications == null)
                return NotFound("No medications found.");

            return Ok(medications);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetMedicationById(int id)
        {
            var medication = await _medicationService.GetMedicationByIdAsync(id);
            if (medication == null)
                return NotFound($"Medication with ID {id} not found.");

            return Ok(medication);
        }
    }
}
