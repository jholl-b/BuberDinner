using BuberDinner.Api.Common.Mapping;

namespace BuberDinner.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddMappings();
        //services.AddControllers(opt => opt.Filters.Add<ErrorHandlingFilterAttribute>());
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }
}