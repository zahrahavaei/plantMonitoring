using PlantMonitorring.Entity;

namespace PlantMonitorring.Models
{
    public class SensorDto
    {
      
            public int Id { get; set; }
            public string Type { get; set; }

            public string Location { get; set; }

            public int PlantId { get; set; }

        public bool IsActive { get; set; } = true;
        public string Unit { get; set; }

    }

}
