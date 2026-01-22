using EventosUy.Application.Interfaces;
using EventosUy.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EventosUy.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services) //De esta froma solo se hace publica la funcion y no las implementaciones
        {
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IEditionService, EditionService>();
            services.AddScoped<IEmploymentService, EmploymentService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IInstitutionService, InstitutionService>();
            services.AddScoped<IJobTitleService, JobTitleService>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IProfessionalProfileService, ProfessionalProfileService>();
            services.AddScoped<IRegisterService, RegisterService>();
            services.AddScoped<IRegisterTypeService, RegisterTypeService>();
            services.AddScoped<ISponsorshipService, SponsorshipService>();

            return services;
        }
    }
}
