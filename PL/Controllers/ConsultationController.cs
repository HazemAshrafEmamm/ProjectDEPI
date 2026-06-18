using BLL.Dtos.Consultion;
using BLL.Dtos.Doctor;
using BLL.Services.AbstractServices.ConsultationModule;
using BLL.Services.AbstractServices.Users;
using Microsoft.AspNetCore.Mvc;
using PL.Extention;
using System.Threading.Tasks;

namespace PL.Controllers
{
    public class ConsultationController(IDoctorService _doctorService, IConsultationService _consultationService) : Controller
    {
        public async Task<IActionResult> Index([FromBody] SearchDoctorDto searchDto)
        {
            var doctors = await _doctorService.SearchDoctorsAsync(searchDto);
            return View(doctors);
        }
        [HttpPost]
        public async Task<IActionResult> RequestConsultation(CreateConsultationDto createConsultationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var UserId = ClaimsPrincipalExtensions.GetUserId(User);
            var cons = await _consultationService.RequestConsultationAsync(UserId, createConsultationDto);
            if (cons == null)
            {
                return BadRequest("Failed to request consultation.");
            }
            return RedirectToAction("MyConsultations"); //to do
        }
        [HttpGet]
        public async Task<IActionResult> MyConsultations()
        {
            var UserId = ClaimsPrincipalExtensions.GetUserId(User);
            var consultations = await _consultationService.GetMyConsultationsAsync(UserId);
            if (consultations == null)
                return BadRequest("Failed to retrieve consultations.");
            return View(consultations);
        }
        
        [HttpGet("/{Id}")]
        public async Task<IActionResult> GetMyConsultationById(int Id)
        {
            var UserId = ClaimsPrincipalExtensions.GetUserId(User);
            var consultation = await _consultationService.GetConsultationByIdAsync(UserId, Id);
            if (consultation == null)
                return BadRequest("Failed to retrieve consultation.");
            return View(consultation);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteConsultation(int Id, int RequeterId)
        {
            try
            {
                await _consultationService.DeleteConsultationAsync(Id, RequeterId);
                return RedirectToAction("MyConsultations");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Consultation not found.");
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while deleting the consultation.");
            }
        }

    }
}
