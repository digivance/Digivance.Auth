using Asp.Versioning;
using Serilog;

namespace Digivance.Auth.Api.Endpoints
{
    /// <summary>
    /// Our health endpoint
    /// </summary>
    public static class HealthEndpoint
    {
        /// <summary>
        /// Adds the health endpoint to DI
        /// </summary>
        /// <param name="services">The ServiceCollection to add the HealthEndpoint to</param>
        /// <param name="config">Appsettings / environment configuration</param>
        /// <returns>IServiceCollection for builder pattern</returns>
        /// <exception cref="Exception">If configuration isn't provided</exception>
        public static IServiceCollection AddHealthEndpointV1(this IServiceCollection services)
        {
            Log.Debug("Adding V1 health dependancies");
            return services;
        }

        /// <summary>
        /// Configures our minimal api endpoint
        /// </summary>
        /// <param name="app">The web application to configure with our endpoint</param>
        /// <returns>WebApplication for builder pattern</returns>
        public static WebApplication UseHealthEndpointV1(this WebApplication app)
        {
            Log.Debug("Configuring V1 health endpoints");
            var version = new ApiVersion(1, 0);
            var versionSet = app.NewApiVersionSet()
                .HasApiVersion(version)
                .ReportApiVersions()
                .Build();

            var group = app.MapGroup("/api/healthz")
                .WithApiVersionSet(versionSet);

            group
                .MapGet("/ready", ReadyAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { Description = "Check the readiness of this application", OperationId = "Health Ready Check" });

            group
                .MapGet("/live", LivenessAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { Description = "Check the liveness of this application", OperationId = "Health Liveness Check" });

            return app;
        }

        /// <summary>
        /// Standard ready check
        /// </summary>
        /// <param name="cancellationToken">Pipeline cancellation token</param>
        /// <returns>IResult.Ok when ready</returns>
        public static Task<IResult> ReadyAsync(CancellationToken cancellationToken)
        {
            Log.Debug("Health ready check request");
            return Task.FromResult(Results.Ok("Ready!"));
        }

        /// <summary>
        /// Standard liveness check
        /// </summary>
        /// <param name="cancellationToken">Pipeline cancellation token</param>
        /// <returns>IResult.Ok when live</returns>
        public static Task<IResult> LivenessAsync(CancellationToken cancellationToken)
        {
            Log.Debug("Health live check request");
            return Task.FromResult(Results.Ok("I'm alive!"));
        }
    }
}
