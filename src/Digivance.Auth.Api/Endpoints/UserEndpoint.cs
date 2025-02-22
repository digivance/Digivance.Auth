using Asp.Versioning;
using Digivance.Auth.Data.Commands;
using Digivance.Auth.Data.EntityFramework.Services;
using Digivance.Auth.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Serilog;

namespace Digivance.Auth.Api.Endpoints
{
    /// <summary>
    /// Defines our user endpoints (/user)
    /// </summary>
    public static class UserEndpoint
    {
        /// <summary>
        /// Registers necessary dependencies
        /// </summary>
        /// <param name="services">The IServiceCollection to configure</param>
        /// <returns>The same IServiceCollection for builder pattern</returns>
        public static IServiceCollection AddUserEndpointV1(this IServiceCollection services)
        {
            services.TryAddScoped<IUserService, EfUserService>();

            return services;
        }

        /// <summary>
        /// Registers our routes with the WebApplication host
        /// </summary>
        /// <param name="app">The WebApplication host to register routes in</param>
        /// <returns>The same WebApplication for builder pattern</returns>
        public static WebApplication UseUserEndpointV1(this WebApplication app)
        {
            Log.Debug("Configuring V1 user endpoints");

            var version = new ApiVersion(1, 0);
            var versionSet = app.NewApiVersionSet()
                .HasApiVersion(version)
                .ReportApiVersions()
                .Build();

            var route = app.MapGroup("/user")
                .WithApiVersionSet(versionSet);

            route
                .MapPost("/", CreateAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { Description = "Create a new user account", OperationId = "CreateUser" });

            route
                .MapDelete("/{userId:guid}", DeleteAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { Description = "Delete an existing user account", OperationId = "DeleteUser" });

            route
                .MapGet("/id-exists/{userId:Guid}", ExistsAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { Description = "Check if a user account exists for this id", OperationId = "UserExists" });

            route
                .MapGet("/email-exists/{email}/{tenantId:guid?}", ExistsByEmailAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { Description = "Check if a user account exists for this email", OperationId = "UserExistsByEmail" });

            route
                .MapGet("/username-exists/{username}/{tenantId:guid?}", ExistsByUsernameAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { Description = "Check if a user account exists for this username", OperationId = "UserExistsByUsername" });

            route
                .MapGet("/by-email/{email}/{tenantId:guid?}", GetByEmailAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { Description = "Get a user by email address", OperationId = "GetUserByEmail" });

            route
                .MapGet("/{userId:guid}", GetByIdAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { Description = "Get a user by unique id", OperationId = "GetUserById" });

            route
                .MapGet("/by-username/{username}/{tenantId:guid?}", GetByUsernameAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { Description = "Get a user by username", OperationId = "GetUserByUsername" });

            route
                .MapPut("/", UpdateAsync)
                .HasApiVersion(version)
                .WithOpenApi(opt => new(opt) { Description = "Update an existing user account", OperationId = "UpdateUser" });

            return app;
        }

        /// <summary>
        /// Creates a new user account
        /// </summary>
        /// <param name="command">The create user command</param>
        /// <param name="service">The IUserService to use</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Ok(user)</returns>
        public static async Task<IResult> CreateAsync
        (
            [FromBody] CreateUser command,
            [FromServices] IUserService service,
            CancellationToken cancellationToken
        )
        {
            var user = await service.CreateAsync(command, cancellationToken);
            return Results.Ok(user);
        }

        /// <summary>
        /// Deletes an existing user account
        /// </summary>
        /// <param name="userId">Unique id of the user account to delete</param>
        /// <param name="service">The IUserService to use</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Ok()</returns>
        public static async Task<IResult> DeleteAsync
        (
            [FromRoute] Guid userId,
            [FromServices] IUserService service,
            CancellationToken cancellationToken
        )
        {
            await service.DeleteByIdAsync(userId, cancellationToken);
            return Results.Ok();
        }

