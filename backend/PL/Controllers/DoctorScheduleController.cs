using BLL.Dtos.Schedule;
using BLL.Services.AbstractServices.AppointmentModule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PL.Extention;
using PresentationLayer.Controller;
using System.Threading.Tasks;

namespace PL.Controllers
{
    [Authorize(Roles = "DOCTOR")]
    public class DoctorScheduleController(IDoctorScheduleService _scheduleService) : ApiControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateSchedule([FromBody] CreateDoctorScheduleDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var schedule = await _scheduleService.CreateScheduleAsync(User.GetUserId(), dto);
            return Ok(schedule);
        }

        [HttpGet("Doctor")]
        public async Task<IActionResult> GetDoctorSchedules()
        {   
            var schedules = await _scheduleService.GetDoctorSchedulesAsync(User.GetUserId());
            return Ok(schedules);
        }

        [HttpPut("{scheduleId}")]
        public async Task<IActionResult> UpdateSchedule(int scheduleId, [FromBody] UpdateDoctorScheduleDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var schedule = await _scheduleService.UpdateScheduleAsync(scheduleId, dto, User.GetUserId());
            return Ok(schedule);
        }

        [HttpDelete("{scheduleId}")]
        public async Task<IActionResult> DeleteSchedule(int scheduleId)
        {
            var result = await _scheduleService.DeleteScheduleAsync(scheduleId, User.GetUserId());
            if (!result)
                return NotFound("Schedule not found or you are not authorized to delete it.");

            return NoContent();
        }
    }
}
