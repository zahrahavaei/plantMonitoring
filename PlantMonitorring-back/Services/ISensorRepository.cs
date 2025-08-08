using PlantMonitorring.Entity;

namespace PlantMonitorring.Services
{
    public interface ISensorRepository
    {
        Task<IEnumerable<Sensor>> GetAllSensorGreenHouseAsync(string GreenHouseName);

        
    }
}
