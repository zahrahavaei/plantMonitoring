using PlantMonitorring.Entity;

namespace PlantMonitorring.Models
{
    public class PlantSensorDataPostDto
    {
       
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }

        public double Value { get; set; }

        public int SensorId { get; set; }
       

        public int PlantId { get; set; }
    }
}
