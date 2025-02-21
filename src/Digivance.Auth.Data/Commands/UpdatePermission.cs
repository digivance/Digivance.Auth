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
        public string Description { get; set; }
    }

    /// <summary>
    /// Fluent validations for our UpdatePermission command
    /// </summary>
    public class UpdatePermissionValidator : AbstractValidator<UpdatePermission>
    {
        public const string ERR_DESCRIPTION_TOO_LONG = "Description must be 4000 characters or less";

        /// <summary>
        /// Constructs our validator and builds our rules
        /// </summary>
        public UpdatePermissionValidator()
        {
            // Description must be 4k or less chars if provided (can be null or empty)
            RuleFor(x => x.Description)
                .MaximumLength(4000)
                    .WithMessage(ERR_DESCRIPTION_TOO_LONG);
        }
    }
}
