using DevSpot.Data;
using DevSpot.Models.Dtos;
using DevSpot.Models.Entities;
using DevSpot.Repositories;


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

        public async Task<bool?> DeletePostingById(int id,  string userId)
        {
            var posting = await _repository.GetById(id);
            if (posting == null) {
                return null;
            }
            if (posting.UserId != userId) 
            {
                return false;
            }
            await _repository.Delete(id);
            return true;
        }

        public async Task<IEnumerable<JobPostingResponseDto>> GetAllPostings()
        {
            return await _repository.GetAllWithUserName();
        }

        public async Task<JobPostingResponseDto?> GetPostingById(int id)
        {
            return await _repository.GetByIdWithUserName(id);
        }

        public async Task<bool?> UpdatePosting(CreateJobPostingDto dto, int id, string userId)
        {
            var posting = await _repository.GetById(id);
            if (posting == null)
                return null;

            if (posting.UserId != userId)
                return false;

            // update fields
            posting.Title = dto.Title;
            posting.Description = dto.Description;
            posting.Company = dto.Company;
            posting.Location = dto.Location;

            await _repository.Update(posting);
            return true;
        }
    }
}
