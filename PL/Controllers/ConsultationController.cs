using BLL.Dtos.Consultion;
using BLL.Dtos.Doctor;
using BLL.Services.AbstractServices.ConsultationModule;
using BLL.Services.AbstractServices.Users;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Patient")]
        [HttpPost("RequestConsultation")]
        public async Task<IActionResult> RequestConsultation([FromBody] CreateConsultationDto createConsultationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cons = await _consultationService.RequestConsultationAsync(User.GetUserId(), createConsultationDto);

            return Ok(cons);
        }
        [Authorize(Roles = "Patient,Doctor")]
        [HttpGet("MyConsultations")]
        public async Task<IActionResult> MyConsultations()
        {
            var consultations = await _consultationService.GetMyConsultationsAsync(User.GetUserId());

            return Ok(consultations);
        }

        [Authorize(Roles = "Patient,Doctor")]
        [HttpGet("GetMyConsultationById/{id}")]
        public async Task<IActionResult> GetMyConsultationById(int id)
        {
            var consultation = await _consultationService.GetConsultationByIdAsync(User.GetUserId(), id);

            return Ok(consultation);
        }
        [Authorize(Roles = "Doctor")]
        [HttpDelete("DeleteConsultation/{id}")]
        public async Task<IActionResult> DeleteConsultation(int id)
        {
            await _consultationService.DeleteConsultationAsync(id, User.GetUserId());
            return NoContent();
            
        }

    }
}