        /// <summary>
        /// Check to see if there is a user for this id
        /// </summary>
        /// <param name="userId">Unique id of the user account to check for</param>
        /// <param name="service">The IUserService to check in</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Ok(true) or NotFound(id)</returns>
        public static async Task<IResult> ExistsAsync
        (
            [FromRoute] Guid userId,
            [FromServices] IUserService service,
            CancellationToken cancellationToken
        )
        {
            var exists = await service.ExistsAsync(userId, cancellationToken);
            return exists ?
                Results.Ok(true) :
                Results.NotFound(userId);
        }

        /// <summary>
        /// Check to see if there is a user for this email
        /// </summary>
        /// <param name="tenantId">Optional unique id of the tenant to check in</param>
        /// <param name="email">Email address of the user account to check for</param>
        /// <param name="service">The IUserService to check in</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Ok(true) or NotFound(id)</returns>
        public static async Task<IResult> ExistsByEmailAsync
        (
            [FromRoute] Guid? tenantId,
            [FromRoute] string email,
            [FromServices] IUserService service,
            CancellationToken cancellationToken
        )
        {
            var exists = await service.ExistsByEmailAsync(tenantId, email, cancellationToken);
            return exists ?
                Results.Ok(true) :
                Results.NotFound(email);
        }

        /// <summary>
        /// Check to see if there is a user for this username
        /// </summary>
        /// <param name="tenantId">Optional unique id of the tenant to check in</param>
        /// <param name="username">Username of the user account to check for</param>
        /// <param name="service">The IUserService to check in</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Ok(true) or NotFound(id)</returns>
        public static async Task<IResult> ExistsByUsernameAsync
        (
            [FromRoute] Guid? tenantId,
            [FromRoute] string username,
            [FromServices] IUserService service,
            CancellationToken cancellationToken
        )
        {
            var exists = await service.ExistsByUsernameAsync(tenantId, username, cancellationToken);
            return exists ?
                Results.Ok(true) :
                Results.NotFound(username);
        }

        /// <summary>
        /// Get user account by email address
        /// </summary>
        /// <param name="tenantId">Optional unique id of the tenant to check in</param>
        /// <param name="email">Email address of the user account to get</param>
        /// <param name="service">The IUserService to check in</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Ok(true) or NotFound(id)</returns>
        public static async Task<IResult> GetByEmailAsync
        (
            [FromRoute] Guid? tenantId,
            [FromRoute] string email,
            [FromServices] IUserService service,
            CancellationToken cancellationToken
        )
        {
            var user = await service.GetByEmailAddressAsync(tenantId, email, cancellationToken);
            return user != null ?
                Results.Ok(user) :
                Results.NotFound(email);
        }

        /// <summary>
        /// Get user account by id
        /// </summary>
        /// <param name="userId">Unique id of the user account to get</param>
        /// <param name="service">The IUserService to check in</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Ok(true) or NotFound(id)</returns>
        public static async Task<IResult> GetByIdAsync
        (
            [FromRoute] Guid userId,
            [FromServices] IUserService service,
            CancellationToken cancellationToken
        )
        {
            var user = await service.GetByIdAsync(userId, cancellationToken);
            return user != null ?
                Results.Ok(user) :
                Results.NotFound(userId);
        }

        /// <summary>
        /// Get user account by username
        /// </summary>
        /// <param name="tenantId">Optional unique id of the tenant to check in</param>
        /// <param name="username">Username of the user account to get</param>
        /// <param name="service">The IUserService to check in</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Ok(true) or NotFound(id)</returns>
        public static async Task<IResult> GetByUsernameAsync
        (
            [FromRoute] Guid? tenantId,
            [FromRoute] string username,
            [FromServices] IUserService service,
            CancellationToken cancellationToken
        )
        {
            var user = await service.GetByUsernameAsync(tenantId, username, cancellationToken);
            return user != null ?
                Results.Ok(user) :
                Results.NotFound(username);
        }

        /// <summary>
        /// Update details of an existing user account
        /// </summary>
        /// <param name="command">The update user command</param>
        /// <param name="service">The IUserService to use</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Ok(user)</returns>
        public static async Task<IResult> UpdateAsync
        (
            [FromBody] UpdateUser command,
            [FromServices] IUserService service,
            CancellationToken cancellationToken
        )
        {
            var user = await service.UpdateAsync(command, cancellationToken);
            return user != null ?
                Results.Ok(user) :
                Results.NotFound(command.Id);
        }
    }
}
