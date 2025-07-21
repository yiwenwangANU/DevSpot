using DevSpot.Data;
using DevSpot.Models;
using DevSpot.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace DevSpot.Tests
{
    public class JobPostingRepositoryTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;
        public JobPostingRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("JobPostingDb")
                .Options;
        }
        private ApplicationDbContext CreateDbContext() => new ApplicationDbContext(_options);

        [Fact]
        public async Task AddAsync_ShouldAddJobPosting()
        {
            var db = CreateDbContext();
            var repository = new JobPostingRepository(db);
            var jobPosting = new JobPosting
            {
                Title = "Test AddAsyn",
                Description = "Test Dec",
                PostedDate = DateTime.Now,
                Company = "Test Company",
                Location = "Test Location",
                UserId = "Test userId"
            };
            await repository.AddAsync(jobPosting);
            var result = db.JobPostings.SingleOrDefault(x => x.Title == "Test AddAsyn");
            Assert.NotNull(result);
            Assert.Equal("Test Dec", result.Description);
        }
        [Fact]
        public async Task GetAsync_ShouldReturnJobPosting()
        {
            var db = CreateDbContext();
            var repository = new JobPostingRepository(db);
            var jobPosting = new JobPosting
            {
                Title = "Test GetAsync",
                Description = "Test Dec",
                PostedDate = DateTime.Now,
                Company = "Test Company",
                Location = "Test Location",
                UserId = "Test userId"
            };

            await db.JobPostings.AddAsync(jobPosting);
            await db.SaveChangesAsync();

            var result = await repository.GetAsync(jobPosting.Id);
            Assert.NotNull(result);
            Assert.Equal("Test Dec", result.Description);
        }
        [Fact]
        public async Task GetAsync_ShouldThrowKeyNotFoundException()
        {
            var db = CreateDbContext();
            var repository = new JobPostingRepository(db);

            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => repository.GetAsync(999)
            );
        }
        [Fact]
        public async Task GetAllAsync_ShouldReturnAllJobPostings()
        {
            var db = CreateDbContext();
            db.JobPostings.RemoveRange(db.JobPostings);
            await db.SaveChangesAsync();
            var repository = new JobPostingRepository(db);
            var jobPosting1 = new JobPosting
            {
                Title = "Test GetAllAsync 1",
                Description = "Test Dec 1",
                PostedDate = DateTime.Now,
                Company = "Test Company 1",
                Location = "Test Location 1",
                UserId = "Test userId 1"
            };
            var jobPosting2 = new JobPosting
            {
                Title = "Test GetAllAsync 2",
                Description = "Test Dec 2",
                PostedDate = DateTime.Now,
                Company = "Test Company 2",
                Location = "Test Location 2",
                UserId = "Test userId 2"
            };

            var jobPostings = new List<JobPosting> { jobPosting1, jobPosting2 };
            await db.JobPostings.AddRangeAsync(jobPostings);
            await db.SaveChangesAsync();

            var result = await repository.GetAllAsync();
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, x => x.Title == "Test GetAllAsync 1" && x.Description == "Test Dec 1");
            Assert.Contains(result, x => x.Title == "Test GetAllAsync 2" && x.Description == "Test Dec 2");
        }
        [Fact]
        public async Task UpdateAsync_ShouldUpdateJobPosting()
        {
            var db = CreateDbContext();
            var repository = new JobPostingRepository(db);
            var jobPosting = new JobPosting
            {
                Title = "Test UpdateAsync",
                Description = "Test Dec 01",
                PostedDate = DateTime.Now,
                Company = "Test Company",
                Location = "Test Location",
                UserId = "Test userId"
            };

            await db.JobPostings.AddAsync(jobPosting);
            await db.SaveChangesAsync();

            jobPosting.Description = "new description";
            await repository.UpdateAsync(jobPosting);
            var result = db.JobPostings.SingleOrDefault(x => x.Title == "Test UpdateAsync");
            Assert.NotNull(result);
            Assert.Equal("new description", result.Description);
        }
        [Fact]
        public async Task DeleteAsync_ShouldDeleteJobPosting()
        {
            var db = CreateDbContext();
            var repository = new JobPostingRepository(db);

            var jobPosting = new JobPosting
            {
                Title = "Test DeleteAsync",
                Description = "Test Dec",
                PostedDate = DateTime.Now,
                Company = "Test Company",
                Location = "Test Location",
                UserId = "Test userId"
            };

            await db.JobPostings.AddAsync(jobPosting);
            await db.SaveChangesAsync();

            await repository.DeleteAsync(jobPosting.Id);

            var result = db.JobPostings.SingleOrDefault(x => x.Title == "Test DeleteAsync");
            Assert.Null(result);
        }
    }
}
