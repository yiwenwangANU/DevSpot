using DevSpot.Data;
using DevSpot.Models.Dtos;
using DevSpot.Models.Entities;
using DevSpot.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DevSpot.Services
{
    public class JobPostingService : IJobPostingService
    {
        private readonly IRepository<JobPostingRepository> _repository;
        private readonly ApplicationDbContext _context;
        public JobPostingService(IRepository<JobPostingRepository> repository, ApplicationDbContext context)
        {
            _repository = repository;
            _context = context;
        }
        public async Task<JobPostingResponseDto> CreateNewPosting(CreateJobPostingDto dto, string userId)
        {
       
            var entity = new JobPosting
            {
                Title = dto.Title,
                Description = dto.Description,
                Location = dto.Location,
                Company = dto.Company,
                UserId = userId
            };
            await _repository.Add(entity);
            var response = await _context.JobPostings
            .Include(j => j.User)
            .Where(j => j.Id == entity.Id)
            .Select(j => new JobPostingResponseDto
            {
                Title = j.Title,
                Description = j.Description,
                Location = j.Location,
                Company = j.Company,
                PostedDate = j.PostedDate,
                UserName = j.User!.UserName!
            })
            .FirstOrDefaultAsync();

            return response == null ? throw new Exception("Job posting could not be retrieved after creation.") : response;
        }

        public async Task DeletePostingById(int id)
        {
            await _repository.Delete(id);
        }

        public async Task<IEnumerable<JobPostingResponseDto>> GetAllPostings()
        {
            var postings = await _context.JobPostings
        .Include(j => j.User)
        .Select(j => new JobPostingResponseDto
        {
            Title = j.Title,
            Description = j.Description,
            Location = j.Location,
            Company = j.Company,
            PostedDate = j.PostedDate,
            UserName = j.User != null ? j.User.UserName! : "Unknown"
        })
        .ToListAsync();

            return postings;
        }

        public Task<JobPostingResponseDto> GetPostingById(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdatePosting(CreateJobPostingDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
