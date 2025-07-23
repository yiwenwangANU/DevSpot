using System.ComponentModel.DataAnnotations;

namespace DevSpot.Models.Entities
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Family { get; set; } = string.Empty;
    }
}
