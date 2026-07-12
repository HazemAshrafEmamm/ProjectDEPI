using BLL.Dtos.Nursing;
using BLL.Services.AbstractServices.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PL.Extention;
using PresentationLayer.Controller;
using System.Threading.Tasks;

namespace PL.Controllers
{
    [Authorize]
    public class NursingController(INursingService _nursingService) : ApiControllerBase
    {
        [AllowAnonymous]
        [HttpGet("Search")]
        public async Task<IActionResult> SearchNurses([FromQuery] SearchNurseDto searchDto)
        {
            var nurses = await _nursingService.SearchNursesAsync(searchDto);
            return Ok(nurses);
        }

        [Authorize(Roles = "Patient")]
        [HttpPost("Request")]
        public async Task<IActionResult> RequestNursing([FromBody] CreateNursingRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var request = await _nursingService.RequestNursingAsync(User.GetUserId(), dto);
            return Ok(request);
        }

        [Authorize(Roles = "Patient,Nurse")]
        [HttpGet("MyRequests")]
        public async Task<IActionResult> GetMyRequests()
        {
            var requests = await _nursingService.GetMyNursingRequestsAsync(User.GetUserId());
            return Ok(requests);
        }

        [Authorize(Roles = "Nurse,Admin")]
        [HttpPut("UpdateStatus/{requestId}")]
        public async Task<IActionResult> UpdateStatus(int requestId, [FromBody] UpdateNursingStatusDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var request = await _nursingService.UpdateNursingStatusAsync(requestId, dto, User.GetUserId());
            return Ok(request);
        }

        [Authorize(Roles = "Patient,Nurse")]
        [HttpPost("Cancel/{requestId}")]
        public async Task<IActionResult> CancelNursing(int requestId)
        {
            var request = await _nursingService.CancelNursingAsync(requestId, User.GetUserId());
            return Ok(request);
        }

        [Authorize(Roles = "Patient")]
        [HttpPost("Review/{requestId}")]
        public async Task<IActionResult> AddReview(int requestId, [FromBody] CreateNursingReviewDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var review = await _nursingService.AddNursingReviewAsync(requestId, User.GetUserId(), dto);
            return Ok(review);
        }

        [HttpGet("Review/{requestId}")]
        public async Task<IActionResult> GetReview(int requestId)
        {
            var review = await _nursingService.GetNursingReviewAsync(requestId);
            if (review == null)
                return NotFound("Review not found for this request.");

            return Ok(review);
        }
    }
}
