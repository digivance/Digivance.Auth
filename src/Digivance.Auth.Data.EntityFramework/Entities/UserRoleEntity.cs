using Digivance.Data.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Digivance.Auth.Data.Models;

namespace Digivance.Auth.Data.EntityFramework.Entities
{
    /// <summary>
    /// Relation entity representing what roles are assigned to what users
    /// </summary>
    public record UserRoleEntity : BaseEntity
    {
        /// <summary>
        /// Navigation property to the role this relation applies to
        /// </summary>
        public RoleEntity Role { get; set; }

        /// <summary>
        /// Unique id of the role this relation applies to
        /// </summary>
        public Guid RoleId { get; set; }

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
    /// Entity framework configuration for our UserRoleEntity
    /// </summary>
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRoleEntity>
    {
        /// <summary>
        /// Configures EF for our UserRoleEntity
        /// </summary>
        /// <param name="builder">The builder to configure</param>
        public void Configure(EntityTypeBuilder<UserRoleEntity> builder)
        {
            builder.ConfigureBaseEntity();

            builder.HasOne(x => x.Role)
                .WithMany(x => x.Users)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.User)
                .WithMany(x => x.Roles)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }

    /// <summary>
    /// UserRole entity mapper configuration
    /// </summary>
    public class UserRoleMapperConfiguration : IEntityMapperConfiguration
    {
        /// <summary>
        /// Configures automapper for UserRoleEntity -> UserAccount OR Permission DTO
        /// </summary>
        /// <param name="cfg">The configuration builder to use</param>
        public void Configure(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<UserRoleEntity, Role>()
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

            cfg.CreateMap<UserRoleEntity, User>()
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
