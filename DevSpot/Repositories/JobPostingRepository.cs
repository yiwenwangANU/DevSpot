using DevSpot.Data;
using DevSpot.Models.Dtos;
using DevSpot.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevSpot.Repositories
{
    public class JobPostingRepository 
    {
        private readonly ApplicationDbContext _context;
        public JobPostingRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Add(JobPosting entity)
        {
            await _context.JobPostings.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var jobPosting = await _context.JobPostings.FindAsync(id);
            if (jobPosting == null) return;

            _context.JobPostings.Remove(jobPosting);  
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<JobPosting>> GetAll()
        {
            return await _context.JobPostings.ToListAsync();
        }
        public async Task<IEnumerable<JobPostingResponseDto>> GetAllWithUserName()
        {
            return await _context.JobPostings
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
        }
        public async Task<JobPosting?> GetById(int id)
        {
            return await _context.JobPostings.FindAsync(id);
        }

        public async Task<JobPostingResponseDto?> GetByIdWithUserName(int id)
        {
            return await _context.JobPostings
                .Include(j => j.User)
                .Where(j => j.Id == id)
                .Select(j => new JobPostingResponseDto
                {
                    Title = j.Title,
                    Description = j.Description,
                    Location = j.Location,
                    Company = j.Company,
                    PostedDate = j.PostedDate,
                    UserName = j.User != null ? j.User.UserName! : "Unknown"
                })
                .FirstOrDefaultAsync();
        }

        public async Task Update(JobPosting entity)
        {   
            _context.JobPostings.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
