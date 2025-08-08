using System.Xml.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlantMonitorring.Entity;
using PlantMonitorring.Enum;

namespace PlantMonitorring.DBContext
{
    public class PlantDataBaseContext: DbContext
    {
        public PlantDataBaseContext(DbContextOptions<PlantDataBaseContext> options) : base(options)
        {
        }
        public DbSet<PlantSensorData> PlantSensorDatas { get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void  OnModelCreating(ModelBuilder modelBuilder)
        {
           
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Plant>()
                 .HasMany(p => p.Sensor)
                 .WithOne(s => s.Plant)
                 .HasForeignKey(s => s.PlantId);
            modelBuilder.Entity<Plant>()
                .HasMany(p => p.PlantSensorData)
                .WithOne(psd => psd.Plant)
                .HasForeignKey(psd => psd.PlantId);

            modelBuilder.Entity<Sensor>()
                .HasMany(s => s.PlantSensorData)
                .WithOne(psd => psd.Sensor)
                .HasForeignKey(psd => psd.SensorId);

            
            modelBuilder.Entity<User>()
                .Property(u => u.UserRole)
                .HasConversion<string>();

            modelBuilder.Entity<User>()
                .HasData(
                 new User
                 {
                     Id = 100,
                     Name = "Sara",
                     UserName = "100",
                     Email = "sara@yahoo.com",
                     UserRole = Role.Admin,
                     Password="",
                    
                 },
            new User
            {
                Id = 101,
                Name = "Ali",
                UserName = "101",
                Email = "Ali@yahoo.com",
                UserRole = Role.User,
                Password = "",
            });
           

            modelBuilder.Entity<PlantSensorData>().ToTable("PlantSensorDatas")
            .HasData(
                new PlantSensorData
            {
                Id = 1,
                Timestamp = new DateTime(2025, 05, 05, 5, 5, 9),
                Value = 24,
                SensorId = 1,
                PlantId = 1
            },
            new PlantSensorData
            {
                Id = 2,
                Timestamp = new DateTime(2025, 05, 05, 5, 5, 9),
                Value = 70,
                SensorId = 2,
                PlantId = 1
            },
            new PlantSensorData
            {
                Id = 3,
                Timestamp = new DateTime(2025, 05, 05, 5, 5, 9),
                Value = 50,
                SensorId = 3,
                PlantId = 1
            },
            new PlantSensorData
            {
                Id = 4,
                Timestamp = new DateTime(2025, 05, 05, 5, 5, 9),
                Value = 70,
                SensorId = 4,
                PlantId = 1
            },
            new PlantSensorData
            {
                Id = 5,
                Timestamp = new DateTime(2025, 05, 05, 5, 5, 9),
                Value = 70,
                SensorId = 5,
                PlantId = 2
            }
            );                                                                      
            modelBuilder.Entity<Plant>()
                 .HasData(
                 new Plant
                 {
                     Id = 1,
                     Name = "Rose",
                     Species = "Rosa",
                     Location = "GreenHouseA",
                     Description = "A beautiful red rose.",
                     PlantingDate = new DateTime(2022, 5, 1),
                     Image = "rose.jpg",
                     IsActive = true
                 },
                 new Plant
                 {
                     Id = 2,
                     Name = "Tulip",
                     Species = "Tulipa",
                     Location = "GreenHouseA",
                     Description = "A vibrant yellow tulip.",
                     PlantingDate = new DateTime(2022, 6, 15),
                     Image = "tulip.jpg",
                     IsActive = true
                 },
                 new Plant
                 {
                     Id = 3,
                     Name = "Cactus",
                     Species = "Cactaceae",
                     Location = "Indoor",
                     Description = "A hardy cactus that thrives in dry conditions.",
                     PlantingDate = new DateTime(2022, 7, 20),
                     Image = "cactus.jpg",
                     IsActive = true
                 }
                );
            modelBuilder.Entity<Sensor>()
              .HasData(

                new Sensor
                {
                    Id = 1,
                    Type = "Temperature",
                    Location = "GreenHouseA",
                    PlantId = 1,
                    IsActive=true,
                    Unit= "Celsius"
                },
                new Sensor
                {
                    Id = 2,
                    Type = "Humidity",
                    Location = "GreenHouseA",
                    PlantId = 1,
                    IsActive = true,
                    Unit = "Percentage"
                },
                new Sensor
                {
                    Id = 3,
                    Type = "Soil Moisture",
                    Location = "GreenHouseA",
                    PlantId = 1,
                    IsActive=true,
                    Unit = "Percentage"
                },
                new Sensor
                {
                    Id = 4,
                    Type = "Light",
                    Location = "GreenHouseA",
                    PlantId = 1,
                    IsActive = true,
                    Unit = "Lux"
                },
                  new Sensor
                  {
                      Id = 5,
                      Type = "Temperature",
                      Location = "GreenHouseB",
                      PlantId = 2,
                      IsActive=true,
                      Unit = "Celsius"
                  },
                  new Sensor
                  {
                      Id = 6,
                      Type = "Humidity",
                      Location = "GreenHouseB",
                      PlantId = 2,
                      IsActive = true,
                      Unit = "Percentage"
                  },
                     new Sensor
                     {
                         Id = 7,
                         Type = "Soil Moisture",
                         Location = "GreenHouseB",
                         PlantId = 2,
                         IsActive = true,
                         Unit = "Percentage"
                     },
                     new Sensor
                     {
                         Id = 8,
                         Type = "Light",
                         Location = "GreenHouseB",
                         PlantId = 2,
                         IsActive = true,
                         Unit = "Lux"
                     }

                );
        }
    }
    
    
}
