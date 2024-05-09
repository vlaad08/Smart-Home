using ConsoleApp1;
using DBComm.Logic;
using DBComm.Logic.Interfaces;
using DBComm.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ITemperatureLogic, TemperatureLogic>();
builder.Services.AddScoped<IHumidityLogic, HumidityLogic>();
builder.Services.AddScoped<ILightLogic, LightLogic>();
builder.Services.AddScoped<INotificationLogic, NotificationLogic>();

builder.Services.AddScoped<IWindowLogic, WindowLogic>();
builder.Services.AddScoped<IBaseRepository, TemperatureRepository>();
builder.Services.AddScoped<IBaseRepository, LightRepository>();
builder.Services.AddScoped<IBaseRepository, HumidityRepository>();

builder.Services.AddScoped<ITemperatureRepository, TemperatureRepository>();
builder.Services.AddScoped<ILigthRepository, LightRepository>();
builder.Services.AddScoped<IHumidityRepository, HumidityRepository>();

builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddDbContext<Context>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())    
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials());



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();