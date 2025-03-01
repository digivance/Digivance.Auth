using Digivance.Auth.Data.Services;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digivance.Auth.Data.Commands
{
    public record UpdateScope
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Optional user friendly description of this scope
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Required unique per tenant name of this scope
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// Fluent validations for our UpdateScope command
    /// </summary>
    public class UpdateScopeValidator : AbstractValidator<UpdateScope>
    {
        public const string ERR_DESCRIPTION_TOO_LONG = "Description must be 4000 characters or less";
        public const string ERR_SCOPE_NAME_EXISTS = "A Scope with this name already exists in this tenant";
        public const string ERR_SCOPEID_NOTFOUND = "No scope found for this id";
        public const string ERR_SCOPENAME_LENGTH = "Scope name must be 100 characters or less";

        private readonly IScopeService scopeService;

        /// <summary>
        /// Constructs our validator and builds our rules
        /// </summary>
        /// <param name="scopeService">The IScopeService implementation to use in rules</param>
        /// <param name="tenantService">The ITenantService implementation to use in rules</param>
        public UpdateScopeValidator(IScopeService scopeService)
        {
            this.scopeService = scopeService;

            // Description must be 4k or less chars if provided (can be null or empty)
            RuleFor(x => x.Description)
                .MaximumLength(4000)
                    .WithMessage(ERR_DESCRIPTION_TOO_LONG);

            // Name must be provided and be unique per tenant id
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100)
                    .WithMessage(ERR_SCOPENAME_LENGTH)
                .MustAsync(BeUniqueNamePerTenantAsync)
                    .WithMessage(ERR_SCOPE_NAME_EXISTS);

            // Tenant with this Id must exists
            RuleFor(x => x.Id)
                .MustAsync(BeExistingScopeAsync)
                    .WithMessage(ERR_SCOPEID_NOTFOUND);
        }

        /// <summary>
        /// Custom rule helper to ensure the provided id exists
        /// </summary>
        /// <param name="id">The id to ensure exists</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>true if a scope exists with this id</returns>
        public Task<bool> BeExistingScopeAsync(Guid id, CancellationToken cancellationToken)
            => scopeService.ExistsAsync(id, cancellationToken);

        /// <summary>
        /// Custom rule helper to ensure this is a unique name for the provided tenant
        /// </summary>
        /// <param name="command">The command we are validating</param>
        /// <param name="name">Is the same as command.Name</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if this is a unique name/tenantid combination</returns>
        public async Task<bool> BeUniqueNamePerTenantAsync(UpdateScope command, string name, CancellationToken cancellationToken)
        {
            var scope = await scopeService.GetByIdAsync(command.Id, cancellationToken);
            var tenantId = scope?.TenantId;

            return !await scopeService.ExistsByNameAsync(tenantId, name, cancellationToken);
        }
            
    }
}
