using Digivance.Auth.Data.Commands;
using Digivance.Auth.Data.EntityFramework.Contexts;
using Digivance.Auth.Data.EntityFramework.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digivance.Auth.Data.EntityFramework.Tests.Services
{
    [TestFixture]
    public class EfTenantServiceTests
    {
        private AuthContext context;
        private EfTenantService service;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var mapper = new EntityMapper();

            var options = new DbContextOptionsBuilder<AuthContext>()
                .UseInMemoryDatabase("EfTenantServiceTests")
                .Options;

            context = new AuthContext(options);
            service = new EfTenantService(context, mapper);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            if (context != null)
                context.Dispose();
        }

        [TestCase("Test Tenant", "Once changed, implement a new EfTenantService for all these methods.Add unit tests for all EfTenantService methods")]
        public async Task Can_CreateAsync(string name, string description)
        {
            var start = DateTime.UtcNow;
            var command = new CreateTenant
            {
                Name = name,
                Description = description
            };

            var newTenant = await service.CreateAsync(command, default);
            await service.DeleteByIdAsync(newTenant.Id, default);

            Assert.Multiple(() =>
            {
                Assert.That(newTenant.CreatedOn, Is.GreaterThanOrEqualTo(start));
                Assert.That(newTenant.CreatedOn, Is.LessThanOrEqualTo(DateTime.UtcNow));
                Assert.That(newTenant.Id, Is.Not.EqualTo(new Guid()));

                Assert.That(newTenant.Name, Is.EqualTo(name));
                Assert.That(newTenant.Description, Is.EqualTo(description));
            });
        }

        [Test]
        public async Task Can_DeleteByIdAsync()
        {
            var command = new CreateTenant
            {
                Name = "newTenant",
                Description = "123456789abc"
            };

            var newTenant = await service.CreateAsync(command, default);
            await service.DeleteByIdAsync(newTenant.Id, default);
            var deletedTenant = await service.GetByIdAsync(newTenant.Id, default);

            Assert.That(deletedTenant, Is.Null);
        }

        [Test]
        public async Task Can_ExistsAsync()
        {
            var command = new CreateTenant
            {
                Name = "newTenant",
                Description = "123456789abc"
            };

            var newTenant = await service.CreateAsync(command, default);
            var exists = await service.ExistsAsync(newTenant.Id, default);
            await service.DeleteByIdAsync(newTenant.Id, default);

            Assert.That(exists, Is.True);
        }

        [Test]
        public async Task Can_ExistsByNameAsync()
        {
            var command = new CreateTenant
            {
                Name = "newTenant",
                Description = "123456789abc"
            };

            var newTenant = await service.CreateAsync(command, default);
            var exists = await service.ExistsByNameAsync(newTenant.Name, default);
            await service.DeleteByIdAsync(newTenant.Id, default);

            Assert.That(exists, Is.True);
        }


        [Test]
        public async Task Can_GetById()
        {
            var command = new CreateTenant
            {
                Name = "newTenant",
                Description = "123456789abc"
            };

            var newTenant = await service.CreateAsync(command, default);
            var model = await service.GetByIdAsync(newTenant.Id, default);
            await service.DeleteByIdAsync(newTenant.Id, default);

            Assert.That(model, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(newTenant.CreatedOn, Is.EqualTo(model.CreatedOn));
                Assert.That(newTenant.Id, Is.EqualTo(model.Id));

                Assert.That(newTenant.Name, Is.EqualTo(model.Name));
                Assert.That(newTenant.Description, Is.EqualTo(model.Description));
            });
        }

        [Test]
        public async Task Can_GetByName()
        {
            var command = new CreateTenant
            {
                Name = "newTenant",
                Description = "123456789abc"
            };

            var newTenant = await service.CreateAsync(command, default);
            var model = await service.GetByNameAsync(newTenant.Name, default);
            await service.DeleteByIdAsync(newTenant.Id, default);

            Assert.That(model, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(newTenant.CreatedOn, Is.EqualTo(model.CreatedOn));
                Assert.That(newTenant.Id, Is.EqualTo(model.Id));

                Assert.That(newTenant.Name, Is.EqualTo(model.Name));
                Assert.That(newTenant.Description, Is.EqualTo(model.Description));
            });
        }


        [Test]
        public async Task Can_Update()
        {
            var createCommand = new CreateTenant
            {
                Name = "oldTenant",
                Description = "123456789abc"
            };

            var newTenant = await service.CreateAsync(createCommand, default);

            var updateCommand = new UpdateTenant
            {
                Name = "newTenant",
                Description = "abc123456789",
                Id = newTenant.Id
            };

            var updatedTenant = await service.UpdateAsync(updateCommand, default);
            await service.DeleteByIdAsync(newTenant.Id, default);

            Assert.Multiple(() =>
            {
                Assert.That(newTenant, Is.Not.Null);
                Assert.That(updatedTenant, Is.Not.Null);
            });

            Assert.Multiple(() =>
            {
                Assert.That(newTenant.Name, Is.EqualTo(createCommand.Name));
                Assert.That(newTenant.Description, Is.EqualTo(createCommand.Description));

                Assert.That(updatedTenant.Name, Is.EqualTo(updateCommand.Name));
                Assert.That(updatedTenant.Description, Is.EqualTo(updateCommand.Description));
            });
        }
    }
}
