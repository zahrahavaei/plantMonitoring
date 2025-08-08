using PlantMonitorring.Entity;

namespace PlantMonitorring.Services
{
    public interface IPlantSensorDataRepository
    {
         Task<PlantSensorData> PostSensorDataAsync(int plantId,
             int SensorId,
            DateOnly date,
            TimeOnly time,
            double Value);

        Task<IEnumerable<PlantSensorData>> GetSensorDataAsync(int sensorId);

        Task<IEnumerable<PlantSensorData>> GetSensorDataByGreenHouseAsync(string greenHouseName);
    }
}
