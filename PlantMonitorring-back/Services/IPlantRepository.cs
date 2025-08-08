using PlantMonitorring.Entity;
using PlantMonitorring.Services.Response;

namespace PlantMonitorring.Services
{
    public interface IPlantRepository
    {
        Task<IEnumerable<Plant>> GetAllPlantsAsync();

        Task<Plant?> GetPlantByIdAsync(int id, bool includeSensor, bool includePlantSensorData);

        Task<PlantResponse> AddPlantAsync(Plant plant);

        Task<PlantResponse> UpdatePlantPutAsync(Plant plant);
    }
}
