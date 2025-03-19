using Asp.Versioning;
using Digivance.Auth.Api.Middleware;
using Digivance.Auth.Data.Commands;
using Digivance.Auth.Data.EntityFramework.Services;
using Digivance.Auth.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Serilog;

namespace Digivance.Auth.Api.Endpoints
{
    /// <summary>
    /// Defines our scope endpoints (/scope)
    /// </summary>
    public static class ScopeEndpoint
    {
        /// <summary>
        /// Registers necessary dependencies
        /// </summary>
        /// <param name="services">The IServiceCollection to configure</param>
        /// <returns>The same IServiceCollection for builder pattern</returns>
        public static IServiceCollection AddScopeEndpointV1(this IServiceCollection services)
        {
            services.TryAddScoped<IScopeService, EfScopeService>();

            return services;
        }

        /// <summary>
        /// Registers our routes with the WebApplication host
        /// </summary>
        /// <param name="app">The WebApplication host to register routes in</param>
        /// <returns>The same WebApplication for builder pattern</returns>
        public static WebApplication UseScopeEndpointV1(this WebApplication app)
        {
            Log.Debug("Configuring V1 scope endpoints");

            var version = new ApiVersion(1, 0);
            var versionSet = app.NewApiVersionSet()
                .HasApiVersion(version)
                .ReportApiVersions()
                .Build();

            var route = app.MapGroup("/scope")
                .WithApiVersionSet(versionSet);

            route
                .MapPost("/", CreateAsync)
                .HasApiVersion(version)
                .Validate<CreateScope>()
                .WithOpenApi(opt => new(opt) { Description = "Create a new scope", OperationId = "Createscope" });

            route
                .MapDelete("/{scopeId:Guid}", DeleteAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { Description = "Delete an existing scope", OperationId = "Deletescope" });

            route
                .MapGet("/id-exists/{scopeId:Guid}", ExistsAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { Description = "Check if a scope exists for this id", OperationId = "scopeExists" });

            route
                .MapGet("/scopename-exists/{scopename}/{tenantId:Guid?}", ExistsByScopenameAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { Description = "Check if a scope exists for this name", OperationId = "scopeExistsByScopename" });

            route
                .MapGet("/{scopeId:Guid}", GetByIdAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { Description = "Get a scope by unique id", OperationId = "GetscopeById" });

            route
                .MapGet("/by-scopename/{scopename}/{tenantId:Guid?}", GetByScopenameAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { Description = "Get a scope by scopename", OperationId = "GetscopeByscopename" });

            route
                .MapPut("/", UpdateAsync)
                .HasApiVersion(version)
                .Validate<UpdateScope>()
                .WithOpenApi(opt => new(opt) { Description = "Update an existing scope account", OperationId = "Updatescope" });

            return app;
        }

        /// <summary>
        /// Creates a new scope account
        /// </summary>
        /// <param name="command">The create scope command</param>
        /// <param name="service">The IScopeService to use</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Ok(scope)</returns>
        public static async Task<IResult> CreateAsync
        (
            [FromBody] CreateScope command,
            [FromServices] IScopeService service,
            CancellationToken cancellationToken
        )
        {
            var scope = await service.CreateAsync(command, cancellationToken);
            return Results.Ok(scope);
        }

        /// <summary>
        /// Deletes an existing scope account
        /// </summary>
        /// <param name="scopeId">Unique id of the scope account to delete</param>
        /// <param name="service">The IScopeService to use</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Ok()</returns>
        public static async Task<IResult> DeleteAsync
        (
            [FromRoute] Guid scopeId,
            [FromServices] IScopeService service,
            CancellationToken cancellationToken
        )
        {
            await service.DeleteAsync(scopeId, cancellationToken);
            return Results.Ok();
        }

        /// <summary>
        /// Check to see if there is a scope for this id
        /// </summary>
        /// <param name="scopeId">Unique id of the scope account to check for</param>
        /// <param name="service">The IScopeService to check in</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Ok(true) or NotFound(id)</returns>
        public static async Task<IResult> ExistsAsync
        (
            [FromRoute] Guid scopeId,
            [FromServices] IScopeService service,
            CancellationToken cancellationToken
        )
        {
            var exists = await service.ExistsAsync(scopeId, cancellationToken);
            return exists ?
                Results.Ok(true) :
                Results.NotFound(scopeId);
        }

        /// <summary>
        /// Check to see if there is a scope for this name
        /// </summary>
        /// <param name="tenantId">Optional unique id of the tenant to check in</param>
        /// <param name="scopename">name of the scope to check for</param>
        /// <param name="service">The IScopeService to check in</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Ok(true) or NotFound(id)</returns>
        public static async Task<IResult> ExistsByScopenameAsync
        (
            [FromRoute] Guid? tenantId,
            [FromRoute] string scopename,
            [FromServices] IScopeService service,
            CancellationToken cancellationToken
        )
        {
            var exists = await service.ExistsAsync(tenantId, scopename, cancellationToken);
            return exists ?
                Results.Ok(true) :
                Results.NotFound(scopename);
        }

        /// <summary>
        /// Get scope by id
        /// </summary>
        /// <param name="scopeId">Unique id of the scope to get</param>
        /// <param name="service">The IScopeService to check in</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Ok(true) or NotFound(id)</returns>
        public static async Task<IResult> GetByIdAsync
        (
            [FromRoute] Guid scopeId,
            [FromServices] IScopeService service,
            CancellationToken cancellationToken
        )
        {
            var scope = await service.GetAsync(scopeId, cancellationToken);
            return scope != null ?
                Results.Ok(scope) :
                Results.NotFound(scopeId);
        }

        /// <summary>
        /// Get scope by name
        /// </summary>
        /// <param name="tenantId">Optional unique id of the tenant to check in</param>
        /// <param name="scopename">scopename of the scope account to get</param>
        /// <param name="service">The IScopeService to check in</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Ok(true) or NotFound(id)</returns>
        public static async Task<IResult> GetByScopenameAsync
        (
            [FromRoute] Guid? tenantId,
            [FromRoute] string scopename,
            [FromServices] IScopeService service,
            CancellationToken cancellationToken
        )
        {
            var scope = await service.GetAsync(tenantId, scopename, cancellationToken);
            return scope != null ?
                Results.Ok(scope) :
                Results.NotFound(scopename);
        }

        /// <summary>
        /// Update details of an existing scope 
        /// </summary>
        /// <param name="command">The update scope command</param>
        /// <param name="service">The IScopeService to use</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Ok(scope)</returns>
        public static async Task<IResult> UpdateAsync
        (
            [FromBody] UpdateScope command,
            [FromServices] IScopeService service,
            CancellationToken cancellationToken
        )
        {
            var scope = await service.UpdateAsync(command, cancellationToken);
            return scope != null ?
                Results.Ok(scope) :
                Results.NotFound(command.Id);
        }
    }
}
