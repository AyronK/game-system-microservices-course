using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Play.Catalog.Service.Core;
using Play.Catalog.Service.Data.Repositories.MongoDb;
using Play.Catalog.Service.Settings;

namespace Play.Catalog.Service.Data.Repositories.Extensions
{
    public static class Extensions
    {
        public static IServiceCollection AddMongo(this IServiceCollection services)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

            services.AddSingleton(serviceProvider =>
                {
                    IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
                    MongoDbSettings mongoDbSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
                    ServiceSettings serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
                    MongoClient mongoDbClient = new(mongoDbSettings.ConnectionString);
                    return mongoDbClient.GetDatabase(serviceSettings.ServiceName);
                }
            );

            return services;
        }

        public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string collectionName) where T: class, IEntity<Guid>
        {
            services.AddScoped<IRepository<Guid, T>>(serviceProvider =>
            {
                IMongoDatabase database = serviceProvider.GetService<IMongoDatabase>();
                return new MongoRepository<T>(database, collectionName);
            });

            return services;
        }
    }
}