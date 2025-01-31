using Digivance.Auth.Data.Models;
using Digivance.Data.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Digivance.Auth.Data.EntityFramework.Entities
{
    /// <summary>
    /// Represents a scope within a tenant. Scopes can be anything the user needs.
    /// </summary>
    public record ScopeEntity : BaseEntity
    {
        /// <summary>
        /// User friendly description of this scope
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// User friendly name of this scope
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// The permissions that apply to this scope (may not be loaded when scope
        /// is a child)
        /// </summary>
        public ICollection<PermissionEntity>? Permissions { get; set; }

        /// <summary>
        /// The roles that apply to this scope (may not be loaded when scope is a child)
        /// </summary>
        public ICollection<RoleEntity>? Roles { get; set; }

        /// <summary>
        /// Unique id of the Tenant this scope belongs to
        /// </summary>
        public Guid TenantId { get; set; }

        /// <summary>
        /// The tenant that this scope belongs to (may not be loaded when scope
        /// is a child, always trust TenantId instead)
        /// </summary>
        public TenantEntity? Tenant { get; set; }

        /// <summary>
        /// Converts to Tenant model
        /// </summary>
        /// <param name="recursion">Stops recursing once BaseModel.RecursionLimit is hit</param>
        /// <returns>Tenant model</returns>
        public Scope ToModel(int? maxDepth = null, int currentDepth = 0)
        {
            if (maxDepth.HasValue && currentDepth >= maxDepth.Value) return null;

            var scope = ToBaseModel<Scope>();
            scope.Name = Name;
            scope.Description = Description;
            scope.Permissions = Permissions?.Select(x => x.ToModel(maxDepth, currentDepth + 1)).ToList();
            scope.Roles = Roles?.Select(r => r.ToModel(maxDepth, currentDepth + 1)).ToList();
            scope.Tenant = Tenant?.ToModel(maxDepth, currentDepth + 1);

            return scope;
        }
    }

    /// <summary>
    /// Entity framework configuration for our ScopeEntity
    /// </summary>
    public class ScopeEntityConfiguration : IEntityTypeConfiguration<ScopeEntity>
    {
        /// <summary>
        /// Configures EF for our ScopeEntity
        /// </summary>
        /// <param name="builder">Builder to configure</param>
        public void Configure(EntityTypeBuilder<ScopeEntity> builder)
        {
            builder.ConfigureBaseEntity();

            builder.Property(x => x.Name)
                .HasMaxLength(255)
                .IsRequired(true);

            builder.HasIndex(x => x.Name)
                .IsUnique(true);

            builder.HasMany(x => x.Roles)
                .WithOne(x => x.Scope);

            builder.HasMany(x => x.Permissions)
                .WithOne(x => x.Scope);
        }
    }
}
