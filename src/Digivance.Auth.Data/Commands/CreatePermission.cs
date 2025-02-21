using Digivance.Auth.Data.Services;
using FluentValidation;

namespace Digivance.Auth.Data.Commands
{
    /// <summary>
    /// Command to create a new permission
    /// </summary>
    public record CreatePermission
    {
        /// <summary>
        /// Optional user friendly description to create this permission with
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Required unique per scope name for this permission
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Optional unique id of the scope this permission exists in (null represents
        /// global for this tenant)
        /// </summary>
        public Guid? ScopeId { get; set; }

        /// <summary>
        /// Required unique id of the tenant this permission exists in
        /// </summary>
        public Guid TenantId { get; set; }
    }

    /// <summary>
    /// Fluent validations for our CreatePermission command
    /// </summary>
    public class CreatePermissionValidator : AbstractValidator<CreatePermission>
    {
        public const string ERR_DESCRIPTION_TOO_LONG = "Description must be 4000 characters or less";
        public const string ERR_PERMISSION_NAME_EXISTS = "A permission with this name already exists in this scope";
        public const string ERR_SCOPE_MUST_EXISTS = "Scope must exist, or be omitted for global";
        public const string ERR_TENANT_MUST_EXIST = "A tenant was not found with this id";

        private readonly IPermissionService permissionService;
        private readonly IScopeService scopeService;
        private readonly ITenantService tenantService;

        /// <summary>
        /// Constructs our validator and builds our rules
        /// </summary>
        /// <param name="service">The IPermissionService implementation to use in rules</param>
        public CreatePermissionValidator(IPermissionService permissionService, IScopeService scopeService, ITenantService tenantService)
        {
            this.permissionService = permissionService;
            this.scopeService = scopeService;
            this.tenantService = tenantService;

            // Description must be 4k or less chars if provided (can be null or empty)
            RuleFor(x => x.Description)
                .MaximumLength(4000)
                    .WithMessage(ERR_DESCRIPTION_TOO_LONG);

            // Name must be provided and be unique per scope id
            RuleFor(x => x.Name)
                .NotEmpty()
                .MustAsync(BeUniqueNamePerScopeAsync)
                    .WithMessage(ERR_PERMISSION_NAME_EXISTS);

            // ScopeId must exist if provided
            RuleFor(x => x.ScopeId)
                .MustAsync(BeExistingScopeOrNullAsync)
                    .WithMessage(ERR_SCOPE_MUST_EXISTS);

            // TenantId is required and must exist
            RuleFor(x => x.TenantId)
                .NotEmpty()
                .MustAsync(BeExistingTenantAsync)
                    .WithMessage(ERR_TENANT_MUST_EXIST);
        }

        /// <summary>
        /// Custom rule helper to ensure the requested scope exists if ScopeId is provided
        /// </summary>
        /// <param name="scopeId">The scopeId to ensure exists</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>true if scopeId is null, or a scope exists for this scopeId</returns>
        public Task<bool> BeExistingScopeOrNullAsync(Guid? scopeId, CancellationToken cancellationToken)
        {
            if (scopeId == null)
                return Task.FromResult(true);

            return scopeService.ExistsAsync(scopeId.Value, cancellationToken);
        }

        /// <summary>
        /// Custom rule helper to ensure the provided tenant id exists
        /// </summary>
        /// <param name="tenantId">The tenantId to ensure exists</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>true if a tenant exists with this id</returns>
        public Task<bool> BeExistingTenantAsync(Guid tenantId, CancellationToken cancellationToken)
            => tenantService.ExistsAsync(tenantId, cancellationToken);

        /// <summary>
        /// Custom rule helper to ensure this is a unique name for the provided scope
        /// </summary>
        /// <param name="command">The command we are validating</param>
        /// <param name="name">Is the same as command.Name</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if this is a unique name/scopeid combination</returns>
        public async Task<bool> BeUniqueNamePerScopeAsync(CreatePermission command, string name, CancellationToken cancellationToken) =>
            !await permissionService.ExistsByNameAsync(command.ScopeId, name, cancellationToken);
    }
}
