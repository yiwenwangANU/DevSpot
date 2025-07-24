using DevSpot.Models.Dtos;
using DevSpot.Models.Entities;
using DevSpot.Repositories;
using DevSpot.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Security.Claims;

namespace DevSpot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobPostingsController : ControllerBase
    {
        private readonly IJobPostingService _service;
        private readonly UserManager<IdentityUser> _userManager;
        public JobPostingsController(
            IJobPostingService service,
            UserManager<IdentityUser> userManager)
        {
            _service = service;
            _userManager = userManager;
        }
        [Authorize]
        [HttpPost("createPosting")]
        public async Task<IActionResult> createPosting(CreateJobPostingDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) { return Unauthorized(); }
            
            var entity = await _service.CreateNewPosting(dto, userId);
            return Ok(entity);
        }

        [HttpGet("getPostings")]
        public async Task<IActionResult> GetPostings()
        {
            var jobPostings = await _service.GetAllPostings();
            return Ok(new { jobPostings });
        }

        [HttpGet("getPosting/{id}")]
        public async Task<IActionResult> GetPostingById(int id)
        {
            var jobPosting = await _service.GetPostingById(id);
            if (jobPosting == null) return NotFound();
            return Ok(new { jobPosting });
        }

        [Authorize]
        [HttpPost("updatePosting/{id}")]
        public async Task<IActionResult> UpdatePosting(CreateJobPostingDto dto, int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) { return Unauthorized(); }

            var success = await _service.UpdatePosting(dto, id, userId);
            return success ? Ok():Forbid();
        }

        [HttpDelete("deletePosting/{id}")]
        public async Task<IActionResult> DeletePostingById(int id)
        {
            var jobPosting = await _service.GetPostingById(id);
            if(jobPosting == null) return NotFound();
            await _service.DeletePostingById(id);
            return NoContent();
        }
    }
}
