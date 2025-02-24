using Digivance.Auth.Data.Commands;
using Digivance.Auth.Data.EntityFramework.Contexts;
using Digivance.Auth.Data.EntityFramework.Entities;
using Digivance.Auth.Data.Models;
using Digivance.Auth.Data.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digivance.Auth.Data.EntityFramework.Services
{
    /// <summary>
    /// Entity framework implementation of our IRoleService
    /// </summary>
    /// <param name="context">The AuthContext to use</param>
    /// <param name="mapper">The EntityMapper we will use when converting to DTOs</param>
    public class EfRoleService(AuthContext context, EntityMapper mapper) : IRoleService
    {
        /// <summary>
        /// Internally used AuthContext
        /// </summary>
        private readonly AuthContext context = context;

        /// <summary>
        /// Internally used EntityMapper
        /// </summary>
        private readonly EntityMapper mapper = mapper;

        /// <inheritdoc />
        public async Task<Role> CreateAsync(CreateRole command, CancellationToken cancellationToken)
        {
            var role = new RoleEntity
            {
                Description = command.Description,
                Name = command.Name,
                ScopeId = command.ScopeId
            };

            context.Roles.Add(role);
            await context.SaveChangesAsync(cancellationToken);
            return mapper.Map<Role>(role);
        }

        /// <inheritdoc />
        public async Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var role = await context.Roles
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (role != null)
            {
                context.Roles.Remove(role);
                await context.SaveChangesAsync(cancellationToken);
            }
        }

        /// <inheritdoc />
        public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
            => context.Roles
                .Where(x => x.Id == id)
                .AnyAsync(cancellationToken);

        /// <inheritdoc />
        public Task<bool> ExistsByNameAsync(Guid scopeId, string name, CancellationToken cancellationToken)
            => context.Roles
                    .Where(x => x.ScopeId == scopeId)
                    .Where(x => x.Name == name)
                    .AnyAsync(cancellationToken);

        /// <inheritdoc />
        public async Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var role = await context.Roles
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (role == null)
                return null;

            return mapper.Map<Role>(role);
        }

        /// <inheritdoc />
        public async Task<Role?> GetByNameAsync(Guid? scopeId, string name, CancellationToken cancellationToken)
        {
            var role = await context.Roles
                .Where(x => x.ScopeId == scopeId)
                .Where(x => x.Name == name)
                .FirstOrDefaultAsync(cancellationToken);

            if (role == null)
                return null;

            return mapper.Map<Role>(role);
        }

        /// <inheritdoc />
        public async Task<Role?> UpdateAsync(UpdateRole command, CancellationToken cancellationToken)
        {
            var role = await context.Roles
                .Where(x => x.Id == command.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (role == null)
                return null;

            role.Description = command.Description;
            role.Name = command.Name;
            await context.SaveChangesAsync(cancellationToken);

            return mapper.Map<Role>(role);
        }
    }
}
