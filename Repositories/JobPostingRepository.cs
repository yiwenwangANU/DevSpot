using DevSpot.Data;
using DevSpot.Models;
using Microsoft.EntityFrameworkCore;

namespace DevSpot.Repositories
{
    public class JobPostingRepository : IRepository<JobPosting>
    {
        private readonly ApplicationDbContext _context;
        public JobPostingRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(JobPosting entity)
        {
            await _context.JobPostings.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var jobPosting = await _context.JobPostings.FindAsync(id);
            if(jobPosting != null)
            {
                _context.JobPostings.Remove(jobPosting); 
                await _context.SaveChangesAsync();
            }
            
        }

        public async Task<IEnumerable<JobPosting>> GetAllAsync()
        {
            return await _context.JobPostings.ToListAsync();
        }

        public async Task<JobPosting> GetAsync(string id)
        {
            return await _context.JobPostings.FindAsync(id);
        }

        public Task UpdateAsync(JobPosting entity)
        {
            throw new NotImplementedException();
        }
    }
}
