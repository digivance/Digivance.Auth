using Asp.Versioning;
using Digivance.Auth.Api.Middleware;
using Digivance.Auth.Data.Commands;
using Digivance.Auth.Data.EntityFramework.Services;
using Digivance.Auth.Data.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Serilog;
using System.Security.Cryptography;

namespace Digivance.Auth.Api.Endpoints
{
    /// <summary>
    /// Defines our auth endpoints (/auth)
    /// </summary>
    public static class AuthEndpoint
    {
        /// <summary>
        /// Registers necessary dependancies
        /// </summary>
        /// <param name="services">The IServiceCollection to configure</param>
        /// <returns>The same IServiceCollection for builder pattern</returns>
        public static IServiceCollection AddAuthEndpointV1(this IServiceCollection services)
        {
            // For now, this will come from secure config in the near future
            var authOptions = new EfAuthServiceOptions
            {
                BearerExpiry = TimeSpan.FromMinutes(15),
                JwtAudience = "AuthTestAudience",
                JwtIssuer = "AuthTestIssuer",
                JwtSigningKey = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)),
                RefreshExpiry = TimeSpan.FromHours(4)
            };

            services.TryAddSingleton(Options.Create(authOptions));
            services.TryAddScoped<IAuthService, EfAuthService>();
            services.TryAddScoped<IValidator<AuthenticateUserCredentials>, AuthenticateUserCredentialsValidator>();

            return services;
        }

        /// <summary>
        /// Registers our routes with the WebApplication host
        /// </summary>
        /// <param name="app">The WebApplication host to register routes in</param>
        /// <returns>The same WebApplication for builder pattern</returns>
        public static WebApplication UseAuthEndpointV1(this WebApplication app)
        {
            Log.Debug("Configuring V1 auth endpoints");

            var version = new ApiVersion(1, 0);
            var versionSet = app.NewApiVersionSet()
                .HasApiVersion(version)
                .ReportApiVersions()
                .Build();

            var route = app.MapGroup("/auth")
                .WithApiVersionSet(versionSet);

            route
                .MapPost("/refresh-token", RefreshTokenAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { Description = "Refresh bearer token", OperationId = "RefreshBearerToken" });

            route
                .MapPost("/user-credentials", UserCredentialsAsync)
                .Validate<AuthenticateUserCredentials>()
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { Description = "Authenticate from user credentials", OperationId = "AuthenticateUserCredentials" });

            return app;
        }

        /// <summary>
        /// Refresh an expired bearer token using your refresh code
        /// </summary>
        /// <param name="authHeader">Expected to be an expired but otherwise valid bearer header</param>
        /// <param name="command">The refresh code to use</param>
        /// <param name="service">The IAuthService to use</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Ok(AuthenticationResult) or throws resulting in BadRequest(ProblemDetail)</returns>
        public static async Task<IResult> RefreshTokenAsync
        (
            [FromHeader(Name = "Authorization")] string? authHeader,
            [FromBody] RefreshToken command,
            [FromServices] IAuthService service,
            CancellationToken cancellationToken
        )
        {
            if (authHeader == null)
                authHeader = "";
            else if (authHeader.Length > 7)
                authHeader = authHeader.Substring(7);

            var res = await service.RefreshTokenAsync(authHeader.Replace("Bearer ", ""), command.Code, cancellationToken);
            return Results.Ok(res);
        }

        /// <summary>
        /// Authenticate from user credentials
        /// </summary>
        /// <param name="command">The AuthenticateUserCredentials command</param>
        /// <param name="service">The IAuthService from DI</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Ok(AuthenticationResult) or throws resulting in BadRequest(ProblemDetail)</returns>
        public static async Task<IResult> UserCredentialsAsync
        (
            [FromBody] AuthenticateUserCredentials command,
            [FromServices] IAuthService service,
            CancellationToken cancellationToken
        )
        {
            var res = await service.AuthenticateAsync(command, cancellationToken);
            return Results.Ok(res);
        }
    }
}
