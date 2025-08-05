using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlantMonitorring.Entity;

namespace PlantMonitorring.DBContext
{
    public static class DatabaseSeeder
    {
        public static void SeedDatabase(PlantDataBaseContext context)
        {
            context.Database.Migrate();
            var userHasher=new PasswordHasher<User>();
            var changeMode = false;
            foreach(var user in context.Users.Where(u=>string. IsNullOrEmpty(u.Password)))
            {
                user.Password = userHasher.HashPassword(user, user.UserName);
                context.Update(user);
                changeMode =true;
            }
            if (changeMode)
            {
                context.SaveChanges();
            }
        }
    }
}
