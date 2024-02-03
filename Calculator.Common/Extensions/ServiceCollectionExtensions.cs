using System.Reflection;
using Calculator.Common.Configurations;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Calculator.Common.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterRabbit(this IServiceCollection services, RabbitSettings? rabbitSettings, params Assembly[] assemblies)
    {
        ArgumentNullException.ThrowIfNull(rabbitSettings);

        services.AddMassTransit(x =>
        {
            x.AddConsumers(assemblies);
            x.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(rabbitSettings.Host, h =>
                {
                    h.Username(rabbitSettings.Username);
                    h.Password(rabbitSettings.Password);
                });
                configurator.ConfigureEndpoints(context);
            });
        });

        return services;
    }

    public static IServiceCollection RegisterMongo(this IServiceCollection services, MongoSettings? mongoSettings)
    {
        ArgumentNullException.ThrowIfNull(mongoSettings);

        services.AddOptions<MongoSettings>().BindConfiguration(nameof(MongoSettings));
        services.AddSingleton<IMongoClient>(x => new MongoClient(mongoSettings.ConnectionString));

        return services;
    }
}
