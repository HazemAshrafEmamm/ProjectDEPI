using BLL.Dtos.Consultion;
using BLL.Services.AbstractServices.ConsultationModule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PL.Extention;
using PresentationLayer.Controller;
using System.Threading.Tasks;

namespace PL.Controllers
{
    [Authorize]
    public class ConsultationReviewController(IConsultationReviewService _reviewService) : ApiControllerBase
    {
        [Authorize(Roles = "PATIENT")]
        [HttpPost("{consultationId}")]
        public async Task<IActionResult> AddReview(int consultationId, [FromBody] CreateConsultationReviewDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var review = await _reviewService.AddReviewAsync(consultationId, User.GetUserId(), dto);
            return Ok(review);
        }

        [HttpGet("{consultationId}")]
        public async Task<IActionResult> GetReview(int consultationId)
        {
            var review = await _reviewService.GetConsultationReviewsByConsultationId(consultationId);
            if (review == null)
                return NotFound("Review not found for this consultation.");

            return Ok(review);
        }
    }
}
