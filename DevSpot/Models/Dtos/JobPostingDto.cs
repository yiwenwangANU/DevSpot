using System.ComponentModel.DataAnnotations;

namespace DevSpot.Models.Dtos
{
    public class JobPostingDto
    {
        [Required]
        public required string Title { get; set; }
        [Required]
        public required string Description { get; set; }
        [Required]
        public required string Company { get; set; }
        [Required]
        public required string Location { get; set; }
    }
}
