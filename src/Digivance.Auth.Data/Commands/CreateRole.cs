using Digivance.Auth.Data.Services;
using FluentValidation;

namespace Digivance.Auth.Data.Commands
{
    /// <summary>
    /// Command to create a new role
    /// </summary>
    public record CreateRole
    {
        /// <summary>
        /// User friendly description of the role to create
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Required unique per scope name of this role
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Required, the unique if of the scope to create this role in
        /// </summary>
        public Guid ScopeId { get; set; }

        /// <summary>
        /// Required unique id of the tenant this role exists in
        /// </summary>
        public Guid? TenantId { get; set; }
    }

    /// <summary>
    /// Fluent validations for our CreateRole command
    /// </summary>
    public class CreateRoleValidator : AbstractValidator<CreateRole>
    {
        public const string ERR_DESCRIPTION_TOO_LONG = "Description must be 4000 characters or less";
        public const string ERR_ROLE_NAME_EXISTS = "A Role with this name already exists in this scope";
        public const string ERR_SCOPE_MUST_EXISTS = "Scope must exist, or be omitted for global";
        public const string ERR_TENANT_MUST_EXIST = "A tenant was not found with this id";
        public const string ERR_ROLENAME_LENGTH = "Role name must be 100 characters or less";
        public const string ERR_ROLE_NAME_EMPTY = "Role name is required";

        private readonly IRoleService roleService;
        private readonly IScopeService scopeService;
        private readonly ITenantService tenantService;

        /// <summary>
        /// Constructs our validator and builds our rules
        /// </summary>
        /// <param name="service">The IRoleService implementation to use in rules</param>
        public CreateRoleValidator(IRoleService roleService, IScopeService scopeService, ITenantService tenantService)
        {
            this.roleService = roleService;
            this.scopeService = scopeService;
            this.tenantService = tenantService;

            // Description must be 4k or less chars if provided (can be null or empty)
            RuleFor(x => x.Description)
                .MaximumLength(4000)
                    .WithMessage(ERR_DESCRIPTION_TOO_LONG);

            // Name must be provided and be unique per scope id
            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithMessage(ERR_ROLE_NAME_EMPTY)
                .MaximumLength(100)
                    .WithMessage(ERR_ROLENAME_LENGTH)
                .MustAsync(BeUniqueNamePerScopeAsync)
                    .WithMessage(ERR_ROLE_NAME_EXISTS);

            // ScopeId must exist
            RuleFor(x => x.ScopeId)
                .MustAsync(BeExistingScopeAsync)
                    .WithMessage(ERR_SCOPE_MUST_EXISTS);

            RuleFor(x => x.TenantId)
                .MustAsync(BeExistingTenantAsync)
                    .WithMessage(ERR_TENANT_MUST_EXIST);
        }

        /// <summary>
        /// Custom rule helper to ensure the requested scope exists if ScopeId
        /// </summary>
        /// <param name="scopeId">The scopeId to ensure exists</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>true if scope exists for this scopeId</returns>
        public Task<bool> BeExistingScopeAsync(Guid scopeId, CancellationToken cancellationToken)
            => scopeService.ExistsAsync(scopeId, cancellationToken);

        /// <summary>
        /// Custom rule helper to ensure the provided tenant id exists
        /// </summary>
        /// <param name="tenantId">The tenantId to ensure exists</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>true if a tenant exists with this id</returns>
        public Task<bool> BeExistingTenantAsync(Guid? tenantId, CancellationToken cancellationToken)
            => tenantService.ExistsAsync(tenantId, cancellationToken);

        /// <summary>
        /// Custom rule helper to ensure this is a unique name for the provided scope
        /// </summary>
        /// <param name="command">The command we are validating</param>
        /// <param name="name">Is the same as command.Name</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if this is a unique name/scopeid combination</returns>
        public async Task<bool> BeUniqueNamePerScopeAsync(CreateRole command, string name, CancellationToken cancellationToken) =>
            !await roleService.ExistsAsync(command.ScopeId, name, cancellationToken);
    }
}
