using Microsoft.EntityFrameworkCore;
using PlantMonitorring.DBContext;
using PlantMonitorring.Entity;
using PlantMonitorring.Services.Response;

namespace PlantMonitorring.Services
{
    public class PlantRepository : IPlantRepositort

    {
        private readonly PlantDataBaseContext _context;
        private readonly ILogger<PlantRepository> _logger;
        public PlantRepository(PlantDataBaseContext context, ILogger<PlantRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Plant>> GetAllPlantsAsync()
        {
            return await _context.Plants.OrderBy(p => p.PlantingDate).ToListAsync();
        }
        //................................
        public async Task<Plant?> GetPlantByIdAsync(int id, bool includeSensor, bool includePlantSensorData)
        {
            var Query = _context.Plants.AsQueryable();

            if (includeSensor && includePlantSensorData)
            {
                Query = Query.Include(p => p.Sensor)
                             .Include(p => p.PlantSensorData);
            }
            else if (includeSensor)
            {
                Query = Query.Include(p => p.Sensor);
            }
            else if (includePlantSensorData)
            {
                Query = Query.Include(p => p.PlantSensorData);
            }
            var result = await Query.FirstOrDefaultAsync(p => p.Id == id);
            if (result == null)
            {
                _logger.LogWarning($"Plant with ID {id} not found.");
                Console.WriteLine($"Plant with ID {id} not found.");

            }
            else
            {
                _logger.LogInformation($"Plant with ID {id} retrieved successfully.");
                Console.WriteLine($"Plant with ID {id} retrieved successfully.");

            }
            return result;
        }
        //...........................
        public async Task<PlantResponse> AddPlantAsync(Plant plant)
        {
            await _context.Plants.AddAsync(plant);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                _logger.LogInformation($"Plant with ID {plant.Id} added successfully.");
                return new PlantResponse
                {
                    Plant = plant,
                    Message = "Plant added successfully."
                };
            }
            else
            {
                _logger.LogError("Failed to add plant.");
                return new PlantResponse
                {
                    Plant = null,
                    Message = "Failed to add plant."
                };
            }
        }
        //......................
        public async Task<PlantResponse> UpdatePlantPutAsync(Plant plant)
        {
            var existingPlant = await _context.Plants.FindAsync(plant.Id);
            if (existingPlant == null)
            {
                _logger.LogWarning($"Plant with ID {plant.Id} not found for update.");
                return new PlantResponse
                {
                    Plant = null,
                    Message = "Plant not found."
                };
            }

            _context.Plants.Update(plant);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                _logger.LogInformation($"Plant with ID {plant.Id} updated successfully.");
                return new PlantResponse
                {
                    Plant = plant,
                    Message = "Plant updated successfully."
                };
            }
            else
            {
                _logger.LogError("Failed to update plant.");
                return new PlantResponse
                {
                    Plant = null,
                    Message = "Failed to update plant."
                };
            }
        }
    }
}