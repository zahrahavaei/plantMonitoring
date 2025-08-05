using System.ComponentModel.DataAnnotations;

namespace PlantMonitorring.Models
{
    public class PlantForCreationDto
    {
        [Required(ErrorMessage = "Plant Name Is Required!")]
        public string Name { get; set; } = string.Empty;
        public string? Species { get; set; }

        [Required(ErrorMessage = "Plant Location Is Required!")]
        public string Location { get; set; } = string.Empty;
        public string? Description { get; set; }

        [Required(ErrorMessage = "Planting Date Is Required!")]
        public DateOnly PlantingDate { get; set; }
        public TimeOnly PlantingTime { get; set; }
        public IFormFile Image { get; set; }

        [Required(ErrorMessage = "ISACtive Is Required!")]
        public bool IsActive { get; set; } = true;
    }
}
