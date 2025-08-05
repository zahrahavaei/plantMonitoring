using System.Collections.Concurrent;
using System.IO;
using System.IO.Pipes;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using PlantMonitorring.DBContext;
using PlantMonitorring.Entity;
using PlantMonitorring.Models;
using PlantMonitorring.Services;
using SQLitePCL;
using static System.Net.Mime.MediaTypeNames;

namespace PlantMonitorring.Controllers
{
    [Authorize]
    [Route("api/plant")]
    [ApiController]
    public class PlantController : ControllerBase
    {
        private readonly IPlantRepositort _plantRepository;
        private readonly PlantDataBaseContext _context;

        public PlantController(IPlantRepositort plantRepository, PlantDataBaseContext context)
        {
            _plantRepository = plantRepository;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPlants()
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";// in controll base it is abuilt in property
            var plants = await _plantRepository.GetAllPlantsAsync();
            if (!plants.Any())
            {
                return NotFound(new { message = "Plant not found" });
            }
            var result = plants.Select(p => new PlantDto
            {
                Id = p.Id,
                Name = p.Name,
                Species = p.Species,
                Location = p.Location,
                Description = p.Description,
                PlantingDate = p.PlantingDate,
                ImageUrl = $"{baseUrl}/img/plant/{p.Image}",
                IsActive = p.IsActive
            });

            return Ok(result);
        }
        //.......................................
        [HttpGet("{plantid}", Name = "GetPlant")]
        public async Task<ActionResult<PlantDetailedDto>> GetPlantAsync(int plantId,
            bool includeSensor, bool includePlantSensorData)
        {
            var plant = await _plantRepository.GetPlantByIdAsync(plantId, includeSensor, includePlantSensorData);
            if (plant == null)
            {
                return NotFound(new { message = "Plant not found" });
            }
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var plantDto = new PlantDetailedDto
            {
                Id = plant.Id,
                Name = plant.Name,
                Species = plant.Species,
                Location = plant.Location,
                Description = plant.Description,
                PlantingDate = plant.PlantingDate,
                ImageUrl = $"{baseUrl}/img/plant/{plant.Image}",
                IsActive = plant.IsActive,
            };
            if (includeSensor)
            {
                plantDto.Sensors = plant.Sensor.Select(s => new SensorDto
                {
                    Id = s.Id,
                    Type = s.Type,
                    Location = s.Location,
                    PlantId = s.PlantId,
                    Unit = s.Unit,
                    IsActive = s.IsActive,
                }).ToList();
            }

            if (includePlantSensorData)
            {
                plantDto.PlantSensorData = plant.PlantSensorData.Select(psd => new PlantSensorDataDto
                {
                    Id = psd.Id,
                    SensorId = psd.SensorId,
                    PlantId = psd.PlantId,
                    Value = psd.Value,
                    Timestamp = psd.Timestamp
                }).ToList();
            }

            return Ok(plantDto);

        }
        //........................................
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<PlantDto>> CreatePlantAsync([FromForm] PlantForCreationDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Validation Error" });
            }
            var plantingDateTime = dto.PlantingDate.ToDateTime(dto.PlantingTime);
            var plant = new Plant()
            {
                Name = dto.Name,
                Species = dto.Species,
                Description = dto.Description,
                Location = dto.Location,
                PlantingDate = plantingDateTime,
                IsActive = dto.IsActive,

            };
            if (dto.Image != null && dto.Image.Length > 0)
            {
                var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/plant");
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + dto.Image.FileName;

                var filePath = Path.Combine(uploadFolder, uniqueFileName);
                try
                {
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await dto.Image.CopyToAsync(fileStream);
                    }

                }
                catch (Exception exc)
                {
                    return StatusCode(500, new { message = "faild to upload plant photo" });
                }
                plant.Image = uniqueFileName;
            }

