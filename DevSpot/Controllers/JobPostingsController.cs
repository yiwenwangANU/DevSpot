using DevSpot.Models.Dtos;
using DevSpot.Models.Entities;
using DevSpot.Repositories;
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
        private readonly IRepository<JobPosting> _repository;
        private readonly UserManager<IdentityUser> _userManager;
        public JobPostingsController(
            IRepository<JobPosting> repository,
            UserManager<IdentityUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }
        [Authorize]
        [HttpPost("createPosting")]
        public async Task<IActionResult> createPosting(JobPostingDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) { return Unauthorized(); }
            var entity = new JobPosting
            {
                Title = dto.Title,
                Description = dto.Description,
                Location = dto.Location,
                Company = dto.Company,
                UserId = userId
            };
            await _repository.AddAsync(entity);
            return Ok(entity);
        }
        [HttpGet("getPostings")]
        public async Task<IActionResult> GetPostings()
        {
            var jobPostings = await _repository.GetAllAsync();
            return Ok(new { jobPostings });
        }
        [HttpGet("getPosting/{id}")]
        public async Task<IActionResult> GetPostingById(int id)
        {
            var jobPosting = await _repository.GetAsync(id);
            if (jobPosting == null) return NotFound();
            return Ok(new { jobPosting });
        }
        [HttpDelete("deletePosting/{id}")]
        public async Task<IActionResult> DeletePostingById(int id)
        {
            var jobPosting = await _repository.GetAsync(id);
            if(jobPosting == null) return NotFound();
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
