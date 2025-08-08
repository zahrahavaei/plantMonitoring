using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PlantMonitorring.DBContext;
using PlantMonitorring.Services;
using Microsoft.AspNetCore.Identity;
using PlantMonitorring.Entity;
using PlantMonitorring.Convertor;//convertor for date only time only 
using System.Text.Json.Serialization;
using System.ComponentModel;//Time only DateOnly


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
//.............serialize all enums as strings in JSON responses

/*builder.Services
    .AddControllers()
.AddNewtonsoftJson(options =>  // <-- keep only if you're using JsonPatchDocument
{
     options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
   
}); */
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
        options.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IPlantRepository, PlantRepository>();
builder.Services.AddScoped<IPlantSensorDataRepository, PlantSensorDataRepository>();
builder.Services.AddScoped<ISensorRepository, SensorRepository>();

//......................

//setup connection string for sqlite
var connection =builder.Configuration.GetConnectionString("PlantDbConnectionString");
builder.Services.AddDbContext<PlantDataBaseContext>(options =>
                 options.UseSqlite(connection));


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
