using PlantMonitorring.Entity;

namespace PlantMonitorring.Models
{
    public class PlantSensorDataBasicDto
    {
       public int Id {  get; set; }  
        public Double Value { get; set; }
        public TimeOnly Time  { get; set; }
        public DateOnly Date { get; set; }

        public string SensorType { get; set; }
    }


}
