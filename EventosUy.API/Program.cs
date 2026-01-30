using EventosUy.API;
using EventosUy.Application;
using EventosUy.Infrastructure;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddInfrastructure(); //Maneja todos los repositorios Internos
builder.Services.AddApplication(); //Maneja todos los servicios internos
builder.Services.AddController(); //Manjea todos los validadores

builder.Services.AddControllers().AddJsonOptions(options => 
{
    options.JsonSerializerOptions.Converters.Add( new JsonStringEnumConverter() );
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
