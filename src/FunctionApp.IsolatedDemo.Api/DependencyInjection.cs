using FunctionApp.IsolatedDemo.Api.Interfaces;
using FunctionApp.IsolatedDemo.Api.Persistence;
using FunctionApp.IsolatedDemo.Api.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly.Extensions.Http;
using Polly;
using System;
using System.Net.Http;
using FunctionApp.IsolatedDemo.Api.Persistence.Repositories;

namespace FunctionApp.IsolatedDemo.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddScoped<INoteService, NoteService>();

            return services;
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();

            AddPersistence(services, configuration);

            services
                .AddHttpClient<INotificationService, NotificationService>(client =>
                {
                    client.BaseAddress = new Uri(configuration.GetValue<string>("NotificationApiUrl"));
                })
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy());

            return services;
        }

        private static void AddPersistence(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IDbConnectionFactory>(_ =>
                new NpgsqlConnectionFactory(configuration.GetConnectionString("Database")!));
            services.AddSingleton<DatabaseInitializer>();
            services.AddSingleton<INoteRepository, NoteRepository>();
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrTransientHttpStatusCode()
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrTransientHttpStatusCode()
                .CircuitBreakerAsync(3, TimeSpan.FromMinutes(2));
        }
    }
}
