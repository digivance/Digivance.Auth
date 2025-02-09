using Digivance.Data.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Digivance.Auth.Data.Models;

namespace Digivance.Auth.Data.EntityFramework.Entities
{
    /// <summary>
    /// Relation entity representing what permissions are assigned to what roles
    /// </summary>
    public record RolePermissionEntity : BaseEntity
    {
        /// <summary>
        /// Navigation property to the permission this relation applies to
        /// </summary>
        public PermissionEntity Permission { get; set; }

        /// <summary>
        /// Unique id of the permission this relation applies to
        /// </summary>
        public Guid PermissionId { get; set; }

        /// <summary>
        /// Navigation property of the role that this relation applies to
        /// </summary>
        public RoleEntity Role { get; set; }

        /// <summary>
        /// Unique id of the role that this relation applies to
        /// </summary>
        public Guid RoleId { get; set; }
    }

    /// <summary>
    /// Entity framework configuration for our RolePermissionEntity
    /// </summary>
    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermissionEntity>
    {
        /// <summary>
        /// Configures EF for our RolePermissionEntity
        /// </summary>
        /// <param name="builder">The builder to configure</param>
        public void Configure(EntityTypeBuilder<RolePermissionEntity> builder)
        {
            builder.ConfigureBaseEntity();

            builder.HasOne(x => x.Permission)
                .WithMany(x => x.Roles)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Role)
                .WithMany(x => x.Permissions)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }

    /// <summary>
    /// RolePermission entity mapper configuration
    /// </summary>
    public class RolePermissionMapperConfiguration : IEntityMapperConfiguration
    {
        /// <summary>
        /// Configures automapper for RolePermissionEntity -> Role OR Permission DTO
        /// </summary>
        /// <param name="cfg">The configuration builder to use</param>
        public void Configure(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<RolePermissionEntity, Permission>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.Permission.CreatedBy))
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => src.Permission.CreatedOn))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Permission.Description))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Permission.Id))
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.Permission.ModifiedBy))
                .ForMember(dest => dest.ModifiedOn, opt => opt.MapFrom(src => src.Permission.ModifiedOn))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Permission.Name))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Permission.Roles))
                .ForMember(dest => dest.Scope, opt => opt.MapFrom(src => src.Permission.Scope))
                .ForMember(dest => dest.ScopeId, opt => opt.MapFrom(src => src.Permission.ScopeId))
                .ForMember(dest => dest.Tenant, opt => opt.MapFrom(src => src.Permission.Tenant))
                .ForMember(dest => dest.TenantId, opt => opt.MapFrom(src => src.Permission.TenantId))
                .PreserveReferences();

            cfg.CreateMap<RolePermissionEntity, Role>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.Role.CreatedBy))
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => src.Role.CreatedOn))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Role.Description))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Role.Id))
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.Role.ModifiedBy))
                .ForMember(dest => dest.ModifiedOn, opt => opt.MapFrom(src => src.Role.ModifiedOn))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Role.Name))
                .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.Role.Permissions))
                .ForMember(dest => dest.Scope, opt => opt.MapFrom(src => src.Role.Scope))
                .ForMember(dest => dest.ScopeId, opt => opt.MapFrom(src => src.Role.ScopeId))
                .ForMember(dest => dest.Tenant, opt => opt.MapFrom(src => src.Role.Tenant))
                .ForMember(dest => dest.TenantId, opt => opt.MapFrom(src => src.Role.TenantId))
                .PreserveReferences();
        }
    }
}
