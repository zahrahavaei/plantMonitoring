namespace PlantMonitorring.Entity
{
    public class Sensor
    {
        public int Id { get; set; }
        public string Type { get; set; } 

        public string Location { get; set; }
        public bool IsActive { get; set; } = true;
        public string Unit { get; set; } 

        public int PlantId { get; set; }
        public Plant Plant { get; set; } // Navigation property for related plant data

       public ICollection<PlantSensorData> PlantSensorData { get; set; } = new List<PlantSensorData>();
        
    }
}
