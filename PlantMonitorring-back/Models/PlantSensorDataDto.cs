using PlantMonitorring.Entity;

namespace PlantMonitorring.Models
{
    public class PlantSensorDataDto
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }


        public double Value { get; set; }

        public int SensorId { get; set; }
       
        public int PlantId { get; set; }
    }
}
