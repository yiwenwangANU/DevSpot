using DevSpot.Models;
using DevSpot.Models.Dtos;
using DevSpot.Models.Entities;
using DevSpot.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        public async Task<IActionResult> createPosting(JobPostingDto jobPostingDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) { return BadRequest()};
            var entity = new JobPosting
            {
                Title = jobPostingDto.Title,
                Description = jobPostingDto.Description,
                Location = jobPostingDto.Location,
                Company = jobPostingDto.Company,
                UserId = userId
            };
            await _repository.AddAsync(JobPosting entity)
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
    }
}
