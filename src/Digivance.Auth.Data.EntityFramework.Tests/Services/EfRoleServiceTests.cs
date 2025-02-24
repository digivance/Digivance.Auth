using Digivance.Auth.Data.Commands;
using Digivance.Auth.Data.EntityFramework.Contexts;
using Digivance.Auth.Data.EntityFramework.Entities;
using Digivance.Auth.Data.EntityFramework.Services;
using Digivance.Auth.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digivance.Auth.Data.EntityFramework.Tests.Services
{
    [TestFixture]
    public class EfRoleServiceTests
    {
        private AuthContext context;
        private EfRoleService service;
        private Guid scopeId;

        [OneTimeSetUp]
        public async Task OneTimeSetup()
        {
            var mapper = new EntityMapper();

            var options = new DbContextOptionsBuilder<AuthContext>()
                .UseInMemoryDatabase("EFRoleServiceTests")
                .Options;

            context = new AuthContext(options);
            service = new EfRoleService(context, mapper);

            //Create tenant
            var scope = new ScopeEntity
            {
                Name = "newScope",
                Description = "123456789abc"
            };

            context.Scopes.Add(scope);
            await context.SaveChangesAsync(default);
            scopeId = scope.Id;
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
            var command = new CreateRole
            {
                Description = "Test Role",
                Name = "TEST-Role"
            };

            var role = await service.CreateAsync(command, default);
            role.ScopeId = scopeId;
            await context.SaveChangesAsync(default);

            await service.DeleteByIdAsync(role.Id, default);

            Assert.Multiple(() =>
            {
                Assert.That(role.CreatedOn, Is.GreaterThanOrEqualTo(start));
                Assert.That(role.CreatedOn, Is.LessThanOrEqualTo(DateTime.UtcNow));
                Assert.That(role.Id, Is.Not.EqualTo(new Guid()));
                
                Assert.That(role.Description, Is.EqualTo(command.Description));
                Assert.That(role.Name, Is.EqualTo(command.Name));
                Assert.That(role.ScopeId, Is.EqualTo(scopeId));
            });
        }

        [Test]
        public async Task Can_DeleteByIdAsync()
        {
            var command = new CreateRole
            {
                Description = "Test role",
                Name = "TEST-role",
                ScopeId = scopeId
            };

            var role = await service.CreateAsync(command, default);
            await service.DeleteByIdAsync(role.Id, default);
            var deletedRole = await service.GetByIdAsync(role.Id, default);

            Assert.That(deletedRole, Is.Null);
        }

        [Test]
        public async Task Can_ExistsAsync()
        {
            var command = new CreateRole
            {
                Description = "Test role",
                Name = "TEST-role",
                ScopeId = scopeId
            };

            var role = await service.CreateAsync(command, default);
            var exists = await service.ExistsAsync(role.Id, default);
            await service.DeleteByIdAsync(role.Id, default);

            Assert.That(exists, Is.True);
        }

        [Test]
        public async Task Can_ExistsByNameAsync()
        {
            var command = new CreateRole
            {
                Description = "Test role",
                Name = "TEST-role",
                ScopeId = scopeId
            };

            var role = await service.CreateAsync(command, default);
            var exists = await service.ExistsByNameAsync(command.ScopeId, command.Name, default);
            await service.DeleteByIdAsync(role.Id, default);

            Assert.That(exists, Is.True);
        }

        [Test]
        public async Task Can_GetByName()
        {
            var command = new CreateRole
            {
                Description = "Test role",
                Name = "TEST-role",
                ScopeId = scopeId
            };

            var role = await service.CreateAsync(command, default);
            var model = await service.GetByNameAsync(command.ScopeId, command.Name, default);
            await service.DeleteByIdAsync(role.Id, default);

            Assert.That(model, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(role.CreatedOn, Is.EqualTo(model.CreatedOn));
                Assert.That(role.Id, Is.EqualTo(model.Id));
                Assert.That(role.ScopeId, Is.EqualTo(scopeId));

            });
        }

        [Test]
        public async Task Can_GetById()
        {
            var command = new CreateRole
            {
                Description = "Test role",
                Name = "TEST-role",
                ScopeId = scopeId
            };

            var role = await service.CreateAsync(command, default);
            var model = await service.GetByIdAsync(role.Id, default);
            await service.DeleteByIdAsync(role.Id, default);

            Assert.That(model, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(model.CreatedOn, Is.EqualTo(role.CreatedOn));
                Assert.That(model.Id, Is.EqualTo(role.Id));

                Assert.That(model.Description, Is.EqualTo(command.Description));
                Assert.That(model.Name, Is.EqualTo(command.Name));
                Assert.That(role.ScopeId, Is.EqualTo(scopeId));
            });
        }

        [Test]
        public async Task Can_Update()
        {
            var createCommand = new CreateRole
            {
                Description = "Test role",
                Name = "TEST-role",
                ScopeId = scopeId
            };

            var role = await service.CreateAsync(createCommand, default);

            var updateCommand = new UpdateRole
            {
                Description = "New Description",
                Name = "newrole",
                Id = role.Id
            };

            var updated = await service.UpdateAsync(updateCommand, default);
            await service.DeleteByIdAsync(role.Id, default);

            Assert.Multiple(() =>
            {
                Assert.That(role, Is.Not.Null);
                Assert.That(updated, Is.Not.Null);
            });

            Assert.Multiple(() =>
            {
                Assert.That(role.Description, Is.EqualTo(createCommand.Description));
                Assert.That(role.Name, Is.EqualTo(createCommand.Name));

                Assert.That(updated.Description, Is.EqualTo(updateCommand.Description));
                Assert.That(role.ScopeId, Is.EqualTo(scopeId));
            });
        }
    }
}
