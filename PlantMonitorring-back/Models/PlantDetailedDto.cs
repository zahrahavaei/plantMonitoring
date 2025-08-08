namespace PlantMonitorring.Models
{
    public class PlantDetailedDto
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

        

        public ICollection<SensorBasicDto> Sensors { get; set; } = 
            new List<SensorBasicDto>();
        public ICollection<PlantSensorDataBasicDto> PlantSensorsData { get; set; } =
            new List<PlantSensorDataBasicDto>();
    }
}
