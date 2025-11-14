using Microsoft.EntityFrameworkCore;
using property.Application.Interfaces;
using property.Application.Services;
using property.Domain.Entities;
using property.Domain.Infrastructure;
using property.Infrastructure.Data;
using property.Infrastructure.Repositories;
using property.Infrastructure.Services;
using property.Infrastructure.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//llamado de la variable de entorno para la BD
var connectionString = builder.Configuration.GetConnectionString("Default");

//BD
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, MySqlServerVersion.AutoDetect(connectionString)));

//añadimos los repositories
builder.Services.AddScoped<IRepository<Property>, PropertiesRepository>();




builder.Services.AddScoped<IPropertyService, PropertyService>();



//---Controllers
builder.Services.AddControllers();

//Cloudinary
//---------------------------------------------------
builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("CloudinarySettings")
);

builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();
//----------------------------------------------------


var app = builder.Build();

app.MapControllers();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
