using Digivance.Auth.Data.Commands;
using Digivance.Auth.Data.EntityFramework.Contexts;
using Digivance.Auth.Data.EntityFramework.Services;
using Microsoft.EntityFrameworkCore;

namespace Digivance.Auth.Data.EntityFramework.Tests.Services
{
    [TestFixture]
    public class EfPermissionServiceTests
    {
        private AuthContext context;
        private EfPermissionService service;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var mapper = new EntityMapper();

            var options = new DbContextOptionsBuilder<AuthContext>()
                .UseInMemoryDatabase("EFPermissionServiceTests")
                .Options;

            context = new AuthContext(options);
            service = new EfPermissionService(context, mapper);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            if (context != null)
                context.Dispose();
        }

        [Test]
        public async Task Can_CreateAsync()
        {
            var start = DateTime.UtcNow;
            var command = new CreatePermission
            {
                Description = "Test Permission",
                Name = "TEST-PERMISSION"
            };

            // Create and delete our user, we can still Assert expectations against newUser below
            var permission = await service.CreateAsync(command, default);
            await service.DeleteByIdAsync(permission.Id, default);

            Assert.Multiple(() =>
            {
                // Stuff we expect EF / DB did...
                Assert.That(permission.CreatedOn, Is.GreaterThanOrEqualTo(start));
                Assert.That(permission.CreatedOn, Is.LessThanOrEqualTo(DateTime.UtcNow));
                Assert.That(permission.Id, Is.Not.EqualTo(new Guid()));

                // Stuff we expect got persisted correctly
                Assert.That(permission.Description, Is.EqualTo(command.Description));
                Assert.That(permission.Name, Is.EqualTo(command.Name));
            });
        }

        [Test]
        public async Task Can_DeleteByIdAsync()
        {
            var command = new CreatePermission
            {
                Description = "Test Permission",
                Name = "TEST-PERMISSION"
            };

            var permission = await service.CreateAsync(command, default);
            await service.DeleteByIdAsync(permission.Id, default);
            var deletedUser = await service.GetByIdAsync(permission.Id, default);

            Assert.That(deletedUser, Is.Null);
        }

        [Test]
        public async Task Can_ExistsAsync()
        {
            var command = new CreatePermission
            {
                Description = "Test Permission",
                Name = "TEST-PERMISSION"
            };

            var permission = await service.CreateAsync(command, default);
            var exists = await service.ExistsAsync(permission.Id, default);
            await service.DeleteByIdAsync(permission.Id, default);

            Assert.That(exists, Is.True);
        }

        [Test]
        public async Task Can_ExistsByNameAsync()
        {
            var command = new CreatePermission
            {
                Description = "Test Permission",
                Name = "TEST-PERMISSION"
            };

            var permission = await service.CreateAsync(command, default);
            var exists = await service.ExistsByNameAsync(command.ScopeId, command.Name, default);
            await service.DeleteByIdAsync(permission.Id, default);

            Assert.That(exists, Is.True);
        }

        [Test]
        public async Task Can_GetByName()
        {
            var command = new CreatePermission
            {
                Description = "Test Permission",
                Name = "TEST-PERMISSION"
            };

            var permission = await service.CreateAsync(command, default);
            var model = await service.GetByNameAsync(command.ScopeId, command.Name, default);
            await service.DeleteByIdAsync(permission.Id, default);

            Assert.That(model, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(permission.CreatedOn, Is.EqualTo(model.CreatedOn));
                Assert.That(permission.Id, Is.EqualTo(model.Id));

            });
        }

        [Test]
        public async Task Can_GetById()
        {
            var command = new CreatePermission
            {
                Description = "Test Permission",
                Name = "TEST-PERMISSION"
            };

            var permission = await service.CreateAsync(command, default);
            var model = await service.GetByIdAsync(permission.Id, default);
            await service.DeleteByIdAsync(permission.Id, default);

            Assert.That(model, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(model.CreatedOn, Is.EqualTo(permission.CreatedOn));
                Assert.That(model.Id, Is.EqualTo(permission.Id));

                Assert.That(model.Description, Is.EqualTo(command.Description));
                Assert.That(model.Name, Is.EqualTo(command.Name));
            });
        }

        [Test]
        public async Task Can_Update()
        {
            var createCommand = new CreatePermission
            {
                Description = "Test Permission",
                Name = "TEST-PERMISSION"
            };

            var permission = await service.CreateAsync(createCommand, default);

            var updateCommand = new UpdatePermission
            {
                Description = "New Description"
            };

            var updated = await service.UpdateAsync(permission.Id, updateCommand, default);
            await service.DeleteByIdAsync(permission.Id, default);

            Assert.Multiple(() =>
            {
                Assert.That(permission, Is.Not.Null);
                Assert.That(updated, Is.Not.Null);
            });

            Assert.Multiple(() =>
            {
                Assert.That(permission.Description, Is.EqualTo(createCommand.Description));
                Assert.That(permission.Name, Is.EqualTo(createCommand.Name));

                Assert.That(updated.Description, Is.EqualTo(updateCommand.Description));
            });
        }
    }
}
