using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PlantMonitorring.DBContext;
using PlantMonitorring.Services;
using Microsoft.AspNetCore.Identity;
using PlantMonitorring.Entity;

var builder = WebApplication.CreateBuilder(args);

//configure Cors connect React to Asp.net
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost3000",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials(); // Required if using cookies or JWT in headers
        });
});
//.........paswordHasher..........

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
//......................
//...........Jwt authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
             Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
//.................
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IPlantRepositort, PlantRepository>();

//setup connection string for sqlite
var connection =builder.Configuration.GetConnectionString("PlantDbConnectionString");
builder.Services.AddDbContext<PlantDataBaseContext>(options =>
                 options.UseSqlite(connection));

//..............microsoft netonsoft json patch document
builder.Services.AddControllers()
       .AddNewtonsoftJson();
      
//...................................
var app = builder.Build();


//........seeding dataBase because HasData in ModelBuilder in PlantDBContext can not use hashpassword
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PlantDataBaseContext>();
    DatabaseSeeder.SeedDatabase(db);
}
//.........
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowLocalhost3000");
app.UseStaticFiles();//for showing picture in browser
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
