using Asp.Versioning;
using Digivance.Auth.Api.Middleware;
using Digivance.Auth.Data.Commands;
using Digivance.Auth.Data.EntityFramework.Services;
using Digivance.Auth.Data.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Serilog;

namespace Digivance.Auth.Api.Endpoints
{
    /// <summary>
    /// Defines our permission endpoints (/permission)
    /// </summary>
    public static class PermissionEndpoint
    {
        /// <summary>
        /// Registers necessary dependencies
        /// </summary>
        /// <param name="services">The IServiceCollection to configure</param>
        /// <returns>The same IServiceCollection for builder pattern</returns>
        public static IServiceCollection AddPermissionEndpointV1(this IServiceCollection services)
        {
            services.TryAddScoped<IPermissionService, EfPermissionService>();

            return services;
        }

        /// <summary>
        /// Registers our routes with the WebApplication host
        /// </summary>
        /// <param name="app">The WebApplication host to register routes in</param>
        /// <returns>The same WebApplication for builder pattern</returns>
        public static WebApplication UsePermissionEndpointV1(this WebApplication app)
        {
            Log.Debug("Configuring V1 permission endpoints");

            var version = new ApiVersion(1, 0);
            var versionSet = app.NewApiVersionSet()
                .HasApiVersion(version)
                .ReportApiVersions()
                .Build();

            var route = app.MapGroup("/permission")
                .WithApiVersionSet(versionSet);

            route
                .MapPost("/", CreateAsync)
                .HasApiVersion(version)
                .Validate<CreatePermission>()
                .WithOpenApi(opt => new(opt) { Description = "Create a new permission", OperationId = "Createpermission" });

            route
                .MapDelete("/{permissionId:Guid}", DeleteAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { Description = "Delete an existing permission", OperationId = "Deletepermission" });

            route
                .MapGet("/id-exists/{permissionId:Guid}", ExistsAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { Description = "Check if a permission exists for this id", OperationId = "permissionExists" });

            route
                .MapGet("/permissionname-exists/{entityAccess}/{entityType}/{entityId:Guid?}/{scopeId:Guid}", ExistsByNameAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { Description = "Check if a permission exists for this name", OperationId = "permissionExistsByScopeId" });

            route
                .MapGet("/{permissionId:Guid}", GetAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { Description = "Get a permission by unique id", OperationId = "GetpermissionById" });

            route
                .MapGet("/by-permissionname/{entityAccess}/{entityType}/{entityId:Guid?}/{scopeId:Guid}", GetByNameAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { Description = "Get a permission by permissionname", OperationId = "GetpermissionByName" });

            route
                .MapPut("/", UpdateAsync)
                .HasApiVersion(version)
                .Validate<UpdatePermission>()
                .WithOpenApi(opt => new(opt) { Description = "Update an existing permission ", OperationId = "Updatepermission" });

            return app;
        }

        /// <summary>
        /// Creates a new permission 
        /// </summary>
        /// <param name="command">The create permission command</param>
        /// <param name="service">The IPermissionService to use</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Ok(permission)</returns>
        public static async Task<IResult> CreateAsync
        (
            [FromBody] CreatePermission command,
            [FromServices] IPermissionService service,
            CancellationToken cancellationToken
        )
        {
            var permission = await service.CreateAsync(command, cancellationToken);
            return Results.Ok(permission);
        }

        /// <summary>
        /// Deletes an existing permission 
        /// </summary>
        /// <param name="permissionId">Unique id of the permission to delete</param>
        /// <param name="service">The IPermissionService to use</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Ok()</returns>
        public static async Task<IResult> DeleteAsync
        (
            [FromRoute] Guid permissionId,
            [FromServices] IPermissionService service,
            CancellationToken cancellationToken
        )
        {
            await service.DeleteAsync(permissionId, cancellationToken);
            return Results.Ok();
        }

        /// <summary>
        /// Check to see if there is a permission for this id
        /// </summary>
        /// <param name="permissionId">Unique id of the permission  to check for</param>
        /// <param name="service">The IPermissionService to check in</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Ok(true) or NotFound(id)</returns>
        public static async Task<IResult> ExistsAsync
        (
            [FromRoute] Guid permissionId,
            [FromServices] IPermissionService service,
            CancellationToken cancellationToken
        )
        {
            var exists = await service.ExistsAsync(permissionId, cancellationToken);
            return exists ?
                Results.Ok(true) :
                Results.NotFound(permissionId);
        }

        /// <summary>
        /// Checks to see if a permission exists by it's name within a given scope
        /// </summary>
        /// <param name="scopeId">Unique id of the scope to look in</param>
        /// <param name="entityAccess">The access permission to check for (such as "Create", "Read", "Update", "Delete")</param>
        /// <param name="entityId">Unique id of the entity to check for permission to</param>
        /// <param name="entityType">The type of entity to check for permission to</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if a permission with this name exists within this scope</returns>
        public static async Task<IResult> ExistsByNameAsync
        (
            [FromRoute] Guid scopeId,
            [FromRoute] string entityAccess,
            [FromRoute] string entityType,
            [FromRoute] Guid? entityId,
            [FromServices] IPermissionService service,
            CancellationToken cancellationToken
        )
        {
            var exists = await service.ExistsAsync(scopeId, entityAccess, entityType, entityId, cancellationToken);
            return exists ?
                Results.Ok(true) :
                Results.NotFound(scopeId);
        }

        /// <summary>
        /// Get permission by id
        /// </summary>
        /// <param name="permissionId">Unique id of the permission to get</param>
        /// <param name="service">The IPermissionService to check in</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Ok(true) or NotFound(id)</returns>
        public static async Task<IResult> GetAsync
        (
            [FromRoute] Guid permissionId,
            [FromServices] IPermissionService service,
            CancellationToken cancellationToken
        )
        {
            var permission = await service.GetAsync(permissionId, cancellationToken);
            return permission != null ?
                Results.Ok(permission) :
                Results.NotFound(permissionId);
        }

        /// <summary>
        /// Get a Permission by scope id and name
        /// </summary>
        /// <param name="scopeId">Unique id of the scope to get permission from</param>
        /// <param name="entityAccess">The access permission to check for (such as "Create", "Read", "Update", "Delete")</param>
        /// <param name="entityId">Unique id of the entity to check for permission to</param>
        /// <param name="entityType">The type of entity to check for permission to</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Ok(true) or NotFound(id)</returns>
        public static async Task<IResult> GetByNameAsync
        (
            [FromRoute] Guid scopeId,
            [FromRoute] string entityAccess,
            [FromRoute] string entityType,
            [FromRoute] Guid? entityId,
            [FromServices] IPermissionService service,
            CancellationToken cancellationToken
        )
        {
            var permission = await service.GetAsync(scopeId, entityAccess, entityType, entityId, cancellationToken);
            return permission != null ?
                Results.Ok(permission) :
                Results.NotFound(scopeId);
        }

        /// <summary>
        /// Update details of an existing permission 
        /// </summary>
        /// <param name="command">The update permission command</param>
        /// <param name="service">The IPermissionService to use</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Ok(permission)</returns>
        public static async Task<IResult> UpdateAsync
        (
            [FromBody] UpdatePermission command,
            [FromServices] IPermissionService service,
            CancellationToken cancellationToken
        )
        {
            var permission = await service.UpdateAsync(command, cancellationToken);
            return permission != null ?
                Results.Ok(permission) :
                Results.NotFound(command.Id);
        }
    }
}
