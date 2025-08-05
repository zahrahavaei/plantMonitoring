using PlantMonitorring.Entity;
using PlantMonitorring.Services.Response;

namespace PlantMonitorring.Services
{
    public interface IPlantRepositort
    {
        Task<IEnumerable<Plant>> GetAllPlantsAsync();

        Task<Plant?> GetPlantByIdAsync(int id, bool includeSensor, bool includePlantSensorData);

        Task<PlantResponse> AddPlantAsync(Plant plant);

        Task<PlantResponse> UpdatePlantPutAsync(Plant plant);
    }
}
