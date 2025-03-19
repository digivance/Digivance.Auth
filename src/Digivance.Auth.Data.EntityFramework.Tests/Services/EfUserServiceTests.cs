using Digivance.Auth.Data.Commands;
using Digivance.Auth.Data.EntityFramework.Contexts;
using Digivance.Auth.Data.EntityFramework.Services;
using Microsoft.EntityFrameworkCore;

namespace Digivance.Auth.Data.EntityFramework.Tests.Services
{
    [TestFixture]
    public class EfUserServiceTests
    {
        private AuthContext context;
        private EfUserService service;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var mapper = new EntityMapper();

            var options = new DbContextOptionsBuilder<AuthContext>()
                .UseInMemoryDatabase("EFUserServiceTests")
                .Options;

            context = new AuthContext(options);
            service = new EfUserService(context, mapper);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            context?.Dispose();
        }

        [TestCase("Test User", "valid@address.com", "password", "testuser")]
        public async Task Can_CreateAsync(string displayName, string emailAddress, string password, string username)
        {
            var start = DateTime.UtcNow;
            var command = new CreateUser
            {
                DisplayName = displayName,
                EmailAddress = emailAddress,
                Password = password,
                Username = username
            };

            // Create and delete our user, we can still Assert expectations against newUser below
            var newUser = await service.CreateAsync(command, default);
            await service.DeleteByIdAsync(newUser.Id, default);

            Assert.Multiple(() =>
            {
                // Stuff we expect EF / DB did...
                Assert.That(newUser.CreatedOn, Is.GreaterThanOrEqualTo(start));
                Assert.That(newUser.CreatedOn, Is.LessThanOrEqualTo(DateTime.UtcNow));
                Assert.That(newUser.Id, Is.Not.EqualTo(new Guid()));

                // Stuff we expect got persisted correctly
                Assert.That(newUser.DisplayName, Is.EqualTo(displayName));
                Assert.That(newUser.EmailAddress, Is.EqualTo(emailAddress));
                Assert.That(newUser.IsEmailVerified, Is.False);
                Assert.That(newUser.Username, Is.EqualTo(username));
            });
        }

        [Test]
        public async Task Can_DeleteByIdAsync()
        {
            var command = new CreateUser
            {
                DisplayName = "",
                EmailAddress = "email@address.com",
                Password = "password",
                Username = "username"
            };

            var newUser = await service.CreateAsync(command, default);
            await service.DeleteByIdAsync(newUser.Id, default);
            var deletedUser = await service.GetByIdAsync(newUser.Id, default);

            Assert.That(deletedUser, Is.Null);
        }

        [Test]
        public async Task Can_ExistsAsync()
        {
            var command = new CreateUser
            {
                DisplayName = "",
                EmailAddress = "email@address.com",
                Password = "password",
                Username = "username"
            };

            var newUser = await service.CreateAsync(command, default);
            var exists = await service.ExistsAsync(newUser.Id, default);
            await service.DeleteByIdAsync(newUser.Id, default);

            Assert.That(exists, Is.True);
        }

        [Test]
        public async Task Can_ExistsByEmailAsync()
        {
            var command = new CreateUser
            {
                DisplayName = "",
                EmailAddress = "email@address.com",
                Password = "password",
                Username = "username"
            };

            var newUser = await service.CreateAsync(command, default);
            var exists = await service.ExistsByEmailAsync(command.TenantId, newUser.EmailAddress, default);
            await service.DeleteByIdAsync(newUser.Id, default);

            Assert.That(exists, Is.True);
        }

        [Test]
        public async Task Can_ExistsByUsernameAsync()
        {
            var command = new CreateUser
            {
                DisplayName = "",
                EmailAddress = "email@address.com",
                Password = "password",
                Username = "username"
            };

            var newUser = await service.CreateAsync(command, default);
            var exists = await service.ExistsByUsernameAsync(newUser.TenantId, newUser.Username!, default);
            await service.DeleteByIdAsync(newUser.Id, default);

            Assert.That(exists, Is.True);
        }

