namespace PlantMonitorring.Models
{
    public class PlantResultDto
    {
        public string Message { get; set; } = string.Empty;
        public PlantDto PlantDto { get; set; } = new PlantDto();
    }
}
