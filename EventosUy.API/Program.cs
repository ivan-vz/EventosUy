using EventosUy.API.Validators;
using EventosUy.Application;
using EventosUy.Application.DTOs.DataTypes.Insert;
using EventosUy.Application.DTOs.DataTypes.Update;
using EventosUy.Application.Interfaces;
using EventosUy.Application.Services;
using EventosUy.Infrastructure;
using FluentValidation;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddInfrastructure(); //Maneja todos los repositorios Internos
builder.Services.AddApplication(); //Maneja todos los servicios internos

builder.Services.AddControllers().AddJsonOptions(options => 
{
    options.JsonSerializerOptions.Converters.Add( new JsonStringEnumConverter() );
});

// Services
builder.Services.AddScoped<IInstitutionService, InstitutionService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IEditionService, EditionService>();
builder.Services.AddScoped<IRegisterTypeService, RegisterTypeService>();
builder.Services.AddScoped<ISponsorshipService, SponsorshipService>();
builder.Services.AddScoped<IRegisterService, RegisterService>();
builder.Services.AddScoped<IVoucherService, VoucherService>();

// Validators
builder.Services.AddScoped<IValidator<DTInsertClient>, ClientInsertValidator>();
builder.Services.AddScoped<IValidator<DTUpdateClient>, ClientUpdateValidator>();
builder.Services.AddScoped<IValidator<DTInsertInstitution>, InstitutionInsertValidator>();
builder.Services.AddScoped<IValidator<DTUpdateInstitution>, InstitutionUpdateValidator>();
builder.Services.AddScoped<IValidator<DTInsertEvent>, EventInsertValidator>();
builder.Services.AddScoped<IValidator<DTUpdateEvent>, EventUpdateValidator>();
builder.Services.AddScoped<IValidator<DTInsertEdition>, EditionInsertValidator>();
builder.Services.AddScoped<IValidator<DTUpdateEdition>, EditionUpdateValidator>();
builder.Services.AddScoped<IValidator<DTInsertRegisterType>, RegisterTypeInsertValidator>();
builder.Services.AddScoped<IValidator<DTInsertSponsorship>, SponsorshipInsertValidator>();
builder.Services.AddScoped<IValidator<DTInsertRegisterWithVoucher>, RegisterInsertWithVoucherValidator>();
builder.Services.AddScoped<IValidator<DTInsertRegisterWithoutVoucher>, RegisterInsertWithoutVoucherValidator>();
builder.Services.AddScoped<IValidator<DTInsertVoucherWithSponsor>, VoucherInsertWithSponsorValidator>();
builder.Services.AddScoped<IValidator<DTInsertVoucherWithoutSponsor>, VoucherInsertWithoutSponsorValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
