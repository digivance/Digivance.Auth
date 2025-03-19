using Digivance.Auth.Data.Services;
using FluentValidation;

namespace Digivance.Auth.Data.Commands
{
    public record UpdateTenant
    {
        /// <summary>
        /// Unique id of the tenant we want to update
        /// </summary>
        public Guid Id { get; set; }

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
    /// Fluent validations for our UpdateTenant command
    /// </summary>
    public class UpdateTenantValidator : AbstractValidator<UpdateTenant>
    {
        public const string ERR_DESCRIPTION_LENGTH = "Description must be 4000 characters or less";
        public const string ERR_TENANTID_NOTFOUND = "No tenant found for this tenant id";
        public const string ERR_TENANTNAME_LENGTH = "Tenant name must be 100 characters or less";
        public const string ERR_TENANTNAME_UNIQUE = "An Tenant already exists with this name";
        public const string ERR_TENANT_NAME_EMPTY = "Tenant name is required";

        private readonly ITenantService tenantService;

        /// <summary>
        /// Standard constructor
        /// </summary>
        /// <param name="tenantService">The tenantService service to validate with</param>
        public UpdateTenantValidator(ITenantService tenantService)
        {
            this.tenantService = tenantService;

            // Description must be 4k or less chars if provided (can be null or empty)
            RuleFor(x => x.Description)
                .MaximumLength(4000)
                    .WithMessage(ERR_DESCRIPTION_LENGTH);

            // Tenant with this Id must exists
            RuleFor(x => x.Id)
                .MustAsync(BeExistingTenant)
                    .WithMessage(ERR_TENANTID_NOTFOUND);

            // Name must be provided and be unique
            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithMessage(ERR_TENANT_NAME_EMPTY)
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
            => !await tenantService.ExistsAsync(tenantname, cancellationToken);

        /// <summary>
        /// Ensures the provided tenant id exists
        /// </summary>
        /// <param name="tenantId">Unique id of the tenant we are validating this command for</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if this tenant exists</returns>
        public Task<bool> BeExistingTenant(Guid tenantId, CancellationToken cancellationToken)
            => tenantService.ExistsAsync(tenantId, cancellationToken);
    }
}
