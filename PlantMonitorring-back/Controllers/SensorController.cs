using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlantMonitorring.Models;
using PlantMonitorring.Services;

namespace PlantMonitorring.Controllers
{
    [Route("api/sensor")]
    [ApiController]
    public class SensorController : ControllerBase
    {
        private readonly ISensorRepository _sensorRepository;

        public SensorController(ISensorRepository sensorRepository)
        {
            _sensorRepository = sensorRepository;
        }

        [HttpGet("greenhouse")]
        public async Task<ActionResult<IEnumerable<SensorDto>>> GetAllSensorGreenHouseAsync(string GreenHouseName)
        {
            var sensors = await _sensorRepository.GetAllSensorGreenHouseAsync(GreenHouseName);
            if (sensors == null || !sensors.Any())
            {
                return NotFound("No active sensors found in GreenHouse A.");
            }
            var sensorsDto=sensors.Select(s => new SensorDto
            {
                Id = s.Id,
                Type = s.Type,
                Location = s.Location,
                IsActive = s.IsActive,
                Unit = s.Unit,
                PlantId = s.PlantId,
                PlantName=s.Plant.Name
            });
            return Ok(sensorsDto);
        }
        //..........
       
    }
}
