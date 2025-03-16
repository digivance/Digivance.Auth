

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
    /// Defines our tenant endpoints (/tenant)
    /// </summary>
    public static class TenantEndpoint
    {
        /// <summary>
        /// Registers necessary dependencies
        /// </summary>
        /// <param name="services">The IServiceCollection to configure</param>
        /// <returns>The same IServiceCollection for builder pattern</returns>
        public static IServiceCollection AddTenantEndpointV1(this IServiceCollection services)
        {
            services.TryAddScoped<ITenantService, EfTenantService>();

            return services;
        }

        /// <summary>
        /// Registers our routes with the WebApplication host
        /// </summary>
        /// <param name="app">The WebApplication host to register routes in</param>
        /// <returns>The same WebApplication for builder pattern</returns>
        public static WebApplication UseTenantEndpointV1(this WebApplication app)
        {
            Log.Debug("Configuring V1 tenant endpoints");

            var version = new ApiVersion(1, 0);
            var versionSet = app.NewApiVersionSet()
                .HasApiVersion(version)
                .ReportApiVersions()
                .Build();

            var route = app.MapGroup("/tenant")
                .WithApiVersionSet(versionSet);

            route
                .MapPost("/", CreateAsync)
                .HasApiVersion(version)
                .Validate<CreateTenant>()
                .WithOpenApi(opt => new(opt) { Description = "Create a new tenant account", OperationId = "Createtenant" });

            route
                .MapDelete("/{tenantId:Guid}", DeleteAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { Description = "Delete an existing tenant account", OperationId = "Deletetenant" });

            route
                .MapGet("/id-exists/{tenantId:Guid}", ExistsAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { Description = "Check if a tenant account exists for this id", OperationId = "tenantExists" });

            route
                .MapGet("/tenantname-exists/{tenantname}", ExistsByNameAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { Description = "Check if a tenant account exists", OperationId = "tenantExistsByName" });

            route
                .MapGet("/{tenantId:Guid}", GetByIdAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { Description = "Get a tenant by unique id", OperationId = "GettenantById" });

            route
                .MapGet("/by-tenantname/{tenantname}", GetByNameAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { Description = "Get a tenant by it's name", OperationId = "GettenantByName" });

            route
                .MapPut("/", UpdateAsync)
                .HasApiVersion(version)
                .Validate<UpdateTenant>()
                .WithOpenApi(opt => new(opt) { Description = "Update an existing tenant account", OperationId = "Updatetenant" });

            return app;
        }

        /// <summary>
        /// Creates a new tenant account
        /// </summary>
        /// <param name="command">The create tenant command</param>
        /// <param name="service">The ITenantService to use</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Ok(tenant)</returns>
        public static async Task<IResult> CreateAsync
        (
            [FromBody] CreateTenant command,
            [FromServices] ITenantService service,
            CancellationToken cancellationToken
        )
        {
            var tenant = await service.CreateAsync(command, cancellationToken);
            return Results.Ok(tenant);
        }

        /// <summary>
        /// Deletes an existing tenant account
        /// </summary>
        /// <param name="tenantId">Unique id of the tenant account to delete</param>
        /// <param name="service">The ITenantService to use</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Ok()</returns>
        public static async Task<IResult> DeleteAsync
        (
            [FromRoute] Guid tenantId,
            [FromServices] ITenantService service,
            CancellationToken cancellationToken
        )
        {
            await service.DeleteByIdAsync(tenantId, cancellationToken);
            return Results.Ok();
        }

        /// <summary>
        /// Check to see if there is a tenant for this id
        /// </summary>
        /// <param name="tenantId">Unique id of the tenant account to check for</param>
        /// <param name="service">The ITenantService to check in</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Ok(true) or NotFound(id)</returns>
        public static async Task<IResult> ExistsAsync
        (
            [FromRoute] Guid tenantId,
            [FromServices] ITenantService service,
            CancellationToken cancellationToken
        )
        {
            var exists = await service.ExistsAsync(tenantId, cancellationToken);
            return exists ?
                Results.Ok(true) :
                Results.NotFound(tenantId);
        }

        /// <summary>
        /// Check to see if there is a tenant with this Name
        /// </summary>
        /// <param name="tenantname">tenantname of the tenant account to check for</param>
        /// <param name="service">The ITenantService to check in</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Ok(true) or NotFound(id)</returns>
        public static async Task<IResult> ExistsByNameAsync
        (
            [FromRoute] string tenantname,
            [FromServices] ITenantService service,
            CancellationToken cancellationToken
        )
        {
            var exists = await service.ExistsByNameAsync(tenantname, cancellationToken);
            return exists ?
                Results.Ok(true) :
                Results.NotFound(tenantname);
        }

        /// <summary>
        /// Get tenant account by id
        /// </summary>
        /// <param name="tenantId">Unique id of the tenant account to get</param>
        /// <param name="service">The ITenantService to check in</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Ok(true) or NotFound(id)</returns>
        public static async Task<IResult> GetByIdAsync
        (
            [FromRoute] Guid tenantId,
            [FromServices] ITenantService service,
            CancellationToken cancellationToken
        )
        {
            var tenant = await service.GetByIdAsync(tenantId, cancellationToken);
            return tenant != null ?
                Results.Ok(tenant) :
                Results.NotFound(tenantId);
        }

        /// <summary>
        /// Get tenant account by name
        /// </summary>
        /// <param name="tenantname">Name of the tenant account to get</param>
        /// <param name="service">The ITenantService to check in</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Ok(true) or NotFound(id)</returns>
        public static async Task<IResult> GetByNameAsync
        (
            [FromRoute] string tenantname,
            [FromServices] ITenantService service,
            CancellationToken cancellationToken
        )
        {
            var tenant = await service.GetByNameAsync(tenantname, cancellationToken);
            return tenant != null ?
                Results.Ok(tenant) :
                Results.NotFound(tenantname);
        }

        /// <summary>
        /// Update details of an existing tenant account
        /// </summary>
        /// <param name="command">The update tenant command</param>
        /// <param name="service">The ITenantService to use</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Ok(tenant)</returns>
        public static async Task<IResult> UpdateAsync
        (
            [FromBody] UpdateTenant command,
            [FromServices] ITenantService service,
            CancellationToken cancellationToken
        )
        {
            var tenant = await service.UpdateAsync(command, cancellationToken);
            return tenant != null ?
                Results.Ok(tenant) :
                Results.NotFound(command.Id);
        }
    }
}
