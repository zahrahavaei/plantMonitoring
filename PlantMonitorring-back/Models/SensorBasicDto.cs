namespace PlantMonitorring.Models
{
    public class SensorBasicDto
    {
        public int Id { get; set; }
        public string Type { get; set; }

        public string Location { get; set; }


        public bool IsActive { get; set; } = true;
        public string Unit { get; set; }
       
    }
}
