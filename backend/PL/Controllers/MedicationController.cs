using BLL.Dtos.Medication;
using BLL.Services.AbstractServices.MedicationModule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PL.Extention;
using PL.Models.MedicationModels;
using PresentationLayer.Controller;

namespace PL.Controllers
{
    public class MedicationController(IMedicationService _medicationService) : ApiControllerBase
    {
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllMedications(string? SearchName)
        {
            var medications = await _medicationService.GetAllMedicationsAsync(SearchName);

            return Ok(medications);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetMedicationById(int id)
        {
            var medication = await _medicationService.GetMedicationByIdAsync(id);
            return Ok(medication);
        }

        #region Pharmcist - Functionality 
        [Authorize(Roles = "PHARMACIST,ADMIN")]
        [HttpPost("UpdateMedication/{Id}")]
        public async Task<IActionResult> UpdateMedication(int Id, UpdateMedicationModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var medication = await _medicationService.GetMedicationByIdAsync(Id);
            medication.Id = Id;
            medication.Name = model.Name;
            medication.Price = model.Price;
            medication.Stock = model.Stock;
            medication.IsAvailable = model.IsAvailable;
            await _medicationService.UpdateMedicationAsync(User.GetUserId(), medication);

            return Ok(medication);
        }
        [Authorize(Roles = "PHARMACIST,ADMIN")]
        [HttpPost("CreateMedication")]
        public async Task<IActionResult> CreateMedication([FromForm] CreateMedicationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var medication = await _medicationService.CreateMedicationAsync(User.GetUserId(), dto);

            return Ok(medication);
        }
        [Authorize(Roles = "PHARMACIST,ADMIN")]
        [HttpDelete("DeleteMedication/{id}")]
        public async Task<IActionResult> DeleteMedication(int id)
        {
            await _medicationService.DeleteMedicationAsync(User.GetUserId(), id);
            return NoContent();
        } 
        #endregion
    }
}
