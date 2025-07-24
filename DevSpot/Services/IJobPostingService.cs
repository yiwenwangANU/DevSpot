using DevSpot.Models.Dtos;

namespace DevSpot.Services
{
    public interface IJobPostingService
    {
        Task<T> CreateNewPost(CreateJobPostingDto);
    }
}
