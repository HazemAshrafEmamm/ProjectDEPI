using BLL.Dtos.Consultion;
using BLL.Dtos.Doctor;
using BLL.Services.AbstractServices.ConsultationModule;
using BLL.Services.AbstractServices.Users;
using Microsoft.AspNetCore.Mvc;
using PL.Extention;
using PresentationLayer.Controller;
using System.Threading.Tasks;

namespace PL.Controllers
{
    public class ConsultationController(IConsultationService _consultationService) : ApiControllerBase
    {
        [HttpGet("GetAllDoctors")]
        public async Task<IActionResult> GetAllDoctors([FromQuery] SearchDoctorDto searchDto)
        {
            var doctors = await _consultationService.SearchDoctorsAsync(searchDto);
            return Ok(doctors);
        }

        [HttpPost("RequestConsultation")]
        public async Task<IActionResult> RequestConsultation([FromBody] CreateConsultationDto createConsultationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = ClaimsPrincipalExtensions.GetUserId(User);
            var cons = await _consultationService.RequestConsultationAsync(userId, createConsultationDto);

            return Ok(cons);
        }
        [HttpGet]
        [HttpGet("MyConsultations")]
        public async Task<IActionResult> MyConsultations()
        {
            var userId = ClaimsPrincipalExtensions.GetUserId(User);
            var consultations = await _consultationService.GetMyConsultationsAsync(userId);

            return Ok(consultations);
        }


        [HttpGet("GetMyConsultationById/{id}")]
        public async Task<IActionResult> GetMyConsultationById(int id)
        {
            var userId = ClaimsPrincipalExtensions.GetUserId(User);
            var consultation = await _consultationService.GetConsultationByIdAsync(userId, id);

            return Ok(consultation);
        }
        [HttpDelete("DeleteConsultation/{id}")]
        public async Task<IActionResult> DeleteConsultation(int id, [FromQuery] int requesterId)
        {

                await _consultationService.DeleteConsultationAsync(id, requesterId);
                return NoContent();
            
        }

    }
}
