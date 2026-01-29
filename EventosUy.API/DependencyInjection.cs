using EventosUy.API.Validators;
using EventosUy.Application.DTOs.DataTypes.Insert;
using EventosUy.Application.DTOs.DataTypes.Update;
using FluentValidation;

namespace EventosUy.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddController(this IServiceCollection services) //De esta froma solo se hace publica la funcion y no las implementaciones
        {
            services.AddScoped<IValidator<DTInsertClient>, ClientInsertValidator>();
            services.AddScoped<IValidator<DTUpdateClient>, ClientUpdateValidator>();
            services.AddScoped<IValidator<DTInsertInstitution>, InstitutionInsertValidator>();
            services.AddScoped<IValidator<DTUpdateInstitution>, InstitutionUpdateValidator>();
            services.AddScoped<IValidator<DTInsertEvent>, EventInsertValidator>();
            services.AddScoped<IValidator<DTUpdateEvent>, EventUpdateValidator>();
            services.AddScoped<IValidator<DTInsertEdition>, EditionInsertValidator>();
            services.AddScoped<IValidator<DTUpdateEdition>, EditionUpdateValidator>();
            services.AddScoped<IValidator<DTInsertRegisterType>, RegisterTypeInsertValidator>();
            services.AddScoped<IValidator<DTInsertSponsorship>, SponsorshipInsertValidator>();
            services.AddScoped<IValidator<DTInsertRegisterWithVoucher>, RegisterInsertWithVoucherValidator>();
            services.AddScoped<IValidator<DTInsertRegisterWithoutVoucher>, RegisterInsertWithoutVoucherValidator>();
            services.AddScoped<IValidator<DTInsertVoucherWithSponsor>, VoucherInsertWithSponsorValidator>();
            services.AddScoped<IValidator<DTInsertVoucherWithoutSponsor>, VoucherInsertWithoutSponsorValidator>();

            return services;
        }
    }
}
