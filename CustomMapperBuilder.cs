using CustomMapper.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CustomMapper;

public static class CustomMapperBuilder
{
    public static IServiceCollection AddCustomMapper(this IServiceCollection services)
    {
        var registry = new CustomMapperRegistry();

        var registrations = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(ICustomMapRegistration).IsAssignableFrom(t) && t is { IsInterface: false, IsAbstract: false })
            .Select(t => (ICustomMapRegistration)Activator.CreateInstance(t)!);

        foreach (var reg in registrations)
        {
            reg.Register(registry);
        }

        services.AddSingleton<ICustomMapperRegistry>(registry);
        services.AddSingleton<ICustomMapperService, CustomMapperService>();

        return services;
    }
}