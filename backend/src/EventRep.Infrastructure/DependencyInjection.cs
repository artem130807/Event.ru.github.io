using EventRep.Application.Contracts.Persistence;
using EventRep.Domain.Contracts;
using EventRep.Infrastructure.Persistence;
using EventRep.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventRep.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' is not configured.");

        services.AddDbContext<EventRepDbContext>(options =>
            options.UseNpgsql(
                connectionString,
                npgsqlOptions => npgsqlOptions.EnableRetryOnFailure()));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IEventFeedbackRepository, EventFeedbackRepository>();
        services.AddScoped<IPortfolioRepository, PortfolioRepository>();
        services.AddScoped<IPortfolioElementRepository, PortfolioElementRepository>();
        services.AddScoped<IResumeRepository, ResumeRepository>();
        services.AddScoped<IEventReadRepository, EventReadRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
