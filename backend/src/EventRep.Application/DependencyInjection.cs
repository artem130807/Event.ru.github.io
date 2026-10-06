using FluentValidation;
using EventRep.Application.Common.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace EventRep.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(
                typeof(DependencyInjection).Assembly);
            configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
            configuration.AddOpenBehavior(typeof(UnitOfWorkBehavior<,>));
        });

        return services;
    }
}
