using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PlantMonitorring.DBContext;
using PlantMonitorring.Entity;

namespace PlantMonitorring.Services
{
    public class PlantSensorDataRepository:IPlantSensorDataRepository
    {
        private readonly PlantDataBaseContext _context;
        private readonly ILogger<PlantSensorData> _logger;

        public PlantSensorDataRepository(PlantDataBaseContext context,
                                          ILogger<PlantSensorData> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<PlantSensorData>PostSensorDataAsync(int plantId,int SensorId,
            DateOnly date,
            TimeOnly time,
            double Value)
        {
            var timestamp = date.ToDateTime(time);
            var newPlantSensorData = new PlantSensorData
            {
                Value = Value,
                PlantId = plantId,
                SensorId = SensorId,
                Timestamp = timestamp
            };
             await _context.PlantSensorDatas.AddAsync(newPlantSensorData);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"sansor data added, {newPlantSensorData}");
            await _context.Entry(newPlantSensorData).Reference(ps => ps.Plant).LoadAsync();
            await _context.Entry(newPlantSensorData).Reference(ps => ps.Sensor).LoadAsync();
           /* var result = await _context.PlantSensorDatas.Include(ps => ps.Plant)
                                                      .Include(ps => ps.Sensor)
                                                      .FirstOrDefaultAsync(ps => ps.Id == newPlantSensorData.Id);*/
            return newPlantSensorData;
        }
        //..........
       public async  Task<IEnumerable<PlantSensorData>> GetSensorDataAsync(int sensorId)
        {
            var sensorData = await _context.PlantSensorDatas.Where
                                                   (ps => ps.SensorId == sensorId)
                                                   .Include(ps=>ps.Sensor)
                                                   .Include(ps=>ps.Plant)
                                                   .ToListAsync();
            if (!sensorData.Any())
            {
                _logger.LogInformation($"No data related to {sensorId} was found");
            }
           
            return sensorData;
           
        }
        //...................................................
        public async Task<IEnumerable<PlantSensorData>>GetSensorDataByGreenHouseAsync(string greenHouseName)
        {
            var sensorData = await _context.PlantSensorDatas.Where(ps => ps.Sensor.Location.ToLower()
                                                                   == greenHouseName.ToLower())
                                                           .Include(ps => ps.Sensor)
                                                           .Include(ps => ps.Plant)
                                                           .ToListAsync();
            if(!sensorData.Any())
            {
                _logger.LogInformation($"No data is available for {greenHouseName}");
            }
            return sensorData;
        }
    }
}
