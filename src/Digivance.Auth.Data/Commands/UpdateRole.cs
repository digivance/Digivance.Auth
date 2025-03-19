using Digivance.Auth.Data.Services;
using FluentValidation;

namespace Digivance.Auth.Data.Commands
{
    /// <summary>
    /// Command to update an existing role
    /// </summary>
    public record UpdateRole
    {
        /// <summary>
        /// User friendly description of the role to Update
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Unique id of the role to update
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Required unique per scope name of this role
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Optional collection of unique id's of permissions to assign to this role
        /// </summary>
        public ICollection<Guid>? PermissionIds { get; set; }
    }

    /// <summary>
    /// Fluent validations for our UpdateRole command
    /// </summary>
    public class UpdateRoleValidator : AbstractValidator<UpdateRole>
    {
        public const string ERR_DESCRIPTION_TOO_LONG = "Description must be 4000 characters or less";
        public const string ERR_ROLE_NAME_EXISTS = "A Role with this name already exists in this scope";
        public const string ERR_ROLENAME_LENGTH = "Role name must be 100 characters or less";
        public const string ERR_ROLEID_NOTFOUND = "No role found for this id";
        public const string ERR_ROLE_NAME_EMPTY = "Role name is required";

        private readonly IRoleService roleService;

        /// <summary>
        /// Constructs our validator and builds our rules
        /// </summary>
        /// <param name="service">The IRoleService implementation to use in rules</param>
        public UpdateRoleValidator(IRoleService roleService)
        {
            this.roleService = roleService;

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


            // Role with this Id must exists
            RuleFor(x => x.Id)
                .MustAsync(BeExistingRoleAsync)
                    .WithMessage(ERR_ROLEID_NOTFOUND);
        }

        /// <summary>
        /// Custom rule helper to ensure the provided id exists
        /// </summary>
        /// <param name="id">The id to ensure exists</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>true if a role exists with this id</returns>
        public Task<bool> BeExistingRoleAsync(Guid id, CancellationToken cancellationToken)
            => roleService.ExistsAsync(id, cancellationToken);

        /// <summary>
        /// Custom rule helper to ensure this is a unique name for the provided scope
        /// </summary>
        /// <param name="command">The command we are validating</param>
        /// <param name="name">Is the same as command.Name</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if this is a unique name/scopeid combination</returns>
        public async Task<bool> BeUniqueNamePerScopeAsync(UpdateRole command, string name, CancellationToken cancellationToken)
        {
            var scopeId = await roleService.GetScopeId(command.Id, cancellationToken);
            if (scopeId == null)
                return false;

            return !await roleService.ExistsAsync(scopeId.Value, name, cancellationToken);
        }
    }
}
