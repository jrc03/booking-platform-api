using Application.Interfaces.Auth;
using Infrastructure.Security;
using Domain.Interfaces;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Application.Interfaces.Email;
using Infrastructure.Services.Email;


namespace Infrastructure;

// Usamos el patrón de "Service Collection Extension" para mantener la configuración acoplada a Infraestructura
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. Agregar el DbContext (Se configura SQL Server leyendo del archivo appsettings.json)
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        // 2. Registrar el UnitOfWork
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // 3. Registrar los repositorios
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPropertyRepository, PropertyRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();

        // Registros de Autenticación temporales/stubs
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        // Configuración fuertemente tipada (Options Pattern) permitida en Infra
        services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
        // Registrar Servicio de Correos
        services.AddScoped<IEmailService, MailKitEmailService>();
        return services;
    }
}