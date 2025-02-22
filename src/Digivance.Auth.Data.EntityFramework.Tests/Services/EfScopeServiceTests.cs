using Digivance.Auth.Data.Commands;
using Digivance.Auth.Data.EntityFramework.Contexts;
using Digivance.Auth.Data.EntityFramework.Entities;
using Digivance.Auth.Data.EntityFramework.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Digivance.Auth.Data.EntityFramework.Tests.Services
{
    [TestFixture]
    public class EfScopeServiceTests
    {
        private AuthContext context;
        private EfScopeService service;
        private Guid tenantId;

        [OneTimeSetUp]
        public async Task OneTimeSetup()
        {
            var mapper = new EntityMapper();

            var options = new DbContextOptionsBuilder<AuthContext>()
                .UseInMemoryDatabase("EFScopeServiceTests")
                .Options;

            context = new AuthContext(options);
            service = new EfScopeService(context, mapper);

            //Create tenant
            var tenant = new TenantEntity
            {
                Name = "newTenant",
                Description = "123456789abc"
            };

            context.Tenants.Add(tenant);
            await context.SaveChangesAsync(default);
            tenantId = tenant.Id;
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            context?.Dispose();
        }

        [Test]
        public async Task Can_CreateAsync()
        {
            var start = DateTime.UtcNow;
    
            var command = new CreateScope
            {
                Description = "Test Scope",
                Name = "TEST-SCOPE"
            };

            var newScope = await service.CreateAsync(command, default);
            newScope.TenantId = tenantId;
            await context.SaveChangesAsync(default);

            await service.DeleteByIdAsync(newScope.Id, default);

            Assert.Multiple(() =>
            {
                Assert.That(newScope.CreatedOn, Is.GreaterThanOrEqualTo(start));
                Assert.That(newScope.CreatedOn, Is.LessThanOrEqualTo(DateTime.UtcNow));
                Assert.That(newScope.Id, Is.Not.EqualTo(new Guid()));

                Assert.That(newScope.Description, Is.EqualTo(command.Description));
                Assert.That(newScope.Name, Is.EqualTo(command.Name));
                Assert.That(newScope.TenantId, Is.EqualTo(tenantId));
            });
        }

        [Test]
        public async Task Can_DeleteByIdAsync()
        {
            var command = new CreateScope
            {
                Description = "Test Scope",
                Name = "TEST-SCOPE",
                TenantId = tenantId
            };

            var newScope = await service.CreateAsync(command, default);
            await service.DeleteByIdAsync(newScope.Id, default);
            var deletedScope = await service.GetByIdAsync(newScope.Id, default);

            Assert.That(deletedScope, Is.Null);
        }

        [Test]
        public async Task Can_ExistsAsync()
        {
            var command = new CreateScope
            {
                Description = "Test Scope",
                Name = "TEST-SCOPE",
                TenantId = tenantId
            };

            var newScope = await service.CreateAsync(command, default);

            var exists = await service.ExistsAsync(newScope.Id, default);
            await service.DeleteByIdAsync(newScope.Id, default);

            Assert.That(exists, Is.True);
        }

        [Test]
        public async Task Can_ExistsByNameAsync()
        {
            var command = new CreateScope
            {
                Description = "Test Scope",
                Name = "TEST-SCOPE",
                TenantId = tenantId
            };

            var newScope = await service.CreateAsync(command, default);
            var exists = await service.ExistsByNameAsync(tenantId, newScope.Name, default);
            await service.DeleteByIdAsync(newScope.Id, default);

            Assert.That(exists, Is.True);
        }

        [Test]
        public async Task Can_GetById()
        {
            var command = new CreateScope
            {
                Description = "Test Scope",
                Name = "TEST-SCOPE",
                TenantId = tenantId
            };

            var newScope = await service.CreateAsync(command, default);
            var model = await service.GetByIdAsync(newScope.Id, default);
            await service.DeleteByIdAsync(newScope.Id, default);

            Assert.That(model, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(newScope.CreatedOn, Is.EqualTo(model.CreatedOn));
                Assert.That(newScope.Id, Is.EqualTo(model.Id));

                Assert.That(newScope.Name, Is.EqualTo(model.Name));
                Assert.That(newScope.Description, Is.EqualTo(model.Description));
                Assert.That(newScope.TenantId, Is.EqualTo(tenantId));
            });
        }

        [Test]
        public async Task Can_GetByName()
        {
            var command = new CreateScope
            {
                Description = "Test Scope",
                Name = "TEST-SCOPE",
                TenantId = tenantId
            };

            var newScope = await service.CreateAsync(command, default);
            var model = await service.GetByNameAsync(tenantId, newScope.Name, default);
            await service.DeleteByIdAsync(newScope.Id, default);

            Assert.That(model, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(newScope.CreatedOn, Is.EqualTo(model.CreatedOn));
                Assert.That(newScope.Id, Is.EqualTo(model.Id));

                Assert.That(newScope.Name, Is.EqualTo(model.Name));
                Assert.That(newScope.Description, Is.EqualTo(model.Description));
                Assert.That(newScope.TenantId, Is.EqualTo(tenantId));
            });
        }

        [Test]
        public async Task Can_Update()
        {
            var createCommand = new CreateScope
            {
                Description = "Test Scope",
                Name = "TEST-SCOPE",
                TenantId = tenantId
            };

            var newScope = await service.CreateAsync(createCommand, default);

            var updateCommand = new UpdateScope
            {
                Description = "New Scope",
                Name = "NEW-SCOPE",
                Id = newScope.Id
            };

            var updatedScope = await service.UpdateAsync(updateCommand, default);
            await service.DeleteByIdAsync(newScope.Id, default);

            Assert.Multiple(() =>
            {
                Assert.That(newScope, Is.Not.Null);
                Assert.That(updatedScope, Is.Not.Null);
            });

            Assert.Multiple(() =>
            {
                Assert.That(newScope.Name, Is.EqualTo(createCommand.Name));
                Assert.That(newScope.Description, Is.EqualTo(createCommand.Description));

                Assert.That(updatedScope.Name, Is.EqualTo(updateCommand.Name));
                Assert.That(updatedScope.Description, Is.EqualTo(updateCommand.Description));
                Assert.That(newScope.TenantId, Is.EqualTo(tenantId));
            });
        }
    }
}
