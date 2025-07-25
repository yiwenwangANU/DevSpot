using System.ComponentModel.DataAnnotations;

namespace DevSpot.Models.Dtos
{
    public class JobPostingResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime PostedDate { get; set; }
        public required string UserName {  get; set; }
    }
}
