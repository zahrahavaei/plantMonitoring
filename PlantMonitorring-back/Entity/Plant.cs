namespace PlantMonitorring.Entity
{
    public class Plant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Species { get; set; }
        public string Location { get; set; }
        public string? Description { get; set; }
        public DateTime PlantingDate { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; } = true;
 
       public ICollection<Sensor> Sensor { get; set; } = new List<Sensor>();
        public ICollection<PlantSensorData> PlantSensorData { get; set; } = new List<PlantSensorData>();


    }
}
