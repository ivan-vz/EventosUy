using EventosUy.Domain.Interfaces;
using EventosUy.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;


namespace EventosUy.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services) 
        {
            services.AddScoped<ICategoryRepo, CategoryRepo>();
            services.AddScoped<IClientRepo, ClientRepo>();
            services.AddScoped<IInstitutionRepo, InstitutionRepo>();
            services.AddScoped<IEventRepo, EventRepo>();
            services.AddScoped<IEditionRepo, EditionRepo>();
            services.AddScoped<IRegisterTypeRepo, RegisterTypeRepo>();
            services.AddScoped<ISponsorshipRepo, SponsorshipRepo>();
            services.AddScoped<IVoucherRepo, VoucherRepo>();
            services.AddScoped<IRegisterRepo, RegisterRepo>();

            return services;
        }
    }
}
