using AutoMapper;
using Digivance.Auth.Data.Models;
using Digivance.Data.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Digivance.Auth.Data.EntityFramework.Entities
{
    /// <summary>
    /// Represents a permission in our system
    /// </summary>
    public record PermissionEntity : BaseEntity
    {
        /// <summary>
        /// Human friendly description of this permission
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// This is a string representing the access that this permission allows. Can
        /// be one of the EntityAccess system permissions or custom
        /// </summary>
        public string EntityAccess { get; set; }

        /// <summary>
        /// This is the unique id of the entity that this permission is granting access to.
        /// If null, it is granting this EntityAccess to ALL entities of this type. If null,
        /// this permission may be limited to ScopeId.  See ScopeId
        /// </summary>
        public Guid? EntityId { get; set; }

        /// <summary>
        /// The type name of the entity that this permission applies to (such as the name
        /// of the model that this permission applies to)
        /// </summary>
        public string EntityType { get; set; }

        /// <summary>
        /// Navigation property of the role permissions containing this permission
        /// </summary>
        public ICollection<RolePermissionEntity> Roles { get; set; }

        /// <summary>
        /// The scope that this permission grants access to. Always present but only applicable 
        /// when EntityId is null, See Scope
        /// </summary>
        public ScopeEntity Scope { get; set; }

        /// <summary>
        /// Unique id of the scope that this permission belongs to. Always present but only 
        /// applicable when EntityId is null, See Scope
        /// </summary>
        public Guid ScopeId { get; set; }

        /// <summary>
        /// Users that have this permission assigned explicitly
        /// </summary>
        public ICollection<UserPermissionEntity> Users { get; set; }
    }

    /// <summary>
    /// Entity framework configuration for PermissionEntity
    /// </summary>
    public class PermissionEntityConfiguration : IEntityTypeConfiguration<PermissionEntity>
    {
        /// <summary>
        /// Configures EF for our PermissionEntity
        /// </summary>
        /// <param name="builder">The builder to configure</param>
        public void Configure(EntityTypeBuilder<PermissionEntity> builder)
        {
            builder.ConfigureBaseEntity();

            builder.Property(x => x.Description)
                .HasMaxLength(4000)
                .IsRequired(false);

            builder.Property(x => x.EntityAccess)
                .HasMaxLength(255)
                .IsRequired(true);

            builder.Property(x => x.EntityId)
                .IsRequired(false);

            builder.Property(x => x.EntityType)
                .HasMaxLength(255)
                .IsRequired(true);

            builder.HasMany(x => x.Roles)
                .WithOne(x => x.Permission);

            builder.HasOne(x => x.Scope)
                .WithMany(x => x.Permissions);

            // Not sure about this seems pretty wide but lets try it...  This index
            // should support WHERE EntityAccess AND EntityId AND EntityType AND ScopeId
            // also constrains unique per all 4
            builder.HasIndex(x => new { x.EntityAccess, x.EntityId, x.EntityType, x.ScopeId })
                .IsUnique(true);
        }
    }

    /// <summary>
    /// Permission entity mapper configuration
    /// </summary>
    public class PermissionMapperConfiguration : IEntityMapperConfiguration
    {
        /// <summary>
        /// Configures automapper for PermissionEntity -> Permission DTO
        /// </summary>
        /// <param name="cfg">The configuration builder to use</param>
        public void Configure(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<PermissionEntity, Permission>()
                .PreserveReferences();
        }
    }
}
