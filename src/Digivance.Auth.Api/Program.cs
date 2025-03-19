using Asp.Versioning;
using Digivance.Auth.Api.Endpoints;
using Digivance.Auth.Api.Middleware;
using Digivance.Auth.Data.Commands;
using Digivance.Auth.Data.EntityFramework;
using Digivance.Auth.Data.EntityFramework.Contexts;
using Digivance.Auth.Data.EntityFramework.Entities;
using Digivance.Auth.Data.EntityFramework.Services;
using Digivance.Auth.Data.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
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

            // Build app and seed database
            var app = builder.Build();
            SeedDatabase(app.Services);

            // This will host our client application from /wwwroot, note hot reloading does
            // not work when viewing from this host.
            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
                app.MapOpenApi();

            // Auth
            app.UseAuthorization();

            // Global middleware
            app.UseMiddleware<ExceptionFilter>();

            // Endpoints
            app
                .UseAuthEndpointV1()
                .UseHealthEndpointV1()
                .UseTenantEndpointV1()
                .UseScopeEndpointV1()
                .UseRoleEndpointV1()
                .UseUserEndpointV1();

            // Swagger
            app
                .UseSwagger()
                .UseSwaggerUI();

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

            // In memory database for now...
            services.AddDbContext<AuthContext>(options => options.UseInMemoryDatabase("alpha"), ServiceLifetime.Singleton);

            // Global middleware
            services
                .AddScoped<ExceptionFilter>()
                .AddScoped<EntityMapper>()
                .AddEndpointsApiExplorer();

            // Validators
            services.AddScoped<IValidator<CreateScope>, CreateScopeValidator>();
            services.AddScoped<IValidator<CreateTenant>, CreateTenantValidator>();
            services.AddScoped<IValidator<CreateUser>, CreateUserValidator>();
            services.AddScoped<IValidator<CreateRole>, CreateRoleValidator>();

            services.AddScoped<IValidator<UpdateScope>, UpdateScopeValidator>();
            services.AddScoped<IValidator<UpdateTenant>, UpdateTenantValidator>();
            services.AddScoped<IValidator<UpdateUser>, UpdateUserValidator>();
            services.AddScoped<IValidator<UpdateRole>, UpdateRoleValidator>();

            // Temporary, remove when we .AddPermissionEndpointV1() below
            services
                .AddScoped<IPermissionService, EfPermissionService>();

            // Endpoints
            services
                .AddAuthEndpointV1()
                .AddHealthEndpointV1()
                .AddRoleEndpointV1()
                .AddScopeEndpointV1()
                .AddTenantEndpointV1()
                .AddUserEndpointV1();

            // Swagger
            services
                .AddSwaggerGen(config =>
                {
                    var jwtSecurityScheme = new OpenApiSecurityScheme
                    {
                        BearerFormat = "JWT",
                        Name = "JWT Authentication",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        Scheme = JwtBearerDefaults.AuthenticationScheme,
                        Description = "Enter a currently valid JWT token here",

                        Reference = new OpenApiReference
                        {
                            Id = JwtBearerDefaults.AuthenticationScheme,
                            Type = ReferenceType.SecurityScheme
                        }
                    };

                    config.SwaggerDoc("v1", new OpenApiInfo { Title = "Digivance Auth - API", Version = "v1" });
                    config.AddSecurityDefinition("Bearer", jwtSecurityScheme);
                    config.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        { jwtSecurityScheme, Array.Empty<string>() }
                    });
                });

            return builder;
        }

        /// <summary>
        /// We can use this to seed an initial state of the Digivance Auth database
        /// </summary>
        /// <param name="services">The DI ServiceProvider to get authContext from</param>
        protected static void SeedDatabase(IServiceProvider services)
        {
            var user = new UserEntity
            {
                DisplayName = "Alpha Test",
                EmailAddress = "test@digivance.com",
                Password = PasswordHelper.Hash("R@nd0pass"),
                Username = "alpha.test"
            };

            var authContext = services.GetRequiredService<AuthContext>();
            authContext.UserAccounts.Add(user);
            authContext.SaveChangesAsync(default).Wait();
        }
    }
}
