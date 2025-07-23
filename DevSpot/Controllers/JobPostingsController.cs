using DevSpot.Models;
using DevSpot.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

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
        [HttpPost("creatPosting")]
        public async Task<IActionResult> createPosting()
        {
            return NotFound();
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
