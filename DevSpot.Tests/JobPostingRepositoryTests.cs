using DevSpot.Data;
using DevSpot.Models;
using DevSpot.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;


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
                Title = "Test",
                Description = "Test Dec",
                PostedDate = DateTime.Now,
                Company = "Test Company",
                Location = "Test Location",
                UserId = "Test userId"
            };
            await repository.AddAsync(jobPosting);
            var result = db.JobPostings.SingleOrDefault(x => x.Title == "Test");
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
                Title = "Test",
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
    }
}
