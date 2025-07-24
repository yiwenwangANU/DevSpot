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
        private readonly IRepository _repository;
        public JobPostingService(IRepository repository)
        {
            _repository = repository;
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
            var response = await _repository.GetByIdWithUserName(entity.Id);

            return response == null ? throw new Exception("Job posting could not be retrieved after creation.") : response;
        }

        public async Task DeletePostingById(int id)
        {
            await _repository.Delete(id);
        }

        public async Task<IEnumerable<JobPostingResponseDto>> GetAllPostings()
        {
            return await _repository.GetAllWithUserName();
        }

        public async Task<JobPostingResponseDto> GetPostingById(int id)
        {
            var response = await _repository.GetByIdWithUserName(id);
            return response == null ? throw new Exception("Job posting could not be retrieved.") : response;
        }

        public async Task UpdatePosting(CreateJobPostingDto dto, string userId)
        {
            var entity = new JobPosting
            {
                Title = dto.Title,
                Description = dto.Description,
                Location = dto.Location,
                Company = dto.Company,
                UserId = userId
            };
            await _repository.Update(entity);
        }
    }
}
