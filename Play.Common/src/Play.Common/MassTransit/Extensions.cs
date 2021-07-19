using System;
using System.Reflection;
using GreenPipes;
using MassTransit;
using MassTransit.Definition;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Play.Common.Settings;

namespace Play.Common.MassTransit
{
    public static class Extensions
    {
        public static IServiceCollection AddMassTransitWithRabbitMQ(this IServiceCollection services)
        {
            services.AddMassTransit(c =>
            {
                c.AddConsumers(Assembly.GetEntryAssembly());
                
                c.UsingRabbitMq((context, config) =>
                {
                    IConfiguration configuration = context.GetRequiredService<IConfiguration>();
                    ServiceSettings serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
                    RabbitMQSettings rabbitMqSettings = configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
                    
                    config.Host(rabbitMqSettings.Host);
                    config.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(serviceSettings.ServiceName, false));
                    config.UseMessageRetry(retryConfig =>
                    {
                        retryConfig.Interval(3, TimeSpan.FromSeconds(5));
                    });
                });
            });

            services.AddMassTransitHostedService();
            
            return services;
        }
    }
}