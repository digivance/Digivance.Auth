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
    /// Defines our role endpoints (/role)
    /// </summary>
    public static class RoleEndpoint
    {
        /// <summary>
        /// Registers necessary dependencies
        /// </summary>
        /// <param name="services">The IServiceCollection to configure</param>
        /// <returns>The same IServiceCollection for builder pattern</returns>
        public static IServiceCollection AddRoleEndpointV1(this IServiceCollection services)
        {
            services.TryAddScoped<IRoleService, EfRoleService>();

            return services;
        }

        /// <summary>
        /// Registers our routes with the WebApplication host
        /// </summary>
        /// <param name="app">The WebApplication host to register routes in</param>
        /// <returns>The same WebApplication for builder pattern</returns>
        public static WebApplication UseRoleEndpointV1(this WebApplication app)
        {
            Log.Debug("Configuring V1 role endpoints");

            var version = new ApiVersion(1, 0);
            var versionSet = app.NewApiVersionSet()
                .HasApiVersion(version)
                .ReportApiVersions()
                .Build();

            var route = app.MapGroup("/role")
                .WithApiVersionSet(versionSet);

            route
                .MapPost("/", CreateAsync)
                .HasApiVersion(version)
                .Validate<CreateRole>()
                .WithOpenApi(opt => new(opt) { Description = "Create a new role", OperationId = "Createrole" });

            route
                .MapDelete("/{roleId:Guid}", DeleteAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { Description = "Delete an existing role", OperationId = "Deleterole" });

            route
                .MapGet("/id-exists/{roleId:Guid}", ExistsAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { Description = "Check if a role exists for this id", OperationId = "roleExists" });

            route
                .MapGet("/rolename-exists/{rolename}/{scopeId:Guid?}", ExistsByRolenameAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { Description = "Check if a role exists for this name", OperationId = "roleExistsByRolename" });

            route
                .MapGet("/{roleId:Guid}", GetByIdAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { Description = "Get a role by unique id", OperationId = "GetroleById" });

            route
                .MapGet("/by-rolename/{rolename}/{scopeId:Guid?}", GetByRolenameAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { Description = "Get a role by rolename", OperationId = "GetRoleByRolename" });

            route
                .MapPut("/", UpdateAsync)
                .HasApiVersion(version)
                .Validate<UpdateRole>()
                .WithOpenApi(opt => new(opt) { Description = "Update an existing role ", OperationId = "Updaterole" });

            return app;
        }

        /// <summary>
        /// Creates a new role
        /// </summary>
        /// <param name="command">The create role command</param>
        /// <param name="service">The IRoleService to use</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Ok(role)</returns>
        public static async Task<IResult> CreateAsync
        (
            [FromBody] CreateRole command,
            [FromServices] IRoleService service,
            CancellationToken cancellationToken
        )
        {
            var role = await service.CreateAsync(command, cancellationToken);
            return Results.Ok(role);
        }

        /// <summary>
        /// Deletes an existing role 
        /// </summary>
        /// <param name="roleId">Unique id of the role to delete</param>
        /// <param name="service">The IRoleService to use</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Ok()</returns>
        public static async Task<IResult> DeleteAsync
        (
            [FromRoute] Guid roleId,
            [FromServices] IRoleService service,
            CancellationToken cancellationToken
        )
        {
            await service.DeleteAsync(roleId, cancellationToken);
            return Results.Ok();
        }

        /// <summary>
        /// Check to see if there is a role for this id
        /// </summary>
        /// <param name="roleId">Unique id of the role to check for</param>
        /// <param name="service">The IRoleService to check in</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Ok(true) or NotFound(id)</returns>
        public static async Task<IResult> ExistsAsync
        (
            [FromRoute] Guid roleId,
            [FromServices] IRoleService service,
            CancellationToken cancellationToken
        )
        {
            var exists = await service.ExistsAsync(roleId, cancellationToken);
            return exists ?
                Results.Ok(true) :
                Results.NotFound(roleId);
        }

        /// <summary>
        /// Check to see if there is a role for this name
        /// </summary>
        /// <param name="scopeId">Unique id of the scope to check in</param>
        /// <param name="rolename">name of the role to check for</param>
        /// <param name="service">The IRoleService to check in</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Ok(true) or NotFound(id)</returns>
        public static async Task<IResult> ExistsByRolenameAsync
        (
            [FromRoute] Guid scopeId,
            [FromRoute] string rolename,
            [FromServices] IRoleService service,
            CancellationToken cancellationToken
        )
        {
            var exists = await service.ExistsAsync(scopeId, rolename, cancellationToken);
            return exists ?
                Results.Ok(true) :
                Results.NotFound(rolename);
        }

        /// <summary>
        /// Get role by id
        /// </summary>
        /// <param name="roleId">Unique id of the role to get</param>
        /// <param name="service">The IRoleService to check in</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Ok(true) or NotFound(id)</returns>
        public static async Task<IResult> GetByIdAsync
        (
            [FromRoute] Guid roleId,
            [FromServices] IRoleService service,
            CancellationToken cancellationToken
        )
        {
            var role = await service.GetAsync(roleId, cancellationToken);
            return role != null ?
                Results.Ok(role) :
                Results.NotFound(roleId);
        }

        /// <summary>
        /// Get role by it's name
        /// </summary>
        /// <param name="scopeId">Unique id of the scope to check in</param>
        /// <param name="rolename">rolename of the role  to get</param>
        /// <param name="service">The IRoleService to check in</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Ok(true) or NotFound(id)</returns>
        public static async Task<IResult> GetByRolenameAsync
        (
            [FromRoute] Guid scopeId,
            [FromRoute] string rolename,
            [FromServices] IRoleService service,
            CancellationToken cancellationToken
        )
        {
            var role = await service.GetAsync(scopeId, rolename, cancellationToken);
            return role != null ?
                Results.Ok(role) :
                Results.NotFound(rolename);
        }

        /// <summary>
        /// Returns a ScopeId through roleId of role
        /// </summary>
        /// <param name="roleId">Unique id of the role</param>
        /// <param name="service">The IRoleService to check in</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Ok(true) or NotFound(id)</returns>
        public static async Task<IResult> GetScopeByRoleId
        (
            [FromRoute] Guid roleId,
            [FromServices] IRoleService service,
            CancellationToken cancellationToken
        )
        {
            var role = await service.GetScopeId(roleId, cancellationToken);
            return role != null ?
                Results.Ok(role) :
                Results.NotFound(roleId);
        }




        /// <summary>
        /// Update details of an existing role 
        /// </summary>
        /// <param name="command">The update role command</param>
        /// <param name="service">The IRoleService to use</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Ok(role)</returns>
        public static async Task<IResult> UpdateAsync
        (
            [FromBody] UpdateRole command,
            [FromServices] IRoleService service,
            CancellationToken cancellationToken
        )
        {
            var role = await service.UpdateAsync(command, cancellationToken);
            return role != null ?
                Results.Ok(role) :
                Results.NotFound(command.Id);
        }
    }
}
