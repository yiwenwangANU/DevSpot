using DevSpot.Models.Dtos;
using DevSpot.Models.Entities;

namespace DevSpot.Repositories
{
    public interface IRepository
    {
        Task<IEnumerable<JobPosting>> GetAll();
        Task<IEnumerable<JobPostingResponseDto>> GetAllWithUserName();
        Task<JobPosting?> GetById(int id);
        Task<JobPostingResponseDto?> GetByIdWithUserName(int id);
        Task Add(JobPosting entity);
        Task Update(JobPosting entity);
        Task Delete(int id);
        
    }
}
