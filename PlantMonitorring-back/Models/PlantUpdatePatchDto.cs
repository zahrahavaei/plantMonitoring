namespace PlantMonitorring.Models
{
    public class PlantUpdatePatchDto
    {
        public string? Name { get; set; } = string.Empty;
        public string? Species { get; set; }


        public string? Location { get; set; } = string.Empty;
        public string? Description { get; set; }


        public DateOnly? PlantingDate { get; set; }
        public TimeOnly? PlantingTime { get; set; }
      //  public IFormFile? Image { get; set; }  because jsonPatch does not support Iform 


        public bool? IsActive { get; set; } = true;
    }
}
