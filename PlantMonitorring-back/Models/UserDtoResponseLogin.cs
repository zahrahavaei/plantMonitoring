using PlantMonitorring.Enum;
using System.ComponentModel.DataAnnotations;

namespace PlantMonitorring.Models
{
    public class UserDtoResponseLogin
    {
        public int Id { get; set; }
        public string Name { get; set; }
       
        public string UserName { get; set; }
       
        public string Email { get; set; }
       
      
        public Role UserRole { get; set; } = Role.User;
    }
}
