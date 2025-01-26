using Asp.Versioning;
using Digivance.Auth.Api.Endpoints;
using Serilog;

namespace Digivance.Auth.Api
{
    /// <summary>
    /// Application / Program object, this contains our entry point
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Our main entry point, builds and launches our dotnet api server
        /// </summary>
        /// <param name="args">Command line arguments</param>
        public static void Main(string[] args)
        {
            var builder = ConfigureServices(args);
            var app = ConfigureApplication(builder);

            app.Run();
        }

        /// <summary>
        /// Configures the application
        /// </summary>
        /// <param name="builder">The builder that we have configured services on</param>
        /// <returns>The WebApplication to run</returns>
        protected static WebApplication ConfigureApplication(WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();

            var app = builder.Build();

            // This will host our client application from /wwwroot, note hot reloading does
            // not work when viewing from this host.
            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseAuthorization();

            app.UseHealthEndpointV1();

            return app;
        }

        /// <summary>
        /// Configures our services and returns a builder object that we can send
        /// to the ConfigureApplication method to prepare for launch.
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <returns>WebApplicationBuilder</returns>
        protected static WebApplicationBuilder ConfigureServices(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var services = builder.Services;

            // Auth
            services.AddAuthorization();

            // Helpers
            services.AddOpenApi();

            // Versioning
            services.AddApiVersioning(opt =>
            {
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.ReportApiVersions = true;
                opt.ApiVersionReader = ApiVersionReader.Combine
                (
                    new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("x-api-version"),
                    new MediaTypeApiVersionReader("x-api-version")
                );
            });

            // Endpoints
            services
                .AddHealthEndpointV1();

            return builder;
        }
    }
}
