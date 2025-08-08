using PlantMonitorring.Entity;

namespace PlantMonitorring.Models
{
    public class PlantDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public DateOnly Date { get; set; }

        public TimeOnly Time { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; } = true;
       

    }
}
