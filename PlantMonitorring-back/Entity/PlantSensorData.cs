namespace PlantMonitorring.Entity
{
    public class PlantSensorData
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
       
       
        public double Value { get; set; } 
         
        public int SensorId { get; set; }
        public Sensor Sensor { get; set; } // Navigation property for related sensor data


        public Plant Plant { get; set; }   //navigation property 
        public int PlantId { get; set; }
    }
}
