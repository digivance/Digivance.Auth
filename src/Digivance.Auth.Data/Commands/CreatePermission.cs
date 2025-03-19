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
        /// The access this permission allows, such as Create, Read, Update or Delete
        /// </summary>
        public string EntityAccess { get; set; }

        /// <summary>
        /// The type of entity this permission applies to (Database or custom entity type name)
        /// </summary>
        public string EntityType { get; set; }

        /// <summary>
        /// The unique id of the entity this permission applies to, or null for entities of this
        /// type in this scope. (Like global access)
        /// </summary>
        public Guid? EntityId { get; set; }

        /// <summary>
        /// Optional unique id of the scope this permission exists in (null represents
        /// global for this tenant)
        /// </summary>
        public Guid ScopeId { get; set; }
    }

    /// <summary>
    /// Fluent validations for our CreatePermission command
    /// </summary>
    public class CreatePermissionValidator : AbstractValidator<CreatePermission>
    {
        public const string ERR_DESCRIPTION_TOO_LONG = "Description must be 4000 characters or less";
        public const string ERR_ENTITY_ACCESS_TOO_LONG = "Entity access must be 255 characters or less";
        public const string ERR_ENTITY_TYPE_TOO_LONG = "Entity type must be 255 characters or less";
        public const string ERR_SCOPE_MUST_EXISTS = "Scope must exist, or be omitted for global";

        private readonly IPermissionService permissionService;
        private readonly IScopeService scopeService;

        /// <summary>
        /// Constructs our validator and builds our rules
        /// </summary>
        /// <param name="service">The IPermissionService implementation to use in rules</param>
        public CreatePermissionValidator(IPermissionService permissionService, IScopeService scopeService)
        {
            this.permissionService = permissionService;
            this.scopeService = scopeService;

            // Description must be 4k or less chars if provided (can be null or empty)
            RuleFor(x => x.Description)
                .MaximumLength(4000)
                    .WithMessage(ERR_DESCRIPTION_TOO_LONG);

            RuleFor(x => x.EntityAccess)
                .NotEmpty()
                .MaximumLength(255)
                    .WithMessage(ERR_ENTITY_ACCESS_TOO_LONG);

            RuleFor(x => x.EntityType)
                .NotEmpty()
                .MaximumLength(255)
                    .WithMessage(ERR_ENTITY_TYPE_TOO_LONG);

            // ScopeId must exist if provided
            RuleFor(x => x.ScopeId)
                .MustAsync(BeExistingScopeAsync)
                    .WithMessage(ERR_SCOPE_MUST_EXISTS);
        }

        /// <summary>
        /// Custom rule helper to ensure the requested scope exists if ScopeId is provided
        /// </summary>
        /// <param name="scopeId">The scopeId to ensure exists</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>true if scopeId is null, or a scope exists for this scopeId</returns>
        public Task<bool> BeExistingScopeAsync(Guid scopeId, CancellationToken cancellationToken)
            => scopeService.ExistsAsync(scopeId, cancellationToken);
    }
}
