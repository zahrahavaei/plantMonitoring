using System.ComponentModel.DataAnnotations;
using PlantMonitorring.Enum;
namespace PlantMonitorring.Entity
{
    public class User
    {
       
        public int Id { get; set; }
        public string Name { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public Role UserRole { get; set; } = Role.User;
    }
}
