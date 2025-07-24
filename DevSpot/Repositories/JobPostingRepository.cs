using DevSpot.Data;
using DevSpot.Models.Entities;
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

        public async Task<JobPosting> GetById(int id)
        {
            return await _context.JobPostings.FindAsync(id);
        }

        public async Task Update(JobPosting entity)
        {   
            _context.JobPostings.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
