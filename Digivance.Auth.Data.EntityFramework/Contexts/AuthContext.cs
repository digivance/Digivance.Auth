using Digivance.Auth.Data.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Digivance.Auth.Data.EntityFramework.Contexts
{
    /// <summary>
    /// Our EF database context
    /// </summary>
    public class AuthContext : DbContext
    {
        public DbSet<PermissionEntity> Permissions { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<TenantEntity> Tenants { get; set; }
        public DbSet<UserAccountEntity> UserAccounts { get; set; }

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="options">The DbContextOptions to build with</param>
        public AuthContext(DbContextOptions options) : base(options) { }

        /// <summary>
        /// Configures our entities when model is "building"
        /// </summary>
        /// <param name="modelBuilder">The model builder to configure</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // TODO: Change this if we micro service out the various contexts
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Sets audit fields then saves changes normally
        /// </summary>
        /// <returns>The number or records affected</returns>
        public override int SaveChanges()
        {
            SetAuditFields();
            return base.SaveChanges();
        }

        /// <summary>
        /// Sets audit fields then saves changes normally
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The number or records affected</returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            SetAuditFields();
            var res = await base.SaveChangesAsync(cancellationToken);
            ChangeTracker.Clear();
            return res;
        }

        /// <summary>
        /// Sets some common audit fields on all added or modified BaseModels (or derivitives)
        /// </summary>
        protected void SetAuditFields()
        {
            var baseEntries = ChangeTracker.Entries()
                .Where(entry => entry.State == EntityState.Added || entry.State == EntityState.Modified)
                .Where(entry => entry.Entity is BaseEntity)
                .ToList();

            foreach (var entry in baseEntries)
            {
                if (entry.Entity is BaseEntity entity)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedOn = DateTime.UtcNow;
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        entity.ModifiedOn = DateTime.UtcNow;
                    }
                }
            }
        }
    }
}
