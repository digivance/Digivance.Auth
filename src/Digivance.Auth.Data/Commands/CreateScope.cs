using Digivance.Auth.Data.Services;
using FluentValidation;

namespace Digivance.Auth.Data.Commands
{
    /// <summary>
    /// Command to create a new scope
    /// </summary>
    public record CreateScope
    {
        /// <summary>
        /// Optional user friendly description of this scope
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Required unique per tenant name of this scope
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Required unique id of the tenant this scope exists in
        /// </summary>
        public Guid? TenantId { get; set; }
    }

    /// <summary>
    /// Fluent validations for our CreateScope command
    /// </summary>
    public class CreateScopeValidator : AbstractValidator<CreateScope>
    {
        public const string ERR_DESCRIPTION_TOO_LONG = "Description must be 4000 characters or less";
        public const string ERR_SCOPE_NAME_EXISTS = "A Scope with this name already exists in this tenant";
        public const string ERR_TENANT_MUST_EXIST = "A tenant was not found with this id";

        private readonly IScopeService scopeService;
        private readonly ITenantService tenantService;

        /// <summary>
        /// Constructs our validator and builds our rules
        /// </summary>
        /// <param name="scopeService">The IScopeService implementation to use in rules</param>
        /// <param name="tenantService">The ITenantService implementation to use in rules</param>
        public CreateScopeValidator(IScopeService scopeService, ITenantService tenantService)
        {
            this.scopeService = scopeService;
            this.tenantService = tenantService;

            // Description must be 4k or less chars if provided (can be null or empty)
            RuleFor(x => x.Description)
                .MaximumLength(4000)
                    .WithMessage(ERR_DESCRIPTION_TOO_LONG);

            // Name must be provided and be unique per tenant id
            RuleFor(x => x.Name)
                .NotEmpty()
                .MustAsync(BeUniqueNamePerTenantAsync)
                    .WithMessage(ERR_SCOPE_NAME_EXISTS);

            // TenantId is required and must exist
            RuleFor(x => x.TenantId)
                .MustAsync(BeExistingTenantAsync)
                    .WithMessage(ERR_TENANT_MUST_EXIST);
        }

        /// <summary>
        /// Custom rule helper to ensure the provided tenant id exists
        /// </summary>
        /// <param name="tenantId">The tenantId to ensure exists</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>true if a tenant exists with this id</returns>
        public Task<bool> BeExistingTenantAsync(Guid? tenantId, CancellationToken cancellationToken)
            => tenantService.ExistsAsync(tenantId, cancellationToken);

        /// <summary>
        /// Custom rule helper to ensure this is a unique name for the provided tenant
        /// </summary>
        /// <param name="command">The command we are validating</param>
        /// <param name="name">Is the same as command.Name</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if this is a unique name/tenantid combination</returns>
        public async Task<bool> BeUniqueNamePerTenantAsync(CreateScope command, string name, CancellationToken cancellationToken) =>
            !await scopeService.ExistsByNameAsync(command.TenantId, name, cancellationToken);
    }
}
