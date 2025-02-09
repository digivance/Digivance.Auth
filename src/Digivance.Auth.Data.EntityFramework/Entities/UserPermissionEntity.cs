using Digivance.Data.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Digivance.Auth.Data.Models;

namespace Digivance.Auth.Data.EntityFramework.Entities
{
    /// <summary>
    /// Relation entity representing what permissions are assigned to what users explicitly
    /// </summary>
    public record UserPermissionEntity : BaseEntity
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
        /// Navigation property of the UserAccount that this relation applies to
        /// </summary>
        public UserEntity User { get; set; }

        /// <summary>
        /// Unique id of the role that this relation applies to
        /// </summary>
        public Guid UserId { get; set; }
    }

    /// <summary>
    /// Entity framework configuration for our UserPermissionEntity
    /// </summary>
    public class UserPermissionConfiguration : IEntityTypeConfiguration<UserPermissionEntity>
    {
        /// <summary>
        /// Configures EF for our UserPermissionEntity
        /// </summary>
        /// <param name="builder">The builder to configure</param>
        public void Configure(EntityTypeBuilder<UserPermissionEntity> builder)
        {
            builder.ConfigureBaseEntity();

            builder.HasOne(x => x.Permission)
                .WithMany(x => x.Users)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.User)
                .WithMany(x => x.Permissions)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }

    /// <summary>
    /// UserAccountPermission entity mapper configuration
    /// </summary>
    public class UserPermissionMapperConfiguration : IEntityMapperConfiguration
    {
        /// <summary>
        /// Configures automapper for UserAccountPermissionEntity -> UserAccount OR Permission DTO
        /// </summary>
        /// <param name="cfg">The configuration builder to use</param>
        public void Configure(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<UserPermissionEntity, Permission>()
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

            cfg.CreateMap<UserPermissionEntity, User>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.User.CreatedBy))
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => src.User.CreatedOn))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.User.DisplayName))
                .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.User.EmailAddress))
                .ForMember(dest => dest.EmailVerifiedOn, opt => opt.MapFrom(src => src.User.EmailVerifiedOn))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.User.Id))
                .ForMember(dest => dest.IsEmailVerified, opt => opt.MapFrom(src => src.User.IsEmailVerified))
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.User.ModifiedBy))
                .ForMember(dest => dest.ModifiedOn, opt => opt.MapFrom(src => src.User.ModifiedOn))
                .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.User.Permissions))
                .ForMember(dest => dest.Tenant, opt => opt.MapFrom(src => src.User.Tenant))
                .ForMember(dest => dest.TenantId, opt => opt.MapFrom(src => src.User.TenantId))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username))
                .PreserveReferences();
        }
    }
}
