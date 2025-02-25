using Digivance.Auth.Data.Services;
using FluentValidation;
using System.Net.Mail;

namespace Digivance.Auth.Data.Commands
{
    public record CreateTenant
    {
        /// <summary>
        /// User friendly description of this tenant
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// User friendly name of this tenant
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// Fluent validations for our CreateTenant command
    /// </summary>
    public class CreateTenantValidator : AbstractValidator<CreateTenant>
    {
        public const string ERR_DESCRIPTION_LENGTH = "Description must be 4000 characters or less";
       

        public const string ERR_TENANTNAME_LENGTH = "Tenant name must be 100 characters or less";
        public const string ERR_TENANTNAME_UNIQUE = "An Tenant already exists with this name";

        private readonly ITenantService tenantService;

        /// <summary>
        /// Standard constructor
        /// </summary>
        /// <param name="tenantService">The tenantService service to validate with</param>
        public CreateTenantValidator(ITenantService tenantService)
        {
            this.tenantService = tenantService;

            // Description must be 4k or less chars if provided (can be null or empty)
            RuleFor(x => x.Description)
                .MaximumLength(4000)
                    .WithMessage(ERR_DESCRIPTION_LENGTH);

            // Name must be provided and be unique
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100)
                    .WithMessage(ERR_TENANTNAME_LENGTH)
                .MustAsync(BeUniqueTenantname)
                    .WithMessage(ERR_TENANTNAME_UNIQUE);
        }

        /// <summary>
        /// Checks to see if this Tenantname is already in use
        /// </summary>
        /// <param name="tenantname">The Tenantname to check</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if this is a unique Tenantname</returns>
        public async Task<bool> BeUniqueTenantname(string tenantname, CancellationToken cancellationToken)
            => !await tenantService.ExistsByNameAsync(tenantname, cancellationToken);
    }
}
