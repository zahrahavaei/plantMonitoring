using Microsoft.EntityFrameworkCore;
using PlantMonitorring.DBContext;
using PlantMonitorring.Entity;

namespace PlantMonitorring.Services
{
    public class SensorRepository : ISensorRepository
    {
        private readonly PlantDataBaseContext _Context;
        private readonly ILogger<Sensor> _logger;

        public SensorRepository(PlantDataBaseContext context, ILogger<Sensor> logger)
        {
            _Context = context;
            _logger = logger;
        }
        public async Task<IEnumerable<Sensor>> GetAllSensorGreenHouseAsync(string GreenHouseName)
        {
            var sensor = await _Context.Sensors.Where(s => s.Location == GreenHouseName
                                                     && s.IsActive == true)
                                                   .Include(s=>s.Plant)
                                                  .ToListAsync();

            if (!sensor.Any())
            {
                _logger.LogInformation($"there is no Active sensore in GreenHousse {GreenHouseName} ");
            }
            return sensor;
        }
        
    }
}