        [Test]
        public async Task Can_GetByEmail()
        {
            var command = new CreateUser
            {
                DisplayName = "",
                EmailAddress = "email@address.com",
                Password = "password",
                Username = "username"
            };

            var newUser = await service.CreateAsync(command, default);
            var model = await service.GetByEmailAddressAsync(command.TenantId, newUser.EmailAddress, default);
            await service.DeleteByIdAsync(newUser.Id, default);

            Assert.That(model, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(newUser.CreatedOn, Is.EqualTo(model.CreatedOn));
                Assert.That(newUser.Id, Is.EqualTo(model.Id));

                Assert.That(newUser.DisplayName, Is.EqualTo(model.DisplayName));
                Assert.That(newUser.EmailAddress, Is.EqualTo(model.EmailAddress));
                Assert.That(newUser.IsEmailVerified, Is.False);
                Assert.That(newUser.Username, Is.EqualTo(model.Username));
            });
        }

        [Test]
        public async Task Can_GetById()
        {
            var command = new CreateUser
            {
                DisplayName = "",
                EmailAddress = "email@address.com",
                Password = "password",
                Username = "username"
            };

            var newUser = await service.CreateAsync(command, default);
            var model = await service.GetByIdAsync(newUser.Id, default);
            await service.DeleteByIdAsync(newUser.Id, default);

            Assert.That(model, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(newUser.CreatedOn, Is.EqualTo(model.CreatedOn));
                Assert.That(newUser.Id, Is.EqualTo(model.Id));

                Assert.That(newUser.DisplayName, Is.EqualTo(model.DisplayName));
                Assert.That(newUser.EmailAddress, Is.EqualTo(model.EmailAddress));
                Assert.That(newUser.IsEmailVerified, Is.False);
                Assert.That(newUser.Username, Is.EqualTo(model.Username));
            });
        }

        [Test]
        public async Task Can_GetByUsername()
        {
            var command = new CreateUser
            {
                DisplayName = "",
                EmailAddress = "email@address.com",
                Password = "password",
                Username = "username"
            };

            var newUser = await service.CreateAsync(command, default);
            var model = await service.GetByUsernameAsync(newUser.TenantId, newUser.Username!, default);
            await service.DeleteByIdAsync(newUser.Id, default);

            Assert.That(model, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(newUser.CreatedOn, Is.EqualTo(model.CreatedOn));
                Assert.That(newUser.Id, Is.EqualTo(model.Id));

                Assert.That(newUser.DisplayName, Is.EqualTo(model.DisplayName));
                Assert.That(newUser.EmailAddress, Is.EqualTo(model.EmailAddress));
                Assert.That(newUser.IsEmailVerified, Is.False);
                Assert.That(newUser.Username, Is.EqualTo(model.Username));
            });
        }

        [Test]
        public async Task Can_Update()
        {
            var createCommand = new CreateUser
            {
                DisplayName = "",
                EmailAddress = "email@address.com",
                Password = "password",
                Username = "username"
            };

            var newUser = await service.CreateAsync(createCommand, default);

            var updateCommand = new UpdateUser
            {
                DisplayName = "New Display Name",
                Id = newUser.Id,
                Username = "NewUsername"
            };

            var updatedUser = await service.UpdateAsync(updateCommand, default);
            await service.DeleteByIdAsync(newUser.Id, default);

            Assert.Multiple(() =>
            {
                Assert.That(newUser, Is.Not.Null);
                Assert.That(updatedUser, Is.Not.Null);
            });

            Assert.Multiple(() =>
            {
                Assert.That(newUser.DisplayName, Is.EqualTo(createCommand.DisplayName));
                Assert.That(newUser.EmailAddress, Is.EqualTo(createCommand.EmailAddress));
                Assert.That(newUser.Username, Is.EqualTo(createCommand.Username));

                Assert.That(updatedUser.DisplayName, Is.EqualTo(updateCommand.DisplayName));
                Assert.That(updatedUser.Username, Is.EqualTo(updateCommand.Username));
            });
        }
    }
}
