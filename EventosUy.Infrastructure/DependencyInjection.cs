using EventosUy.Domain.Interfaces;
using EventosUy.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;


namespace EventosUy.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services) 
        {
            services.AddSingleton<ICategoryRepo, CategoryRepo>();
            services.AddSingleton<IClientRepo, ClientRepo>();
            services.AddSingleton<IInstitutionRepo, InstitutionRepo>();
            services.AddSingleton<IEventRepo, EventRepo>();
            services.AddSingleton<IEditionRepo, EditionRepo>();
            services.AddSingleton<IRegisterTypeRepo, RegisterTypeRepo>();
            services.AddSingleton<ISponsorshipRepo, SponsorshipRepo>();
            services.AddSingleton<IVoucherRepo, VoucherRepo>();
            services.AddSingleton<IRegisterRepo, RegisterRepo>();

            return services;
        }
    }
}
