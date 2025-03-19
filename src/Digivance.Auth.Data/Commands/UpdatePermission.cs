using Digivance.Auth.Data.Services;
using FluentValidation;

namespace Digivance.Auth.Data.Commands
{
    /// <summary>
    /// Command to update a permission record
    /// </summary>
    public record UpdatePermission
    {
        /// <summary>
        /// Human friendly description of this permission
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Id of the permission to update
        /// </summary>
        public Guid Id { get; set; }
    }

    /// <summary>
    /// Fluent validations for our UpdatePermission command
    /// </summary>
    public class UpdatePermissionValidator : AbstractValidator<UpdatePermission>
    {
        public const string ERR_DESCRIPTION_TOO_LONG = "Description must be 4000 characters or less";
        public const string ERR_PERMISSIONID_NOTFOUND = "No permission found for this id";
        private readonly IPermissionService permissionService;
        /// <summary>
        /// Constructs our validator and builds our rules
        /// </summary>
        public UpdatePermissionValidator(IPermissionService permissionService)
        {
            this.permissionService = permissionService;

            // Description must be 4k or less chars if provided (can be null or empty)
            RuleFor(x => x.Description)
                .MaximumLength(4000)
                    .WithMessage(ERR_DESCRIPTION_TOO_LONG);

            // Permission with this Id must exists
            RuleFor(x => x.Id)
                .MustAsync(BeExistingPermissionAsync)
                    .WithMessage(ERR_PERMISSIONID_NOTFOUND);
        }

        /// <summary>
        /// Custom rule helper to ensure the provided id exists
        /// </summary>
        /// <param name="id">The id to ensure exists</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>true if a permission exists with this id</returns>
        public Task<bool> BeExistingPermissionAsync(Guid id, CancellationToken cancellationToken)
            => permissionService.ExistsAsync(id, cancellationToken);
    }
}