            var result = await _plantRepository.AddPlantAsync(plant);
            if (result.Plant != null)
            {
                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                var plantDto = new PlantDto
                {
                    Id = result.Plant.Id,
                    Name = result.Plant.Name,
                    Species = result.Plant.Species,
                    Location = result.Plant.Location,
                    Description = result.Plant.Description,
                    PlantingDate = result.Plant.PlantingDate,
                    ImageUrl = $"{baseUrl}/img/plant/{result.Plant.Image}",
                    IsActive = result.Plant.IsActive
                };
                return CreatedAtRoute("GetPlant", new { plantid = plantDto.Id }, plantDto);
            }
            else
            {
                return StatusCode(500, new { message = "Failed to create plant" });

            }
        }
        //....................................................
        [HttpPut("{plantId}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdatePlantAsync(int plantId,
            [FromForm] PlantForUpdateDto dto)
        {
            var plant = await _plantRepository.GetPlantByIdAsync(plantId, false, false);
            if (plant == null)
            {
                return NotFound(new 
                {
                   
                    Message = "Plant not found"
                });
            }
            if(!string.IsNullOrWhiteSpace(dto.Name) )
            {
                plant.Name = dto.Name;
            }
            if (!string.IsNullOrWhiteSpace(dto.Species))
            {
                plant.Species = dto.Species;
            }
            if (!string.IsNullOrWhiteSpace(dto.Location))
            {
                plant.Location = dto.Location;
            }

            if (!string.IsNullOrWhiteSpace(dto.Description))
            {
                plant.Description = dto.Description;
            }
          
          
            if(dto.PlantingDate != null && dto.PlantingTime != null)
            {
                plant.PlantingDate = dto.PlantingDate.ToDateTime(dto.PlantingTime);
            }

            if (dto.Image != null && dto.Image.Length > 0)
            {
                var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/plant");
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + dto.Image.FileName;
                var filePath = Path.Combine(uploadFolder, uniqueFileName);
                try
                {
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await dto.Image.CopyToAsync(fileStream);
                    }
                    plant.Image = uniqueFileName;
                }
                catch (Exception exc)
                {
                    return StatusCode(500, new 
                    {
                        Message = "Failed to upload plant photo",
                       
                    });
                }
            }
            var result = await _plantRepository.UpdatePlantPutAsync(plant);
            if(result.Plant != null)
            {
                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                var plantDto = new PlantDto
                {
                    Id = result.Plant.Id,
                    Name = result.Plant.Name,
                    Species = result.Plant.Species,
                    Location = result.Plant.Location,
                    Description = result.Plant.Description,
                    PlantingDate = result.Plant.PlantingDate,
                    ImageUrl = $"{baseUrl}/img/plant/{result.Plant.Image}",
                    IsActive = result.Plant.IsActive
                };
                return Ok(new PlantResultDto
                {
                   PlantDto=plantDto,
                    Message = "Plant updated successfully"
                });
            }
            else
            {
                return StatusCode(500, new 
                {
                    Message = "Failed to update plant",
                   
                });
            }
        }
        //........................
        [HttpPatch("{plantId}")]
        public async Task<ActionResult<PlantDto>>UpdatePlantPatchAsync(int plantId,
               JsonPatchDocument<PlantForUpdateDto>jsonPatchDocument)
        {
            var plant = await _context.Plants.FindAsync(plantId);
            if (plant == null)
            {
                return NotFound(new { message = "plant Not Found!" });
            }
            var plantToPatch = new PlantForUpdateDto
            {
                Name = plant.Name,
                Species = plant.Species,
                Description = plant.Description,
                Location = plant.Location,
                PlantingDate = DateOnly.FromDateTime(plant.PlantingDate),
                PlantingTime = TimeOnly.FromDateTime(plant.PlantingDate),
                IsActive = plant.IsActive
            };

            if(jsonPatchDocument==null)
            {
                return BadRequest(new { message = "Empty JsonPatch Document!" });
            }
            jsonPatchDocument.ApplyTo(plantToPatch,ModelState);

            if(!ModelState.IsValid)
            {
                return BadRequest(new { message = "Validation Error" });
            }
            plant.Name = plantToPatch.Name;
            plant.Species = plantToPatch.Species;
            plant.Description = plantToPatch.Description;
            plant.Location = plantToPatch.Location;
            plant.IsActive = plantToPatch.IsActive;
            if(plantToPatch.PlantingDate!=null && plantToPatch.PlantingTime!=null)
            {
                var date = plantToPatch.PlantingDate;
                var time = plantToPatch.PlantingTime;
                plant.PlantingDate = date.ToDateTime(time);
            }
             _context.Plants.Update(plant);

            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                var plantDto = new PlantDto
                {
                    Id = plant.Id,
                    Name = plant.Name,
                    Species = plant.Species,
                    Location = plant.Location,
                    Description = plant.Description,
                    PlantingDate = plant.PlantingDate,
                    ImageUrl = $"{baseUrl}/img/plant/{plant.Image}",
                    IsActive = plant.IsActive
                };
                return Ok(plantDto);
            }
            else
            {
                return StatusCode(500, new { message = "Failed to update plant" });
            }
        }
    }
}