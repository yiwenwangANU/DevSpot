using System.ComponentModel.DataAnnotations;

namespace DevSpot.Models.Dtos
{
    public class CreateJobPostingDto
    {
        [Required(ErrorMessage = "Title is required.")]
        [MinLength(1, ErrorMessage = "Title cannot be empty.")]
        public required string Title { get; set; }
        [Required]
        public required string Description { get; set; }
        [Required]
        public required string Company { get; set; }
        [Required]
        public required string Location { get; set; }
    }
}
