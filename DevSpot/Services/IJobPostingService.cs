using DevSpot.Models.Dtos;

namespace DevSpot.Services
{
    public interface IJobPostingService 
    {
        Task<JobPostingResponseDto> CreateNewPosting(CreateJobPostingDto dto, string userId);
        Task<IEnumerable<JobPostingResponseDto>> GetAllPostings();
        Task<JobPostingResponseDto> GetPostingById(int id);
        Task DeletePostingById(int id);
        Task<bool> UpdatePosting(CreateJobPostingDto dto, int id, string userId);
    }
}
