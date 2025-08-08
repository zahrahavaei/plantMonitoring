using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PlantMonitorring.Entity;
using PlantMonitorring.Models;
using PlantMonitorring.Services;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PlantMonitorring.Controllers
{
    [Route("api/sensordata")]
    [ApiController]
    public class PlantSensorDataController : ControllerBase
    {
        private readonly IPlantSensorDataRepository _plantSensorDataRepository;
        private readonly ILogger<PlantSensorData> _logger;

        public PlantSensorDataController(IPlantSensorDataRepository plantSensorDataRepo,
                                         ILogger<PlantSensorData> logger)
        {
            _plantSensorDataRepository = plantSensorDataRepo;
            _logger = logger;
        }
        [HttpPost]
        public async Task <IActionResult>PostSensorData(PlantSensorDataPostDto dto)
        {
            var plantId = dto.PlantId;
            var SensorId = dto.SensorId;
            var date=dto.Date;
            var time = dto.Time;
            var Value = dto.Value;
           var result= await _plantSensorDataRepository.PostSensorDataAsync(
                 plantId,
                 SensorId,
             date,
             time,
            Value);
            if (result!=null)
            {
                return Ok(new PlantSensorDataDto
                {
                    Id = result.Id,
                    PlantId = result.PlantId,
                    SensorId = result.SensorId,
                    Value = result.Value,
                    Date = DateOnly.FromDateTime(result.Timestamp),
                    Time = TimeOnly.FromDateTime(result.Timestamp),
                    PlantName = result?.Plant.Name,
                    SensorType = result?.Sensor.Type
                });
            }
            return StatusCode(500, "Sensor data could not be saved.");
        }
        [HttpGet("sensorid/{sensorid}")]
        public async Task<ActionResult<IEnumerable<PlantSensorDataDto>>>GetSensorDataAsync(int sensorid)
        {
            var sensorDatas = await _plantSensorDataRepository.GetSensorDataAsync(sensorid);

            if (!sensorDatas.Any())
            {
                return NotFound();
            }
            var sensorDataDto = sensorDatas.Select(sensorData => new PlantSensorDataDto
            {
                Id = sensorData.Id,
                Date = DateOnly.FromDateTime(sensorData.Timestamp),
                Time = TimeOnly.FromDateTime(sensorData.Timestamp),

                Value = sensorData.Value,

                SensorId = sensorData.SensorId,

                PlantId = sensorData.PlantId,
                PlantName=sensorData?.Plant.Name,
                SensorType=sensorData?.Sensor.Type
            });
            return Ok(sensorDataDto);
        }
        //............................................................
        [HttpGet("greenhouse/{greenhousename}")]
        public async Task<ActionResult<IEnumerable<PlantSensorDataDto>>> GetSensorDataByGreenHouseAsync
                                                                                  (string greenhousename)
        {
            var sensorsData = await _plantSensorDataRepository.GetSensorDataByGreenHouseAsync(greenhousename);
            _logger.LogInformation($"Request for greenhouse: {greenhousename}");
            if (!sensorsData.Any())
            {
                return NotFound($"No Sensor DAta is Available for {greenhousename}");
            }
            /* var sensorsDataDto = sensorsData.Select(sd => new PlantSensorDataDto
             {
                 Id=sd.Id,
                 Value=sd.Value,
                 Date=DateOnly.FromDateTime(sd.Timestamp),
                 Time=TimeOnly.FromDateTime(sd.Timestamp),
                 PlantName=sd.Plant.Name,
                 SensorType=sd.Sensor.Type,
                 GreenHouseName=sd.Sensor.Location,
                 SensorId=sd.SensorId,
                PlantId=sd.PlantId
             });
             return Ok(sensorsDataDto);*/
            var groupedData = sensorsData
         .GroupBy(sd => new { sd.SensorId, sd.Sensor.Type, sd.Plant.Name })
         .Select(g => new {
             SensorId = g.Key.SensorId,
             SensorType = g.Key.Type,
             PlantName = g.Key.Name,
             Data = g.Select(sd => new {
                 Date = sd.Timestamp.ToString("yyyy-MM-dd"),
                 Time = sd.Timestamp.ToString("HH:mm"),
                 Value = sd.Value
             }).OrderBy(d => d.Date).ThenBy(d => d.Time)
         });

            return Ok(groupedData);
        }
    }
}
